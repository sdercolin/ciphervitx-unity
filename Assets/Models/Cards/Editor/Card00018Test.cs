using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00018Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『サジマジバーツ』〖阵型技〗【自】[味方の『サジ』と『マジ』を行動済みにする]このユニットが攻撃した時、コストを支払うなら、戦闘終了まで、このユニットの戦闘力は＋５０され、このユニットが攻撃で破壊するオーブは２つになる。
        /// </summary>
        /// 
        /// <summary>
        /// スキル2
        /// 『戦士の心得』【常】自分のターン中、このユニットの戦闘力は＋２０される。
        /// </summary>
        Game.Initialize();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(18, player);// 1C巴兹  
        var card1 = CardFactory.CreateCard(16, player);// サジ
        var card2 = CardFactory.CreateCard(17, player);// マジ
        var support = CardFactory.CreateCard(2, player);

        player.FrontField.AddCard(card);
        player.FrontField.AddCard(card1);
        player.FrontField.AddCard(card2);
        player.Deck.AddCard(support);

        var card4 = CardFactory.CreateCard(1, rival);
        var rivalOrb1 = CardFactory.CreateCard(6, rival);
        var rivalOrb2 = CardFactory.CreateCard(6, rival);
        var rivalSupport = CardFactory.CreateCard(2, rival);

        rival.FrontField.AddCard(card4);
        rival.Deck.AddCard(rivalSupport);
        rival.Orb.AddCard(rivalOrb1);
        rival.Orb.AddCard(rivalOrb2);

        Request.SetNextResult(new List<Card>() { card1});
        Request.SetNextResult(new List<Card>() { card2});
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Game.DoBattle(card, card4);
        Game.DoAutoCheckTiming();

        // 40+20+50+support30
        Assert.IsTrue(card.Power == 140);
        Assert.IsTrue(rival.Orb.Count == 0);
    }
}
