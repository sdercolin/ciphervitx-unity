using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00125Test
{
    [Test]
    public void Skill2Test()
    {
        /// <summary>
        /// スキル2
        /// 『バイオリズム・奇数』【常】自分の絆カードの枚数が奇数の場合、このユニットの戦闘力は＋１０される。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var card = CardFactory.CreateCard(125, player);
        player.FrontField.AddCard(card);
        var bond1 = CardFactory.CreateCard(2, player);
        player.Bond.AddCard(bond1);
        var bond2 = CardFactory.CreateCard(2, player);

        Game.TryDoMessage(new EmptyMessage());
        Assert.IsTrue(card.Power == 30);

        player.Bond.AddCard(bond2);
        Game.TryDoMessage(new EmptyMessage());

        Assert.IsTrue(card.Power == 20);
    }
}
