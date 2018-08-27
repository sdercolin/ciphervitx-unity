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
        player.Deck.ImportCard(marth);
        player.Deck.ImportCard(myLowCostUnit1);
        player.Deck.ImportCard(myLowCostUnit2);
        player.Deck.ImportCard(myHighCostUnit);
        marth.MoveTo(player.FrontField);
        myLowCostUnit1.MoveTo(player.Hand);
        myLowCostUnit2.MoveTo(player.Hand);
        myHighCostUnit.MoveTo(player.Hand);
        var hisUnit1 = new Card00006(rival);
        var hisUnit2 = new Card00007(rival);
        var hisUnit3 = new Card00008(rival);
        rival.Deck.ImportCard(hisUnit1);
        rival.Deck.ImportCard(hisUnit2);
        rival.Deck.ImportCard(hisUnit3);
        hisUnit1.MoveTo(rival.FrontField);
        hisUnit2.MoveTo(rival.BackField);
        hisUnit3.MoveTo(rival.BackField);

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
