using System;
using System.Collections.Generic;

public static class Game
{
    public static void Initialize()
    {
        Player = new Player();
        Rival = new Rival();
    }

    public static Player Player;
    public static Rival Rival;

    //回合信息
    public static User TurnPlayer;
    public static int TurnCount = 0;
    public static Phase CurrentPhase;

    //战斗用
    public static Card AttackingUnit = null; //攻击单位
    public static Card DefencingUnit = null; //防御单位
    public static List<Card> BattlingUnits { get { return new List<Card> { AttackingUnit, DefencingUnit }; } } //战斗单位
    public static int PowerUpByCritical = 0; //必杀攻击增加的战斗力
    public static int PowerUpBySupport = 0; //支援增加的战斗力

    public static List<Card> AllCards
    {
        get
        {
            List<Card> allCards = new List<Card>();
            ForEachCard(card => allCards.Add(card));
            return allCards;
        }
    }

    public static List<Area> AllAreas
    {
        get
        {
            List<Area> allAreas = new List<Area>();
            allAreas.AddRange(Player.AllAreas);
            allAreas.AddRange(Rival.AllAreas);
            return allAreas;
        }
    }

    public static void ForEachCard(Action<Card> action)
    {
        Player.ForEachCard(action);
        Rival.ForEachCard(action);
    }

    public static bool TrueForAllCard(Predicate<Card> predicate)
    {
        return Player.TrueForAllCard(predicate) && Rival.TrueForAllCard(predicate);
    }

    public static Card GetCardByGuid(string guid)
    {
        foreach (Card card in AllCards)
        {
            if (card.Guid == guid)
            {
                return card;
            }
        }
        return null;
    }

    public static Area GetAreaByGuid(string guid)
    {
        foreach (Area area in AllAreas)
        {
            if (area.Guid == guid)
            {
                return area;
            }
        }
        return null;
    }

    public static User GetUserByGuid(string guid)
    {
        if (Player.Guid == guid)
        {
            return Player;
        }
        else if (Rival.Guid == guid)
        {
            return Rival;
        }
        else
        {
            return null;
        }
    }

    public static IAttachable GetItemByGuid(string guid)
    {
        foreach (Card card in AllCards)
        {
            foreach (var item in card.AttachableList)
            {
                if (item.Guid == guid)
                {
                    return item;
                }
            }
        }
        return null;
    }

    public static object GetObject(string guid)
    {
        object result = GetCardByGuid(guid);
        if (result == null)
        {
            result = GetAreaByGuid(guid);
        }
        if (result == null)
        {
            result = GetUserByGuid(guid);
        }
        if (result == null)
        {
            result = GetItemByGuid(guid);
        }
        return result;
    }

    public static void Start(bool ifFirstPlay)
    {
        if (ifFirstPlay)
        {
            TurnPlayer = Player;
            DoBeginningPhase();
        }
        else
        {
            WaitTurn();
        }
    }

    private static void WaitTurn()
    {
        //Wait until rival turn ends
        DoBeginningPhase();
    }

    private static void DoBeginningPhase()
    {
        TurnPlayer.StartTurn();
        DoAutoCheckTiming();
        TurnPlayer.RefreshUnit(TurnPlayer.Field.Filter(card => card.IsHorizontal), null);
        DoAutoCheckTiming();
        if (TurnCount > 1)
        {
            TurnPlayer.DrawCard(1);
            DoAutoCheckTiming();
        }
    }

    private static void DoBondPhase()
    {
        TurnPlayer.GoToBondPhase();
        DoAutoCheckTiming();
        TurnPlayer.ChooseSetToBond(TurnPlayer.Hand.Cards, true, 0, 1);
        DoAutoCheckTiming();
    }

    private static void DoAutoCheckTiming()
    {

    }
}

public enum Phase
{
    BeginningPhase,
    BondPhase,
    DeploymentPhase,
    ActionPhase,
    EndPhase
}
