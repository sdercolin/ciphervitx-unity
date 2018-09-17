using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00020Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『ライブ』【起】[横置，翻面2]自分の退避エリアから『レナ』以外のカードを１枚選び、手札に加える。
        /// </summary>
        /// 
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        Game.TurnPlayer = player;

        var leina1 = CardFactory.CreateCard(20, player);
        var maersi = CardFactory.CreateCard(1, player);
        var leina2 = CardFactory.CreateCard(20, player);

        player.FrontField.AddCard(leina1);
        player.Retreat.AddCard(leina2);

        // 无费，不可发
        int count = leina1.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 0);

        var bond1 = CardFactory.CreateCard(1, player);
        var bond2 = CardFactory.CreateCard(1, player);
        var bond3 = CardFactory.CreateCard(1, player);
        var bond4 = CardFactory.CreateCard(1, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);
        player.Bond.AddCard(bond3);
        player.Bond.AddCard(bond4);

        // 横置，不可发
        leina1.IsHorizontal = true;
        count = leina1.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 0);

        // 可发，无墓地，空发
        leina1.IsHorizontal = false;
        count = leina1.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 1);
        Request.SetNextResult(new List<Card>() { bond1, bond2 });
        Request.SetNextResult(new List<Card>() { });
        Game.DoActionSkill(leina1.GetUsableActionSkills()[0]);
        Assert.IsTrue(player.Hand.Count == 0);

        // 正常发
        leina1.IsHorizontal = false;
        player.Retreat.AddCard(maersi);
        Assert.IsTrue(player.Hand.Count == 0);
        Request.SetNextResult(new List<Card>() { bond3, bond4 });
        Request.SetNextResult(new List<Card>() { maersi });
        Game.DoActionSkill(leina1.GetUsableActionSkills()[0]);
        Assert.IsTrue(player.Hand.Count == 1);
        Assert.IsTrue(player.Hand.Contains(maersi));
    }

    [Test]
    public void Skill2Test()
    {
        /// <summary>
        /// スキル2
        /// 『ジュリアンとの絆』【常】味方の『ジュリアン』の戦闘力は＋１０される。
        /// </summary>
        /// 
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        Game.TurnPlayer = player;

        var leina = CardFactory.CreateCard(20, player);
        var zhulian = CardFactory.CreateCard(73, player);

        player.FrontField.AddCard(zhulian);
        player.Hand.AddCard(leina);

        Assert.IsTrue(zhulian.Power == 50);

        Game.DoDeployment(leina, true);

        Assert.IsTrue(zhulian.Power == 60);

    }
}
