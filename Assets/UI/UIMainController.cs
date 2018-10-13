using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Threading.Tasks;

public class UIMainController : MonoBehaviour
{
    public static UIMainController GetUIMainController()
    {
        GameObject go = GameObject.Find("MainCamera");
        return go.GetComponent<UIMainController>();
    }

    [SerializeField]
    private RequesterBase requester;

    [SerializeField]
    private SinglePlayerField myField;

    [SerializeField]
    private SinglePlayerField enemyField;

    public void SetRequester(RequesterBase r)
    {
        requester = r;
    }

    public void ReadGameMessage(Message message)
    {
        MultipleMessage multipleMessage = message as MultipleMessage;
        if (multipleMessage != null)
        {
            foreach (Message msg in multipleMessage.Elements)
            {
                ReadGameMessage(msg);
            }
            return;
        }

        SetDeckMessage setDeckMessage = message as SetDeckMessage;
        if (setDeckMessage != null)
        {
            int size = setDeckMessage.User.Deck.Count;
            var field = IsPlayer(setDeckMessage.User) ? myField : enemyField;
            field.SetDeckSize(size);
            return;
        }

        DeployMessage deployMessage = message as DeployMessage;
        if (deployMessage != null)
        {
            int n = deployMessage.Targets.Count;
            for (int i = 0; i < n; i++)
            {
                Card card = deployMessage.Targets[i];
                var field = BelongField(card);
                bool toFront = deployMessage.ToFrontFieldList[i];
                bool actioned = deployMessage.ActionedList[i];
                field.DeployCardToBattleField(card, toFront, actioned);
            }
            return;
        }

        LevelUpMessage levelUpMessage = message as LevelUpMessage;
        if (levelUpMessage != null)
        {
            var field = BelongField(levelUpMessage.Target);
            field.UpdateCardOfBattleField(levelUpMessage.BaseUnit, levelUpMessage.Target);
            return;
        }

        MoveMessage moveMessage = message as MoveMessage;
        if (moveMessage != null)
        {
            foreach (Card card in moveMessage.Targets)
            {
                var field = BelongField(card);
                field.MoveCardOfBattleField(card, card.BelongedRegion == card.Controller.FrontField);
            }
            return;
        }

        ReverseBondMessage reverseBondMessage = message as ReverseBondMessage;
        if (reverseBondMessage != null)
        {
            foreach (Card card in reverseBondMessage.Targets)
            {
                var field = BelongField(card);
                field.ReverseBond(card);
            }
            return;
        }

        ToBondMessage toBondMessage = message as ToBondMessage;
        if (toBondMessage != null)
        {
            foreach (Card card in toBondMessage.Targets)
            {
                var field = BelongField(card);
                field.AddBond(card);
                if (!toBondMessage.TargetFrontShown)
                {
                    field.ReverseBond(card);
                }
            }
            return;
        }

        SetSupportMessage setSupportMessage = message as SetSupportMessage;
        if (setSupportMessage != null)
        {
            //todo
            return;
        }

        SendToRetreatMessage sendToRetreatMessage = message as SendToRetreatMessage;
        if (sendToRetreatMessage != null)
        {
            //todo find target cards from any area?
            return;
        }
    }

    private SinglePlayerField BelongField(Card card)
    {
        return IsControlByPlayer(card) ? myField : enemyField;
    }

    private static string GetImageNameOfCard(Card card)
    {
        return card.Serial;
    }

    private static bool IsControlByPlayer(Card card)
    {
        return card.Controller == Game.Player;
    }

    private static bool IsPlayer(User user)
    {
        return user == Game.Player;
    }


    //request
    public async Task<List<T>> RequestChoose<T>(List<T> choices, int min, int max, string desc)
    {
        List<T> ret;
        if (requester != null)
        {
            requester.gameObject.SetActive(true);
            ret = await requester.Request<T>(choices, min, max, desc);
            requester.gameObject.SetActive(false);
        }
        else
        {
            ret = new List<T>();
            for (int i = 0; i < min; i++)
            {
                ret.Add(choices[i]);
            }
        }
        return ret;
    }

    public async Task<bool> RequestChooseBool(string desc)
    {
        List<bool> choices = new List<bool>()
        {
            true,
            false
        };

        List<bool> result = await RequestChoose<bool>(choices, 1, 1, desc);
        return result[0];
    }
}
