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
        var player = Game.Player as Player;
        var card = new Card00004(player);
        var card1 = new Card00006(player);
        var card2 = new Card00007(player);
        var card3 = new Card00001(player);
        player.Deck.ImportCard(card);
        player.Deck.ImportCard(card1);
        player.Deck.ImportCard(card2);
        player.Deck.ImportCard(card3);
        card.MoveTo(player.FrontField);
        card1.MoveTo(player.Hand);
        card2.MoveTo(player.Hand);
        card3.MoveTo(player.Hand);

        Assert.IsTrue(card.Power == 60);
        Game.DoDeployment(card1, true);
        Assert.IsTrue(card1.Power == 50);
        Assert.IsTrue(card.Power == 70);
        Game.DoDeployment(card2, true);
        Assert.IsTrue(card2.Power == 40);
        Assert.IsTrue(card.Power == 80);
        Game.DoDeployment(card3, true);
        Assert.IsTrue(card3.Power == 70);
        Assert.IsTrue(card.Power == 80);
    }
}
