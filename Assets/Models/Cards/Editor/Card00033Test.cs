using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00033Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『闘う修道女』〖转职技〗【自】このユニットの攻撃で敵を撃破した時、自分の退避エリアから『リズ』以外で出撃コストが３以下のカードを１枚選び、手札に加える。（はこのユニットがクラスチェンジしていなければ発動しない）
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var lizi = CardFactory.CreateCard(33, player);
        player.FrontField.AddCard(lizi);
        var adv_lizi = CardFactory.CreateCard(33, player);
        player.Hand.AddCard(adv_lizi);
        var bonus = CardFactory.CreateCard(33, player);
        player.Deck.AddCard(bonus);

        var support = CardFactory.CreateCard(84, player);//3C天马，支援区，不可回收
        player.Deck.AddCard(support);
        var card1 = CardFactory.CreateCard(33, player); //3C莉兹，不可回收
        player.Retreat.AddCard(card1);
        var card2 = CardFactory.CreateCard(108, player);//3C维奥尔，可回收
        player.Retreat.AddCard(card2);
        var card3 = CardFactory.CreateCard(1, player);//4C，不可回收
        player.Retreat.AddCard(card3);

        // 敌方配置
        var enemy = CardFactory.CreateCard(33, rival);
        enemy.IsHero = true;
        rival.FrontField.AddCard(enemy);
        var support2 = CardFactory.CreateCard(33, rival);
        rival.Deck.AddCard(support2);
        var orb = CardFactory.CreateCard(33, rival);
        rival.Orb.AddCard(orb);

        Game.DoLevelUp(adv_lizi, true);
        Assert.IsTrue(player.Hand.Contains(bonus));

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Request.SetNextResult(); //拿走一个宝玉
        Request.SetNextResult();//触发技能
        Request.SetNextResult();//拿第一个
        Game.DoBattle(adv_lizi, enemy);
        Assert.IsTrue(player.Hand.Contains(card2));//只能拿3C维奥尔
        Assert.IsTrue(rival.Orb.Count == 0); //应该被击破了

    }

}
