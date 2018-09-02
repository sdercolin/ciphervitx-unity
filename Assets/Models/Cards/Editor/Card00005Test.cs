using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00005Test
{
    // 先测一下没发的时候，可不可以正常回避（因为没测过回避）
    [Test]
    public void Skill1Test1()
    {
        Game.Initialize();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(5, player);
        card.IsHero = true;
        player.FrontField.AddCard(card);
        var bond1 = CardFactory.CreateCard(1, player);
        var bond2 = CardFactory.CreateCard(1, player);
        var bond3 = CardFactory.CreateCard(1, player);
        var support = CardFactory.CreateCard(2, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);
        player.Bond.AddCard(bond3);
        player.Deck.AddCard(support);

        var rivalUnit = CardFactory.CreateCard(6, rival);
        rivalUnit.IsHero = true;
        rival.FrontField.AddCard(rivalUnit);
        var rivalSupport = CardFactory.CreateCard(2, rival);
        rival.Deck.AddCard(rivalSupport);
        var rivalHand = CardFactory.CreateCard(6, rival);
        rival.Hand.AddCard(rivalHand);
        var rivalOrb = CardFactory.CreateCard(6, rival);
        rival.Orb.AddCard(rivalOrb);

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(true); //回避
        Request.SetNextResult(new List<Card>() { rivalHand }); //选择回避丢弃的手牌
        Game.DoBattle(card, rivalUnit);
        Assert.IsTrue(rival.Orb.Count == 1); //应该没有击破
    }

    //然后测发动这个能力以后能不能回避
    [Test]
    public void Skill1Test2()
    {
        Game.Initialize();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(5, player);
        player.FrontField.AddCard(card);
        var bond1 = CardFactory.CreateCard(1, player);
        var bond2 = CardFactory.CreateCard(1, player);
        var bond3 = CardFactory.CreateCard(1, player);
        var support = CardFactory.CreateCard(2, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);
        player.Bond.AddCard(bond3);
        player.Deck.AddCard(support);

        var rivalUnit = CardFactory.CreateCard(6, rival);
        rival.FrontField.AddCard(rivalUnit);
        var rivalSupport = CardFactory.CreateCard(2, rival);
        rival.Deck.AddCard(rivalSupport);
        var rivalHand = CardFactory.CreateCard(6, rival);
        rival.Hand.AddCard(rivalHand);

        Request.SetNextResult(new List<Card>() { bond1, bond2, bond3 }); //设定要翻的费
        Game.DoActionSkill(card.GetUsableActionSkills()[0]); //发动

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(true); //回避，但是应该不需要选择，因为不能回避
        Game.DoBattle(card, rivalUnit);
        Assert.IsTrue(rival.Orb.Count == 0); //应该被击破了
    }

    [Test]
    public void Skill2Test()
    {
        Game.Initialize();
        var player = Game.Player;
        Game.TurnPlayer = player;
<<<<<<< HEAD
        var card00005 = new Card00005(player);
        var card00001 = new Card00001(player);
        var card00003 = new Card00003(player);
        var bondCard1 = CardFactory.CreateCard(1, player);
        var bondCard2 = CardFactory.CreateCard(1, player);
        var bondCard3 = CardFactory.CreateCard(1, player);
=======
        var card00005 = CardFactory.CreateCard(5, player);
        var card00001 = CardFactory.CreateCard(1, player);
        var card00003 = CardFactory.CreateCard(3, player);
>>>>>>> model

        player.FrontField.AddCard(card00005);
        player.Hand.AddCard(card00001);
        player.Hand.AddCard(card00003);

        card00005.IsHero = true;

        Game.TryDoMessage(new EmptyMessage());
        Assert.IsTrue(card00005.Power == 70);

        player.Deploy(card00001, true);
        Assert.IsTrue(card00005.Power == 60);

        card00005.IsHero = false;
        card00001.IsHero = true;
        Game.TryDoMessage(new EmptyMessage());
        Assert.IsTrue(card00005.Power == 70);

        player.Deploy(card00003, true);
        Assert.IsTrue(card00005.Power == 60);

        Request.SetNextResult(new List<Card>() { bondCard1, bondCard2, bondCard3 });
    }
}
