using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class GameTest
{
    [Test]
    public void StartTurnTest()
    {
        Game.Initialize();
        Game.SetTestMode();
        Game.TurnPlayer = Game.Player;
        var card1 = CardFactory.CreateCard(1, Game.Player);
        Game.Player.FrontField.AddCard(card1);
        card1.IsHorizontal = true;
        var card2 = CardFactory.CreateCard(2, Game.Player);
        Game.Player.FrontField.AddCard(card2);

        var card3 = CardFactory.CreateCard(3, Game.Player);
        Game.Player.Deck.AddCard(card3);

        Assert.IsTrue(card1.IsHorizontal);
        Assert.IsTrue(!card2.IsHorizontal);
        Assert.IsTrue(Game.Player.Hand.Count == 0);

        Request.SetNextResult(new List<Card>() { });
        Game.StartTurn();

        Assert.IsTrue(!card1.IsHorizontal);
        Assert.IsTrue(!card2.IsHorizontal);
        Assert.IsTrue(Game.Player.Hand.Count == 0);

        Game.TurnCount = 2;
        Request.SetNextResult(new List<Card>() { });
        Game.StartTurn();
        Assert.IsTrue(Game.Player.Hand.Count == 1);
    }

    [Test]
    public void BattleTest1()
    {
        Game.Initialize();
        Game.SetTestMode();
        Game.TurnPlayer = Game.Player;
        var player = Game.Player;
        var rival = Game.Rival;
        var hero1 = CardFactory.CreateCard(6, player);
        hero1.IsHero = true;
        var card1 = CardFactory.CreateCard(9, player);
        var support1 = CardFactory.CreateCard(3, player);
        var support3 = CardFactory.CreateCard(2, player);
        player.FrontField.AddCard(hero1);
        player.FrontField.AddCard(card1);
        player.Deck.AddCard(support1);
        player.Deck.AddCard(support3);
        var hero2 = CardFactory.CreateCard(6, rival);
        hero2.IsHero = true;
        var card2 = CardFactory.CreateCard(9, rival);
        var support2 = CardFactory.CreateCard(2, rival);
        var support4 = CardFactory.CreateCard(2, rival);
        rival.FrontField.AddCard(hero2);
        rival.FrontField.AddCard(card2);
        rival.Deck.AddCard(support2);
        rival.Deck.AddCard(support4);

        Request.SetNextResult(false);
        Request.SetNextResult(false);
        Game.DoBattle(card1, card2);
        Assert.IsTrue(rival.Field.Count == 2);

        Request.SetNextResult(false);
        Request.SetNextResult(false);
        Game.DoBattle(hero1, card2);
        Assert.IsTrue(rival.Field.Count == 1);
    }


    [Test]
    public void BattleTest2()
    {
        Game.Initialize();
        Game.SetTestMode();
        Game.TurnPlayer = Game.Player;
        var player = Game.Player;
        var rival = Game.Rival;
        var hero1 = CardFactory.CreateCard(6, player);
        hero1.IsHero = true;
        var card1 = CardFactory.CreateCard(9, player);
        var support1 = CardFactory.CreateCard(3, player);
        var support3 = CardFactory.CreateCard(15, player);
        player.FrontField.AddCard(hero1);
        player.FrontField.AddCard(card1);
        player.Deck.AddCard(support1);
        player.Deck.AddCard(support3);
        var hero2 = CardFactory.CreateCard(6, rival);
        hero2.IsHero = true;
        var card2 = CardFactory.CreateCard(9, rival);
        var support2 = CardFactory.CreateCard(12, rival);
        var support4 = CardFactory.CreateCard(2, rival);
        rival.FrontField.AddCard(hero2);
        rival.FrontField.AddCard(card2);
        rival.Deck.AddCard(support2);
        rival.Deck.AddCard(support4);

        Request.SetNextResult(false);
        Request.SetNextResult(false);
        Game.DoBattle(card1, card2);
        Assert.IsTrue(rival.Field.Count == 2);

        Request.SetNextResult(false);
        Request.SetNextResult(false);
        Game.DoBattle(hero1, card2);
        Assert.IsTrue(rival.Field.Count == 1);
    }

    [Test]
    public void BattleTest3()
    {
        Game.Initialize();
        Game.SetTestMode();
        Game.TurnPlayer = Game.Player;
        var player = Game.Player;
        var rival = Game.Rival;
        var hero1 = CardFactory.CreateCard(6, player);
        hero1.IsHero = true;
        var card1 = CardFactory.CreateCard(9, player);
        var support1 = CardFactory.CreateCard(9, player);
        var support3 = CardFactory.CreateCard(6, player);
        player.FrontField.AddCard(hero1);
        player.FrontField.AddCard(card1);
        player.Deck.AddCard(support1);
        player.Deck.AddCard(support3);
        var hero2 = CardFactory.CreateCard(6, rival);
        hero2.IsHero = true;
        var card2 = CardFactory.CreateCard(9, rival);
        var support2 = CardFactory.CreateCard(5, rival);
        var support4 = CardFactory.CreateCard(9, rival);
        rival.FrontField.AddCard(hero2);
        rival.FrontField.AddCard(card2);
        rival.Deck.AddCard(support2);
        rival.Deck.AddCard(support4);

        Request.SetNextResult(false);
        Request.SetNextResult(false);
        Game.DoBattle(card1, card2);
        Assert.IsTrue(rival.Field.Count == 2);

        Request.SetNextResult(false);
        Request.SetNextResult(false);
        Game.DoBattle(hero1, card2);
        Assert.IsTrue(rival.Field.Count == 1);
    }

    [Test]
    public void BattleTest4()
    {
        Game.Initialize();
        Game.SetTestMode();
        Game.TurnPlayer = Game.Player;
        var player = Game.Player;
        var rival = Game.Rival;
        var hero1 = CardFactory.CreateCard(6, player);
        hero1.IsHero = true;
        var support1 = CardFactory.CreateCard(2, player);
        player.FrontField.AddCard(hero1);
        player.Deck.AddCard(support1);
        var hero2 = CardFactory.CreateCard(6, rival);
        hero2.IsHero = true;
        var orb1 = CardFactory.CreateCard(9, rival);
        var support2 = CardFactory.CreateCard(2, rival);
        rival.FrontField.AddCard(hero2);
        rival.Orb.AddCard(orb1);
        rival.Deck.AddCard(support2);

        Request.SetNextResult(false);
        Request.SetNextResult(false);
        Request.SetNextResult(new List<Card>() { orb1 });
        Game.DoBattle(hero1, hero2);
        Assert.IsTrue(rival.Orb.Count == 0);
        Assert.IsTrue(rival.Hand.Count == 1);
    }
    
    [Test]
    public void DeckReplenishTest()
    {
        Game.Initialize();
        Game.SetTestMode();
        Game.DeckReplenishProcessDisabled = false;
        Game.TurnPlayer = Game.Player;
        var player = Game.Player;
        var rival = Game.Rival;
        var hero1 = CardFactory.CreateCard(6, player);
        hero1.IsHero = true;
        var support1 = CardFactory.CreateCard(2, player);
        var retreat = CardFactory.CreateCard(3, player);
        player.FrontField.AddCard(hero1);
        player.Deck.AddCard(support1);
        player.Retreat.AddCard(retreat);
        var hero2 = CardFactory.CreateCard(6, rival);
        hero2.IsHero = true;
        var orb1 = CardFactory.CreateCard(9, rival);
        var support2 = CardFactory.CreateCard(2, rival);
        rival.FrontField.AddCard(hero2);
        rival.Orb.AddCard(orb1);
        rival.Deck.AddCard(support2);
        Request.SetNextResult(false);
        Request.SetNextResult(false);
        Request.SetNextResult(new List<Card>() { orb1 });
        Game.DoBattle(hero1, hero2);

        Assert.IsTrue(player.Deck.Contains(retreat));
    }
}
