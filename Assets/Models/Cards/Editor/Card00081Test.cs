using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00081Test
{

    [Test]
    public void Skill11Test()
    {
        /// <summary>
        /// スキル1
        /// 『叡智の泉』【自】〖1回合1次〗[翻面1]出撃コストが２以下の味方が出撃した時、コストを支払うなら、カードを１枚引く。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(81, player);
        var myLowCostUnit = CardFactory.CreateCard(9, player);
        var bond = CardFactory.CreateCard(9, player);
        var deck = CardFactory.CreateCard(9, player);
        player.FrontField.AddCard(card);
        player.Hand.AddCard(myLowCostUnit);
        player.Bond.AddCard(bond);
        player.Deck.AddCard(deck);

        Request.SetNextResult(); //默认选择第一个Induction
        Request.SetNextResult(true); //选择使用
        Request.SetNextResult(); //翻面
        Game.DoDeployment(myLowCostUnit, true).Wait();

        Assert.IsTrue(player.Hand.Count == 1);
    }
}
