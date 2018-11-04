using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00006Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『若き英雄』【起】[横置，他の味方を１体行動済みにする]敵を１体選び、移動させる。このスキルはこのユニットが前衛でなければ使用できない。
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card1 = CardFactory.CreateCard(6, player);
        player.FrontField.AddCard(card1);
        var card2 = CardFactory.CreateCard(2, player);
        player.FrontField.AddCard(card2);

        var card3 = CardFactory.CreateCard(3, rival);
        rival.FrontField.AddCard(card3);
        var card4 = CardFactory.CreateCard(4, rival);
        rival.FrontField.AddCard(card4);

        Request.SetNextResult();//选择横置
        Request.SetNextResult();//选择移动
        Game.DoActionSkill(card1.GetUsableActionSkills()[0]).Wait();

        Assert.IsTrue(rival.BackField.Count==1);
    }
}
