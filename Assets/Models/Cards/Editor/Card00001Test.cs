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
    public void Skill1Test()
    {
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;
        var marth = CardFactory.CreateCard(1, player);
        var myLowCostUnit1 = CardFactory.CreateCard(9, player);
        var myLowCostUnit2 = CardFactory.CreateCard(11, player);
        var myHighCostUnit = CardFactory.CreateCard(3, player);
        player.FrontField.AddCard(marth);
        player.Hand.AddCard(myLowCostUnit1);
        player.Hand.AddCard(myLowCostUnit2);
        player.Hand.AddCard(myHighCostUnit);
        var hisUnit1 = CardFactory.CreateCard(6, rival);
        var hisUnit2 = CardFactory.CreateCard(7, rival);
        var hisUnit3 = CardFactory.CreateCard(8, rival);
        rival.FrontField.AddCard(hisUnit1);
        rival.BackField.AddCard(hisUnit2);
        rival.BackField.AddCard(hisUnit3);

        Game.DoDeployment(myHighCostUnit, true); //什么都没发生

        Request.SetNextResult(); //默认选择第一个Induction
        Request.SetNextResult(true); //选择使用
        Request.SetNextResult(new List<Card>() { hisUnit2 }); //选择对象
        Game.DoDeployment(myLowCostUnit1, true);
        Assert.IsTrue(rival.BackField.Cards.SequenceEqual(new List<Card>() { hisUnit3 }));
    }
}
