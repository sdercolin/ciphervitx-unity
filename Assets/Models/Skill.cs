using System;
using System.Collections.Generic;
using System.Reflection;

public abstract class Skill : IAttachable
{
    public string Guid { get; set; }
    public override string ToString()
    {
        return "{\"guid\": \"" + Guid + "\" }";
    }

    /// <summary>
    /// 持有该能力的卡
    /// </summary>
    public Card Owner { get; set; }

    /// <summary>
    /// 控制者
    /// </summary>
    public User Controller { get { return Owner.Controller; } }

    /// <summary>
    /// 控制者的对手
    /// </summary>
    public User Opponent { get { return Owner.Controller.Opponent; } }

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

    public Skill()
    {
        Guid = System.Guid.NewGuid().ToString();
    }

    public bool OnlyAvailableWhenFrontShown { get; set; } = true;

    public virtual void Attached() { }
    public virtual void Detach() { }

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
///     2.费用：写在Cost函数，返回Cost类对象
///     3.效果：写在Do函数中
/// </summary>
public abstract class ActionSkill : Skill
{
    public ActionSkill() : base() { }

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
            Cost = DefineCost();
            return Cost.Check();
        }
    }

    /// <summary>
    /// 解决能力
    /// </summary>
    public void Solve()
    {
        //Owner.Controller.Broadcast(new Message(MessageType.UseSkill, new System.Collections.ArrayList { this }));
        Cost.Pay();
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
    /// 定义费用
    /// </summary>
    public abstract Cost DefineCost();
    public Cost Cost;

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
///     3.费用：写在Cost函数，返回Cost类对象
///     4.效果：写在Do函数中，实行条件与费用检查都通过时，实行该能力的效果，若为选发效果，也可以选择不发，此时停止解决该能力，视为本回合未使用过该能力
/// </summary>
public abstract class AutoSkill : Skill
{
    public AutoSkill() : base() { }

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
            if (Game.InducedSkillSetList.Count == 0)
            {
                Game.InducedSkillSetList.Add(new List<AutoSkill>());
            }
            List<AutoSkill> skillSet = Game.InducedSkillSetList[Game.InducedSkillSetList.Count - 1];
            skillSet.Add(this);
        }
    }

    private void UnInduceAll()
    {
        foreach (var skillSet in Game.InducedSkillSetList)
        {
            skillSet.RemoveAll(skill => skill == this);
        }
        Game.InducedSkillSetList.RemoveAll(set => set.Count == 0);
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
    public bool Solve()
    {
        InducedCount--;
        Cost = DefineCost();
        if (CheckConditions() && Cost.Check())
        {
            if (Optional)
            {
                if (!Request.AskIfUse(this, Controller))
                {
                    return false;
                }
            }
            //Owner.Controller.Broadcast(new Message(MessageType.UseSkill, new System.Collections.ArrayList { this }));
            Cost.Pay();
            Do();
            if (OncePerTurn)
            {
                UsedInThisTurn = true;
                InducedCount = 0;
                UnInduceAll();
            }
            return true;
        }
        return false;
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
    /// 定义费用
    /// </summary>
    public abstract Cost DefineCost();
    public Cost Cost;

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
    public PermanentSkill() : base() { }

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
        Targets.Add(target);
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
        if (!Available)
        {
            DetachAll();
            return;
        }
        if (!TypeSymbols.Contains(SkillTypeSymbol.Special) && !Owner.IsOnField)
        {
            DetachAll();
            return;
        }
        foreach (Card card in Game.AllCards)
        {
            if (CanTarget(card) && !Targets.Contains(card))
            {
                ItemsToApply.Clear();
                SetItemToApply();
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
    public abstract void SetItemToApply();
}

/// <summary>
/// 支援型能力
/// </summary>
public abstract class SupportSkill : Skill
{
    public SupportSkill() : base() { }

    /// <summary>
    /// 支援能力种类
    /// </summary>
    public SupportSkillType Type;

    public bool Optional = false;

    /// <summary>
    /// 能力解决，在流程中被调用
    /// </summary>
    /// <param name="AttackingUnit">攻击单位</param>
    /// <param name="AttackedUnit">被攻击单位</param>
    public void Solve(Card AttackingUnit, Card AttackedUnit)
    {
        Cost = DefineCost();
        if (CheckConditions(AttackingUnit, AttackedUnit) && Cost.Check())
        {
            if (Optional)
            {
                if (!Request.AskIfUse(this, Controller))
                {
                    return;
                }
            }
            //Owner.Controller.Broadcast(new Message(MessageType.UseSkill, new System.Collections.ArrayList { this }));
            Cost.Pay();
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

    /// <summary>
    /// 定义费用
    /// </summary>
    public abstract Cost DefineCost();
    public Cost Cost;
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
    public DisableSkill(Skill origin, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType) { }

    Skill Target { get { return field1; } set { field1 = value; } }

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
public class CanNotBePlacedInBond : SubSkill
{
    public CanNotBePlacedInBond(Skill origin, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType) { }

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