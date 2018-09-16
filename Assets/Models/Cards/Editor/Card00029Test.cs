using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00029Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『聖痕の輝き』【常】クラスチェンジしている他の味方１体につき、このユニットの戦闘力は＋１０される。
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var kuluomu = CardFactory.CreateCard(29, player);
        player.FrontField.AddCard(kuluomu);

        var card1 = CardFactory.CreateCard(48, player);
        player.FrontField.AddCard(card1);
        var card2 = CardFactory.CreateCard(47, player);// 54马尔斯
        player.Hand.AddCard(card2);

        var card3 = CardFactory.CreateCard(51, player);
        player.FrontField.AddCard(card3);
        var card4 = CardFactory.CreateCard(50, player);// 43希达
        player.Hand.AddCard(card4);

        var bonus = CardFactory.CreateCard(47, player);
        player.Deck.AddCard(bonus);
        var bonus2 = CardFactory.CreateCard(47, player);
        player.Deck.AddCard(bonus2);

        Assert.IsTrue(kuluomu.Power == 40);
        Game.DoLevelUp(card2, true);
        Assert.IsTrue(kuluomu.Power == 50);
        Game.DoLevelUp(card4, true);
        Assert.IsTrue(kuluomu.Power == 60);
    }
}
