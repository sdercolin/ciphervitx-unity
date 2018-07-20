using System;
using System.Collections.Generic;

public partial class Message
{
    /// <summary>
    /// 是否是自己发送的消息
    /// </summary>
    public bool SendBySelf = true;

    public virtual Message Clone()
    {
        Type messageType = GetType();
        Message clone = Activator.CreateInstance(messageType) as Message;
        clone.reasonCard = reasonCard;
        clone.Reason = Reason;
        clone.SendBySelf = SendBySelf;
        clone.Targets = ListUtils.Clone(Targets);
        clone.Value = Value;
        return clone;
    }

    public virtual void Do() { }

    /// <summary>
    /// 完成基础字段的序列化
    /// </summary>
    /// <returns>Json字符串</returns>
    public override string ToString()
    {
        string json = "";
        json += "\"Targets\": \"" + ListUtils.ToString(Targets) + "\"";
        json += ", \"Value\": \"" + Value.ToString() + "\"";
        json += ", \"ReasonCard\": " + reasonCard == null ? "null" : "\"" + reasonCard.ToString() + "\"";
        json += ", \"Reason\": " + Reason == null ? "null" : "\"" + Reason.ToString() + "\"";
        return "{" + json + "}";
    }

    public List<Card> Targets = new List<Card>();
    public int? Value = null;
    protected Card reasonCard = null;
    public Card ReasonCard
    {
        set
        {
            reasonCard = value;
        }
        get
        {
            if (Reason != null && reasonCard == null)
            {
                return Reason.Owner;
            }
            else
            {
                return reasonCard;
            }
        }
    }
    public Skill Reason = null;

    public bool TrueForAnyTarget(Predicate<Card> predicate)
    {
        foreach (Card card in Targets)
        {
            if (predicate(card))
            {
                return true;
            }
        }
        return false;
    }

    public bool TrueForAllTargets(Predicate<Card> predicate)
    {
        foreach (Card card in Targets)
        {
            if (!predicate(card))
            {
                return false;
            }
        }
        return true;
    }

    public List<Card> GetTargetsTrueFor(Predicate<Card> predicate)
    {
        List<Card> results = new List<Card>();
        foreach (Card card in Targets)
        {
            if (predicate(card))
            {
                results.Add(card);
            }
        }
        return results;
    }
}

