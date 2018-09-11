using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMainController
{
    public static void ReadGameMessage(Message message)
    {
        DeployMessage deployMessage = message as DeployMessage;
        if (deployMessage != null)
        {
            //todo
            return;
        }
    }

    private static string GetImageNameOfCard(Card card)
    {
        return card.Serial;
    }

    private static bool IsControlByPlayer(Card card)
    {
        return card.Controller == Game.Player;
    }
}
