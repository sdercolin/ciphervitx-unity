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
    public void Skill2Test()
    {
        Game.Initialize();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;
        var thisCard = CardFactory.CreateCard(2, player);
        var bondCard = CardFactory.CreateCard(1, player);
        player.FrontField.AddCard(thisCard);
        player.Bond.AddCard(bondCard);
        var hisUnit1 = CardFactory.CreateCard(6, rival);
        var hisUnit2 = CardFactory.CreateCard(7, rival);
        rival.FrontField.AddCard(hisUnit1);
        rival.BackField.AddCard(hisUnit2);

        Assert.IsTrue(thisCard.GetAttackableUnits().SequenceEqual(new List<Card>() { hisUnit1 }));

        Request.SetNextResult(new List<Card>() { bondCard }); //设定要翻的费
        Game.DoActionSkill(thisCard.GetUsableActionSkills()[0]);
        Assert.IsTrue(thisCard.GetAttackableUnits().SequenceEqual(new List<Card>() { hisUnit1, hisUnit2 }));
    }
}
