using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class GameTest
{
    [Test]
    public void StartTurnTest()
    {
        Game.Initialize();
        Game.TurnPlayer = Game.Player;
        var card1 = CardFactory.CreateCard(1, Game.Player);
        Game.Player.FrontField.AddCard(card1);
        card1.IsHorizontal = true;
        var card2 = CardFactory.CreateCard(2, Game.Player);
        Game.Player.FrontField.AddCard(card2);

        var card3 = CardFactory.CreateCard(3, Game.Player);
        Game.Player.Deck.AddCard(card3);

        Assert.IsTrue(card1.IsHorizontal);
        Assert.IsTrue(!card2.IsHorizontal);
        Assert.IsTrue(Game.Player.Hand.Count == 0);

        Game.StartTurn();

        Assert.IsTrue(!card1.IsHorizontal);
        Assert.IsTrue(!card2.IsHorizontal);
        Assert.IsTrue(Game.Player.Hand.Count == 0);

        Game.TurnCount = 2;
        Game.StartTurn();
        Assert.IsTrue(Game.Player.Hand.Count == 1);
    }
}
