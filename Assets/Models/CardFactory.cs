﻿using System;
using System.Reflection;

public static class CardFactory
{
    public static Card CreateCard(int serial, User controller)
    {
        try
        {
            return Activator.CreateInstance(Assembly.GetExecutingAssembly().GetType("Card" + serial.ToString("00000")), controller) as Card;
        }
        catch
        {
            return null;
        }
    }
}
