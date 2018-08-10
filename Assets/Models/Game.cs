﻿using System;
using System.Collections.Generic;

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
    public static int TurnCount = 0;
    public static Phase CurrentPhase;

    //战斗用
    public static Card AttackingUnit = null; //攻击单位
    public static Card DefencingUnit = null; //防御单位
    public static List<Card> BattlingUnits { get { return new List<Card> { AttackingUnit, DefencingUnit }; } } //战斗单位
    public static int PowerUpByCritical = 0; //必杀攻击增加的战斗力
    public static int PowerUpBySupport = 0; //支援增加的战斗力

    //自动处理检查时点相关
    public static List<Card> CCBonusList = new List<Card>(); //存放触发了CC Bonus的卡
    public static List<List<AutoSkill>> InducedSkillSetList = new List<List<AutoSkill>>(); //存放处于诱发状态的能力组



    public static List<Card> AllCards
    {
        get
        {
            List<Card> allCards = new List<Card>();
            ForEachCard(card => allCards.Add(card));
            return allCards;
        }
    }

    public static List<Area> AllAreas
    {
        get
        {
            List<Area> allAreas = new List<Area>();
            allAreas.AddRange(Player.AllAreas);
            allAreas.AddRange(Rival.AllAreas);
            return allAreas;
        }
    }

    public static List<User> AllUsers
    {
        get
        {
            return new List<User>() { Player, Rival };
        }
    }

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
        foreach (Card card in AllCards)
        {
            if (card.Guid == guid)
            {
                return card;
            }
        }
        return null;
    }

    public static Area GetAreaByGuid(string guid)
    {
        foreach (Area area in AllAreas)
        {
            if (area.Guid == guid)
            {
                return area;
            }
        }
        return null;
    }

    public static User GetUserByGuid(string guid)
    {
        if (Player.Guid == guid)
        {
            return Player;
        }
        else if (Rival.Guid == guid)
        {
            return Rival;
        }
        else
        {
            return null;
        }
    }

    public static IAttachable GetItemByGuid(string guid)
    {
        foreach (Card card in AllCards)
        {
            foreach (var item in card.AttachableList)
            {
                if (item.Guid == guid)
                {
                    return item;
                }
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
    /// 尝试并实现消息定义的操作
    /// </summary>
    /// <param name="message"></param>
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
    /// 实现消息定义的操作
    /// </summary>
    /// <param name="message"></param>
    public static void DoMessage(Message message)
    {
        message.Do();
        Broadcast(message);
    }

    /// <summary>
    /// 广播询问是否允许某操作
    /// </summary>
    /// <param name="message">表示该操作的消息</param>
    /// <param name="substitute">拒绝该操作时表示作为代替的动作的的消息</param>
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

    public static void Start(bool ifFirstPlay)
    {
        if (ifFirstPlay)
        {
            TurnPlayer = Player;
            DoBeginningPhase();
        }
        else
        {
            WaitTurn();
        }
    }

    public static void WaitTurn()
    {
        //Wait until rival turn ends
        DoBeginningPhase();
    }

    public static void DoBeginningPhase()
    {
        TurnPlayer.StartTurn();
        DoAutoCheckTiming();
        TurnPlayer.RefreshUnit(TurnPlayer.Field.Filter(card => card.IsHorizontal), null);
        DoAutoCheckTiming();
        if (TurnCount > 1)
        {
            TurnPlayer.DrawCard(1);
            DoAutoCheckTiming();
        }
    }

    public static void DoBondPhase()
    {
        TurnPlayer.GoToBondPhase();
        DoAutoCheckTiming();
        TurnPlayer.ChooseSetToBond(TurnPlayer.Hand.Cards, true, 0, 1);
        DoAutoCheckTiming();
    }

    //自動処理チェックタイミング
    public static void DoAutoCheckTiming()
    {
        while (true)
        {
            DoCCBonusProcess();
            while (DoRuleProcess()) { }
            if (!DoInducedSkillProcess())
            {
                break;
            }
        }
    }

    //クラスチェンジボーナス処理
    public static void DoCCBonusProcess()
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
    public static bool DoRuleProcess()
    {
        bool done = false;
        while (DoSameNameProcess())
        {
            done = true;
        }
        return done;
    }

    //同名処理
    public static bool DoSameNameProcess()
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
                        cardsToSendToRetreat.AddRange(user.ChooseDiscardedCardsSameNameProcess(user.Field.SearchCard(name), name));
                    }
                }
            }
        }
        if (cardsToSendToRetreat.Count > 0)
        {
            var sameNameProcessMessage = new SameNameProcessMessage()
            {
                Targets = cardsToSendToRetreat
            };
            DoMessage(sameNameProcessMessage);
            return true;
        }
        return false;
    }

    //撃破処理
    public static bool DestructionProcess()
    {
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
        readyForDestructionProcessMessage = TryDoMessage(readyForDestructionProcessMessage) as ReadyForDestructionProcessMessage;
        if (readyForDestructionProcessMessage != null)
        {
            cardsToSendToRetreat = readyForDestructionProcessMessage.CardsToSendToRetreat;
            orbsDetructionCountDict = readyForDestructionProcessMessage.OrbsDetructionCountDict;
            foreach (var pair in orbsDetructionCountDict)
            {
                var user = pair.Key;
                var count = Math.Min(pair.Value, user.Orb.Count);
                for (int i = 0; i < count; i++)
                {
                    DoMessage(new ObtainOrbDestructionProcessMessage()
                    {
                        Target = Request.ChooseOne(user.Orb.Cards, user)
                    });
                }
            }
        }
        return false;
    }

    //自動型スキル誘発処理
    public static bool DoInducedSkillProcess()
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
                skillSelected = Request.ChooseOne(myInducedSkillList, Player);
            }
            else if (hisInducedSkillList.Count > 0)
            {
                skillSelected = Request.ChooseOne(hisInducedSkillList, Rival);
            }
            else
            {
                return false;
            }
            inducedSkillList.Remove(skillSelected);
            bool solved = skillSelected.Solve();
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
}

public enum Phase
{
    BeginningPhase,
    BondPhase,
    DeploymentPhase,
    ActionPhase,
    EndPhase
}
