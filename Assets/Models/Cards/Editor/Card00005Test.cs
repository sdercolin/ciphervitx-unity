using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00005Test
{

    [Test]
    public void SkillTest()
    {
        Game.Initialize();
        var player = Game.Player;
        Game.TurnPlayer = player;
        var card00005 = CardFactory.CreateCard(5, player);
        var card00001 = CardFactory.CreateCard(1, player);
        var card00003 = CardFactory.CreateCard(3, player);

        player.FrontField.AddCard(card00005);
        player.Hand.AddCard(card00001);
        player.Hand.AddCard(card00003);

        card00005.IsHero = true;

        Game.TryDoMessage(new EmptyMessage());
        Assert.IsTrue(card00005.Power == 70);

        player.Deploy(card00001, true);
        Assert.IsTrue(card00005.Power == 60);

        card00005.IsHero = false;
        card00001.IsHero = true;
        Game.TryDoMessage(new EmptyMessage());
        Assert.IsTrue(card00005.Power == 70);

        player.Deploy(card00003, true);
        Assert.IsTrue(card00005.Power == 60);
    }
}
