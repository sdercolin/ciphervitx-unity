using System.Collections.Generic;
/// <summary>
/// 所有附加值的基类
/// </summary>
public abstract class Buff : IAttachable
{
    /// <summary>
    /// 附加值的构造函数
    /// </summary>
    /// <param name="giver">产生该附加值的卡</param>
    /// <param name="origin">产生该附加值的能力</param>
    /// <param name="lastingType">持续类型</param>
    public Buff(Card giver, Skill origin, LastingTypeEnum lastingType)
    {
        Giver = giver;
        Origin = origin;
        LastingType = lastingType;
        Guid = System.Guid.NewGuid().ToString();
    }

    public string Guid;
    public bool OnlyAvailableWhenFrontShown { get; set; }
    public List<Area> AvailableAreas { get; set; }
    public Card Giver;
    public Card Owner { get; set; }
    public Skill Origin;
    public LastingTypeEnum LastingType;

    public virtual void Attached()
    {
        OnlyAvailableWhenFrontShown = true;
        AvailableAreas = new List<Area>() { Owner.Controller.FrontField, Owner.Controller.BackField };
    }

    public void Detach()
    {
        Owner.AttachableList.Remove(this);
        Owner = null;
    }

    public void Read(Message message)
    {
        if (OnlyAvailableWhenFrontShown)
        {
            if (!Owner.FrontShown)
            {
                Detach();
                return;
            }
        }

        if (!AvailableAreas.Contains(Owner.BelongedRegion))
        {
            Detach();
            return;
        }

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

    public bool Try(Message message, ref Message substitute)
    {
        return true;
    }
}

/// <summary>
/// 持续类型
/// </summary>
public enum LastingTypeEnum
{
    UntilBattleEnds, //直到战斗结束
    UntilTurnEnds, //直到回合结束
    UntilNextOpponentTurnEnds, //直到下个对手的回合结束
    Forever //永续
}

/// <summary>
/// 单位名附加值
/// </summary>
public class UnitNameBuff : Buff
{
    public UnitNameBuff(Card giver, Skill origin, bool isAdding, string value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(giver, origin, lastingType)
    {
        IsAdding = isAdding;
        Value = value;
    }

    /// <summary>
    /// 是否为添加该单位名
    /// </summary>
    public bool IsAdding;

    /// <summary>
    /// 附加值的值
    /// </summary>
    public string Value;

    public override void Attached()
    {
        base.Attached();
        AvailableAreas = ListUtils.Clone(Owner.Controller.AllAreas);
    }
}

/// <summary>
/// 出击费用附加值
/// </summary>
public class DeployCostBuff : Buff
{
    public DeployCostBuff(Card giver, Skill origin, int value, bool isBecome, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(giver, origin, lastingType)
    {
        Value = value;
        IsBecome = isBecome;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value;


    /// <summary>
    /// 是否为“变为”类型
    /// </summary>
    public bool IsBecome;

    public override void Attached()
    {
        base.Attached();
        AvailableAreas = ListUtils.Clone(Owner.Controller.AllAreas);
    }
}

/// <summary>
/// 转职费用附加值
/// </summary>
public class ClassChangeCostBuff : Buff
{
    public ClassChangeCostBuff(Card giver, Skill origin, int value, bool isBecome, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(giver, origin, lastingType)
    {
        Value = value;
        IsBecome = isBecome;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value;

    /// <summary>
    /// 是否为“变为”类型
    /// </summary>
    public bool IsBecome;

    public override void Attached()
    {
        base.Attached();
        AvailableAreas = ListUtils.Clone(Owner.Controller.AllAreas);
    }
}

/// <summary>
/// 战斗力附加值
/// </summary>
public class PowerBuff : Buff
{
    public PowerBuff(Card giver, Skill origin, int value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(giver, origin, lastingType)
    {
        Value = value;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value;
}

/// <summary>
/// 支援力附加值
/// </summary>
public class SupportBuff : Buff
{
    public SupportBuff(Card giver, Skill origin, int value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(giver, origin, lastingType)
    {
        Value = value;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value;

    public override void Attached()
    {
        base.Attached();
        AvailableAreas.Add(Owner.Controller.Support);
    }
}

/// <summary>
/// 势力附加值
/// </summary>
public class SymbolBuff : Buff
{
    public SymbolBuff(Card giver, Skill origin, bool isAdding, SymbolEnum value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(giver, origin, lastingType)
    {
        IsAdding = isAdding;
        Value = value;
    }

    /// <summary>
    /// 是否为添加该势力
    /// </summary>
    public bool IsAdding;

    /// <summary>
    /// 附加值的值
    /// </summary>
    public SymbolEnum Value;
}

/// <summary>
/// 武器附加值
/// </summary>
public class WeaponBuff : Buff
{
    public WeaponBuff(Card giver, Skill origin, bool isAdding, WeaponEnum value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(giver, origin, lastingType)
    {
        IsAdding = isAdding;
        Value = value;
    }

    /// <summary>
    /// 是否为添加该武器
    /// </summary>
    public bool IsAdding;

    /// <summary>
    /// 附加值的值
    /// </summary>
    public WeaponEnum Value;

    public override void Attached()
    {
        base.Attached();
        AvailableAreas = ListUtils.Clone(Owner.Controller.AllAreas);
    }
}

/// <summary>
/// 性别附加值
/// </summary>
public class GenderBuff : Buff
{
    public GenderBuff(Card giver, Skill origin, bool isAdding, GenderEnum value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(giver, origin, lastingType)
    {
        IsAdding = isAdding;
        Value = value;
    }

    /// <summary>
    /// 是否为添加该性别
    /// </summary>
    public bool IsAdding;

    /// <summary>
    /// 附加值的值
    /// </summary>
    public GenderEnum Value;

    public override void Attached()
    {
        base.Attached();
        AvailableAreas = ListUtils.Clone(Owner.Controller.AllAreas);
    }
}

/// <summary>
/// 属性附加值
/// </summary>
public class TypeBuff : Buff
{
    public TypeBuff(Card giver, Skill origin, bool isAdding, TypeEnum value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(giver, origin, lastingType)
    {
        IsAdding = isAdding;
        Value = value;
    }

    /// <summary>
    /// 是否为添加该属性
    /// </summary>
    public bool IsAdding;

    /// <summary>
    /// 附加值的值
    /// </summary>
    public TypeEnum Value;

    public override void Attached()
    {
        base.Attached();
        AvailableAreas = ListUtils.Clone(Owner.Controller.AllAreas);
    }
}

/// <summary>
/// 射程附加值
/// </summary>
public class RangeBuff : Buff
{
    public RangeBuff(Card giver, Skill origin, bool isAdding, RangeEnum value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(giver, origin, lastingType)
    {
        IsAdding = isAdding;
        Value = value;
    }

    /// <summary>
    /// 是否为添加该射程
    /// </summary>
    public bool IsAdding;

    /// <summary>
    /// 附加值的值
    /// </summary>
    public RangeEnum Value;
}