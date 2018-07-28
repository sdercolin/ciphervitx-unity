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

    protected string guid;
    public string Guid { get; set; }
    public override string ToString()
    {
        Dictionary<string, dynamic> toSerialize = new Dictionary<string, dynamic>();
        toSerialize.Add("type", GetType().Name);
        toSerialize.Add("guid", Guid);
        toSerialize.Add("onlyAvailableWhenFrontShown", StringUtils.CreateFromAny(OnlyAvailableWhenFrontShown));
        toSerialize.Add("availableAreas", StringUtils.CreateFromAny(AvailableAreas));
        if (Giver != null)
        {
            toSerialize.Add("giver", Giver);
        }
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
    public static Buff CreateFromString(string json)
    {
        string[] splited = json.Trim(new char[] { '{', '}' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
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
        Type buffType = Assembly.GetExecutingAssembly().GetType(typename);
        List<object> parameters = new List<object>();
        List<string> parameterNames = new List<string>();
        Dictionary<string, dynamic> otherFields = new Dictionary<string, dynamic>();
        var constructorInfo = buffType.GetConstructors()[0];
        foreach (var param in constructorInfo.GetParameters())
        {
            parameterNames.Add(param.Name);
            parameters.Add(null);
        }

        foreach (var item in splited)
        {
            if (item.Contains("\"type\": \""))
            {
                continue;
            }
            string[] splited2 = item.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
            string name = splited2[0].Trim(new char[] { '\"' });
            object value = StringUtils.ParseAny(splited2[1]);
            if (parameterNames.Contains(name))
            {
                parameters[parameterNames.IndexOf(name)] = value;
            }
            else
            {
                otherFields.Add(name, value);
            }
        }
        var newBuff = Activator.CreateInstance(buffType, parameters) as Buff;
        foreach (var pair in otherFields)
        {
            typeof(Buff).GetField(pair.Key, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(newBuff, pair.Value);
        }
        return newBuff;
    }

    protected bool onlyAvailableWhenFrontShown;
    public bool OnlyAvailableWhenFrontShown { get; set; }
    protected List<Area> availableAreas;
    public List<Area> AvailableAreas { get; set; }
    protected Card giver;
    public Card Giver { get; set; }
    protected Card owner;
    public Card Owner { get; set; }

    protected Skill origin;
    public Skill Origin { get; set; }
    protected LastingTypeEnum lastingType;
    public LastingTypeEnum LastingType { get; set; }

    protected bool? isAdding;
    protected bool? isBecoming;
    protected dynamic value;

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
    public bool IsAdding { get { return (bool)isAdding; } set { isAdding = value; } }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public string Value { get { return value; } set { base.value = value; } }

    public override void Attached()
    {
        base.Attached();
        AvailableAreas = Owner.Controller.AllAreas;
    }
}

/// <summary>
/// 出击费用附加值
/// </summary>
public class DeployCostBuff : Buff
{
    public DeployCostBuff(Card giver, Skill origin, int value, bool isBecoming, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(giver, origin, lastingType)
    {
        Value = value;
        IsBecoming = isBecoming;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value { get { return value; } set { base.value = value; } }

    /// <summary>
    /// 是否为“变为”类型
    /// </summary>
    public bool IsBecoming { get { return (bool)isBecoming; } set { isBecoming = value; } }

    public override void Attached()
    {
        base.Attached();
        AvailableAreas = Owner.Controller.AllAreas;
    }
}

/// <summary>
/// 转职费用附加值
/// </summary>
public class ClassChangeCostBuff : Buff
{
    public ClassChangeCostBuff(Card giver, Skill origin, int value, bool isBecoming, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(giver, origin, lastingType)
    {
        Value = value;
        IsBecoming = isBecoming;
    }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public int Value { get { return value; } set { base.value = value; } }

    /// <summary>
    /// 是否为“变为”类型
    /// </summary>
    public bool IsBecoming { get { return (bool)isBecoming; } set { isBecoming = value; } }

    public override void Attached()
    {
        base.Attached();
        AvailableAreas = Owner.Controller.AllAreas;
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
    public int Value { get { return value; } set { base.value = value; } }
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
    public int Value { get { return value; } set { base.value = value; } }

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
    public bool IsAdding { get { return (bool)isAdding; } set { isAdding = value; } }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public SymbolEnum Value { get { return value; } set { base.value = value; } }
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
    public bool IsAdding { get { return (bool)isAdding; } set { isAdding = value; } }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public WeaponEnum Value { get { return value; } set { base.value = value; } }

    public override void Attached()
    {
        base.Attached();
        AvailableAreas = Owner.Controller.AllAreas;
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
    public bool IsAdding { get { return (bool)isAdding; } set { isAdding = value; } }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public GenderEnum Value { get { return value; } set { base.value = value; } }

    public override void Attached()
    {
        base.Attached();
        AvailableAreas = Owner.Controller.AllAreas;
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
    public bool IsAdding { get { return (bool)isAdding; } set { isAdding = value; } }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public TypeEnum Value { get { return value; } set { base.value = value; } }

    public override void Attached()
    {
        base.Attached();
        AvailableAreas = Owner.Controller.AllAreas;
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
    public bool IsAdding { get { return (bool)isAdding; } set { isAdding = value; } }

    /// <summary>
    /// 附加值的值
    /// </summary>
    public RangeEnum Value { get { return value; } set { base.value = value; } }
}