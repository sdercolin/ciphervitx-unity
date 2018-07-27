using System;
using System.Collections.Generic;

/// <summary>
/// 玩家类
/// </summary>
public abstract class User
{
    public User(Game game)
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
        Game = game;
        Guid = System.Guid.NewGuid().ToString();
    }

    public string Guid;
    public override string ToString()
    {
        return "{\"Guid\": \"" + Guid + "\" }";
    }

    public Game Game;
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
    protected void TryDoMessage(Message message)
    {
        Message substitute = new EmptyMessage();
        while (!BroadcastTry(message, ref substitute))
        {
            message = substitute;
        }
        message.Do();
        Broadcast(message);
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
        UseBondMessage useBondMessage = new UseBondMessage()
        {
            Targets = targets,
            Reason = reason
        };
        TryDoMessage(useBondMessage);
    }

    public void SetToBond(Card target, bool frontShown, Skill reason)
    {
        if (target != null)
        {
            List<Card> targets = new List<Card> { target };
            List<bool> frontShownTable = new List<bool> { frontShown };
            SetToBond(targets, frontShownTable, reason);
        }
    }

    public void SetToBond(List<Card> targets, List<bool> frontShownTable, Skill reason)
    {
        int count = targets.Count;
        ToBondMessage toBondMessage = new ToBondMessage
        {
            Targets = targets,
            TargetsFrontShown = frontShownTable
        };
        TryDoMessage(toBondMessage);
    }
    #endregion
}

/// <summary>
/// 自己
/// </summary>
public class Player : User
{
    public Player(Game game) : base(game) { }

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
    public Rival(Game game) : base(game) { }

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