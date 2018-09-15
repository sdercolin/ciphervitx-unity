using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00061Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『きずぐすり』【自】[横置，このユニットを撃破する]他の味方が攻撃された時、コストを支払うなら、戦闘終了まで、その防御ユニットの戦闘力は＋２０される。
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = rival;

        var card1 = CardFactory.CreateCard(61, player);
        var card2 = CardFactory.CreateCard(2, player);//50希达
        var support1 = CardFactory.CreateCard(1, player);//20支援

        player.FrontField.AddCard(card1);
        player.FrontField.AddCard(card2);
        player.Deck.AddCard(support1);

        var card3 = CardFactory.CreateCard(3, rival);//70杰刚
        var rivalSupport1 = CardFactory.CreateCard(4, rival);//10支援

        rival.FrontField.AddCard(card3);
        rival.Deck.AddCard(rivalSupport1);

        Request.SetNextResult();//选择被诱发的Skill
        Request.SetNextResult(true); //选择发动Skill
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避

        Game.DoBattle(card3, card2);
        Assert.IsFalse(card1.IsOnField);//击破里弗
        Assert.IsTrue(card2.IsOnField);//希达没死
    }
}
