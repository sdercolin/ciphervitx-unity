using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00071Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『聖者の祝福』【自】〖1回合1次〗[翻面1]出撃コストが２以下の味方が出撃した時、コストを支払うなら、自分の退避エリアから『レナ』以外で出撃コストが１のカードを１枚選び、手札に加える。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(71, player);
        var myLowCostUnit = CardFactory.CreateCard(9, player);
        var myHighCostUnit = CardFactory.CreateCard(3, player);
        var retreat = CardFactory.CreateCard(6, player);
        var bond = CardFactory.CreateCard(1, player);

        player.FrontField.AddCard(card);
        player.Hand.AddCard(myLowCostUnit);
        player.Hand.AddCard(myHighCostUnit);
        player.Retreat.AddCard(retreat);
        player.Bond.AddCard(bond);

        Game.DoDeployment(myHighCostUnit, true).Wait();
        Assert.IsTrue(player.Retreat.Count == 1); //什么都没发生

        Request.SetNextResult();//选择Induction
        Request.SetNextResult(true);//选择使用
        Request.SetNextResult();//翻面1
        Request.SetNextResult();//选择
        Game.DoDeployment(myLowCostUnit, true).Wait();
        Assert.IsTrue(player.Retreat.Count == 0);
    }
}
