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
        Game.SetTestMode();
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
        
        Request.SetNextResult();  //默认选择第一个Induction
        Game.DoDeployment(card1, true).Wait();
        Assert.IsTrue(card1.Power == 50);
        Assert.IsTrue(card.Power == 70);
        
        Request.SetNextResult(); //默认选择第一个Induction
        Game.DoDeployment(card2, true).Wait();
        Assert.IsTrue(card2.Power == 40);
        Assert.IsTrue(card.Power == 80);
        
        Game.DoDeployment(card3, true).Wait();
        Assert.IsTrue(card.Power == 80);
    }
}
