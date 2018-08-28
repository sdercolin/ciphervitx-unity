using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00002Test
{

    [Test]
    public void SkillTest()
    {
        Game.Initialize();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;
        var thisCard = new Card00002(player);
        var bondCard = new Card00001(player);
        player.Deck.ImportCard(thisCard);
        player.Deck.ImportCard(bondCard);
        thisCard.MoveTo(player.FrontField);
        bondCard.MoveTo(player.Bond);
        var hisUnit1 = new Card00006(rival);
        var hisUnit2 = new Card00007(rival);
        rival.Deck.ImportCard(hisUnit1);
        rival.Deck.ImportCard(hisUnit2);
        hisUnit1.MoveTo(rival.FrontField);
        hisUnit2.MoveTo(rival.BackField);

        Assert.IsTrue(thisCard.GetAttackableUnits().SequenceEqual(new List<Card>() { hisUnit1 }));

        Request.SetNextResult(new List<Card>() { bondCard });
        player.UseActionSkill(thisCard.GetUsableActionSkills()[0]);
        Assert.IsTrue(thisCard.GetAttackableUnits().SequenceEqual(new List<Card>() { hisUnit1, hisUnit2 }));
    }
}
