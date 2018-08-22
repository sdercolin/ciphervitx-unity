using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class Game
{
    public static void Initialize()
    {
        Player = new Player();
        Rival = new Rival();
    }

    public static Player Player;
    public static Rival Rival;

    //回合信息
    public static User TurnPlayer;
    public static User NotTurnPlayer => TurnPlayer.Opponent;
    public static int TurnCount = 0;
    public static Phase CurrentPhase;

    //战斗用
    public static Card AttackingUnit = null; //攻击单位
    public static Card DefendingUnit = null; //防御单位
    public static List<Card> BattlingUnits => new List<Card> { AttackingUnit, DefendingUnit };  //战斗单位
    public static bool CriticalFlag = false; //是否使用了必杀攻击
    public static bool AvoidFlag = false; //是否使用了神速回避

    //自动处理检查时点相关
    public static List<Card> CCBonusList = new List<Card>(); //存放触发了CC Bonus的卡
    public static List<List<AutoSkill>> InducedSkillSetList = new List<List<AutoSkill>>(); //存放处于诱发状态的能力组

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
        foreach (Card card in AllCards)
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
    public static void Broadcast(Message message)
    {
        //TO DO:发送消息给对方
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
    public static bool BroadcastTry(Message message, ref Message substitute)
    {
        foreach (Card card in AllCards)
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
        message.Do();
        Broadcast(message);
    }

    public static void Start(bool ifFirstPlay)
    {
        //Called by UI
        if (ifFirstPlay)
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
        DoBeginningPhase();
    }

    private async static void DoBeginningPhase()
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
        DoBondPhase();
    }

    private async static void DoBondPhase()
    {
        Player.GoToBondPhase();
        await DoAutoCheckTiming();
        await Player.ChooseSetToBond(Player.Hand.Cards, 0, 1);
        await DoAutoCheckTiming();
        StartDeploymentPhase();
    }

    private async static void StartDeploymentPhase()
    {
        Player.GoToDeploymentPhase();
        await DoAutoCheckTiming();
        //Release
    }

    public async static void EndDeploymentPhase()
    {
        //Called by UI
        await DoAutoCheckTiming();
        StartActionPhase();
    }

    private async static void StartActionPhase()
    {
        TurnPlayer.GoToActionPhase();
        await DoAutoCheckTiming();
        //Release
    }

    public async static void EndActionPhase()
    {
        //Called by UI
        await DoAutoCheckTiming();
        DoEndPhase();
    }

    private async static void DoEndPhase()
    {
        Player.EndTurn();
        await DoAutoCheckTiming();
        Player.ClearStatusEndingTurn();
        Player.SwitchTurn();
        //Release
    }

    public async static Task DoBattle(Card attackingUnit, Card target)
    {
        //攻撃指定ステップ
        await DoAutoCheckTiming();
        TurnPlayer.Attack(attackingUnit, target);
        if (DefendingUnit == null)
        {
            return;
        }
        await DoAutoCheckTiming();
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
        await TurnPlayer.CriticalAttack();
    }

    //自動処理チェックタイミング
    private async static Task DoAutoCheckTiming()
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
    private static void DoCCBonusProcess()
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
    private async static Task<bool> DoRuleProcess()
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
    private async static Task<bool> DoSameNameProcess()
    {
        List<string> nameChecked = new List<string>();
        List<Card> cardsToSendToRetreat = new List<Card>();
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
                Targets = cardsToSendToRetreat
            };
            TryDoMessage(sameNameProcessMessage);
            return true;
        }
        return false;
    }

    //撃破処理
    private async static Task<bool> DoDestructionProcess()
    {
        bool processed = false;
        List<Card> cardsToSendToRetreat = new List<Card>();
        Dictionary<User, int> orbsDetructionCountDict = new Dictionary<User, int>();
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
                            Target = await Request.ChooseOne(user.Orb.Cards, user)
                        });
                        processed = true;
                    }
                }
            }
            if (cardsToSendToRetreat.Count > 0)
            {
                TryDoMessage(new SendToRetreatDestructionProcessMessage()
                {
                    Targets = cardsToSendToRetreat
                });
                processed = true;
            }
        }
        return processed;
    }

    //敗北処理
    private static void DoLosingProcess()
    {
        List<User> losingUsers = new List<User>();
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
    private static bool DoPositionProcess()
    {
        List<Card> cardsToSendToRetreat = new List<Card>();
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
                Targets = cardsToSendToRetreat
            });
            return true;
        }
        else
        {
            return false;
        }
    }

    //進軍処理
    private static bool DoMarchingProcess()
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
    private async static Task<bool> DoInducedSkillProcess()
    {
        if (InducedSkillSetList.Count == 0)
        {
            return false;
        }
        int index = InducedSkillSetList.Count - 1;
        var inducedSkillList = InducedSkillSetList[index];
        InducedSkillSetList[index] = new List<AutoSkill>();
        if (inducedSkillList.Count > 0)
        {
            var myInducedSkillList = new List<AutoSkill>();
            var hisInducedSkillList = new List<AutoSkill>();
            foreach (var skill in inducedSkillList)
            {
                if (skill.Controller == Player)
                {
                    myInducedSkillList.Add(skill);
                }
                else
                {
                    hisInducedSkillList.Add(skill);
                }
            }
            AutoSkill skillSelected;
            if (myInducedSkillList.Count > 0)
            {
                skillSelected = await Request.ChooseOne(myInducedSkillList, Player);
            }
            else if (hisInducedSkillList.Count > 0)
            {
                skillSelected = await Request.ChooseOne(hisInducedSkillList, Rival);
            }
            else
            {
                return false;
            }
            inducedSkillList.Remove(skillSelected);
            bool solved = await skillSelected.Solve();
            if (inducedSkillList.Count > 0)
            {
                InducedSkillSetList.Insert(index, inducedSkillList);
            }
            InducedSkillSetList.RemoveAll(set => set.Count == 0);
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

    private static void Over(List<User> losingUsers)
    {
        //TO DO: Game Over
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
