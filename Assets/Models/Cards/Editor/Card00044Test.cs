using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00044Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『女は近づくな』【常】<女>のカードはこのユニットの支援に失敗する。
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(44, player);         
        var support1 = CardFactory.CreateCard(2, player);//30支援希达

        player.FrontField.AddCard(card);
        player.Deck.AddCard(support1);

        var card1 = CardFactory.CreateCard(2, rival);//50希达
        var rivalSupport1 = CardFactory.CreateCard(1, rival);//20支援
        
        rival.FrontField.AddCard(card1);
        rival.Deck.AddCard(rivalSupport1);
        
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Game.DoBattle(card, card1);
        Assert.IsTrue(card1.IsOnField);

        var support2 = CardFactory.CreateCard(1, player);//20支援马尔斯
        player.Deck.AddCard(support2);
        var rivalSupport2 = CardFactory.CreateCard(1, rival);//20支援
        rival.Deck.AddCard(rivalSupport2);

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Game.DoBattle(card, card1);
        Assert.IsFalse(card1.IsOnField);
    }
}
