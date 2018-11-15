using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00031Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『神軍師の采配』【自】他の味方がクラスチェンジするたび、敵を１体選び、移動させてもよい。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var lufulei = CardFactory.CreateCard(31, player);
        player.FrontField.AddCard(lufulei);
        var myUnit = CardFactory.CreateCard(98, player);
        player.FrontField.AddCard(myUnit);
        var adv_myUnit = CardFactory.CreateCard(97, player);
        player.Hand.AddCard(adv_myUnit);
        var bonus = CardFactory.CreateCard(1, player);
        player.Deck.AddCard(bonus);

        // 前场后场各1人
        var hisUint1 = CardFactory.CreateCard(31, rival);
        rival.FrontField.AddCard(hisUint1);
        var hisUint2 = CardFactory.CreateCard(97, rival);
        rival.BackField.AddCard(hisUint2);

        // 选择前场，应发生进军
        // ==========================================
        Request.SetNextResult();
        Request.SetNextResult(true);
        Request.SetNextResult(new List<Card>() { hisUint1 }); 
        // case2 end
        Game.DoLevelUp(adv_myUnit).Wait();

        Assert.IsTrue(rival.BackField.Cards.Count == 0);
        // ==========================================
    }

    [Test]
    public void Skill1Test2()
    {
        /// <summary>
        /// スキル1
        /// 『神軍師の采配』【自】他の味方がクラスチェンジするたび、敵を１体選び、移動させてもよい。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var lufulei = CardFactory.CreateCard(31, player);
        player.FrontField.AddCard(lufulei);
        var myUnit = CardFactory.CreateCard(98, player);
        player.FrontField.AddCard(myUnit);
        var adv_myUnit = CardFactory.CreateCard(97, player);
        player.Hand.AddCard(adv_myUnit);
        var bonus = CardFactory.CreateCard(1, player);
        player.Deck.AddCard(bonus);

        // 前场后场各1人
        var hisUint1 = CardFactory.CreateCard(31, rival);
        rival.FrontField.AddCard(hisUint1);
        var hisUint2 = CardFactory.CreateCard(97, rival);
        rival.BackField.AddCard(hisUint2);

        // 选择后场
        // ==========================================
        Request.SetNextResult();
        Request.SetNextResult(true);
        Request.SetNextResult(new List<Card>() { hisUint2 });
        Game.DoLevelUp(adv_myUnit).Wait();

        Assert.IsTrue(rival.BackField.Cards.Count == 0);
    }


    [Test]
    public void Skill2Test()
    {
        /// <summary>
        /// スキル2
        /// 『これも、策のうちです』〖转职技〗【起】[翻面3]自分のオーブの数が相手より少ない場合、デッキの１番上のカードをオーブに追加する。（はこのユニットがクラスチェンジしていなければ使用できない）
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var lufulei = CardFactory.CreateCard(31, player);
        player.FrontField.AddCard(lufulei);

        var adv_lufulei = CardFactory.CreateCard(31, player);
        player.Hand.AddCard(adv_lufulei);

        var bond1 = CardFactory.CreateCard(29, player);
        var bond2 = CardFactory.CreateCard(29, player);
        var bond3 = CardFactory.CreateCard(29, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);
        player.Bond.AddCard(bond3);

        var bonus = CardFactory.CreateCard(30, player);
        player.Deck.AddCard(bonus);
        var card = CardFactory.CreateCard(31, player);
        player.Deck.AddCard(card);

        // 未转职不能发
        int count = lufulei.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 0);

        // Class Change
        Game.DoLevelUp(adv_lufulei).Wait();
        Assert.IsTrue(player.Hand.Contains(bonus));

        // 一般场合
        // ==========================================
        var orb = CardFactory.CreateCard(29, rival);
        rival.Orb.AddCard(orb);

        count = adv_lufulei.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 1);

        Request.SetNextResult(new List<Card>() { bond1, bond2, bond3 }); //设定要翻的费
        Game.DoActionSkill(adv_lufulei.GetUsableActionSkills()[0]).Wait();

        Assert.IsTrue(player.Orb.Contains(card));
        // ===========================================
    }

    [Test]
    public void Skill2Test2()
    {
        /// <summary>
        /// スキル2
        /// 『これも、策のうちです』〖转职技〗【起】[翻面3]自分のオーブの数が相手より少ない場合、デッキの１番上のカードをオーブに追加する。（はこのユニットがクラスチェンジしていなければ使用できない）
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        Game.TurnPlayer = player;

        var lufulei = CardFactory.CreateCard(31, player);
        player.FrontField.AddCard(lufulei);

        var adv_lufulei = CardFactory.CreateCard(31, player);
        player.Hand.AddCard(adv_lufulei);

        var bond1 = CardFactory.CreateCard(29, player);
        var bond2 = CardFactory.CreateCard(29, player);
        var bond3 = CardFactory.CreateCard(29, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);
        player.Bond.AddCard(bond3);

        var bonus = CardFactory.CreateCard(30, player);
        player.Deck.AddCard(bonus);
        var card = CardFactory.CreateCard(31, player);
        player.Deck.AddCard(card);

        // 未转职不能发
        int count = lufulei.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 0);

        // Class Change
        Game.DoLevelUp(adv_lufulei).Wait();
        Assert.IsTrue(player.Hand.Contains(bonus));

        // 血一样的场合，应该是能发，但是空发
        // ==========================================

        count = adv_lufulei.GetUsableActionSkills().Count;
        Assert.IsTrue(count == 1);

        Request.SetNextResult(new List<Card>() { bond1, bond2, bond3 }); //设定要翻的费
        Game.DoActionSkill(adv_lufulei.GetUsableActionSkills()[0]).Wait();

        Assert.IsTrue(player.Orb.Count == 0);
        // ===========================================
    }
}
