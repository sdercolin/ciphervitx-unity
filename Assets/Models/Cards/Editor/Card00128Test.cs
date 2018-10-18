using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00128Test
{
    /// <summary>
    /// スキル2
    /// 〖攻击型〗『暗闇の紋章』【支】相手の手札が５枚以上の場合、相手は自分の手札を１枚選び、退避エリアに置く。
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
        var card = CardFactory.CreateCard(128, player);
        player.Deck.AddCard(card);
        var unit = CardFactory.CreateCard(1, player);
        player.FrontField.AddCard(unit);

        // 敌方配置
        var rivalCard = CardFactory.CreateCard(1, rival);//70战斗力
        rival.FrontField.AddCard(rivalCard);
        var rivalSupport = CardFactory.CreateCard(2, rival);//30支援
        rival.Deck.AddCard(rivalSupport);
        var rivalHand1 = CardFactory.CreateCard(1, rival);
        var rivalHand2 = CardFactory.CreateCard(1, rival);
        var rivalHand3 = CardFactory.CreateCard(1, rival);
        var rivalHand4 = CardFactory.CreateCard(1, rival);
        var rivalHand5 = CardFactory.CreateCard(1, rival);
        rival.Hand.AddCard(rivalHand1);
        rival.Hand.AddCard(rivalHand2);
        rival.Hand.AddCard(rivalHand3);
        rival.Hand.AddCard(rivalHand4);
        rival.Hand.AddCard(rivalHand5);

        Request.SetNextResult(); //对手选择丢弃
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Game.DoBattle(unit, rivalCard);

        Assert.IsTrue(rival.Hand.Count == 4);
    }
}
