using System;
using System.Collections.Generic;
using System.Reflection;
/// <summary>
/// 所有附加值的基类
/// </summary>
public abstract class Buff : IAttachable
{
    /// <summary>
    /// 附加值的构造函数
    /// </summary>
    /// <param name="origin">产生该附加值的能力</param>
    /// <param name="lastingType">持续类型</param>
    public Buff(Skill origin, LastingTypeEnum lastingType)
    {
        Origin = origin;
        LastingType = lastingType;
        Guid = System.Guid.NewGuid().ToString();
    }

    protected string guid;
    public string Guid { get; set; }
    public override string ToString()
    {
        var toSerialize = new Dictionary<string, dynamic>();
        toSerialize.Add("type", GetType().Name);
        toSerialize.Add("guid", Guid);
        toSerialize.Add("onlyAvailableWhenFrontShown", StringUtils.CreateFromAny(OnlyAvailableWhenFrontShown));
        if (Owner != null)
        {
            toSerialize.Add("owner", Owner);
        }
        if (Origin != null)
        {
            toSerialize.Add("origin", Origin);
        }
        toSerialize.Add("lastingType", LastingType);
        if (isAdding != null)
        {
            toSerialize.Add("isAdding", isAdding);
        }
        if (isBecoming != null)
        {
            toSerialize.Add("isBecoming", isBecoming);
        }
        if (value != null)
        {
            toSerialize.Add("value", value);
        }
        string json = String.Empty;
        foreach (var pair in toSerialize)
        {
            if (String.IsNullOrEmpty(json))
            {
                json += ", ";
            }
            json += "\"" + pair.Key + "\": " + StringUtils.CreateFromAny(pair.Value);
        }
        return "{" + json + "}";
    }
    public bool OnlyAvailableWhenFrontShown { get; set; } = true;

    protected Card owner;
    public Card Owner { get; set; }

    protected Skill origin;
    public Skill Origin { get; set; }
    protected LastingTypeEnum lastingType;
    public LastingTypeEnum LastingType { get; set; }

    protected bool? isAdding = null;
    protected bool? isBecoming = null;
    protected dynamic value = null;

    public virtual void Attached() { }

    public void Detach()
    {
        Owner.AttachableList.Remove(this);
        Owner = null;
    }

    public void Read(Message message)
    {
        if (Owner == null)
        {
            return;
        }
        if (OnlyAvailableWhenFrontShown)
        {
            if (!Owner.FrontShown)
            {
                Detach();
                return;
            }
        }
    }

    public bool Try(Message message, ref Message substitute)
    {
        return true;
    }

    public bool Equals(IAttachable item)
    {
        var buffItem = item as Buff;
        if (buffItem == null)
        {
            return false;
        }
        if (buffItem.GetType() != GetType())
        {
            return false;
        }
        return LastingType == buffItem.LastingType
            && isAdding == buffItem.isAdding
            && isBecoming == buffItem.isBecoming
            && value == buffItem.value;
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
    public UnitNameBuff(Skill origin, bool isAdding, string value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType)
    {
        IsAdding = isAdding;
        Value = value;
    }

    /// <summary>
    /// 是否为添加该单位名
    /// </summary>
    public bool IsAdding { get { return (bool)isAdding; } set { isAdding = value; } }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public string Value { get { return value; } set { this.value = value; } }
}

/// <summary>
/// 出击费用附加值
/// </summary>
public class DeployCostBuff : Buff
{
    public DeployCostBuff(Skill origin, int value, bool isBecoming, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType)
    {
        Value = value;
        IsBecoming = isBecoming;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value { get { return value; } set { this.value = value; } }

    /// <summary>
    /// 是否为“变为”类型
    /// </summary>
    public bool IsBecoming { get { return (bool)isBecoming; } set { isBecoming = value; } }
}

/// <summary>
/// 转职费用附加值
/// </summary>
public class ClassChangeCostBuff : Buff
{
    public ClassChangeCostBuff(Skill origin, int value, bool isBecoming, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType)
    {
        Value = value;
        IsBecoming = isBecoming;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value { get { return value; } set { this.value = value; } }

    /// <summary>
    /// 是否为“变为”类型
    /// </summary>
    public bool IsBecoming { get { return (bool)isBecoming; } set { isBecoming = value; } }
}

/// <summary>
/// 战斗力附加值
/// </summary>
public class PowerBuff : Buff
{
    public PowerBuff(Skill origin, int value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType)
    {
        Value = value;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value { get { return value; } set { this.value = value; } }
}

/// <summary>
/// 支援力附加值
/// </summary>
public class SupportBuff : Buff
{
    public SupportBuff(Skill origin, int value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType)
    {
        Value = value;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value { get { return value; } set { this.value = value; } }
}

/// <summary>
/// 势力附加值
/// </summary>
public class SymbolBuff : Buff
{
    public SymbolBuff(Skill origin, bool isAdding, SymbolEnum value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType)
    {
        IsAdding = isAdding;
        Value = value;
    }

    /// <summary>
    /// 是否为添加该势力
    /// </summary>
    public bool IsAdding { get { return (bool)isAdding; } set { isAdding = value; } }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public SymbolEnum Value { get { return value; } set { this.value = value; } }
}

/// <summary>
/// 武器附加值
/// </summary>
public class WeaponBuff : Buff
{
    public WeaponBuff(Skill origin, bool isAdding, WeaponEnum value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType)
    {
        IsAdding = isAdding;
        Value = value;
    }

    /// <summary>
    /// 是否为添加该武器
    /// </summary>
    public bool IsAdding { get { return (bool)isAdding; } set { isAdding = value; } }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public WeaponEnum Value { get { return value; } set { this.value = value; } }
}

/// <summary>
/// 性别附加值
/// </summary>
public class GenderBuff : Buff
{
    public GenderBuff(Skill origin, bool isAdding, GenderEnum value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType)
    {
        IsAdding = isAdding;
        Value = value;
    }

    /// <summary>
    /// 是否为添加该性别
    /// </summary>
    public bool IsAdding { get { return (bool)isAdding; } set { isAdding = value; } }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public GenderEnum Value { get { return value; } set { this.value = value; } }
}

/// <summary>
/// 属性附加值
/// </summary>
public class TypeBuff : Buff
{
    public TypeBuff(Skill origin, bool isAdding, TypeEnum value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType)
    {
        IsAdding = isAdding;
        Value = value;
    }

    /// <summary>
    /// 是否为添加该属性
    /// </summary>
    public bool IsAdding { get { return (bool)isAdding; } set { isAdding = value; } }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public TypeEnum Value { get { return value; } set { this.value = value; } }
}

/// <summary>
/// 射程附加值
/// </summary>
public class RangeBuff : Buff
{
    public RangeBuff(Skill origin, bool isAdding, RangeEnum value, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType)
    {
        IsAdding = isAdding;
        Value = value;
    }

    /// <summary>
    /// 是否为添加该射程
    /// </summary>
    public bool IsAdding { get { return (bool)isAdding; } set { isAdding = value; } }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public RangeEnum Value { get { return value; } set { this.value = value; } }
}