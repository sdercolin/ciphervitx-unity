using System;
using System.Collections.Generic;

/// <summary>
/// 玩家类
/// </summary>
public abstract class User
{
    public User()
    {
        Deck = new Deck(this);
        Hand = new Hand(this);
        Retreat = new Retreat(this);
        Support = new Support(this);
        Bond = new Bond(this);
        Orb = new Orb(this);
        Field = new Field(this);
        FrontField = new FrontField(this);
        BackField = new BackField(this);
        Overlay = new Overlay(this);
        Guid = System.Guid.NewGuid().ToString();
    }

    public string Guid;
    public override string ToString()
    {
        return "{\"guid\": \"" + Guid + "\" }";
    }

    public List<Area> AllAreas
    {
        get
        {
            return new List<Area> { Deck, Hand, Retreat, Support, Bond, Orb, FrontField, BackField, Overlay };
        }
    }
    public Deck Deck;
    public Hand Hand;
    public Retreat Retreat;
    public Support Support;
    public Bond Bond;
    public Orb Orb;
    public Field Field;
    public FrontField FrontField;
    public BackField BackField;
    public Overlay Overlay;
    public abstract User Opponent { get; }
    public Card Hero
    {
        get
        {
            foreach (Card card in AllCards)
            {
                if (card.IsHero == true)
                {
                    return card;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 待处理的转职奖励计数
    /// </summary>
    public int CCBonusInducedCount = 0;

    /// <summary>
    /// 败北标志
    /// </summary>
    public bool LoseFlag = false;

    /// <summary>
    /// 本回合中已经出击（升级）过的费用数
    /// </summary>
    public int DeployAndCCCostCount = 0;

    /// <summary>
    /// 行动阶段结束标志
    /// </summary>
    public bool ActionPhaseEnded = false;

    /// <summary>
    /// 附加能力列表
    /// </summary>
    public List<SubSkill> SubSkillList = new List<SubSkill>();

    public List<Card> AllCards
    {
        get
        {
            List<Card> allCards = new List<Card>();
            ForEachCard(card => allCards.Add(card));
            return allCards;
        }
    }

    public void ForEachCard(Action<Card> action)
    {
        AllAreas.ForEach(area => area.ForEachCard(action));
    }

    public bool TrueForAllCard(Predicate<Card> predicate)
    {
        return AllAreas.TrueForAll(area => area.TrueForAllCard(predicate));
    }

    public abstract void Broadcast(Message message);
    public abstract bool BroadcastTry(Message message, ref Message substitute);


    #region 动作
    /// <summary>
    /// 尝试并实行动作
    /// </summary>
    /// <param name="message"></param>
    protected Message TryDoMessage(Message message)
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

    public void Move(Card target, Skill reason)
    {
        if (target != null)
        {
            List<Card> targets = new List<Card> { target };
            Move(targets, reason);
        }
    }

    public void Move(List<Card> targets, Skill reason)
    {
        MoveMessage moveMessage = new MoveMessage()
        {
            Targets = targets,
            Reason = reason
        };
        TryDoMessage(moveMessage);
    }

    public void UseBond(Card target, Skill reason)
    {
        if (target != null)
        {
            List<Card> targets = new List<Card> { target };
            UseBond(targets, reason);
        }
    }

    public void UseBond(List<Card> targets, Skill reason)
    {
        if (targets.Count > 0)
        {
            UseBondMessage useBondMessage = new UseBondMessage()
            {
                Targets = targets,
                Reason = reason
            };
            TryDoMessage(useBondMessage);
        }
    }

    public void StartTurn()
    {
        StartTurnMessage startTurnMessage = new StartTurnMessage()
        {
            TurnPlayer = this
        };
        TryDoMessage(startTurnMessage);
    }

    public void GoToBondPhase()
    {

    }

    public void SetToBond(Card target, bool frontShown, Skill reason = null)
    {
        if (target != null)
        {
            List<Card> targets = new List<Card> { target };
            SetToBond(targets, frontShown, reason);
        }
    }

    public void SetToBond(List<Card> targets, bool frontShown, Skill reason = null)
    {
        if (targets.Count > 0)
        {
            ToBondMessage toBondMessage = new ToBondMessage
            {
                Targets = targets,
                TargetFrontShown = frontShown,
                Reason = reason
            };
            TryDoMessage(toBondMessage);
        }
    }

    public void ChooseSetToBond(List<Card> targets, bool frontShown, int min, int max, Skill reason = null)
    {
        if (targets.Count > 0)
        {
            ReadyToBondMessage readyToBondMessage = new ReadyToBondMessage
            {
                Targets = targets,
                TargetFrontShown = frontShown
            };
            readyToBondMessage = TryDoMessage(readyToBondMessage) as ReadyToBondMessage;
            if (readyToBondMessage != null && readyToBondMessage.Targets.Count > 0)
            {
                SetToBond(Request.Choose(readyToBondMessage.Targets, min, max, this), readyToBondMessage.TargetFrontShown, readyToBondMessage.Reason);
            }
        }
    }

    public void RefreshUnit(Card target, Skill reason)
    {
        if (target != null)
        {
            RefreshUnit(new List<Card>() { target }, reason);
        }
    }

    public void RefreshUnit(List<Card> targets, Skill reason)
    {
        if (targets.Count > 0)
        {
            RefreshUnitMessage refreshUnitMessage = new RefreshUnitMessage()
            {
                Targets = targets,
                Reason = reason
            };
            TryDoMessage(refreshUnitMessage);
        }
    }

    public void DrawCard(int number, Skill reason = null)
    {
        DrawCardMessage drawCardMessage = new DrawCardMessage()
        {
            Player = this,
            Number = number,
            Reason = reason
        };
        TryDoMessage(drawCardMessage);
    }

    public bool DoSameNameProcess(List<Card> units, string name)
    {
        ReadyForSameNameProcessMessage readyForSameNameProcessMessage = new ReadyForSameNameProcessMessage()
        {
            Targets = units,
            Name = name
        };
        readyForSameNameProcessMessage = TryDoMessage(readyForSameNameProcessMessage) as ReadyForSameNameProcessMessage;
        if (readyForSameNameProcessMessage != null && readyForSameNameProcessMessage.Targets.Count > 1)
        {
            Card savedUnit;
            if (readyForSameNameProcessMessage.Targets.Contains(Hero))
            {
                savedUnit = Hero;
            }
            else
            {
                savedUnit = Request.ChooseOne(readyForSameNameProcessMessage.Targets, this);
            }
            List<Card> confirmedTarget = ListUtils.Clone(readyForSameNameProcessMessage.Targets);
            confirmedTarget.Remove(savedUnit);
            SameNameProcessMessage sameNameProcessMessage = new SameNameProcessMessage()
            {
                Targets = confirmedTarget,
                Name = name
            };
            TryDoMessage(sameNameProcessMessage);
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}

/// <summary>
/// 自己
/// </summary>
public class Player : User
{
    public Player() : base() { }

    public override User Opponent
    {
        get
        {
            return Game.Rival;
        }
    }

    /// <summary>
    /// 直接将消息发给对手（不广播给自己的卡）
    /// </summary>
    /// <param name="message">消息</param>
    //public void Tell(Message message) { throw new NotImplementedException(); }

    /// <summary>
    /// 直接将消息发给对手并等待特定类型的回复（不广播给自己的卡）
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="responsetype">要求对方回复的消息种类</param>
    /// <returns>对方的回复</returns>
    //internal Message Ask(Message message, MessageType responsetype) { throw new NotImplementedException(); }

    /// <summary>
    /// 广播消息
    /// </summary>
    /// <param name="message">消息</param>
    public override void Broadcast(Message message)
    {
        //发送消息给对方
        Game.ForEachCard(card =>
        {
            card.Read(message);
        });
    }

    /// <summary>
    /// 广播询问是否允许某操作
    /// </summary>
    /// <param name="message">表示该操作的消息</param>
    /// <param name="substitute">拒绝该操作时表示作为代替的动作的的消息</param>
    /// <returns>如允许，则返回True</returns>
    public override bool BroadcastTry(Message message, ref Message substitute)
    {
        foreach (Card card in Game.AllCards)
        {
            if (!card.Try(message, ref substitute))
            {
                return false;
            }
        }
        return true;
    }
}

/// <summary>
/// 对手
/// </summary>
public class Rival : User
{
    public Rival() : base() { }

    public override User Opponent
    {
        get
        {
            return Game.Player;
        }
    }

    /// <summary>
    /// 复现对手动作时，不再重复广播
    /// </summary>
    public override void Broadcast(Message message) { }

    /// <summary>
    /// 复现对手动作时不需要Try
    /// </summary>
    public override bool BroadcastTry(Message message, ref Message substitute)
    {
        return true;
    }

    /// <summary>
    /// 接受消息并重复其动作
    /// </summary>
    /// <param name="response"></param>
    //internal void DoAsMessage(Message response) { }
}