using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00024Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『クロム自警団』【自】〖1回合1次〗他の味方がクラスチェンジした時、ターン終了まで、その味方の戦闘力は＋２０され、その味方が攻撃で破壊するオーブは２つになる。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var kuluomu = CardFactory.CreateCard(24, player);
        player.FrontField.AddCard(kuluomu);
        var maersi = CardFactory.CreateCard(48, player);
        player.FrontField.AddCard(maersi);
        var maersi_adv = CardFactory.CreateCard(1, player);
        player.Hand.AddCard(maersi_adv);
        var bonus = CardFactory.CreateCard(1, player);// class change bonus
        player.Deck.AddCard(bonus);
        var support = CardFactory.CreateCard(62, player);// 10
        player.Deck.AddCard(support);


        var enemy = CardFactory.CreateCard(24, rival);
        rival.FrontField.AddCard(enemy);
        enemy.IsHero = true;
        var support2 = CardFactory.CreateCard(50, rival);//30
        rival.Deck.AddCard(support2);
        var orb = CardFactory.CreateCard(1, rival);
        rival.Orb.AddCard(orb);
        var orb2 = CardFactory.CreateCard(1, rival);
        rival.Orb.AddCard(orb2);

        Request.SetNextResult();
        Game.DoLevelUp(maersi_adv, true).Wait();
        Assert.IsTrue(player.Hand.Contains(bonus));
        Assert.IsTrue(maersi_adv.Power == 90); // +20

        // 攻击
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Request.SetNextResult(); //拿走一个宝玉
        Request.SetNextResult(); //拿走一个宝玉
        Game.DoBattle(maersi_adv, enemy).Wait();
        Assert.IsTrue(rival.Orb.Count == 0); //应该被击破了
    }

    [Test]
    public void Skill2Test()
    {
        /// <summary>
        /// スキル2
        /// 『封剣 ファルシオン』【常】このユニットが<竜>を攻撃している場合、このユニットの戦闘力は＋２０される。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var kuluomu = CardFactory.CreateCard(24, player);
        player.FrontField.AddCard(kuluomu);
        var support = CardFactory.CreateCard(24, player);// support failed
        player.Deck.AddCard(support);

        var zhiqi = CardFactory.CreateCard(92, rival);
        rival.FrontField.AddCard(zhiqi);
        zhiqi.IsHero = true;
        var support2 = CardFactory.CreateCard(24, rival);//20
        rival.Deck.AddCard(support2);
        var orb = CardFactory.CreateCard(1, rival);
        rival.Orb.AddCard(orb);

        // 攻击
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Request.SetNextResult(); //拿走一个宝玉
        Game.DoBattle(kuluomu, zhiqi).Wait();
        Assert.IsTrue(rival.Orb.Count == 0); //应该被击破了
    }

}
