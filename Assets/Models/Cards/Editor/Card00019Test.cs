using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00019Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『無頼の剣』【常】自分のターン中、このユニットと自分の主人公の他に味方が１体もいない場合、このユニットの戦闘力は＋２０される。
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(19, player);
        var card2 = CardFactory.CreateCard(1, player);

        card.IsHero = true;

        player.FrontField.AddCard(card);
        player.Hand.AddCard(card2);

        Game.TryDoMessage(new EmptyMessage());
        Assert.IsTrue(card.Power == 60);

        Game.DoDeployment(card2, true);
        Assert.IsTrue(card.Power == 40);

        card.IsHero = false;
        card2.IsHero = true;
        Game.TryDoMessage(new EmptyMessage());
        Assert.IsTrue(card.Power == 60);

    }
}
