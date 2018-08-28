using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00004Test
{

    [Test]
    public void SkillTest()
    {
        Game.Initialize();
        var player = Game.Player;
        var card = CardFactory.CreateCard(4, player);
        var card1 = CardFactory.CreateCard(6, player);
        var card2 = CardFactory.CreateCard(7, player);
        var card3 = CardFactory.CreateCard(1, player);
        player.FrontField.AddCard(card);
        player.Hand.AddCard(card1);
        player.Hand.AddCard(card2);
        player.Hand.AddCard(card3);

        Assert.IsTrue(card.Power == 60);

        player.Deploy(card1, true);
        Request.SetNextResult(new List<Induction>() { Game.InductionSetList[0][0] });
        Game.DoAutoCheckTiming();
        Assert.IsTrue(card1.Power == 50);
        Assert.IsTrue(card.Power == 70);

        player.Deploy(card2, true);
        Request.SetNextResult(new List<Induction>() { Game.InductionSetList[0][0] });
        Game.DoAutoCheckTiming();
        Assert.IsTrue(card2.Power == 40);
        Assert.IsTrue(card.Power == 80);

        player.Deploy(card3, true);
        Assert.IsTrue(Game.InductionSetList.Count == 0);
    }
}
