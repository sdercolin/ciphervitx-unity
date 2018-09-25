using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00084Test
{
    /// <summary>
    /// スキル1
    /// 『トライアングルアタック』〖阵型技〗【自】[味方の『カチュア』と『エスト』を行動済みにする]このユニットが攻撃した時、コストを支払うなら、戦闘終了まで、このユニットの戦闘力は＋５０され、攻撃は神速回避されない。
    /// </summary>
    [Test]
    public void Skill1Test()
    {
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var dajie = CardFactory.CreateCard(84, player);
        player.FrontField.AddCard(dajie);
        var erjie = CardFactory.CreateCard(86, player);
        player.FrontField.AddCard(erjie);
        var sanmei = CardFactory.CreateCard(88, player);
        player.FrontField.AddCard(sanmei);
        var support = CardFactory.CreateCard(84, player);//支援失败
        player.Deck.AddCard(support);

        // 敌方配置
        var hisHero = CardFactory.CreateCard(1, rival);//70战斗力
        rival.FrontField.AddCard(hisHero);
        hisHero.IsHero = true;
        var hisOrb = CardFactory.CreateCard(6, rival);
        rival.Orb.AddCard(hisOrb);
        var hisSupport = CardFactory.CreateCard(2, rival);//30支援
        rival.Deck.AddCard(hisSupport);
        var hisHand = CardFactory.CreateCard(1, rival);
        rival.Hand.AddCard(hisHand);


        Request.SetNextResult(); //选择被诱发的Skill
        Request.SetNextResult(true); //选择发动Skill
        Request.SetNextResult(); //横置1
        Request.SetNextResult(); //横置2
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(true); //回避，但无效
        Request.SetNextResult(); //拿一个宝玉
        Game.DoBattle(dajie, hisHero);

        Assert.IsTrue(rival.Orb.Count == 0);
    }

    /// <summary>
    /// スキル2
    /// 『三姉妹の絆』【常】このユニットが『カチュア』か『エスト』に支援されている場合、このユニットの戦闘力は＋１０される。
    /// </summary>
    [Test]
    public void Skill2Test()
    {
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var dajie = CardFactory.CreateCard(84, player);//50攻击
        player.FrontField.AddCard(dajie);
        var erjie = CardFactory.CreateCard(86, player);//40支援
        player.Deck.AddCard(erjie);

        // 敌方配置
        var hisHero = CardFactory.CreateCard(1, rival);//70攻击
        rival.FrontField.AddCard(hisHero);
        hisHero.IsHero = true;
        var hisOrb = CardFactory.CreateCard(6, rival);
        rival.Orb.AddCard(hisOrb);
        var hisSupport = CardFactory.CreateCard(3, rival);//20支援
        rival.Deck.AddCard(hisSupport);

        Request.SetNextResult();
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Request.SetNextResult(); //拿一个宝玉
        Game.DoBattle(dajie, hisHero);

        Assert.IsTrue(rival.Orb.Count == 0);
    }
}
