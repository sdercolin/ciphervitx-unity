using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00030Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『聖なる血脈』【起】〖1回合1次〗[翻面1]ターン終了まで、このユニットと味方の『クロム』の戦闘力は＋１０される。このスキルは味方に『クロム』がいなければ使用できない。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        Game.TurnPlayer = player;

        var luqina = CardFactory.CreateCard(30, player);
        player.FrontField.AddCard(luqina);
        var kuluomu = CardFactory.CreateCard(29, player);
        player.Hand.AddCard(kuluomu);

        var bond = CardFactory.CreateCard(29, player);
        player.Bond.AddCard(bond);

        int count = luqina.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 0);

        Game.DoDeployment(kuluomu, true);
        count = luqina.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 1);

        Request.SetNextResult(new List<Card>() { bond }); // Request.SetNextResult(); 也可
        Game.DoActionSkill(luqina.GetUsableActionSkills()[0]);

        Assert.IsTrue(luqina.Power == 60);
        Assert.IsTrue(kuluomu.Power == 50);

    }
}
