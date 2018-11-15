using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class Game
{
    #region non-game components
    public static MessageManager MessageManager;
    public static IRequestUIController RequestUIController;
    #endregion


    public static void Initialize()
    {
        Config.Load();
        Strings.Load(""); // default language
        Player = new Player();
        Rival = new Rival();
        TurnCount = 0;
        AttackingUnit = null;
        DefendingUnit = null;
        CriticalFlag = false;
        AvoidFlag = false;
        CCBonusList = new List<Card>();
        InductionSetList = new List<List<Induction>>();
        PlayFirstTurn = false;
        Request.ClearNextResults(); // for testing
        LosingProcessDisabled = false; // for testing
        InduceInRivalTurn = false; // for testing
    }

    public static Player Player;
    public static Rival Rival;

    //回合信息
    public static bool PlayFirstTurn;
    public static User TurnPlayer;
    public static User NotTurnPlayer => TurnPlayer.Opponent;
    public static int TurnCount;
    public static Phase CurrentPhase;
    public static bool ProcessingTurn => TurnPlayer is Player;

    //战斗用
    public static Card AttackingUnit; //攻击单位
    public static Card DefendingUnit; //防御单位
    public static List<Card> BattlingUnits => new List<Card> { AttackingUnit, DefendingUnit };  //战斗单位
    public static bool CriticalFlag; //是否使用了必杀攻击
    public static bool AvoidFlag; //是否使用了神速回避

    //自动处理检查时点相关
    public static List<Card> CCBonusList; //存放触发了CC Bonus的卡
    public static List<List<Induction>> InductionSetList;//存放处于诱发状态的能力组

    //测试用
    public static void SetTestMode()
    {
        LosingProcessDisabled = true;
        DeckReplenishProcessDisabled = true;
        InduceInRivalTurn = true;
        Game.TurnPlayer = Game.Player;
    }
    public static bool LosingProcessDisabled;
    public static bool DeckReplenishProcessDisabled;
    public static bool InduceInRivalTurn;

    public static List<Card> AllCards => ListUtils.Combine(Player.AllCards, Rival.AllCards);

    public static List<Area> AllAreas => ListUtils.Combine(Player.AllAreas, Rival.AllAreas);

    public static List<User> AllUsers => new List<User>() { Player, Rival };

    public static void ForEachCard(Action<Card> action)
    {
        Player.ForEachCard(action);
        Rival.ForEachCard(action);
    }

    public static bool TrueForAllCard(Predicate<Card> predicate)
    {
        return Player.TrueForAllCard(predicate) && Rival.TrueForAllCard(predicate);
    }

    public static Card GetCardByGuid(string guid)
    {
        return AllCards.Find(card => card.Guid == guid);
    }

    public static Area GetAreaByGuid(string guid)
    {
        return AllAreas.Find(area => area.Guid == guid);
    }

    public static User GetUserByGuid(string guid)
    {
        return AllUsers.Find(user => user.Guid == guid);
    }

    public static IAttachable GetItemByGuid(string guid)
    {
        foreach (var card in AllCards)
        {
            var itemFound = card.AttachableList.Find(item => item.Guid == guid);
            if (itemFound != null)
            {
                return itemFound;
            }
        }
        return null;
    }

    public static object GetObject(string guid)
    {
        object result = GetCardByGuid(guid);
        if (result == null)
        {
            result = GetAreaByGuid(guid);
        }
        if (result == null)
        {
            result = GetUserByGuid(guid);
        }
        if (result == null)
        {
            result = GetItemByGuid(guid);
        }
        return result;
    }


    /// <summary>
    /// 广播消息
    /// </summary>
    /// <param name="message">消息</param>
    static void Broadcast(Message message)
    {
        LogUtils.Log("Broadcasting message: " + Environment.NewLine
            + SerializationUtils.SerializeAny(message) + Environment.NewLine);
        if (message.SendBySelf)
        {
            Task.Run(() => MessageManager?.Send(message).Wait());
        }
        ForEachCard(card =>
        {
            card.Read(message);
        });
    }

    /// <summary>
    /// 广播询问是否允许某操作
    /// </summary>
    /// <param name="message">表示该操作的消息</param>
    /// <param name="substitute">拒绝该操作时表示作为代替的动作的消息</param>
    /// <returns>如允许，则返回True</returns>
    static bool BroadcastTry(Message message, ref Message substitute)
    {
        //LogUtils.Log("BroadcastTrying message: " + Environment.NewLine
        //    + StringUtils.CreateFromAny(message) + Environment.NewLine);
        foreach (var card in AllCards)
        {
            if (!card.Try(message, ref substitute))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 尝试并实现消息定义的操作
    /// </summary>
    /// <param name="message">表示该操作的消息</param>
    /// <returns>该操作被拒绝时，作为代替的动作的消息</returns>
    public static Message TryDoMessage(Message message)
    {
        if (message is MultipleMessage)
        {
            Message newMessage = new EmptyMessage();
            foreach (var singleMessage in ((MultipleMessage)message).Elements)
            {
                newMessage += TryDoMessage(singleMessage);
            }
            return newMessage;
        }
        Message substitute = new EmptyMessage();
        while (!BroadcastTry(message, ref substitute))
        {
            message = substitute;
        }
        message.Do();
        Broadcast(message);
        return message;
    }

    /// <summary>
    /// 尝试消息定义的操作
    /// </summary>
    /// <param name="message">表示该操作的消息</param>
    /// <returns>该操作被拒绝时，作为代替的动作的消息</returns>
    public static Message TryMessage(Message message)
    {
        if (message is MultipleMessage)
        {
            Message newMessage = new EmptyMessage();
            foreach (var singleMessage in ((MultipleMessage)message).Elements)
            {
                newMessage += TryMessage(singleMessage);
            }
            return newMessage;
        }
        Message substitute = new EmptyMessage();
        while (!BroadcastTry(message, ref substitute))
        {
            message = substitute;
        }
        return message;
    }

    /// <summary>
    /// 实现消息定义的操作
    /// </summary>
    /// <param name="message">表示该操作的消息</param>
    public static void DoMessage(Message message)
    {
        if (message is MultipleMessage)
        {
            foreach (var singleMessage in ((MultipleMessage)message).Elements)
            {
                DoMessage(singleMessage);
            }
            return;
        }
        message.Do();
        Broadcast(message);
    }

    public static void SendUserInformation()
    {
        DoMessage(new UserInformationMessage()
        {
            UserGuid = Player.Guid,
            AreaGuids = Player.AllAreas.ConvertAll(area => area.Guid)
        });
    }

    public static async Task PrepareDeck()
    {
        //Called by UI (Check Game.Rival.Synchronized first)
        string deckFileName;
        bool usePresetHero;
        do
        {
            deckFileName = ""; //TO DO: obtained from UI
            usePresetHero = true; //TO DO: obtained from UI
        }
        while (!await Player.SetDeck(deckFileName, usePresetHero));
    }

    public static async Task PrepareGame()
    {
        //Called by UI of HostPlayer(Check Game.Player.DeckLoaded and Game.Rival.DeckLoaded first)
        Player.SetHeroUnit();
        Rival.SetHeroUnit();
        Player.ShuffleDeck(null);
        Rival.ShuffleDeck(null);
        await DecidePlayingOrder();
        Player.SetFirstHand();
        Rival.SetFirstHand();
        var changeHandAnswers = await Task.WhenAll(Request.AskIfChangeFirstHand(Player), Request.AskIfChangeFirstHand(Rival));
        if (changeHandAnswers[0])
        {
            Player.PutBackFirstHand();
            Player.ShuffleDeck(null);
            Player.SetFirstHand();
        }
        if (changeHandAnswers[1])
        {
            Rival.PutBackFirstHand();
            Rival.ShuffleDeck(null);
            Rival.SetFirstHand();
        }
        Player.SetFirstOrbs();
        Rival.SetFirstOrbs();
        DoMessage(new GameStartMessage());
    }

    public static async Task DecidePlayingOrder()
    {
        bool decided = false;
        do
        {
            var choices = await Task.WhenAll(Request.ChooseRPS(Player), Request.ChooseRPS(Rival));
            User winner = null;
            if (choices[0] != choices[1])
            {
                decided = true;
                switch (choices[0] - choices[1])
                {
                    // 石头：0，剪刀：1，布：2
                    case 1:
                        winner = Rival;
                        break;
                    case 2:
                        winner = Player;
                        break;
                    case -1:
                        winner = Player;
                        break;
                    case -2:
                        winner = Rival;
                        break;
                }
            }
            DoMessage(new ConfirmRPSMessage()
            {
                ResultDict = new Dictionary<User, int>()
                {
                     { Player, choices[0] },
                     { Rival, choices[1] }
                },
                Winner = winner
            });
        } while (!decided);
    }

    public static void Start()
    {
        //Called by Message when everything prepared
        if (PlayFirstTurn)
        {
            TurnPlayer = Player;
            StartTurn();
        }
        else
        {
            TurnPlayer = Rival;
            //Release
        }
    }

    public static void StartTurn()
    {
        //Called by Message when opponent ends turn
        Task.Run(DoBeginningPhase);
    }

    public static async Task DoBeginningPhase()
    {
        Player.StartTurn();
        await DoAutoCheckTiming();
        Player.RefreshUnit(Player.Field.Filter(card => card.IsHorizontal), null);
        await DoAutoCheckTiming();
        if (TurnCount > 1)
        {
            Player.DrawCard(1);
            await DoAutoCheckTiming();
        }
        await DoBondPhase();
    }

    static async Task DoBondPhase()
    {
        Player.GoToBondPhase();
        await DoAutoCheckTiming();
        await Player.ChooseSetToBond(Player.Hand.Cards, 0, 1);
        await DoAutoCheckTiming();
        await StartDeploymentPhase();
    }

    static async Task StartDeploymentPhase()
    {
        Player.GoToDeploymentPhase();
        await DoAutoCheckTiming();
        //Release
    }

    public static async Task EndDeploymentPhase()
    {
        //Called by UI
        await DoAutoCheckTiming();
        await StartActionPhase();
    }

    static async Task StartActionPhase()
    {
        TurnPlayer.GoToActionPhase();
        await DoAutoCheckTiming();
        //Release
    }

    public static async Task EndActionPhase()
    {
        //Called by UI
        await DoAutoCheckTiming();
        await DoEndPhase();
    }

    static async Task DoEndPhase()
    {
        Player.EndTurn();
        await DoAutoCheckTiming();
        Player.ClearStatusEndingTurn();
        Player.SwitchTurn();
        //Release
    }

    public static async Task DoDeployment(Card target, bool toFrontField)
    {
        await DoAutoCheckTiming();
        Player.Deploy(target, toFrontField);
        await DoAutoCheckTiming();
    }

    public static async Task DoLevelUp(Card target)
    {
        await DoAutoCheckTiming();
        await Player.LevelUp(target);
        await DoAutoCheckTiming();
    }

    public static async Task DoMovement(Card target)
    {
        await DoAutoCheckTiming();
        Player.Move(target, null);
        await DoAutoCheckTiming();
    }

    public static async Task DoActionSkill(ActionSkill target)
    {
        await DoAutoCheckTiming();
        await Player.UseActionSkill(target);
        await DoAutoCheckTiming();
    }

    public static async Task DoBattle(Card attackingUnit, Card target)
    {
        //攻撃指定ステップ
        await DoAutoCheckTiming();
        TurnPlayer.Attack(attackingUnit, target);
        await DoAutoCheckTiming();
        if (DefendingUnit == null)
        {
            return;
        }
        //支援ステップ
        await DoAutoCheckTiming();
        TurnPlayer.SetSupportCard();
        NotTurnPlayer.SetSupportCard();
        await DoAutoCheckTiming();
        TurnPlayer.ConfirmSupportCard();
        NotTurnPlayer.ConfirmSupportCard();
        await DoAutoCheckTiming();
        await TurnPlayer.SolveSupportSkills();
        await NotTurnPlayer.SolveSupportSkills();
        await DoAutoCheckTiming();
        TurnPlayer.AddSupportToPower(AttackingUnit);
        NotTurnPlayer.AddSupportToPower(DefendingUnit);
        await DoAutoCheckTiming();
        //必殺攻撃・神速回避ステップ
        if (await TurnPlayer.CriticalAttack())
        {
            TurnPlayer.AttachItem(new PowerBuff(null, AttackingUnit.Power, LastingTypeEnum.UntilBattleEnds), AttackingUnit);
        }
        await DoAutoCheckTiming();
        await NotTurnPlayer.Avoid();
        await DoAutoCheckTiming();
        //判定ステップ
        await DoAutoCheckTiming();
        if (!AvoidFlag && AttackingUnit.Power >= DefendingUnit.Power)
        {
            TryDoMessage(new DestroyMessage()
            {
                DestroyedUnits = new List<Card>() { DefendingUnit },
                ReasonTag = DestructionReasonTag.ByBattle,
                AttackingUnit = AttackingUnit,
                Reason = null,
                Count = 1
            });
        }
        else
        {
            DoMessage(new AttackFailureMessage()
            {
                AttackingUnit = AttackingUnit,
                DefendingUnit = DefendingUnit
            });
        }
        await DoAutoCheckTiming();
        TurnPlayer.RemoveSupportCard();
        NotTurnPlayer.RemoveSupportCard();
        await DoAutoCheckTiming();
        //終了ステップ
        await DoAutoCheckTiming();
        TurnPlayer.EndBattle();
        //TO DO: 一张支援卡有复数个对应“战斗结束时”的能力时自选顺序
        await DoAutoCheckTiming();
        TurnPlayer.ClearStatusEndingBattle();
    }

    //自動処理チェックタイミング
    public static async Task DoAutoCheckTiming()
    {
        while (true)
        {
            DoCCBonusProcess();
            while (await DoRuleProcess()) { }
            if (!await DoInducedSkillProcess())
            {
                break;
            }
        }
    }

    //クラスチェンジボーナス処理
    static void DoCCBonusProcess()
    {
        int playerCount = 0;
        int rivalCount = 0;
        foreach (var card in CCBonusList)
        {
            if (card.Controller == Player)
            {
                playerCount++;
            }
            else
            {
                rivalCount++;
            }
        }
        if (playerCount > 0)
        {
            Player.DrawCard(playerCount);
        }
        if (rivalCount > 0)
        {
            Rival.DrawCard(rivalCount);
        }
        CCBonusList.Clear();
    }

    //ルール処理
    static async Task<bool> DoRuleProcess()
    {
        if (await DoSameNameProcess())
        {
            return true;
        }
        if (await DoDestructionProcess())
        {
            return true;
        }
        DoLosingProcess();
        if (DoPositionProcess())
        {
            return true;
        }
        if (DoMarchingProcess())
        {
            return true;
        }
        return false;
    }

    //同名処理
    static async Task<bool> DoSameNameProcess()
    {
        var nameChecked = new List<string>();
        var cardsToSendToRetreat = new List<Card>();
        foreach (var user in AllUsers)
        {
            foreach (var card in user.Field.Cards)
            {
                foreach (var name in card.AllUnitNames)
                {
                    if (nameChecked.Contains(name))
                    {
                        continue;
                    }
                    nameChecked.Add(name);
                    if (user.Field.SearchCard(name).Count > 1)
                    {
                        cardsToSendToRetreat.AddRange(await user.ChooseDiscardedCardsSameNameProcess(user.Field.SearchCard(name), name));
                    }
                }
            }
        }
        if (cardsToSendToRetreat.Count > 0)
        {
            var sameNameProcessMessage = new SendToRetreatSameNameProcessMessage()
            {
                Targets = cardsToSendToRetreat,
                Reason = null,
                AsCost = false
            };
            TryDoMessage(sameNameProcessMessage);
            return true;
        }
        return false;
    }

    //撃破処理
    static async Task<bool> DoDestructionProcess()
    {
        bool processed = false;
        var cardsToSendToRetreat = new List<Card>();
        var orbsDetructionCountDict = new Dictionary<User, int>();
        foreach (var card in AllCards)
        {
            if (card.DestroyedCount > 0)
            {
                if (card.IsHero)
                {
                    if (orbsDetructionCountDict.ContainsKey(card.Controller))
                    {
                        orbsDetructionCountDict[card.Controller] += card.DestroyedCount;
                    }
                    else
                    {
                        orbsDetructionCountDict.Add(card.Controller, card.DestroyedCount);
                    }
                }
                else
                {
                    cardsToSendToRetreat.Add(card);
                }
            }
        }
        var readyForDestructionProcessMessage = new ReadyForDestructionProcessMessage()
        {
            CardsToSendToRetreat = cardsToSendToRetreat,
            OrbsDetructionCountDict = orbsDetructionCountDict
        };
        readyForDestructionProcessMessage = TryMessage(readyForDestructionProcessMessage) as ReadyForDestructionProcessMessage;
        if (readyForDestructionProcessMessage != null)
        {
            cardsToSendToRetreat = readyForDestructionProcessMessage.CardsToSendToRetreat;
            orbsDetructionCountDict = readyForDestructionProcessMessage.OrbsDetructionCountDict;
            foreach (var pair in orbsDetructionCountDict)
            {
                var user = pair.Key;
                var count = pair.Value;
                if (user.Orb.Count == 0 && count > 0)
                {
                    cardsToSendToRetreat.Add(user.Hero);
                }
                else
                {
                    count = Math.Min(count, user.Orb.Count);
                    for (int i = 0; i < count; i++)
                    {
                        TryDoMessage(new ObtainOrbDestructionProcessMessage()
                        {
                            Target = await Request.ChooseOne(user.Orb.Cards, user),
                            Reason = user.Hero
                        });
                        processed = true;
                    }
                }
            }
            if (cardsToSendToRetreat.Count > 0)
            {
                TryDoMessage(new SendToRetreatDestructionProcessMessage()
                {
                    Targets = cardsToSendToRetreat,
                    Reason = null,
                    AsCost = false
                });
                processed = true;
            }
        }
        return processed;
    }

    //敗北処理
    static void DoLosingProcess()
    {
        if (LosingProcessDisabled)
        {
            return;
        }
        var losingUsers = new List<User>();
        foreach (var user in AllUsers)
        {
            if ((user.Field.Contains(user.Hero)) || (user.Deck.Count == 0 && user.Retreat.Count == 0))
            {
                losingUsers.Add(user);
            }
        }
        if (losingUsers.Count > 0)
        {
            Over(losingUsers);
        }
    }

    //配置処理
    static bool DoPositionProcess()
    {
        var cardsToSendToRetreat = new List<Card>();
        foreach (var card in AllCards)
        {
            foreach (var item in card.SubSkillList)
            {
                if (item is IForbidPosition)
                {
                    foreach (var areaType in ((IForbidPosition)item).ForbiddenAreaTypes)
                    {
                        if (card.BelongedRegion.GetType() == areaType)
                        {
                            if (!cardsToSendToRetreat.Contains(card))
                            {
                                cardsToSendToRetreat.Add(card);
                            }
                        }
                    }
                }
            }
        }
        if (cardsToSendToRetreat.Count > 0)
        {
            TryDoMessage(new SendToRetreatPositionProcessMessage()
            {
                Targets = cardsToSendToRetreat,
                Reason = null,
                AsCost = false
            });
            return true;
        }
        else
        {
            return false;
        }
    }

    //進軍処理
    static bool DoMarchingProcess()
    {
        if (NotTurnPlayer.FrontField.Count == 0 && NotTurnPlayer.BackField.Count > 0)
        {
            TryDoMessage(new MoveMarchingProcessMessage()
            {
                Targets = NotTurnPlayer.BackField.Cards
            });
            return true;
        }
        else
        {
            return false;
        }
    }

    //自動型スキル誘発処理
    static async Task<bool> DoInducedSkillProcess()
    {
        if (InductionSetList.Count == 0)
        {
            return false;
        }
        int index = InductionSetList.Count - 1;
        var inductionSet = InductionSetList[index];
        InductionSetList[index] = new List<Induction>();
        if (inductionSet.Count > 0)
        {
            var myInductionList = new List<Induction>();
            var hisInductionList = new List<Induction>();
            foreach (var induction in inductionSet)
            {
                if (induction.Skill.Controller == Player)
                {
                    myInductionList.Add(induction);
                }
                else
                {
                    hisInductionList.Add(induction);
                }
            }
            Induction inductionSelected;
            if (myInductionList.Count > 0)
            {
                inductionSelected = await Request.ChooseOne(myInductionList, Player);
            }
            else if (hisInductionList.Count > 0)
            {
                inductionSelected = await Request.ChooseOne(hisInductionList, Rival);
            }
            else
            {
                return false;
            }
            inductionSet.Remove(inductionSelected);
            bool solved = await inductionSelected.Skill.Solve(inductionSelected);
            if (inductionSet.Count > 0)
            {
                InductionSetList.Insert(index, inductionSet);
            }
            InductionSetList.RemoveAll(set => set.Count == 0);
            return solved;
        }
        else
        {
            return false;
        }
        ///
        /// TO DO:
        /// 自動処理チェックタイミングのクラスチェンジボーナス処理において
        /// 引いたカードに、味方がクラスチェンジした時に誘発する特殊型スキル
        /// が書いてある場合、その特殊型スキルはすでに誘発状態であったものと
        /// して扱います。
        /// 
        /// 自動型スキル誘発処理を含む一連の自動処理チェックタイミングを実行して
        /// いる最中に、いずれかの領域に配置されていたカードが手札に配置され、その
        /// カードの特殊型スキルが、この自動処理チェックタイミングにおいて、すでに
        /// 選択されたスキルや、まだ選択されていない誘発状態であるスキルと同じ事象
        /// によって誘発状態になる場合、その特殊型スキルを「１０.２.」においてまだ
        /// 選択することができるタイミングがあるなら、その特殊型スキルはすでに誘発
        /// 状態であったものとして扱い、次回以降の「１０.２.」において、選択する
        /// ことができます。
        ///
    }

    public static void Over(List<User> losingUsers, Skill reason = null)
    {
        TryDoMessage(new GameOverMessage()
        {
            LosingUsers = losingUsers,
            Reason = reason
        });
    }
}

public enum Phase
{
    BeginningPhase,
    BondPhase,
    DeploymentPhase,
    ActionPhase,
    EndPhase
}
