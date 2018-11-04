using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00114Test
{
    /// <summary>
    /// スキル1
    /// 『天馬の叫び』〖转职技〗【常】他の味方を支援しているカードが<飛行>の場合、そのカードの支援力は＋１０される。（はこのユニットがクラスチェンジしていなければ有効にならない）
    /// </summary>
    [Test]
    public void Skill1Test()
    {
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var card = CardFactory.CreateCard(114, player);
        player.FrontField.AddCard(card);
        var hand = CardFactory.CreateCard(114, player);
        player.Hand.AddCard(hand);
        var unit = CardFactory.CreateCard(6, player);//40
        player.FrontField.AddCard(unit);
        var support1 = CardFactory.CreateCard(2, player);//30支援飞行
        player.Deck.AddCard(support1);
        var support2 = CardFactory.CreateCard(2, player);//30支援飞行
        player.Deck.AddCard(support2);
        var bond1 = CardFactory.CreateCard(114, player);
        player.Bond.AddCard(bond1);
        var bond2 = CardFactory.CreateCard(114, player);
        player.Bond.AddCard(bond2);

        // 敌方配置
        var rivalCard = CardFactory.CreateCard(1, rival);//70战斗力
        rival.FrontField.AddCard(rivalCard);
        var rivalSupport = CardFactory.CreateCard(4, rival);//10支援
        rival.Deck.AddCard(rivalSupport);

        Game.DoLevelUp(hand, true).Wait();

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避

        Game.DoBattle(unit, rivalCard).Wait();

        Assert.IsTrue(rivalCard.BelongedRegion == rival.Retreat);
    }
}
