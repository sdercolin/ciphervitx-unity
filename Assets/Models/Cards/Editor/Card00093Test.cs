using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00093Test
{
    /// <summary>
    /// スキル2
    /// 〖攻击型〗『竜人の紋章』【支】自分の攻撃ユニットが<光の剣>の場合、自分の手札を１枚選ぶ。そのカードを絆エリアに置いてもよい。
    /// </summary>
    [Test]
    public void Skill2Test()
    {
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var tiki = CardFactory.CreateCard(93, player);
        player.Deck.AddCard(tiki);
        var unit = CardFactory.CreateCard(1, player);
        player.FrontField.AddCard(unit);
        var hand = CardFactory.CreateCard(2, player);
        player.Hand.AddCard(hand);

        // 敌方配置
        var rivalCard = CardFactory.CreateCard(1, rival);//70战斗力
        rival.FrontField.AddCard(rivalCard);
        var rivalSupport = CardFactory.CreateCard(2, rival);//30支援
        rival.Deck.AddCard(rivalSupport);

        Request.SetNextResult(); //选择到羁绊区
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避

        Game.DoBattle(unit, rivalCard);

        Assert.IsTrue(player.Bond.Count == 1);
        Assert.IsTrue(player.Hand.Count == 0);
    }
}
