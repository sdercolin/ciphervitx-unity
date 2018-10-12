using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00078Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『リザーブ』【起】[横置，翻面3]自分の退避エリアから『マリア』以外でユニット名が異なるカードを２枚まで選び、手札に加える。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(78, player);
        var retreat1 = CardFactory.CreateCard(78, player);
        var retreat2 = CardFactory.CreateCard(1, player);
        var retreat3 = CardFactory.CreateCard(1, player);
        var retreat4 = CardFactory.CreateCard(1, player);

        player.FrontField.AddCard(card);
        player.Retreat.AddCard(retreat1);

        // 无费，不可发
        int count = card.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 0);

        var bond1 = CardFactory.CreateCard(1, player);
        var bond2 = CardFactory.CreateCard(1, player);
        var bond3 = CardFactory.CreateCard(1, player);
        var bond4 = CardFactory.CreateCard(1, player);
        var bond5 = CardFactory.CreateCard(1, player);
        var bond6 = CardFactory.CreateCard(1, player);
        var bond7 = CardFactory.CreateCard(1, player);
        var bond8 = CardFactory.CreateCard(1, player);
        var bond9 = CardFactory.CreateCard(1, player);

        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);
        player.Bond.AddCard(bond3);
        player.Bond.AddCard(bond4);
        player.Bond.AddCard(bond5);
        player.Bond.AddCard(bond6);
        player.Bond.AddCard(bond7);
        player.Bond.AddCard(bond8);
        player.Bond.AddCard(bond9);

        // 横置，不可发
        card.IsHorizontal = true;
        count = card.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 0);

        // 可发，无墓地，空发
        card.IsHorizontal = false;
        count = card.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 1);
        Request.SetNextResult();
        Request.SetNextResult();
        Game.DoActionSkill(card.GetUsableActionSkills()[0]);
        Assert.IsTrue(player.Hand.Count == 0);

        // 1墓地
        card.IsHorizontal = false;
        player.Retreat.AddCard(retreat2);
        Request.SetNextResult();
        Request.SetNextResult();
        Game.DoActionSkill(card.GetUsableActionSkills()[0]);
        Assert.IsTrue(player.Hand.Count == 1);
        Assert.IsTrue(player.Hand.Contains(retreat2));

        // 2墓地
        card.IsHorizontal = false;
        player.Retreat.AddCard(retreat3);
        player.Retreat.AddCard(retreat4);
        Request.SetNextResult();
        Request.SetNextResult();
        Game.DoActionSkill(card.GetUsableActionSkills()[0]);
        Assert.IsTrue(player.Hand.Count == 3);
        Assert.IsTrue(player.Hand.Contains(retreat3));
        Assert.IsTrue(player.Hand.Contains(retreat4));
    }

}
