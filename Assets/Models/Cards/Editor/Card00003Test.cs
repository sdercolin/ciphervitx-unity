using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00003Test
{

    [Test]
    public void SkillTest()
    {
        Game.Initialize();
        var player = Game.Player as Player;
        var card = new Card00003(player);
        player.Deck.ImportCard(card);
        card.Read(new EmptyMessage());
        Assert.IsTrue(card.AttachableList.Count == 2);

        player.SetToBond(card, true, null);
        Assert.IsFalse(card.BelongedRegion is Bond);
    }
}
