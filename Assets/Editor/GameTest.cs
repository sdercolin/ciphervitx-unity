using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class GameTest {

    [Test]
    public void DoBeginningPhaseTest() {
        Game.Initialize();
        Game.TurnPlayer = Game.Player;
        var card1 = CardFactory.CreateCard(1, Game.Player);
        Game.Player.FrontField.AddCard(card1);
        card1.IsHorizontal = true;
        var card2 = CardFactory.CreateCard(2, Game.Player);
        Game.Player.FrontField.AddCard(card2);

        Assert.IsTrue(card1.IsHorizontal);
        Assert.IsTrue(!card2.IsHorizontal);

        Game.DoBeginningPhase();

        Assert.IsTrue(!card1.IsHorizontal);
        Assert.IsTrue(!card2.IsHorizontal);
    }
}
