using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00123Test
{
    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『グレゴと幼き竜』【常】このユニットが敵の攻撃で撃破された場合、このユニットを退避エリアに置く代わりに自分の絆エリアに置く。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = rival;

        // 己方配置
        var card = CardFactory.CreateCard(123, player);//40
        player.FrontField.AddCard(card);
        var support1 = CardFactory.CreateCard(2, player);//30支援
        player.Deck.AddCard(support1);

        // 敌方配置
        var rivalCard = CardFactory.CreateCard(1, rival);//70战斗力
        rival.FrontField.AddCard(rivalCard);
        var rivalSupport = CardFactory.CreateCard(4, rival);//10支援
        rival.Deck.AddCard(rivalSupport);

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Request.SetNextResult(); //选择Induction

        Game.DoBattle(rivalCard, card);

        Assert.IsTrue(card.BelongedRegion == player.Bond);
    }
}
