using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00001Test
{

    [Test]
    public void SkillTest()
    {
        Game.Initialize();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;
        var marth = new Card00001(player);
        var myLowCostUnit1 = new Card00009(player);
        var myLowCostUnit2 = new Card00011(player);
        var myHighCostUnit = new Card00003(player);
        player.FrontField.AddCard(marth);
        player.Hand.AddCard(myLowCostUnit1);
        player.Hand.AddCard(myLowCostUnit2);
        player.Hand.AddCard(myHighCostUnit);
        var hisUnit1 = new Card00006(rival);
        var hisUnit2 = new Card00007(rival);
        var hisUnit3 = new Card00008(rival);
        rival.FrontField.AddCard(hisUnit1);
        rival.BackField.AddCard(hisUnit2);
        rival.BackField.AddCard(hisUnit3);

        player.Deploy(myHighCostUnit, true);
        Assert.IsTrue(Game.InductionSetList.Count == 0);
        Request.ClearNextResults();

        player.Deploy(myLowCostUnit1, true);
        Request.SetNextResult(new List<Induction>() { Game.InductionSetList[0][0] });
        Request.SetNextResult(true);
        Request.SetNextResult(new List<Card>() { hisUnit2 });
        Game.DoAutoCheckTiming();
        Assert.IsTrue(rival.BackField.Cards.SequenceEqual(new List<Card>() { hisUnit3 }));
        Request.ClearNextResults();


        player.Deploy(myLowCostUnit2, true);
        Assert.IsTrue(Game.InductionSetList.Count == 0);
    }
}
