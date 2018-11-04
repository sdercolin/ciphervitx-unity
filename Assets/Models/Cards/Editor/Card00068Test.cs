using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00068Test
{

    [Test]
    public void Skill2Test()
    {
        /// <summary>
        /// スキル2
        /// 『二刀流』【自】[翻面1]このユニットの攻撃で敵を撃破した時、コストを支払うなら、主人公以外で出撃コストが２以下の敵を１体選び、撃破する。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card1 = CardFactory.CreateCard(68, player);
        var bond = CardFactory.CreateCard(2, player);
        var support1 = CardFactory.CreateCard(1, player);//20支援

        player.FrontField.AddCard(card1);
        player.Bond.AddCard(bond);
        player.Deck.AddCard(support1);

        var card2 = CardFactory.CreateCard(3, rival);//70杰刚
        var card3 = CardFactory.CreateCard(6, rival);//1C马尔斯
        var rivalSupport1 = CardFactory.CreateCard(1, rival);//20支援

        rival.FrontField.AddCard(card2);
        rival.FrontField.AddCard(card3);
        rival.Deck.AddCard(rivalSupport1);

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Request.SetNextResult();//选择被诱发的Skill
        Request.SetNextResult(true); //选择发动Skill
        Request.SetNextResult();//翻面1
        Request.SetNextResult();//选择击破

        Game.DoBattle(card1, card2).Wait();
        Assert.IsFalse(card2.IsOnField);//击破杰刚
        Assert.IsFalse(card3.IsOnField);//效果击破马尔斯
    }
}
