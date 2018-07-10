using System;
using System.Collections.Generic;

public abstract class Skill : IAttachable
{
    /// <summary>
    /// 持有该能力的卡
    /// </summary>
    public virtual Card Owner { get; set; }

    /// <summary>
    /// 控制者
    /// </summary>
    public User Controller { get { return Owner.Controller; } }

    /// <summary>
    /// 控制者的对手
    /// </summary>
    public User Opponent { get { return Owner.Controller.Opponent; } }

    /// <summary>
    /// 游戏对象
    /// </summary>
    public Game Game { get { return Owner.Game; } }

    /// <summary>
    /// 该能力在卡面上的记述顺序号
    /// </summary>
    public int Number;

    /// <summary>
    /// 该能力的名称
    /// </summary>
    public string Name;

    /// <summary>
    /// 该能力的描述
    /// </summary>
    public string Description;

    /// <summary>
    /// 是否在本回合中使用过
    /// </summary>
    public bool UsedInThisTurn = false;

    /// <summary>
    /// 是否一回合只能使用一次
    /// </summary>
    public bool OncePerTurn = false;

    /// <summary>
    /// 是否有效
    /// </summary>
    public bool Available = true;

    /// <summary>
    /// 该能力持有的关键字
    /// </summary>
    public SkillKeyword Keyword = SkillKeyword.Null;

    /// <summary>
    /// 该能力的种类标志
    /// </summary>
    public List<SkillTypeSymbol> TypeSymbols = new List<SkillTypeSymbol>();

    #region 不使用
    public virtual bool OnlyAvailableWhenFrontShown { get; set; }
    public virtual List<Area> AvailableAreas { get; set; }
    public virtual void Detach() { }
    #endregion

    /// <summary>
    /// 确认该能力是否对某个消息有响应并执行响应 
    /// 自动、常时需确认在场；起动不需
    /// </summary>
    /// <param name="message">该消息</param>
    public virtual void Read(Message message) { }

    /// <summary>
    /// 询问该能力是否允许某操作
    /// </summary>
    /// <param name="message">表示该操作的消息</param>
    /// <param name="substitute">拒绝该操作时表示作为代替的动作的的消息</param>
    /// <returns>如允许，则返回True</returns>
    public virtual bool Try(Message message, ref Message substitute)
    {
        return true;
    }
}

/// <summary>
/// 起动型能力
/// 起动型能力的描述分为以下几部分：
///     1.发动条件：写在CheckConditions函数中，若不满足则不能发动
///     2.费用：检查部分写在CheckCost函数中，若不能支付则不能发动；支付部分写在PayCost函数中
///     3.效果：写在Do函数中
/// </summary>
public abstract class ActionSkill : Skill
{
    /// <summary>
    /// 判断该能力是否可以发动
    /// </summary>
    /// <returns>若可以发动，返回true</returns>
    public bool Check()
    {
        if (!Available)
        {
            return false;
        }
        else if (UsedInThisTurn)
        {
            return false;
        }
        else if (!CheckConditions())
        {
            return false;
        }
        else
        {
            return CheckCost();
        }
    }

    /// <summary>
    /// 解决能力
    /// </summary>
    public void Solve()
    {
        //Owner.Controller.Broadcast(new Message(MessageType.UseSkill, new System.Collections.ArrayList { this }));
        PayCost();
        Do();
        if (OncePerTurn)
        {
            UsedInThisTurn = true;
        }
    }

    public override void Read(Message message)
    {
        base.Read(message);
    }

    public override bool Try(Message message, ref Message substitute)
    {
        return base.Try(message, ref substitute);
    }

    /// <summary>
    /// 判断是否符合发动条件
    /// </summary>
    /// <returns>若符合，则返回true</returns>
    public abstract bool CheckConditions();

    /// <summary>
    /// 判断是否能够支付费用
    /// </summary>
    /// <returns>若能够，则返回true</returns>
    public abstract bool CheckCost();

    /// <summary>
    /// 支付费用
    /// </summary>
    public abstract void PayCost();

    /// <summary>
    /// 实行能力
    /// </summary>
    public abstract void Do();
}

/// <summary>
/// 自动型能力（包括少数特殊型能力）
/// 自动型能力的描述分为以下几部分：
///     1.诱发条件：写在CheckInduceConditions函数中，检查Message是否符合诱发条件，一旦被诱发，一定会在之后的某时刻被解决（即运行Solve函数）
///     2.实行条件：写在CheckConditions函数中，玩家选择解决该诱发能力后首先进行实行条件的判断，若不符合，则停止解决该能力，视为本回合未使用过该能力
///     3.费用：检查部分写在CheckCost函数中，若不能支付则不能发动；支付部分写在PayCost函数中
///     4.效果：写在Do函数中，实行条件与费用检查都通过时，实行该能力的效果，若为选发效果，也可以选择不发，此时停止解决该能力，视为本回合未使用过该能力
/// </summary>
public abstract class AutoSkill : Skill
{
    /// <summary>
    /// 诱发计数
    /// </summary>
    private int InducedCount = 0;

    public bool Optional = false;

    /// <summary>
    /// 诱发
    /// </summary>
    public void Induce()
    {
        if (Available && (!UsedInThisTurn))
        {
            InducedCount++;
            //Owner.Controller.Owner.Game.InducedSkillSet.Add(this);
        }
    }

    /// <summary>
    /// 诱发状态
    /// </summary>
    public bool IsInduced
    {
        get
        {
            return InducedCount > 0;
        }
    }

    /// <summary>
    /// 能力解决
    /// </summary>
    public void Solve()
    {
        InducedCount--;
        if (CheckConditions() && CheckCost())
        {
            if (Optional)
            {
                if (!Request<Skill>.AskIfUse(this))
                {
                    return;
                }
            }
            //Owner.Controller.Broadcast(new Message(MessageType.UseSkill, new System.Collections.ArrayList { this }));
            PayCost();
            Do();
            if (OncePerTurn)
            {
                UsedInThisTurn = true;
            }
        }
    }

    public override void Read(Message message)
    {
        base.Read(message);
        if (CheckInduceConditions(message))
        {
            Induce();
        }
    }

    public override bool Try(Message message, ref Message substitute)
    {
        return base.Try(message, ref substitute);
    }

    /// <summary>
    /// 判断诱发条件
    /// </summary>
    /// <returns>若满足诱发条件，则返回true</returns>
    public abstract bool CheckInduceConditions(Message message);

    /// <summary>
    /// 判断实行条件
    /// </summary>
    /// <returns>若满足实行条件，则返回true</returns>
    public abstract bool CheckConditions();

    /// <summary>
    /// 判断是否能够支付费用
    /// </summary>
    /// <returns>若能够，则返回true</returns>
    public abstract bool CheckCost();

    /// <summary>
    /// 支付费用
    /// </summary>
    public abstract void PayCost();

    /// <summary>
    /// 能力实行
    /// </summary>
    public abstract void Do();
}

/// <summary>
/// 常时型能力
/// Read函数时刻更新能力的状态
/// </summary>
public abstract class PermanentSkill : Skill
{
    protected List<Card> Targets = new List<Card>();
    protected Dictionary<Card, IAttachable[]> ItemsApplied = new Dictionary<Card, IAttachable[]>();
    protected List<IAttachable> ItemsToApply = new List<IAttachable>();

    /// <summary>
    /// 是否在起效中
    /// </summary>
    public bool IsOn
    {
        get
        {
            return Targets.Count > 0;
        }
    }

    protected void Attach(Card target, List<IAttachable> items)
    {
        foreach (var item in items)
        {
            target.Attach(item);
        }
        ItemsApplied.Add(target, items.ToArray());
    }

    protected void Detach(Card target)
    {
        Targets.Remove(target);
        foreach (var item in ItemsApplied[target])
        {
            item.Detach();
        }
        ItemsApplied.Remove(target);
    }

    protected void DetachAll()
    {
        Targets.ForEach(target => Detach(target));
    }

    public override void Read(Message message)
    {
        if (!Available || !Owner.IsOnField)
        {
            DetachAll();
            return;
        }
        foreach (Card card in Game.AllCards)
        {
            if (CanTarget(card) && !Targets.Contains(card))
            {
                ItemsToApply.Clear();
                SetItemToApply(card);
                Attach(card, ItemsToApply);
            }
            else if (!CanTarget(card) && Targets.Contains(card))
            {
                Detach(card);
            }
        }
    }

    public override bool Try(Message message, ref Message substitute)
    {
        return base.Try(message, ref substitute);
    }

    /// <summary>
    /// 判定是否为效果适用的对象
    /// </summary>
    /// <param name="card">待判定的卡</param>
    /// <returns>如是，则返回true</returns>
    public abstract bool CanTarget(Card card);

    /// <summary>
    /// 准备待设置的附加量或附加能力（需填入ItemsToApply）
    /// </summary>
    /// <param name="target">效果的对象</param>
    public abstract void SetItemToApply(Card target);
}

/// <summary>
/// 支援型能力
/// </summary>
public abstract class SupportSkill : Skill
{
    /// <summary>
    /// 支援能力种类
    /// </summary>
    public SupportSkillType Type;

    /// <summary>
    /// 能力解决，在流程中被调用
    /// </summary>
    /// <param name="AttackingUnit">攻击单位</param>
    /// <param name="AttackedUnit">被攻击单位</param>
    public void Solve(Card AttackingUnit, Card AttackedUnit)
    {
        if (CheckConditions(AttackingUnit, AttackedUnit))
        {
            //Owner.Controller.Broadcast(new Message(MessageType.UseSkill, new System.Collections.ArrayList { this }));
            Do(AttackingUnit, AttackedUnit);
        }
    }

    /// <summary>
    /// 判断是否符合发动条件
    /// </summary>
    /// <param name="AttackingUnit">攻击单位</param>
    /// <param name="AttackedUnit">被攻击单位</param>
    /// <returns>若符合，则返回true</returns>
    public abstract bool CheckConditions(Card AttackingUnit, Card AttackedUnit);

    /// <summary>
    /// 实行能力
    /// </summary>
    /// <param name="AttackingUnit">攻击单位</param>
    /// <param name="AttackedUnit">被攻击单位</param>
    public abstract void Do(Card AttackingUnit, Card AttackedUnit);
}

/// <summary>
/// 支援能力种类
/// </summary>
public enum SupportSkillType
{
    Attacking, //攻击型
    Defending, //防御型
    AttackingDefending //攻防型
}

/// <summary>
/// 能力种类标志
/// </summary>
public enum SkillTypeSymbol
{
    Action, //起动
    Auto, //自动
    Special, //特殊
    Permanent, //常时
    Bond, //羁绊
    Support //支援
}

/// <summary>
/// 能力关键字
/// </summary>
public enum SkillKeyword
{
    Null, //无
    FS, //行动技
    CCS, //转职技
    LvS2, //升级技2
    LvS3, //升级技3
    LvS4, //升级技4
    LvS5, //升级技5
    US, //共斗技
    CF, //化形
    BS, //羁绊技
    RM, //龙脉
    IS, //连发技
}

/// <summary>
/// 附加能力
/// </summary>
public abstract class SubSkill : Skill
{
    /// <summary>
    /// 产生该附加能力的能力
    /// </summary>
    public Skill Origin;

    /// <summary>
    /// 持续类型
    /// </summary>
    public LastingTypeEnum LastingType;

    public override Card Owner
    {
        get
        {
            return base.Owner;
        }

        set
        {
            base.Owner = value;
            OnlyAvailableWhenFrontShown = true;
            AvailableAreas = new List<Area>() { base.Owner.Controller.FrontField, base.Owner.Controller.BackField };
            Attached();
        }
    }

    public override bool OnlyAvailableWhenFrontShown { get; set; }
    public override List<Area> AvailableAreas { get; set; }

    protected virtual void Attached() { }
    protected virtual void Detaching() { }

    public override void Detach()
    {
        Detaching();
        Owner.AttachableList.Remove(this);
        Owner = null;
    }

    public override void Read(Message message)
    {
        switch (LastingType)
        {
            case LastingTypeEnum.UntilBattleEnds:
                break;
            case LastingTypeEnum.UntilTurnEnds:
                break;
            case LastingTypeEnum.UntilNextOpponentTurnEnds:
                break;
            case LastingTypeEnum.Forever:
                break;
            default:
                break;
        }
    }
}

/// <summary>
/// 无效能力
/// </summary>
public class DisableSkill : SubSkill
{
    public DisableSkill(Skill target)
    {
        Target = target;
    }

    Skill Target;

    protected override void Attached()
    {
        Target.Available = false;
    }

    protected override void Detaching()
    {
        Target.Available = true;
    }
}

/// <summary>
/// 无效全部能力
/// </summary>
public class DisableAllSkills : SubSkill
{
    protected override void Attached()
    {
        Owner.SkillList.ForEach(skill => skill.Available = false);
    }

    protected override void Detaching()
    {
        Owner.SkillList.ForEach(skill => skill.Available = true);
    }
}

/// <summary>
/// 不能被放置到羁绊区
/// </summary>
public class CanNotBePlacedInBond : SubSkill
{
    protected override void Attached()
    {
        AvailableAreas = ListUtils<Area>.Clone(Owner.Controller.AllAreas);
    }

    public override bool Try(Message message, ref Message substitute)
    {
        if (message is ToBondMessage || message is ReadyToBondMessage)
        {
            if (message.Targets.Contains(Owner))
            {
                substitute = message.Clone();
                substitute.Targets.Remove(Owner);
                return false;
            }
        }
        return true;
    }
}