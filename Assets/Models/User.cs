using System;
using System.Collections.Generic;

/// <summary>
/// 玩家类
/// </summary>
public abstract class User
{
    public User(Game game)
    {
        Deck Deck = new Deck(this);
        Hand Hand = new Hand(this);
        Retreat Retreat = new Retreat(this);
        Support Support = new Support(this);
        Bond Bond = new Bond(this);
        Orb Orb = new Orb(this);
        Field Field = new Field(this);
        FrontField FrontField = new FrontField(this);
        BackField BackField = new BackField(this);
        Overlay Overlay = new Overlay(this);
        AllAreas = new List<Area> { Deck, Hand, Retreat, Support, Bond, Orb, FrontField, BackField, Overlay };
        Game = game;
    }
    public Game Game;
    public List<Area> AllAreas;
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
    public User Opponent;
    public Card Hero = null;

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
    /// 搜索卡片
    /// </summary>
    /// <param name="id">卡的id</param>
    /// <returns>符合条件的卡</returns>
    private Card SearchCard(int id)
    {
        foreach (Area area in AllAreas)
        {
            Card result = area.SearchCard(id);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

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
        List<Card> targets = new List<Card> { target };
        Move(targets, reason);
    }

    public void Move(List<Card> targets, Skill reason)
    {
        Message moveMessage = new MoveMessage()
        {
            Targets = targets,
            Reason = reason
        };
        TryDoMessage(moveMessage);
    }
    #endregion
}

/// <summary>
/// 自己
/// </summary>
public class Player : User
{
    public Player(Game game) : base(game) { }

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
            if(!card.Try(message, ref substitute))
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