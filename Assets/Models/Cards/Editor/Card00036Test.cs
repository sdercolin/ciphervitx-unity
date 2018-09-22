using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00036Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『弓の達人』〖转职技〗【常】他の<弓>の味方１体につき、このユニットの戦闘力は＋１０される。（はこのユニットがクラスチェンジしていなければ有効にならない）
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        Game.TurnPlayer = player;

        var weiaoer = CardFactory.CreateCard(36, player);
        player.FrontField.AddCard(weiaoer);
        var adv_weiaoer = CardFactory.CreateCard(36, player);
        player.Hand.AddCard(adv_weiaoer);
        var bonus = CardFactory.CreateCard(1, player);
        player.Deck.AddCard(bonus);

        var myUint1 = CardFactory.CreateCard(13, player);//哥顿
        player.FrontField.AddCard(myUint1);
        var myUnit2 = CardFactory.CreateCard(80, player);//乔治
        player.FrontField.AddCard(myUnit2);

        Assert.IsTrue(weiaoer.Power == 50);

        Game.DoLevelUp(adv_weiaoer, true);
        Assert.IsTrue(adv_weiaoer.Power == 70);
    }
}
