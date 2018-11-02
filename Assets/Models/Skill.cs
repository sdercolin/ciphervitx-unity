using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

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
    public User Controller => Owner.Controller;

    /// <summary>
    /// 控制者的对手
    /// </summary>
    public User Opponent => Owner.Controller.Opponent;

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

    public virtual bool Equals(IAttachable item)
    {
        return item == this;
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
        foreach (var card in Controller.AllCards)
        {
            foreach (var item in card.SubSkillList)
            {
                if (item is IUserForbidActionSkill)
                {
                    if (((IUserForbidActionSkill)item).ForbiddenSkillName == Name)
                    {
                        return false;
                    }
                }
            }
        }
        if (!Available)
        {
            return false;
        }
        if (UsedInThisTurn)
        {
            return false;
        }
        if (!Owner.IsOnField)
        {
            if (
                !(Controller.Bond.Contains(Owner) && Owner.FrontShown && TypeSymbols.Contains(SkillTypeSymbol.Bond))
                && !(TypeSymbols.Contains(SkillTypeSymbol.Special))
            )
            {
                return false;
            }
        }
        if (Keyword == SkillKeyword.CCS && (!Owner.IsClassChanged))
        {
            return false;
        }
        if (!CheckConditions())
        {
            return false;
        }
        Cost = DefineCost();
        return Cost.Check();
    }

    /// <summary>
    /// 解决能力
    /// </summary>
    public async Task Solve()
    {
        //Owner.Controller.Broadcast(new Message(MessageType.UseSkill, new System.Collections.ArrayList { this }));
        await Cost.Pay();
        await Do();
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
    public abstract Task Do();
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

    public bool Optional = false;

    /// <summary>
    /// 诱发
    /// </summary>
    public void Induce(Induction induction)
    {
        if (Game.InductionSetList.Count == 0)
        {
            Game.InductionSetList.Add(new List<Induction>());
        }
        List<Induction> inductionSet = Game.InductionSetList[Game.InductionSetList.Count - 1];
        inductionSet.Add(induction);
    }

    private void UnInduceAll()
    {
        foreach (var inductionSet in Game.InductionSetList)
        {
            inductionSet.RemoveAll(induction => induction.Skill == this);
        }
        Game.InductionSetList.RemoveAll(set => set.Count == 0);
    }

    /// <summary>
    /// 诱发状态
    /// </summary>
    public bool IsInduced => Inductions.Count > 0;

    /// <summary>
    /// 诱发列表
    /// </summary>
    public List<Induction> Inductions = new List<Induction>();

    /// <summary>
    /// 能力解决
    /// </summary>
    public async Task<bool> Solve(Induction induction)
    {
        Inductions.Remove(induction);
        Cost = DefineCost();
        if (CheckConditions(induction) && Cost.Check())
        {
            if (Optional)
            {
                if (!await Request.AskIfUse(this, Controller))
                {
                    return false;
                }
            }
            //Owner.Controller.Broadcast(new Message(MessageType.UseSkill, new System.Collections.ArrayList { this }));
            await Cost.Pay();
            await Do(induction);
            if (OncePerTurn)
            {
                UsedInThisTurn = true;
                Inductions.Clear();
                UnInduceAll();
            }
            return true;
        }
        return false;
    }

    public override void Read(Message message)
    {
        if (!Available || UsedInThisTurn)
        {
            return;
        }
        if (!TypeSymbols.Contains(SkillTypeSymbol.Special) && !Owner.IsOnField)
        {
            return;
        }
        if (Keyword == SkillKeyword.CCS && (!Owner.IsClassChanged))
        {
            return;
        }
        base.Read(message);
        var induction = CheckInduceConditions(message);
        if (induction != null)
        {
            induction.Message = message;
            induction.Skill = this;
            Induce(induction);
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
    public abstract Induction CheckInduceConditions(Message message);

    /// <summary>
    /// 判断实行条件
    /// </summary>
    /// <returns>若满足实行条件，则返回true</returns>
    public abstract bool CheckConditions(Induction induction);

    /// <summary>
    /// 定义费用
    /// </summary>
    public abstract Cost DefineCost();
    public Cost Cost;

    /// <summary>
    /// 能力实行
    /// </summary>
    public abstract Task Do(Induction induction);


}

public class Induction
{
    public AutoSkill Skill;
    public Message Message;
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
            if (item.Owner != null)
            {
                item.Detach();
            }
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
        if (Keyword == SkillKeyword.CCS && (!Owner.IsClassChanged))
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
                CleanItemsToApply();
                if (ItemsToApply.Count > 0)
                {
                    Attach(card, ItemsToApply);
                }
            }
            else if (!CanTarget(card) && Targets.Contains(card))
            {
                Detach(card);
            }
            else if (CanTarget(card) && Targets.Contains(card))
            {
                ItemsToApply.Clear();
                SetItemToApply();
                CleanItemsToApply();
                if (CheckItemUpdated(card))
                {
                    Detach(card);
                    Attach(card, ItemsToApply);
                }
            }
        }
    }

    private bool CheckItemUpdated(Card card)
    {
        if (ItemsApplied[card].Length != ItemsToApply.Count)
        {
            return true;
        }
        for (int i = 0; i < ItemsToApply.Count; i++)
        {
            if (!ItemsToApply[i].Equals(ItemsApplied[card][i]))
            {
                return true;
            }
        }
        return false;
    }

    public override bool Try(Message message, ref Message substitute)
    {
        return base.Try(message, ref substitute);
    }

    private void CleanItemsToApply()
    {
        ItemsToApply.RemoveAll(item =>
        {
            var powerBuff = item as PowerBuff;
            if (powerBuff != null)
            {
                if (powerBuff.Value == 0)
                {
                    return true;
                }
            }
            var supportBuff = item as SupportBuff;
            if (supportBuff != null)
            {
                if (supportBuff.Value == 0)
                {
                    return true;
                }
            }
            return false;
        });
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
    HS, //英雄技
    TS, //双子技
    IS, //连发技
}