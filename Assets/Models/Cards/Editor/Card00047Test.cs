using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00047Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『英雄の凱歌』【起】[翻面3，自分の手札から『マルス』を１枚退避エリアに置く]次の相手のターン終了まで、すべての味方の戦闘力は＋３０される。
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var maersi = CardFactory.CreateCard(47, player);
        player.FrontField.AddCard(maersi);
        var cost = CardFactory.CreateCard(47, player);
        player.Hand.AddCard(cost);
        var myUnit1 = CardFactory.CreateCard(84, player);//50攻
        player.FrontField.AddCard(myUnit1);
        var myUnit2 = CardFactory.CreateCard(3, player);//70攻
        player.FrontField.AddCard(myUnit2);

        var bond1 = CardFactory.CreateCard(1, player);
        player.Bond.AddCard(bond1);
        var bond2 = CardFactory.CreateCard(2, player);
        player.Bond.AddCard(bond2);
        var bond3 = CardFactory.CreateCard(4, player);
        player.Bond.AddCard(bond3);

        Assert.IsTrue(maersi.Power == 70);
        Assert.IsTrue(myUnit1.Power == 50);
        Assert.IsTrue(myUnit2.Power == 70);

        Request.SetNextResult(new List<Card>() { bond1, bond2, bond3 }); //设定要翻的费
        Request.SetNextResult();//丢同名
        Game.DoActionSkill(maersi.GetUsableActionSkills()[0]);

        Assert.IsTrue(maersi.Power == 100);
        Assert.IsTrue(myUnit1.Power == 80);
        Assert.IsTrue(myUnit2.Power == 100);

    }

}
