using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00077Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『マケドニアの風』【常】<飛行>の味方が他に２体以上いる場合、このユニットの戦闘力は＋１０される。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(77, player);
        var card1 = CardFactory.CreateCard(42, player);
        var card2 = CardFactory.CreateCard(2, player);

        player.FrontField.AddCard(card);
        player.Hand.AddCard(card1);
        player.Hand.AddCard(card2);

        Game.DoDeployment(card1, true).Wait();
        Assert.IsTrue(card.Power == 30); //什么都没发生

        Game.DoDeployment(card2, true).Wait();
        Assert.IsTrue(card.Power == 40);//+10
    }
}
