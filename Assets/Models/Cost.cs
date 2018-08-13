using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Cost
{
    public Skill Reason;
    public List<Card> Choices;

    public Cost(Skill reason)
    {
        Reason = reason;
        Choices = new List<Card>();
    }

    public abstract bool Check();

    public abstract Task Pay();


    #region 工厂方法
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

    public static Cost Null = new NullCost(null);

    public static Cost UseBondCost(Skill reason, int number, Predicate<Card> condition = null)
    {
        return new UseBondCost(reason, number, condition);
    }

    public static Cost ActionCost(Skill reason)
    {
        return new ActionCost(reason);
    }

    public static Cost ActionOthersCost(Skill reason, int number, Predicate<Card> condition = null)
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

public class UseBondCost : Cost
{
    public int Number;
    public Predicate<Card> Condition;

    public UseBondCost(Skill reason, int number, Predicate<Card> condition = null) : base(reason)
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
        Choices = Reason.Controller.Bond.UnusedBonds.FindAll(card => Condition(card) && card.CheckUseBond(Reason));
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
        var targets = await Request.Choose(Choices, Number, Reason.Controller);
        Reason.Controller.UseBond(targets, Reason);
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
        throw new NotImplementedException();
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
        return Reason.Controller.Field.Filter(card => !card.IsHorizontal && Condition(card)).Count >= Number;
    }

    public override Task Pay()
    {
        throw new NotImplementedException();
    }
}