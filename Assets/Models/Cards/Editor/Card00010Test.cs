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
        var card1 = new Card00010(player1);
        player1.Deck.ImportCard(card1);
		card1.MoveTo(player1.FrontField);
		var card2 = new Card00001(player1);
        player1.Deck.ImportCard(card2);
		card2.MoveTo(player1.FrontField);
		var card3 = new Card00006(player1);
        player1.Deck.ImportCard(card3);
		card3.MoveTo(player1.FrontField);
		var card4 = new Card00013(player2);
        player2.Deck.ImportCard(card4);
		card4.MoveTo(player2.BackField);
		
        card1.Read(new EmptyMessage());
		
        Assert.IsFalse(card4.GetAttackableUnits().Contains(card1));
		Assert.IsFalse(card4.GetAttackableUnits().Contains(card3));
		Assert.IsTrue(card4.GetAttackableUnits().Contains (card2));
    }
}
