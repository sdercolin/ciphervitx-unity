using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 附加能力
/// </summary>
public abstract class SubSkill : Skill
{
    public SubSkill(Skill origin, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base()
    {
        Origin = origin;
        LastingType = lastingType;
    }

    /// <summary>
    /// 产生该附加能力的能力
    /// </summary>
    public Skill Origin;

    /// <summary>
    /// 持续类型
    /// </summary>
    public LastingTypeEnum LastingType;

    private static int fieldNumber = 10;
    protected dynamic field1 = null;
    protected dynamic field2 = null;
    protected dynamic field3 = null;
    protected dynamic field4 = null;
    protected dynamic field5 = null;

    public override string ToString()
    {
        Dictionary<string, dynamic> toSerialize = new Dictionary<string, dynamic>();
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
        for (int i = 0; i < fieldNumber; i++)
        {
            dynamic field = GetType().GetField("field" + (i + 1).ToString(), BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            if (field != null)
            {
                toSerialize.Add("field" + (i + 1).ToString(), StringUtils.CreateFromAny(field));
            }
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

    protected virtual void Detaching() { }

    public override void Detach()
    {
        Detaching();
        Owner.AttachableList.Remove(this);
        Owner = null;
    }

    public override void Read(Message message)
    {
        if (OnlyAvailableWhenFrontShown)
        {
            if (!Owner.FrontShown)
            {
                Detach();
                return;
            }
        }
    }
}

/// <summary>
/// 无效能力
/// </summary>
public class DisableSkill : SubSkill
{
    public DisableSkill(Skill origin, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType) { }

    Skill Target { get => field1; set => field1 = value; }

    public override void Attached()
    {
        base.Attached();
        Target.Available = false;
    }

    protected override void Detaching()
    {
        base.Detaching();
        Target.Available = true;
    }
}

/// <summary>
/// 无效全部能力
/// </summary>
public class DisableAllSkills : SubSkill
{
    public DisableAllSkills(Skill origin, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType) { }

    public override void Attached()
    {
        base.Attached();
        Owner.SkillList.ForEach(skill => skill.Available = false);
    }

    protected override void Detaching()
    {
        base.Detaching();
        Owner.SkillList.ForEach(skill => skill.Available = true);
    }
}

/// <summary>
/// 不能被放置到羁绊区
/// </summary>
public class CanNotBePlacedInBond : SubSkill, IForbidPosition
{
    public CanNotBePlacedInBond(Skill origin, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType) { }

    public List<Type> ForbiddenAreaTypes { get => new List<Type>() { typeof(Bond) }; }

    public override bool Try(Message message, ref Message substitute)
    {
        if (message is ToBondMessage)
        {
            var toBondMessage = message as ToBondMessage;
            if (toBondMessage.Targets.Contains(Owner))
            {
                substitute = toBondMessage.Clone();
                ((ToBondMessage)substitute).Targets.Remove(Owner);
                return false;
            }
        }
        if (message is ReadyToBondMessage)
        {
            var readyToBondMessage = message as ReadyToBondMessage;
            if (readyToBondMessage.Targets.Contains(Owner))
            {
                substitute = readyToBondMessage.Clone();
                ((ReadyToBondMessage)substitute).Targets.Remove(Owner);
                return false;
            }
        }
        return true;
    }
}

/// <summary>
/// 不能被神速回避
/// </summary>
public class CanNotBeAvoided : SubSkill
{
    public CanNotBeAvoided(Skill origin, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType) { }

    public override bool Try(Message message, ref Message substitute)
    {
        if (message is AvoidMessage)
        {
            if ((message as AvoidMessage).AttackingUnit == Owner)
            {
                substitute = new EmptyMessage();
                return false;
            }
        }
        else if (message is ReadyToAvoidMessage)
        {
            if ((message as ReadyToAvoidMessage).AttackingUnit == Owner)
            {
                substitute = new EmptyMessage();
                return false;
            }
        }
        return true;
    }
}