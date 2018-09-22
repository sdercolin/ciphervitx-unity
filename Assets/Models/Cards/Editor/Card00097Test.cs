using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00097Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『聖王の威光』【起】[翻面3，自分の手札から『クロム』を１枚退避エリアに置く]敵を好きな数だけ選び、移動させる。ターン終了まで、すべての味方の戦闘力は＋３０される。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = rival;

        // 己方配置
        var kuluomu = CardFactory.CreateCard(97, player);
        player.FrontField.AddCard(kuluomu);
        var cost = CardFactory.CreateCard(97, player);
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

        // 敌方配置
        var hisUint1 = CardFactory.CreateCard(97, rival);
        rival.FrontField.AddCard(hisUint1);
        var hisUint2 = CardFactory.CreateCard(24, rival);
        rival.FrontField.AddCard(hisUint2);
        var hisUint3 = CardFactory.CreateCard(15, rival);
        rival.BackField.AddCard(hisUint3);


        Assert.IsTrue(kuluomu.Power == 70);
        Assert.IsTrue(myUnit1.Power == 50);
        Assert.IsTrue(myUnit2.Power == 70);

        Request.SetNextResult(new List<Card>() { bond1, bond2, bond3 }); //设定要翻的费
        Request.SetNextResult();//丢同名
        Request.SetNextResult(new List<Card>() { hisUint2, hisUint3 });
        Game.DoActionSkill(kuluomu.GetUsableActionSkills()[0]);

        Assert.IsTrue(kuluomu.Power == 100);
        Assert.IsTrue(myUnit1.Power == 80);
        Assert.IsTrue(myUnit2.Power == 100);

        Assert.IsTrue(rival.BackField.Contains(hisUint2));
        Assert.IsTrue(rival.FrontField.Contains(hisUint3));

    }
}
