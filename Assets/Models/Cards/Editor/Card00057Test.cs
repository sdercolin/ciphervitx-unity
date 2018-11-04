using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00057Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『鉄壁の盾』【自】[翻面1]他の味方が攻撃された時、コストを支払うなら、このユニットはその味方の代わりに攻撃される。このスキルはこのユニットが前衛でなければ発動しない。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = rival;

        var card1 = CardFactory.CreateCard(57, player);
        var card2 = CardFactory.CreateCard(2, player);//50希达
        var bond1 = CardFactory.CreateCard(1, player);
        var support1 = CardFactory.CreateCard(1, player);//20支援

        player.FrontField.AddCard(card1);//杜卡在前排
        player.FrontField.AddCard(card2);
        player.Bond.AddCard(bond1);
        player.Deck.AddCard(support1);

        var card3 = CardFactory.CreateCard(1, rival);//70马尔斯
        var rivalSupport1 = CardFactory.CreateCard(3, rival);//20支援

        rival.FrontField.AddCard(card3);
        rival.Deck.AddCard(rivalSupport1);

        Request.SetNextResult();//选择被诱发的Skill
        Request.SetNextResult(true); //选择发动Skill
        Request.SetNextResult(); //翻面1
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避

        Game.DoBattle(card3, card2).Wait();
        Assert.IsFalse(card1.IsOnField);//击破杜卡
        Assert.IsTrue(card2.IsOnField);//希达没死

        var card4 = CardFactory.CreateCard(57, player);
        var bond2 = CardFactory.CreateCard(1, player);
        var support2 = CardFactory.CreateCard(1, player);//20支援

        player.BackField.AddCard(card4);//杜卡在后排
        player.Bond.AddCard(bond2);
        player.Deck.AddCard(support2);

        var rivalSupport2 = CardFactory.CreateCard(3, rival);//20支援
        rival.Deck.AddCard(rivalSupport2);

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避

        Game.DoBattle(card3, card2).Wait();
        Assert.IsFalse(card2.IsOnField);//击破希达
        Assert.IsTrue(card4.IsOnField);//杜卡没死
    }
}
