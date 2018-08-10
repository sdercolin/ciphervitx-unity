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
        ReadyToUseBondMessage readyToUseBondMessage = new ReadyToUseBondMessage()
        {
            Reason = Reason,
            Targets = choices,
        };
        Message substitute = readyToUseBondMessage.Clone();
        Game.BroadcastTry(readyToUseBondMessage, ref substitute);
        if (substitute is ReadyToUseBondMessage)
        {
            if ((substitute as ReadyToUseBondMessage).Targets.Count >= Number)
            {
                Choices = (substitute as ReadyToUseBondMessage).Targets;
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
