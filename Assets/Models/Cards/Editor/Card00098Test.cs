using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00098Test
{
    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『勇敢なる王子』【起】[翻面2]敵を１体選び、移動させる。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(98, player);
        player.FrontField.AddCard(card);
        var bond1 = CardFactory.CreateCard(90, player);
        var bond2 = CardFactory.CreateCard(90, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);

        var rivalUnit1 = CardFactory.CreateCard(6, rival);
        var rivalUnit2 = CardFactory.CreateCard(7, rival);
        rival.FrontField.AddCard(rivalUnit1);
        rival.BackField.AddCard(rivalUnit2);

        Request.SetNextResult(); //翻面
        Request.SetNextResult(); //选择
        Game.DoActionSkill(card.GetUsableActionSkills()[0]);

        Assert.IsTrue(rival.FrontField.Count == 2);
        Assert.IsTrue(rival.BackField.Count == 0);
    }

}
