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
        var player = Game.Player;
        var card = CardFactory.CreateCard(3, player);
        player.Hand.AddCard(card);
        card.Read(new EmptyMessage());
        Assert.IsTrue(card.AttachableList.Count == 2);

        player.SetToBond(card, true, null);
        Assert.IsFalse(card.BelongedRegion is Bond);
    }
}
