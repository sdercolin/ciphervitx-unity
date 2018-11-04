using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00124Test
{
    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『幼き竜』【自】自分のターン終了時、自分の絆カードを１枚選び、手札に加える。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        Game.TurnPlayer = player;

        // 己方配置
        var card = CardFactory.CreateCard(124, player);
        player.FrontField.AddCard(card);
        var bond = CardFactory.CreateCard(2, player);
        player.Bond.AddCard(bond);

        Request.SetNextResult(); //选择Induction
        Request.SetNextResult(); //选择入手

        Game.EndActionPhase();

        Assert.IsTrue(bond.BelongedRegion == player.Hand);
    }
}
