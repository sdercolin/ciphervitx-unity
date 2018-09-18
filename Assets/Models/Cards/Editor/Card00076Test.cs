using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00076Test
{

    [Test]
    public void Skill2Test()
    {
        /// <summary>
        /// スキル2
        /// 『アイオテの盾』【常】すべての敵は『飛行特効』を失い、新たに得ることもできない。（【常】はこのカードがユニットとして戦場にいる間だけ有効になる）
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = rival;

        //测试失去飞行特效
        var card1 = CardFactory.CreateCard(76, player);
        var support1 = CardFactory.CreateCard(1, player);//20支援

        player.FrontField.AddCard(card1);
        player.Deck.AddCard(support1);

        var card2 = CardFactory.CreateCard(14, rival);//30哥顿
        var rivalSupport1 = CardFactory.CreateCard(1, rival);//20支援

        rival.BackField.AddCard(card2);
        rival.Deck.AddCard(rivalSupport1);

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避

        Game.DoBattle(card2, card1);
        Assert.IsTrue(card1.IsOnField);//没有被击破

        //测试不能获得飞行特效
        var support2 = CardFactory.CreateCard(1, player);//20支援

        player.Deck.AddCard(support2);

        var card3 = CardFactory.CreateCard(22, rival);//30玛利克
        var rivalbond = CardFactory.CreateCard(22, rival);
        var rivalSupport2 = CardFactory.CreateCard(1, rival);//20支援

        rival.BackField.AddCard(card3);
        rival.Bond.AddCard(rivalbond);
        rival.Deck.AddCard(rivalSupport2);

        Game.DoActionSkill(card3.GetUsableActionSkills()[0]);

        Request.SetNextResult(); //翻面1
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避

        Game.DoBattle(card3, card1);
        Assert.IsTrue(card1.IsOnField);
    }
}
