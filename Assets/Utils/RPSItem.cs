using System;
using System.Collections.Generic;

public class RPSITem : IChoosable
{
    public static List<RPSITem> CreateRPSSet()
    {
        return new List<RPSITem>{
            new RPSITem(RPSValue.Rock),
            new RPSITem(RPSValue.Paper),
            new RPSITem(RPSValue.Scissors)
        };
    }

    RPSITem(RPSValue value)
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

