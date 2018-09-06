using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00007Test
{

    [Test]
    public void SkillTest()
    {
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        Game.TurnPlayer = player;
        var xida = CardFactory.CreateCard(7, player);
        var costCard = CardFactory.CreateCard(1, player);
        var targetCard = CardFactory.CreateCard(3, player);

        player.FrontField.AddCard(xida);
        player.FrontField.AddCard(costCard);
        player.FrontField.AddCard(costCard);

        Request.SetNextResult(new List<Card>() { costCard });
        Request.SetNextResult(new List<Card>() { targetCard });
        Game.DoActionSkill(xida.GetUsableActionSkills()[0]);

        Assert.IsTrue(targetCard.Power == 80);
    }
}
