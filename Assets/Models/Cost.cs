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
        List<Card> choices = new List<Card>();
        foreach (var card in Reason.Controller.Bond.UnusedBonds)
        {
            if (Condition(card))
            {
                choices.Add(card);
            }
        }
        var readyToUseBondMessage = Game.TryMessage(new UseBondMessage()
        {
            Reason = Reason,
            Targets = choices,
        }) as UseBondMessage;
        if (readyToUseBondMessage != null)
        {
            if ((readyToUseBondMessage as UseBondMessage).Targets.Count >= Number)
            {
                Choices = (readyToUseBondMessage as UseBondMessage).Targets;
                return true;
            }
        }
        return false;
    }

    public override void Pay()
    {
        var targets = Request.Choose(Choices, Number, Reason.Controller);
        Reason.Controller.UseBond(targets, Reason);
    }
}
