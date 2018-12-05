using System;
using System.Collections.Generic;

public class RPSItem : IChoosable
{
    public static List<RPSItem> CreateRPSSet()
    {
        return new List<RPSItem>{
            new RPSItem(RPSValue.Rock),
            new RPSItem(RPSValue.Paper),
            new RPSItem(RPSValue.Scissors)
        };
    }

    RPSItem(RPSValue value)
    {
        Value = value;
    }

    public RPSValue Value;

    public string GetDescription(DescriptionPattern descriptionOption = DescriptionPattern.Default)
    {
        return Value.ToString("G");
    }

    public string GetImagePath()
    {
        throw new NotImplementedException();
    }

    public enum RPSValue
    {
        Rock = 0,
        Scissors = 1,
        Paper = 2
    }
}

