using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00126Test
{
    [Test]
    public void SkillTest1()
    {
        /// <summary>
        /// スキル1
        /// 『ルイン』【起】〖1回合1次〗[翻面3，自分の手札から『サーリャ』を１枚退避エリアに置く]相手は自分の手札を２枚選び、退避エリアに置く。このユニットがクラスチェンジしている場合、代わりに相手は自分の手札を３枚選び、退避エリアに置く。
        /// </summary>
        /// 
        /// <summary>
        /// スキル2
        /// 『禁断の呪い』【常】相手の手札が１枚もない場合、このユニットの戦闘力は＋２０される。
        /// </summary>
        /// 
        ///不转职
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var card = CardFactory.CreateCard(126, player);
        player.FrontField.AddCard(card);
        var bond1 = CardFactory.CreateCard(2, player);
        var bond2 = CardFactory.CreateCard(2, player);
        var bond3 = CardFactory.CreateCard(2, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);
        player.Bond.AddCard(bond3);
        var hand = CardFactory.CreateCard(126, player);
        player.Hand.AddCard(hand);

        // 敌方配置
        var rivalhand1 = CardFactory.CreateCard(2, rival);
        var rivalhand2 = CardFactory.CreateCard(2, rival);
        var rivalhand3 = CardFactory.CreateCard(2, rival);
        rival.Hand.AddCard(rivalhand1);
        rival.Hand.AddCard(rivalhand2);
        rival.Hand.AddCard(rivalhand3);

        Request.SetNextResult();//翻面3
        Request.SetNextResult();//丢萨莉亚
        Request.SetNextResult();//对手丢2
        Game.DoActionSkill(card.GetUsableActionSkills()[0]).Wait();

        Assert.IsTrue(rival.Hand.Count == 1);
        Assert.IsTrue(card.Power == 60);
    }

    [Test]
    public void SkillTest2()
    {
        /// 转职
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var card = CardFactory.CreateCard(126, player);
        player.FrontField.AddCard(card);
        var bond1 = CardFactory.CreateCard(2, player);
        var bond2 = CardFactory.CreateCard(2, player);
        var bond3 = CardFactory.CreateCard(2, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);
        player.Bond.AddCard(bond3);
        var hand1 = CardFactory.CreateCard(126, player);
        var deck = CardFactory.CreateCard(126, player);
        player.Hand.AddCard(hand1);
        player.Deck.AddCard(deck);

        // 敌方配置
        var rivalhand1 = CardFactory.CreateCard(2, rival);
        var rivalhand2 = CardFactory.CreateCard(2, rival);
        var rivalhand3 = CardFactory.CreateCard(2, rival);
        rival.Hand.AddCard(rivalhand1);
        rival.Hand.AddCard(rivalhand2);
        rival.Hand.AddCard(rivalhand3);

        Game.DoLevelUp(hand1).Wait();

        Request.SetNextResult();//翻面3
        Request.SetNextResult();//丢萨莉亚
        Request.SetNextResult();//对手丢2
        Game.DoActionSkill(hand1.GetUsableActionSkills()[0]).Wait();

        Assert.IsTrue(rival.Hand.Count == 0);
        Assert.IsTrue(hand1.Power == 80);
    }
}
