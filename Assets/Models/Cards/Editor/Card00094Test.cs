using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00094Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『オーム』【起】[横置，翻面1]自分の退避エリアから出撃コストが２以下の<光の剣>のカードを１枚選び、出撃させる。そうしたなら、ゲーム終了まで、すべての味方は『オーム』を使用できない。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(94, player);
        var retreat1 = CardFactory.CreateCard(6, player);
        var retreat2 = CardFactory.CreateCard(7, player);
        player.FrontField.AddCard(card);
        player.Retreat.AddCard(retreat1);
        player.Retreat.AddCard(retreat2);

        var bond1 = CardFactory.CreateCard(1, player);
        var bond2 = CardFactory.CreateCard(1, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);

        Request.SetNextResult();//翻面1
        Request.SetNextResult();//选择出击
        Request.SetNextResult(true);//选择前场
        Game.DoActionSkill(card.GetUsableActionSkills()[0]).Wait();
        Assert.IsTrue(player.FrontField.Count == 2);

        card.IsHorizontal = false;
        Assert.IsTrue(card.GetUsableActionSkills().Count == 0);//不能再发
    }

}
