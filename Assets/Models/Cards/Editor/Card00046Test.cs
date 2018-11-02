using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00046Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『天空の運び手』【起】[横置]他の味方を１体選び、移動させる。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        Game.TurnPlayer = player;
        int count = 0;

        var myUnit = CardFactory.CreateCard(46, player);
        player.FrontField.AddCard(myUnit);
        var myUnit2 = CardFactory.CreateCard(1, player);
        player.FrontField.AddCard(myUnit2);

        // 横置时不能发
        myUnit.IsHorizontal = true;
        count = myUnit.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 0);

        // 可以发
        myUnit.IsHorizontal = false;
        count = myUnit.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 1);

        Request.SetNextResult();
        Game.DoActionSkill(myUnit.GetUsableActionSkills()[0]);
        Assert.IsTrue(player.BackField.Contains(myUnit2));

    }
}
