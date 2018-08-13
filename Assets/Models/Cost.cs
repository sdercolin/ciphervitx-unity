using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Cost
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

    public virtual void Pay() { }


    #region 工厂方法
    public static Cost Null = new Cost(null);

    public static Cost UseBondCost(Skill reason, int number, Predicate<Card> condition = null)
    {
        return new UseBondCost(reason, number, condition);
    }
    #endregion
}

class UseBondCost : Cost
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

    public async override void Pay()
    {
        var targets = await Request.Choose(Choices, Number, Reason.Controller);
        Reason.Controller.UseBond(targets, Reason);
    }
}
