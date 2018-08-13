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

    public virtual bool Check()
    {
        return true;
    }

    public abstract Task Pay();


    #region 工厂方法
    public static Cost Null = new NullCost(null);

    public static Cost UseBondCost(Skill reason, int number, Predicate<Card> condition = null)
    {
        return new UseBondCost(reason, number, condition);
    }
    #endregion
}

public class NullCost : Cost
{
    public NullCost(Skill reason) : base(reason) { }

    public override Task Pay()
    {
        return Task.CompletedTask;
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
