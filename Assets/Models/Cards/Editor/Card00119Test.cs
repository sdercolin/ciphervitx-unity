using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00119Test
{
    /// <summary>
    /// スキル1
    /// 『暗殺』【起】[横置]相手のデッキの１番上のカードを公開させる。そのカードの出撃コストが３以上の場合、あなたは翻面2してもよい。そうしたなら、敵を１体選び、撃破する。
    /// </summary>
    /// <summary>
    /// スキル2
    /// 『報酬はスイーツ』〖转职技〗【自】このユニットの『暗殺』で敵を撃破した時、カードを１枚引く。（はこのユニットがクラスチェンジしていなければ発動しない）
    /// </summary>
    [Test]
    public void Skill1Test1()
    {
        // 转职&击破
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var gaiya_1C = CardFactory.CreateCard(119, player);
        player.FrontField.AddCard(gaiya_1C);
        var gaiya = CardFactory.CreateCard(119, player);
        player.Hand.AddCard(gaiya);
        var bonus = CardFactory.CreateCard(84, player);
        player.Deck.AddCard(bonus);
        var draw = CardFactory.CreateCard(42, player);
        player.Deck.AddCard(draw);

        var bond1 = CardFactory.CreateCard(1, player);
        player.Bond.AddCard(bond1);
        var bond2 = CardFactory.CreateCard(2, player);
        player.Bond.AddCard(bond2);

        // 敌方配置
        var hisUint1 = CardFactory.CreateCard(97, rival);
        rival.FrontField.AddCard(hisUint1);
        hisUint1.IsHero = true;
        var hisCard = CardFactory.CreateCard(62, rival);// 43
        rival.Deck.AddCard(hisCard);
        var orb = CardFactory.CreateCard(15, rival);
        rival.Orb.AddCard(orb);

        Game.DoLevelUp(gaiya, true).Wait();

        Request.SetNextResult(true);//选择付费
        Request.SetNextResult(new List<Card>() { bond1, bond2 }); //设定要翻的费
        Request.SetNextResult();//选择对手主人公
        Request.SetNextResult();

        Game.DoActionSkill(gaiya.GetUsableActionSkills()[0]).Wait();
        Assert.IsTrue(rival.Orb.Count == 0);
        Assert.IsTrue(player.Hand.Contains(draw));
    }

    // 未转职&击破
    [Test]
    public void Skill1Test2()
    {
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var gaiya = CardFactory.CreateCard(119, player);
        player.FrontField.AddCard(gaiya);
        //var gaiya = CardFactory.CreateCard(119, player);
        //player.Hand.AddCard(gaiya);
        //var bonus = CardFactory.CreateCard(84, player);
        //player.Deck.AddCard(bonus);
        var draw = CardFactory.CreateCard(42, player);
        player.Deck.AddCard(draw);

        var bond1 = CardFactory.CreateCard(1, player);
        player.Bond.AddCard(bond1);
        var bond2 = CardFactory.CreateCard(2, player);
        player.Bond.AddCard(bond2);

        // 敌方配置
        var hisUint1 = CardFactory.CreateCard(97, rival);
        rival.FrontField.AddCard(hisUint1);
        hisUint1.IsHero = true;
        var hisCard = CardFactory.CreateCard(62, rival);// 43
        rival.Deck.AddCard(hisCard);
        var orb = CardFactory.CreateCard(15, rival);
        rival.Orb.AddCard(orb);

        //Game.DoLevelUp(gaiya, true);

        Request.SetNextResult(true);//选择付费
        Request.SetNextResult(new List<Card>() { bond1, bond2 }); //设定要翻的费
        Request.SetNextResult();//选择对手主人公
        Request.SetNextResult();

        Game.DoActionSkill(gaiya.GetUsableActionSkills()[0]).Wait();
        Assert.IsTrue(rival.Orb.Count == 0);
        Assert.IsTrue(player.Hand.Count  == 0);
    }

    // 未转职&未击破
    [Test]
    public void Skill1Test3()
    {
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var gaiya = CardFactory.CreateCard(119, player);
        player.FrontField.AddCard(gaiya);
        //var gaiya = CardFactory.CreateCard(119, player);
        //player.Hand.AddCard(gaiya);
        //var bonus = CardFactory.CreateCard(84, player);
        //player.Deck.AddCard(bonus);
        var draw = CardFactory.CreateCard(42, player);
        player.Deck.AddCard(draw);

        var bond1 = CardFactory.CreateCard(1, player);
        player.Bond.AddCard(bond1);
        var bond2 = CardFactory.CreateCard(2, player);
        player.Bond.AddCard(bond2);

        // 敌方配置
        var hisUint1 = CardFactory.CreateCard(97, rival);
        rival.FrontField.AddCard(hisUint1);
        hisUint1.IsHero = true;
        var hisCard = CardFactory.CreateCard(77, rival);// 1C
        rival.Deck.AddCard(hisCard);
        var orb = CardFactory.CreateCard(15, rival);
        rival.Orb.AddCard(orb);

        //Game.DoLevelUp(gaiya, true);

        Request.SetNextResult(true);//选择付费
        Request.SetNextResult(new List<Card>() { bond1, bond2 }); //设定要翻的费
        Request.SetNextResult();//选择对手主人公
        Request.SetNextResult();

        Game.DoActionSkill(gaiya.GetUsableActionSkills()[0]).Wait();
        Assert.IsTrue(rival.Orb.Count == 1);
        Assert.IsTrue(player.Hand.Count == 0);
    }
}
