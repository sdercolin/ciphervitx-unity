using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00102Test
{

    [Test]
    public void Skill1Test()
    {
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var card = CardFactory.CreateCard(102, player);
        player.FrontField.AddCard(card);

        Game.TryDoMessage(new EmptyMessage());
        Assert.IsTrue(card.HasUnitNameOf(Strings.Get("card_text_unitname_マルス")));
    }
}
