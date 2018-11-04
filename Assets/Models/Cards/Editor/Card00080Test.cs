using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00080Test
{
    [Test]
    public void Skill1Test1()
    {
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(80, player);
        player.FrontField.AddCard(card);
        var card1 = CardFactory.CreateCard(6, player);
        var support1 = CardFactory.CreateCard(2, player);
        var support2 = CardFactory.CreateCard(2, player);
        player.FrontField.AddCard(card1);
        player.Deck.AddCard(support1);
        player.Deck.AddCard(support2);

        var rivalUnit1 = CardFactory.CreateCard(1, rival);
        var rivalUnit2 = CardFactory.CreateCard(2, rival);
        rival.FrontField.AddCard(rivalUnit1);
        rival.FrontField.AddCard(rivalUnit2);
        var rivalSupport1 = CardFactory.CreateCard(3, rival);
        var rivalSupport2 = CardFactory.CreateCard(3, rival);
        rival.Deck.AddCard(rivalSupport1);
        rival.Deck.AddCard(rivalSupport2);
        var rivalHand1 = CardFactory.CreateCard(1, rival);
        var rivalHand2 = CardFactory.CreateCard(2, rival);
        rival.Hand.AddCard(rivalHand1);
        rival.Hand.AddCard(rivalHand2);

        //攻击马尔斯
        Request.SetNextResult(); //横置
        Game.DoActionSkill(card.GetUsableActionSkills()[0]).Wait(); //发动

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(true); //回避
        Request.SetNextResult(); //选择回避丢弃的手牌
        Game.DoBattle(card, rivalUnit1).Wait();
        Assert.IsTrue(rivalUnit1.IsOnField); //应该没有击破

        card.IsHorizontal = false;
        card1.IsHorizontal = false;

        //攻击希达
        Request.SetNextResult(); //横置
        Game.DoActionSkill(card.GetUsableActionSkills()[0]).Wait(); //发动

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(true); //回避，但是应该不需要选择，因为不能回避
        Game.DoBattle(card, rivalUnit2).Wait();
        Assert.IsFalse(rivalUnit2.IsOnField); //应该没有击破
    }

}
