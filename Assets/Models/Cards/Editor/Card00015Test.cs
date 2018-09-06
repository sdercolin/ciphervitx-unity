using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00015Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『傭兵隊長』【常】自分のターン中、出撃コストが２以下の味方が他に２体以上いる場合、このユニットの戦闘力は＋２０される。
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        Game.TurnPlayer = player;
        // 1C奥古马
        var card = CardFactory.CreateCard(15, player);
        // 43马尔斯
        var card1 = CardFactory.CreateCard(1, player);
        // 1C希达
        var card2 = CardFactory.CreateCard(7, player);
        // 1C哥顿
        var card3 = CardFactory.CreateCard(14, player);

        player.FrontField.AddCard(card);
        player.Hand.AddCard(card1);
        player.Hand.AddCard(card2);
        player.Hand.AddCard(card3);

        player.Deploy(card1, true);
        Assert.IsTrue(card.Power == 40);

        player.Deploy(card2, true);
        Assert.IsTrue(card.Power == 40);

        player.Deploy(card3, true);
        Assert.IsTrue(card.Power == 60);
    }
}
