using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 费用类
/// 定义一个Skill所需要支付的费用
/// </summary>
public abstract class Cost
{
    /// <summary>
    /// 使用该费用的Skill
    /// </summary>
    public Skill Reason;

    /// <summary>
    /// 可以被该费用选择的卡
    /// </summary>
    protected List<Card> Choices;

    public Cost(Skill reason)
    {
        Reason = reason;
        Choices = new List<Card>();
    }

    /// <summary>
    /// 检查是否满足可支付的条件
    /// </summary>
    public abstract bool Check();

    /// <summary>
    /// 执行支付过程（可能被覆写为async Task）
    /// </summar
    public abstract Task Pay();


    #region 工厂方法
    /// <summary>
    /// “+”的重载，用于创建复合费用
    /// </summary>
    public static Cost operator +(Cost a, Cost b)
    {
        List<Cost> newElements = new List<Cost>();
        if (a is MultipleCost)
        {
            if (b is MultipleCost)
            {
                newElements = ListUtils.Combine(((MultipleCost)a).Elements, ((MultipleCost)b).Elements);
            }
            else
            {
                newElements.AddRange(((MultipleCost)a).Elements);
                newElements.Add(b);
            }
        }
        else if (b is MultipleCost)
        {
            newElements.Add(a);
            newElements.AddRange(((MultipleCost)b).Elements);
        }
        else
        {
            newElements.Add(a);
            newElements.Add(b);
        }
        return new MultipleCost(a.Reason, newElements.ToArray());
    }

    /// <summary>
    /// 无费用
    /// </summary>
    public static Cost Null = new NullCost(null);

    /// <summary>
    /// 翻面羁绊卡
    /// </summary>
    /// <param name="reason">使用该费用的能力</param>
    /// <param name="number">要求翻面的数量</param>
    /// <param name="condition">对翻面的卡的要求，如果没有要求可以省略</param>
    /// <returns></returns>
    public static Cost ReverseBond(Skill reason, int number, Predicate<Card> condition = null)
    {
        return new ReverseBondCost(reason, number, condition);
    }

    /// <summary>
    /// 横置
    /// </summary>
    /// <param name="reason">使用该费用的能力</param>
    /// <returns></returns>
    public static Cost Action(Skill reason)
    {
        return new ActionCost(reason);
    }

    /// <summary>
    /// 将其余我方单位转为已行动状态
    /// </summary>
    /// <param name="reason">使用该费用的能力</param>
    /// <param name="number">要求横置的卡的数量</param>
    /// <param name="condition">对横置的卡的要求，如果没有要求可以省略</param>
    /// <returns></returns>
    public static Cost ActionOthers(Skill reason, int number, Predicate<Card> condition = null)
    {
        return new ActionOthersCost(reason, number, condition);
    }
    #endregion
}

public class NullCost : Cost
{
    public NullCost(Skill reason) : base(reason) { }

    public override bool Check()
    {
        return true;
    }

    public override Task Pay()
    {
        return Task.CompletedTask;
    }
}

public class MultipleCost : Cost
{
    public MultipleCost(Skill reason, params Cost[] elements) : base(reason)
    {
        Elements.AddRange(elements);
    }

    public List<Cost> Elements = new List<Cost>();

    public override bool Check()
    {
        return Elements.TrueForAll(element => element.Check());
    }

    public override async Task Pay()
    {
        foreach (var element in Elements)
        {
            await element.Pay();
        }
    }
}

public class ReverseBondCost : Cost
{
    public int Number;
    public Predicate<Card> Condition;

    public ReverseBondCost(Skill reason, int number, Predicate<Card> condition = null) : base(reason)
    {
        Number = number;
        if (condition == null)
        {
            Condition = card => true;
        }
        else
        {
            Condition = condition;
        }
    }

    public override bool Check()
    {
        Choices = Reason.Controller.Bond.UnusedBonds.FindAll(card => Condition(card) && card.CheckReverseBond(Reason));
        if (Choices.Count >= Number)
        {
            return true;
        }
        else
        {
            Choices.Clear();
            return false;
        }
    }

    public async override Task Pay()
    {
        await Reason.Controller.ChooseReverseBond(Choices, Number, Number, Reason);
    }
}

public class ActionCost : Cost
{
    public ActionCost(Skill reason) : base(reason) { }

    public override bool Check()
    {
        return !Reason.Owner.IsHorizontal;
    }
    public override Task Pay()
    {
        Reason.Controller.SetActioned(new List<Card> { Reason.Owner }, Reason);
        return Task.CompletedTask;
    }
}

public class ActionOthersCost : Cost
{
    public int Number;
    public Predicate<Card> Condition;

    public ActionOthersCost(Skill reason, int number, Predicate<Card> condition = null) : base(reason)
    {
        Number = number;
        if (condition == null)
        {
            Condition = card => true;
        }
        else
        {
            Condition = condition;
        }
    }

    public override bool Check()
    {
        Choices = Reason.Controller.Field.Filter(card => !card.IsHorizontal && Condition(card));
        if (Choices.Count >= Number)
        {
            return true;
        }
        else
        {
            Choices.Clear();
            return false;
        }
    }

    public async override Task Pay()
    {
        await Reason.Controller.ChooseSetActioned(Choices, Number, Number, Reason);
    }
}