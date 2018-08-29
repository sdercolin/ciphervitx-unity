using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00010Test
{

    [Test]
    public void SkillTest()
    {
        Game.Initialize();
        var player1 = Game.Player;
        var player2 = Game.Rival;
        var card1 = CardFactory.CreateCard(10, player1);
        player1.FrontField.AddCard(card1);
        var card2 = CardFactory.CreateCard(1, player1);
        player1.FrontField.AddCard(card2);
        var card3 = CardFactory.CreateCard(6, player1);
        player1.FrontField.AddCard(card3);
        var card4 = CardFactory.CreateCard(13, player2);
        player2.BackField.AddCard(card4);


        card1.Read(new EmptyMessage());

        Assert.IsFalse(card4.GetAttackableUnits().Contains(card1));
        Assert.IsFalse(card4.GetAttackableUnits().Contains(card3));
        Assert.IsTrue(card4.GetAttackableUnits().Contains(card2));
    }
}
