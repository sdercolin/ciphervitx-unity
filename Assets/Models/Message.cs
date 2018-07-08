using System;
using System.Collections.Generic;

public class Message
{
    /// <summary>
    /// 是否是自己发送的消息
    /// </summary>
    public bool SendBySelf = true;

    public virtual Message Clone()
    {
        Type messageType = GetType();
        Message clone = Activator.CreateInstance(messageType) as Message;
        clone.reasonCard = reasonCard;
        clone.Reason = Reason;
        clone.SendBySelf = SendBySelf;
        clone.Targets = new List<Card>();
        clone.Targets.AddRange(Targets);
        clone.Value = Value;
        return clone;
    }

    public virtual void Do() { }

    public List<Card> Targets = new List<Card>();
    public int? Value = null;
    protected Card reasonCard = null;
    public Card ReasonCard
    {
        set
        {
            reasonCard = value;
        }
        get
        {
            if (Reason != null && reasonCard == null)
            {
                return Reason.Owner;
            }
            else
            {
                return reasonCard;
            }
        }
    }
    public Skill Reason = null;

    public bool TrueForAnyTarget(Predicate<Card> predicate)
    {
        foreach (Card card in Targets)
        {
            if (predicate(card))
            {
                return true;
            }
        }
        return false;
    }

    public bool TrueForAllTargets(Predicate<Card> predicate)
    {
        foreach (Card card in Targets)
        {
            if (!predicate(card))
            {
                return false;
            }
        }
        return true;
    }

    public List<Card> GetTargetsTrueFor(Predicate<Card> predicate)
    {
        List<Card> results = new List<Card>();
        foreach (Card card in Targets)
        {
            if (predicate(card))
            {
                results.Add(card);
            }
        }
        return results;
    }
}

public class EmptyMessage : Message
{
    public override void Do() { }
}
public class DeployMessage : Message { }

public class MoveMessage : Message
{
    public override void Do()
    {
        foreach (Card card in Targets)
        {
            if (card.BelongedRegion == card.Controller.BackField)
            {
                card.MoveTo(card.Controller.FrontField);
            }
            else if (card.BelongedRegion == card.Controller.FrontField)
            {
                card.MoveTo(card.Controller.BackField);
            }
        }
    }
}


///// <summary>
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
