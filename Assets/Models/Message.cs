using System;
using System.Collections.Generic;
using System.Reflection;

public class Message
{
    /// <summary>
    /// 是否是自己发送的消息
    /// </summary>
    public bool SendBySelf = true;

    /// <summary>
    /// 数据
    /// </summary>
    private static readonly int fieldNumber = 10;
    protected dynamic field1 = null;
    protected dynamic field2 = null;
    protected dynamic field3 = null;
    protected dynamic field4 = null;
    protected dynamic field5 = null;
    protected dynamic field6 = null;
    protected dynamic field7 = null;
    protected dynamic field8 = null;
    protected dynamic field9 = null;
    protected dynamic field10 = null;

    public Message Clone()
    {
        var messageType = GetType();
        var clone = Activator.CreateInstance(messageType) as Message;
        clone.SendBySelf = SendBySelf;
        for (int i = 0; i < fieldNumber; i++)
        {
            dynamic field = GetType().GetField("field" + (i + 1).ToString(), BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            if (field != null)
            {
                if (field is System.Collections.IList)
                {
                    clone.GetType().GetField("field" + (i + 1).ToString(), BindingFlags.NonPublic | BindingFlags.Instance).SetValue(clone, ListUtils.Clone(field));
                }
                else
                {
                    clone.GetType().GetField("field" + (i + 1).ToString(), BindingFlags.NonPublic | BindingFlags.Instance).SetValue(clone, field);
                }
            }
        }
        return clone;
    }

    public virtual void Do() { }
    public List<T> Filter<T>(List<T> list, Predicate<T> predicate)
    {
        var results = new List<T>();
        foreach (var item in list)
        {
            if (predicate(item))
            {
                results.Add(item);
            }
        }
        return results;
    }
    public bool TrueForAny<T>(List<T> list, Predicate<T> predicate)
    {
        foreach (var item in list)
        {
            if (predicate(item))
            {
                return true;
            }
        }
        return false;
    }
    public bool TrueForAll<T>(List<T> list, Predicate<T> predicate)
    {
        foreach (var item in list)
        {
            if (!predicate(item))
            {
                return false;
            }
        }
        return true;
    }

    public override string ToString()
    {
        string json = "\"type\": \"" + GetType().Name + "\"";
        for (int i = 0; i < fieldNumber; i++)
        {
            dynamic field = GetType().GetField("field" + (i + 1).ToString(), BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            if (field != null)
            {
                json += ", \"field" + (i + 1).ToString() + "\": " + SerializationUtils.SerializeAny(field);
            }
        }
        return "{" + json + "}";
    }

    public static Message FromString(string json)
    {
        string[] splited = json.Trim(new char[] { '{', '}' }).SplitProtectingWrappers(", ", StringSplitOptions.RemoveEmptyEntries, "[]", "{}", "<>");
        string typename = null;
        foreach (var item in splited)
        {
            if (item.Contains("\"type\": \""))
            {
                typename = item.Replace("\"type\": \"", "").Trim('\"');
                break;
            }
        }
        if (typename == null)
        {
            return null;
        }
        var messageType = Assembly.GetExecutingAssembly().GetType(typename);
        try
        {
            var newMessage = Activator.CreateInstance(messageType) as Message;
            foreach (var item in splited)
            {
                if (item.Contains("\"type\": "))
                {
                    continue;
                }
                object value = SerializationUtils.Deserialize(item.SplitOnce(": ")[1]);
                typeof(Message).GetField(item.SplitOnce(": ")[0].Trim(new char[] { '\"' }), BindingFlags.NonPublic | BindingFlags.Instance).SetValue(newMessage, value);
            }
            return newMessage;
        }
        catch
        {
            return null;
        }
    }

    public static Message operator +(Message a, Message b)
    {
        if (a is EmptyMessage || a == null)
        {
            return b;
        }
        else if (b is EmptyMessage || b == null)
        {
            return a;
        }
        else
        {
            var newElements = new List<Message>();
            if (a is MultipleMessage)
            {
                if (b is MultipleMessage)
                {
                    newElements = ListUtils.Combine(((MultipleMessage)a).Elements, ((MultipleMessage)b).Elements);
                }
                else
                {
                    newElements.AddRange(((MultipleMessage)a).Elements);
                    newElements.Add(b);
                }
            }
            else if (b is MultipleMessage)
            {
                newElements.Add(a);
                newElements.AddRange(((MultipleMessage)b).Elements);
            }
            else
            {
                newElements.Add(a);
                newElements.Add(b);
            }
            return new MultipleMessage(newElements.ToArray());
        }
    }
}

public class MultipleMessage : Message
{
    public MultipleMessage(params Message[] elements) : base()
    {
        Elements.AddRange(elements);
    }

    public List<Message> Elements = new List<Message>();
}

public class EmptyMessage : Message
{
    public override void Do() { }
}

public class UserInformationMessage : Message
{
    public string UserGuid { get { return field1; } set { field1 = value; } }
    public List<string> AreaGuids { get { return field1; } set { field1 = value; } }

    public override void Do()
    {
        if (SendBySelf)
        {
            return;
        }
        Game.Rival.Guid = UserGuid;
        int index = 0;
        foreach (var area in Game.Rival.AllAreas)
        {
            area.Guid = AreaGuids[index];
            index++;
        }
        Game.Rival.Synchronized = true;
    }

}

public class SetDeckMessage : Message
{
    public User User { get { return field1; } set { field1 = value; } }
    public Dictionary<string, int> CardDict { get { return field2; } set { field2 = value; } }
    public string HeroGuid { get { return field3; } set { field3 = value; } }
    public Dictionary<string, string[]> CardSkillDict { get { return field4; } set { field4 = value; } }

    public override void Do()
    {
        foreach (var guid in CardDict.Keys)
        {
            var newCard = CardFactory.CreateCard(CardDict[guid], User);
            User.Deck.ImportCard(newCard);
            newCard.Guid = guid;
            if (guid == HeroGuid)
            {
                newCard.IsHero = true;
            }
            for (int i = 0; i < newCard.SkillList.Count; i++)
            {
                newCard.SkillList[i].Guid = CardSkillDict[guid][i];
            }
        }
        User.DeckLoaded = true;
    }
}

public class SetHeroUnitMessage : Message
{
    public User User { get { return field1; } set { field1 = value; } }

    public override void Do()
    {
        User.Hero.MoveTo(User.FrontField);
    }
}

public class ConfirmRPSMessage : Message
{
    public Dictionary<User, int> ResultDict { get { return field1; } set { field1 = value; } }
    public User Winner { get { return field2; } set { field2 = value; } }

    public override void Do()
    {
        if (Winner == Game.Player)
        {
            Game.PlayFirstTurn = true;
        }
    }
}

public class SetFirstHandMessage : Message
{
    public User User { get { return field1; } set { field1 = value; } }

    public override void Do()
    {
        for (int i = 0; i < 6; i++)
        {
            User.Deck.Top.MoveTo(User.Hand);
        }
    }
}

public class PutBackFirstHandMessage : Message
{
    public User User { get { return field1; } set { field1 = value; } }

    public override void Do()
    {
        while (User.Hand.Count != 0)
        {
            User.Hand.Cards[0].MoveTo(User.Deck);
        }
    }
}

public class SetFirstOrbsMessage : Message
{
    public User User { get { return field1; } set { field1 = value; } }

    public override void Do()
    {
        for (int i = 0; i < 5; i++)
        {
            User.Deck.Top.MoveTo(User.Orb);
        }
    }
}

public class GameStartMessage : Message
{
    public override void Do()
    {
        Game.Start();
    }
}

public class DeployMessage : Message
{
    public List<Card> Targets { get { return field1; } set { field1 = value; } }
    public List<bool> ToFrontFieldList { get { return field2; } set { field2 = value; } }
    public List<bool> ActionedList { get { return field3; } set { field3 = value; } }
    public Skill Reason { get { return field4; } set { field4 = value; } }
    public override void Do()
    {
        foreach (var card in Targets)
        {
            if (Reason == null)
            {
                card.Controller.DeployAndCCCostCount += card.DeployCost;
            }
            Area toArea;
            if (ToFrontFieldList[Targets.IndexOf(card)])
            {
                toArea = card.Controller.FrontField;
            }
            else
            {
                toArea = card.Controller.BackField;
            }
            card.MoveTo(toArea);
            card.IsHorizontal = ActionedList[Targets.IndexOf(card)];
        }
    }

    public bool RemoveTarget(Card target)
    {
        if (Targets.Contains(target))
        {
            int index = Targets.IndexOf(target);
            Targets.Remove(target);
            ToFrontFieldList.RemoveAt(index);
            ActionedList.RemoveAt(index);
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class LevelUpMessage : Message
{
    public Card Target { get { return field1; } set { field1 = value; } }
    public Card BaseUnit { get { return field2; } set { field2 = value; } }
    public Skill Reason { get { return field3; } set { field3 = value; } }

    public bool IsClassChange => Target.ClassChangeCost > 0;

    public override void Do()
    {
        if (Reason == null)
        {
            Target.Controller.DeployAndCCCostCount += IsClassChange ? Target.ClassChangeCost : Target.DeployCost;
        }
        Target.StackOver(BaseUnit);
        Target.IsLevelUpedInThisTurn = true;
        if (IsClassChange)
        {
            Game.CCBonusList.Add(Target);
            Target.IsClassChangedInThisTurn = true;
        }
    }
}

public class MoveMessage : Message
{
    public List<Card> Targets { get { return field1; } set { field1 = value; } }
    public Skill Reason { get { return field2; } set { field2 = value; } }
    public override void Do()
    {
        foreach (var card in Targets)
        {
            if (card.BelongedRegion == card.Controller.BackField)
            {
                card.MoveTo(card.Controller.FrontField);
            }
            else if (card.BelongedRegion == card.Controller.FrontField)
            {
                card.MoveTo(card.Controller.BackField);
            }
            if (Reason == null)
            {
                card.IsHorizontal = true;
            }
        }
    }
}

public class ReverseBondMessage : Message
{
    public List<Card> Targets { get { return field1; } set { field1 = value; } }
    public Skill Reason { get { return field2; } set { field2 = value; } }
    public bool AsCost { get { return field3; } set { field3 = value; } }
    public override void Do()
    {
        foreach (var card in Targets)
        {
            card.FrontShown = false;
        }
    }
}

public class ToBondMessage : Message
{
    public List<Card> Targets { get { return field1; } set { field1 = value; } }
    public bool TargetFrontShown { get { return field2; } set { field2 = value; } }
    public Skill Reason { get { return field3; } set { field3 = value; } }
    public override void Do()
    {
        foreach (var card in Targets)
        {
            card.MoveTo(card.Controller.Bond);
            if (!TargetFrontShown)
            {
                card.FrontShown = false;
            }
        }
    }
}

/// <summary>
/// 发起攻击，对应“攻击时/被攻击时”
/// </summary>
public class AttackMessage : Message
{
    public Card AttackingUnit { get { return field1; } set { field1 = value; } }
    public Card DefendingUnit { get { return field2; } set { field2 = value; } }
    public override void Do()
    {
        AttackingUnit.IsHorizontal = true;
        AttackingUnit.HasAttackedInThisTurn = true;
        Game.AttackingUnit = AttackingUnit;
        Game.DefendingUnit = DefendingUnit;
    }
}

/// <summary>
/// 配置支援卡，对应“被放置到支援区时”
/// </summary>
public class SetSupportMessage : Message
{
    public User User { get { return field1; } set { field1 = value; } }
    public override void Do()
    {
        User.Deck.Top.MoveTo(User.Support);
    }
}

/// <summary>
/// 判断支援是否成功，对应“被XXX支援时”
/// </summary>
public class ConfirmSupportMessage : Message
{
    public Card Unit { get { return field1; } set { field1 = value; } }
    public Card SupportCard { get { return field2; } set { field2 = value; } }
    public bool IsSuccessful { get { return field3; } set { field3 = value; } }
    public override void Do()
    {
        if (!IsSuccessful && SupportCard != null)
        {
            SupportCard.MoveTo(SupportCard.Controller.Retreat);
        }
    }
}

/// <summary>
/// 将支援卡放置到退避区
/// </summary>
public class RemoveSupportMessage : SendToRetreatMessage { }

/// <summary>
/// 战斗结束
/// </summary>
public class EndBattleMessage : Message
{
    public Card AttackingUnit { get { return field1; } set { field1 = value; } }
    public Card DefendingUnit { get { return field2; } set { field2 = value; } }
}

public class SendToRetreatMessage : Message
{
    public List<Card> Targets { get { return field1; } set { field1 = value; } }
    public Skill Reason { get { return field2; } set { field2 = value; } }
    public bool AsCost { get { return field3; } set { field3 = value; } }
    public override void Do()
    {
        Targets.ForEach(card => card.MoveTo(card.Controller.Retreat));
    }
}

public class SetActionedMessage : Message
{
    public List<Card> Targets { get { return field1; } set { field1 = value; } }
    public Skill Reason { get { return field2; } set { field2 = value; } }
    public bool AsCost { get { return field3; } set { field3 = value; } }
    public override void Do()
    {
        Targets.ForEach(card => card.IsHorizontal = true);
    }
}

public class DiscardHandMessage : SendToRetreatMessage { }

public class AddToHandMessage : Message
{
    public List<Card> Targets { get { return field1; } set { field1 = value; } }
    public Skill Reason { get { return field2; } set { field2 = value; } }
    public bool Show { get { return field3; } set { field3 = value; } }
    public override void Do()
    {
        Targets.ForEach(card => card.MoveTo(card.Controller.Hand));
    }
}

public class AddToOrbMessage : Message
{
    public Card Target { get { return field1; } set { field1 = value; } }
    public Skill Reason { get { return field2; } set { field2 = value; } }
    public override void Do()
    {
        Target.MoveTo(Target.Controller.Orb);
    }
}

public class ClearStatusEndingBattleMessage : Message
{
    public Card AttackingUnit { get { return field1; } set { field1 = value; } }
    public Card DefendingUnit { get { return field2; } set { field2 = value; } }
    public override void Do()
    {
        Game.ForEachCard(card => card.ClearStatusEndingBattle());
        Game.CriticalFlag = false;
        Game.AvoidFlag = false;
        Game.AttackingUnit = null;
        Game.DefendingUnit = null;
    }
}

public class CriticalAttackMessage : Message
{
    public Card AttackingUnit { get { return field1; } set { field1 = value; } }
    public Card DefendingUnit { get { return field2; } set { field2 = value; } }
    public Card Cost { get { return field3; } set { field3 = value; } }
    public override void Do()
    {
        Cost.MoveTo(Cost.Controller.Retreat);
        Game.CriticalFlag = true;
    }
}

public class AvoidMessage : Message
{
    public Card AttackingUnit { get { return field1; } set { field1 = value; } }
    public Card DefendingUnit { get { return field2; } set { field2 = value; } }
    public Card Cost { get { return field3; } set { field3 = value; } }
    public override void Do()
    {
        Cost.MoveTo(Cost.Controller.Retreat);
        Game.AvoidFlag = true;
    }
}

public class DestroyMessage : Message
{
    public List<Card> DestroyedUnits { get { return field1; } set { field1 = value; } }
    public DestructionReasonTag ReasonTag { get { return field2; } set { field2 = value; } }
    public Card AttackingUnit { get { return field3; } set { field3 = value; } }
    public Skill Reason { get { return field4; } set { field4 = value; } }
    public int Count { get { return field5; } set { field5 = value; } }
    public override void Do()
    {
        DestroyedUnits.ForEach(unit =>
        {
            unit.DestructionReasonTag = ReasonTag;
            unit.DestroyedCount = Count;
        });
    }
}

public class AttackFailureMessage : Message
{
    public Card AttackingUnit { get { return field1; } set { field1 = value; } }
    public Card DefendingUnit { get { return field2; } set { field2 = value; } }
}

public class StartTurnMessage : Message
{
    public User TurnPlayer { get { return field1; } set { field1 = value; } }
    public override void Do()
    {
        Game.TurnPlayer = TurnPlayer;
        Game.CurrentPhase = Phase.BeginningPhase;
        Game.TurnCount++;
        TurnPlayer.DeployAndCCCostCount = 0;
    }
}

public class GoToBondPhaseMessage : Message
{
    public User TurnPlayer { get { return field1; } set { field1 = value; } }
    public override void Do()
    {
        Game.CurrentPhase = Phase.BondPhase;
    }
}

public class GoToDeploymentPhaseMessage : Message
{
    public User TurnPlayer { get { return field1; } set { field1 = value; } }
    public override void Do()
    {
        Game.CurrentPhase = Phase.DeploymentPhase;
    }
}

public class GoToActionPhaseMessage : Message
{
    public User TurnPlayer { get { return field1; } set { field1 = value; } }
    public override void Do()
    {
        Game.CurrentPhase = Phase.ActionPhase;
    }
}

public class EndTurnMessage : Message
{
    public User TurnPlayer { get { return field1; } set { field1 = value; } }
    public override void Do()
    {
        Game.CurrentPhase = Phase.EndPhase;
    }
}

public class ClearStatusEndingTurnMessage : Message
{
    public User TurnPlayer { get { return field1; } set { field1 = value; } }
    public override void Do()
    {
        Game.ForEachCard(card => card.ClearStatusEndingTurn());
    }
}
public class SwitchTurnMessage : Message
{
    public User NextTurnPlayer { get { return field1; } set { field1 = value; } }
    public override void Do()
    {
        if (Game.Player == NextTurnPlayer)
        {
            Game.StartTurn();
        }
    }
}
public class RefreshUnitMessage : Message
{
    public List<Card> Targets { get { return field1; } set { field1 = value; } }
    public Skill Reason { get { return field2; } set { field2 = value; } }
    public override void Do()
    {
        foreach (var unit in Targets)
        {
            unit.IsHorizontal = false;
        }
    }
}

public class DrawCardMessage : Message
{
    public int Number { get { return field1; } set { field1 = value; } }
    public User Player { get { return field2; } set { field2 = value; } }
    public Skill Reason { get { return field3; } set { field3 = value; } }

    public override void Do()
    {
        for (int i = 0; i < Number; i++)
        {
            if (Player.Deck.Count > 0)
            {
                Player.Deck.Top.MoveTo(Player.Hand);
            }
        }
    }
}

public class ReadyForSameNameProcessPartialMessage : Message
{
    public List<Card> Targets { get { return field1; } set { field1 = value; } }
    public string Name { get { return field2; } set { field2 = value; } }
}

public class SendToRetreatSameNameProcessMessage : SendToRetreatMessage { }

public class ReadyForDestructionProcessMessage : Message
{
    public List<Card> CardsToSendToRetreat { get { return field1; } set { field1 = value; } }
    public Dictionary<User, int> OrbsDetructionCountDict { get { return field2; } set { field2 = value; } }
}

public class ObtainOrbDestructionProcessMessage : Message
{
    public Card Target { get { return field1; } set { field1 = value; } }
    public Card Reason { get { return field2; } set { field2 = value; } }

    public override void Do()
    {
        Target.MoveTo(Target.Controller.Hand);
        Reason.DestroyedCount--;
        if (Reason.Controller.Orb.Count == 0)
        {
            Reason.DestroyedCount = 0;
        }
    }
}

public class SendToRetreatDestructionProcessMessage : SendToRetreatMessage { }

public class SendToRetreatPositionProcessMessage : SendToRetreatMessage { }

public class MoveMarchingProcessMessage : Message
{
    public List<Card> Targets { get { return field1; } set { field1 = value; } }

    public override void Do()
    {
        Targets.ForEach(target =>
        {
            target.MoveTo(target.Controller.FrontField);
        });
    }
}

public class ShowCardsMessage : Message
{
    public List<Card> Targets { get { return field1; } set { field1 = value; } }
    public Skill Reason { get { return field2; } set { field2 = value; } }
}

public class AttachItemMessage : Message
{
    public IAttachable Item { get { return field1; } set { field1 = value; } }
    public Card Target { get { return field2; } set { field2 = value; } }

    public override void Do()
    {
        Target.Attach(Item);
    }
}

public class GrantSkillMessage : AttachItemMessage { }

public class ChangeDefendingUnitMessage : Message
{
    public Card FromUnit { get { return field1; } set { field1 = value; } }
    public Card ToUnit { get { return field2; } set { field2 = value; } }
    public Skill Reason { get { return field3; } set { field3 = value; } }

    public override void Do()
    {
        Game.DefendingUnit = ToUnit;
    }
}

public class ShuffleDeckMessage : Message
{
    public User User { get { return field1; } set { field1 = value; } }
    public List<int> Order { get { return field2; } set { field2 = value; } }
    public Skill Reason { get { return field3; } set { field3 = value; } }

    public override void Do()
    {
        User.Deck.ApplyOrder(Order);
    }
}

public class SetToDeckTopMessage : Message
{
    public List<Card> Targets { get { return field1; } set { field1 = value; } }
    public Skill Reason { get { return field2; } set { field2 = value; } }

    public override void Do()
    {
        foreach (var target in Targets)
        {
            target.MoveTo(target.Controller.Deck.Top);
        }
    }
}

public class GameOverMessage : Message
{
    public List<User> LosingUsers { get { return field1; } set { field1 = value; } }
    public Skill Reason { get { return field2; } set { field2 = value; } }
}
///// 消息种类
///// </summary>
//public enum MessageType
//{
//    /// <summary>
//    /// 猜拳，0：<int> 0 = 石头, 1 = 剪刀, 2 = 布
//    /// </summary>
//    DecideFirst,

//    /// <summary>
//    /// 抽卡
//    /// </summary>
//    Draw,

//    /// <summary>
//    /// 无反应，可以继续
//    /// </summary>
//    Continue,

//    /// <summary>
//    /// 卡组切洗
//    /// </summary>
//    DeckShuffle,

//    /// <summary>
//    /// 卡组补充
//    /// </summary>
//    ReplendishDeck,

//    /// <summary>
//    /// 放置到退避区，0：<Card>被放置到退避区的卡片，1：<Area>来自的区域，2：<Skill>如果是因为能力，该能力
//    /// </summary>
//    SendToRetreat,

//    /// <summary>
//    /// 放置到宝玉区，0：<Card>被放置到宝玉区的卡片，1：<Area>来自的区域，2：<Skill>如果是因为能力，该能力
//    /// </summary>
//    SendToOrb,

//    /// <summary>
//    /// 宝玉击破，0：<Card>宝玉卡
//    /// </summary>
//    GetOrb,

//    /// <summary>
//    /// 单位被击破，0：<Card>被击破的单位，1：<Card>击破该单位的卡，2：<Skill>如果是因为能力，该能力
//    /// </summary>
//    UnitDestroyed,

//    /// <summary>
//    /// 败北
//    /// </summary>
//    Lose,

//    /// <summary>
//    /// 进军
//    /// </summary>
//    March,

//    /// <summary>
//    /// 诱发能力处理开始
//    /// </summary>
//    InducedSkillProcess_Starting,

//    /// <summary>
//    /// 本轮诱发能力处理中回合玩家的能力全部处理完毕
//    /// </summary>
//    InducedSkillProcess_TurnPlayerSideFinished,

//    /// <summary>
//    /// 诱发能力处理结束
//    /// </summary>
//    InducedSkillProcess_Ending,

//    /// <summary>
//    /// 移动单位，0：<Card>被移动的单位，1：<Skill>如果是因为能力而移动，该能力
//    /// </summary>
//    MoveUnit,

//    /// <summary>
//    /// 向对手询问复数个单位是否可以被移动，仅被Ask使用，0：<List<Cards>>可能被移动的单位；1：<Skill>实施移动行为的能力
//    /// </summary>
//    TryMoveUnits,

//    /// <summary>
//    /// 对手回复可以被移动的复数单位，0：<List<Cards>>可以被移动的单位
//    /// </summary>
//    UnitsCanBeMoved,

//    /// <summary>
//    /// 出击，0：<Card>出击的单位，1：<Area>来自的区域，2：<Area>出击到的区域，3：<bool> 若以未行动状态出击则为 true，4：<Skill>如果是因为能力，该能力
//    /// </summary>
//    Deploy,

//    /// <summary>
//    /// 设定攻击对象，0：<Card>进行攻击的单位，1：<Card>被攻击的单位
//    /// </summary>
//    SetAttackTarget,

//    /// <summary>
//    ///  攻击结束时，0：<Card>进行攻击的单位，1：<Card>被攻击的单位
//    /// </summary>
//    AttackEnding,

//    /// <summary>
//    /// 攻击结束后，0：<Card>进行攻击的单位，1：<Card>被攻击的单位
//    /// </summary>
//    AttackEnded,

//    /// <summary>
//    /// 将羁绊卡翻面，0：<Card>被翻面的羁绊卡
//    /// </summary>
//    UseBond,

//    /// <summary>
//    /// 回合结束（收到消息时已经是下一个玩家的回合）
//    /// </summary>
//    TurnEnd,

//    /// <summary>
//    /// 给某个单位增加Buff，0：<Buff>增加的Buff，1：<Card>对象单位；2：<Skill>增加Buff的能力
//    /// </summary>
//    AddBuffToUnit,

//    /// <summary>
//    /// 移除某个单位的Buff，0：<Buff>移除的Buff，1：<Card>对象单位
//    /// </summary>
//    RemoveBuffFromUnit,

//    /// <summary>
//    /// 回合开始
//    /// </summary>
//    TurnStart,

//    /// <summary>
//    /// 将羁绊卡移回左边
//    /// </summary>
//    RenewBonds,

//    /// <summary>
//    /// 回合开始时将所有单位转为未行动状态
//    /// </summary>
//    RenewUnits,

//    /// <summary>
//    /// 将某个单位转为已行动状态，0：<Card>该单位
//    /// </summary>
//    SetUnitActioned,

//    /// <summary>
//    /// 将某个单位转为未行动状态，0：<Card>该单位
//    /// </summary>
//    UnSetUnitActioned,

//    /// <summary>
//    /// 羁绊阶段开始
//    /// </summary>
//    BondPhaseStart,

//    /// <summary>
//    /// 将一张卡放置到羁绊区，0：<Card>该卡，1：<Area>该卡来自的区域，2：<Skill>如果是因为能力，该能力
//    /// </summary>
//    SetBond,

//    /// <summary>
//    /// 结束阶段开始
//    /// </summary>
//    EndPhaseStart,

//    /// <summary>
//    /// 出击阶段开始
//    /// </summary>
//    DeploymentPhaseStart,

//    /// <summary>
//    /// 升级，0：<Card>将要上场的卡，1：<该卡来自的区域>，2：<Card>将要被升级的单位，3：<bool>是否为转职，4：<Skill>如果是因为能力，该能力
//    /// </summary>
//    LevelUp,

//    /// <summary>
//    /// 实行同名处理，0：<string>触发同名处理的字段，1：<Card>被保留的单位；2：<List<Card>>同名的卡列表
//    /// </summary>
//    DoSameNameProcess,

//    /// <summary>
//    /// 升级前检查是否符合同名，仅被Try使用，若返回false，则说明和常规相反，0：<Card>将要上场的卡，1：<该卡来自的区域>，2：<Card>将要被升级的卡，3：<bool>是否为转职，4：<Skill>如果是因为能力，该能力
//    /// </summary>
//    LevelUpCheckName,

//    /// <summary>
//    /// 离场（非动作），0：<Card>离场的卡
//    /// </summary>
//    LeaveField,

//    /// <summary>
//    /// 行动阶段开始
//    /// </summary>
//    ActionPhaseStart,

//    /// <summary>
//    /// 向对手询问复数个单位是否可以被攻击，仅被Ask使用，0：<List<Cards>>可能被攻击的卡；1：<Card>进行攻击的单位
//    /// </summary>
//    TryAttackUnits,

//    /// <summary>
//    /// 对手回复可以被攻击的复数单位，0：<List<Cards>>可以被攻击的单位
//    /// </summary>
//    UnitsCanBeAttacked,

//    /// <summary>
//    /// 使用能力，0：<Skill>被使用的能力
//    /// </summary>
//    UseSkill,

//    /// <summary>
//    /// 发起攻击，0：<Card>发起攻击的卡
//    /// </summary>
//    AttackStart,

//    /// <summary>
//    /// 开始支援，0：<Card>被支援的单位，1：<Card>支援卡
//    /// </summary>
//    StartSupport,

//    /// <summary>
//    /// 支援判定，0：<Card>被支援的单位，1：<Card>支援卡，2：<bool>支援成功与否
//    /// </summary>
//    DetermineSupport,

//    /// <summary>
//    /// 回合玩家的支援能力处理完毕
//    /// </summary>
//    SolveSupportSkillFinished,

//    /// <summary>
//    /// 战斗力变化（支援），0：<Card>战斗力变化的卡，1：<int>原数值，2：<int>新数值
//    /// </summary>
//    AddSupportToPower,

//    /// <summary>
//    /// 发动必杀攻击，0：<Card>进行攻击的单位，1：<Card>被攻击的单位，2：<Card>丢弃的手牌
//    /// </summary>
//    CriticalAttack,

//    /// <summary>
//    /// 卡片位置发生移动后的提示（非动作），0：<Card>移动的卡，1：<Area>原区域，2：<Area>目标区域
//    /// </summary>
//    CardMoved,

//    /// <summary>
//    /// 请求神速回避结果
//    /// </summary>
//    AskIfAvoid,

//    /// <summary>
//    /// 神速回避，0：<Card>进行攻击的单位，1：<Card>被攻击的单位，2：<Card>丢弃的手牌
//    /// </summary>
//    Avoid,

//    /// <summary>
//    /// 单位变为被击破状态，0：<Card>被击破的单位，1：<Card>击破该单位的卡，2：<Skill>如果是因为能力，该能力
//    /// </summary>
//    DestroyUnit,

//    /// <summary>
//    /// 支援判定失败，0：<Card>被支援的单位，1：<Card>支援卡
//    /// </summary>
//    FailSupport,

//    /// <summary>
//    /// 结束支援判定，0：<Card>被支援的单位，1：<Card>支援卡
//    /// </summary>
//    EndSupport,

//    /// <summary>
//    /// 战斗结束时战斗力恢复，0：<Card>战斗力恢复的单位，1：<int>战斗力恢复后的数值
//    /// </summary>
//    AttackEndedPowerRenew,

//    /// <summary>
//    /// 诱发能力处理中非回合玩家无需要处理的能力
//    /// </summary>
//    InducedSkillProcess_NoneTurnPlayerNoReply,

//    /// <summary>
//    /// 请求转职抽卡处理
//    /// </summary>
//    AskForCCBonusProcess,

//    /// <summary>
//    /// 请求同名处理
//    /// </summary>
//    AskForSameNameProcess,

//    /// <summary>
//    /// 请求击破处理
//    /// </summary>
//    AskForDestroyedProcess,

//    /// <summary>
//    /// 请求败北处理
//    /// </summary>
//    AskForLoseProcess,

//    /// <summary>
//    /// 请求进军处理
//    /// </summary>
//    AskForMarchProcess,

//    /// <summary>
//    /// 给某个单位增加附加能力，0：<SubSkill>增加的附加能力，1：<Card>对象单位；2：<Skill>增加附加能力的能力
//    /// </summary>
//    AddSubSkillToUnit,

//    /// <summary>
//    /// 移除某个单位的附加能力，0：<SubSkill>移除的附加能力，1：<Card>对象单位
//    /// </summary>
//    RemoveSubSkillFromUnit,

//    /// <summary>
//    /// 将卡加入手牌，0：<Card>加入手牌的卡，1：<Area>来自的区域，2：<Skill>如果是因为能力，该能力
//    /// </summary>
//    AddCardToHand,

//    /// <summary>
//    /// 代替攻击，0：<Card>发起攻击的单位，1：<Card>原本被攻击的单位，2：<Card>代替被攻击的单位，3：<Skill>导致攻击对象改变的能力
//    /// </summary>
//    ChangeAttackedTarget,
//}