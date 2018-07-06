using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 所有附加值的基类
/// </summary>
public abstract class Buff
{
    /// <summary>
    /// 附加值的构造函数
    /// </summary>
    /// <param name="Owner">该附加值作用的对象</param>
    /// <param name="Giver">产生该附加值的卡</param>
    /// <param name="Origin">产生该附加值的能力</param>
    /// <param name="LastingType">持续类型</param>
    public Buff(Card Giver, Skill Origin, BuffLastingTypeEnum LastingType)
    {
        this.Giver = Giver;
        this.Origin = Origin;
        this.LastingType = LastingType;
    }
    public Card Owner;
    public Card Giver;
    public Skill Origin;
    public BuffLastingTypeEnum LastingType;
}

/// <summary>
/// 持续类型
/// </summary>
public enum BuffLastingTypeEnum
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
    public UnitNameBuff(Card Giver, Skill Origin, bool IsAdding, string Value, BuffLastingTypeEnum LastingType = BuffLastingTypeEnum.Forever) : base(Giver, Origin, LastingType)
    {
        this.IsAdding = IsAdding;
        this.Value = Value;
    }

    /// <summary>
    /// 是否为添加该单位名
    /// </summary>
    public bool IsAdding;

    /// <summary>
    /// 附加值的值
    /// </summary>
    public string Value;
}

/// <summary>
/// 出击费用附加值
/// </summary>
public class DeployCostBuff : Buff
{
    public DeployCostBuff(Card Giver, Skill Origin, int Value, BuffLastingTypeEnum LastingType = BuffLastingTypeEnum.Forever) : base(Giver, Origin, LastingType)
    {
        this.Value = Value;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value;


    /// <summary>
    /// 是否为“变为”类型
    /// </summary>
    public bool isBecome = false;
}

/// <summary>
/// 转职费用附加值
/// </summary>
public class ClassChangeCostBuff : Buff
{
    public ClassChangeCostBuff(Card Giver, Skill Origin, int Value, BuffLastingTypeEnum LastingType = BuffLastingTypeEnum.Forever) : base(Giver, Origin, LastingType)
    {
        this.Value = Value;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value;

    /// <summary>
    /// 是否为“变为”类型
    /// </summary>
    public bool isBecome = false;
}

/// <summary>
/// 战斗力附加值
/// </summary>
public class PowerBuff : Buff
{
    public PowerBuff(Card Giver, Skill Origin, int Value, BuffLastingTypeEnum LastingType = BuffLastingTypeEnum.Forever) : base(Giver, Origin, LastingType)
    {
        this.Value = Value;
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
    public SupportBuff(Card Giver, Skill Origin, int Value, BuffLastingTypeEnum LastingType = BuffLastingTypeEnum.Forever) : base(Giver, Origin, LastingType)
    {
        this.Value = Value;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value;
}

/// <summary>
/// 势力附加值
/// </summary>
public class SymbolBuff : Buff
{
    public SymbolBuff(Card Giver, Skill Origin, bool IsAdding, SymbolEnum Value, BuffLastingTypeEnum LastingType = BuffLastingTypeEnum.Forever) : base(Giver, Origin, LastingType)
    {
        this.IsAdding = IsAdding;
        this.Value = Value;
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
    public WeaponBuff(Card Giver, Skill Origin, bool IsAdding, WeaponEnum Value, BuffLastingTypeEnum LastingType = BuffLastingTypeEnum.Forever) : base(Giver, Origin, LastingType)
    {
        this.IsAdding = IsAdding;
        this.Value = Value;
    }

    /// <summary>
    /// 是否为添加该武器
    /// </summary>
    public bool IsAdding;

    /// <summary>
    /// 附加值的值
    /// </summary>
    public WeaponEnum Value;
}

/// <summary>
/// 性别附加值
/// </summary>
public class GenderBuff : Buff
{
    public GenderBuff(Card Giver, Skill Origin, bool IsAdding, GenderEnum Value, BuffLastingTypeEnum LastingType = BuffLastingTypeEnum.Forever) : base(Giver, Origin, LastingType)
    {
        this.IsAdding = IsAdding;
        this.Value = Value;
    }

    /// <summary>
    /// 是否为添加该性别
    /// </summary>
    public bool IsAdding;

    /// <summary>
    /// 附加值的值
    /// </summary>
    public GenderEnum Value;
}

/// <summary>
/// 属性附加值
/// </summary>
public class TypeBuff : Buff
{
    public TypeBuff(Card Giver, Skill Origin, bool IsAdding, TypeEnum Value, BuffLastingTypeEnum LastingType = BuffLastingTypeEnum.Forever) : base(Giver, Origin, LastingType)
    {
        this.IsAdding = IsAdding;
        this.Value = Value;
    }

    /// <summary>
    /// 是否为添加该属性
    /// </summary>
    public bool IsAdding;

    /// <summary>
    /// 附加值的值
    /// </summary>
    public TypeEnum Value;
}

/// <summary>
/// 射程附加值
/// </summary>
public class RangeBuff : Buff
{
    public RangeBuff(Card Giver, Skill Origin, bool IsAdding, RangeEnum Value, BuffLastingTypeEnum LastingType = BuffLastingTypeEnum.Forever) : base(Giver, Origin, LastingType)
    {
        this.IsAdding = IsAdding;
        this.Value = Value;
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