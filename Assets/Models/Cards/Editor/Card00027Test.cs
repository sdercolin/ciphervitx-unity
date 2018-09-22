using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00027Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『魔道研究』【起】[横置，自分の手札を１枚退避エリアに置く]カードを１枚引く。
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        Game.TurnPlayer = player;

        var milieer = CardFactory.CreateCard(27, player);
        player.FrontField.AddCard(milieer);
        var card1 = CardFactory.CreateCard(48, player);
        player.Hand.AddCard(card1);
        var card2 = CardFactory.CreateCard(1, player);
        player.Deck.AddCard(card2);

        Request.SetNextResult();
        Game.DoActionSkill(milieer.GetUsableActionSkills()[0]);

        Assert.IsTrue(player.Hand.Contains(card2));
        Assert.IsTrue(player.Retreat.Contains(card1));
    }
}
