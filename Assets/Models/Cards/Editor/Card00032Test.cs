using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00032Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『軍師の才』【常】自分のオーブの数が相手より少ない場合、このユニットの戦闘力は＋１０される。
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card1 = CardFactory.CreateCard(32, player);
        player.FrontField.AddCard(card1);

        Assert.IsTrue(card1.Power == 30);//相等

        var orb1 = CardFactory.CreateCard(32, rival);
        rival.Orb.AddCard(orb1);
        Game.TryDoMessage(new EmptyMessage());

        Assert.IsTrue(player.Orb.Count < rival.Orb.Count);//少
        Assert.IsTrue(card1.Power == 40);
    }
}
