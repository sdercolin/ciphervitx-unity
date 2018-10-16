using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00121Test
{
    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『疾風迅雷』【自】〖1回合1次〗このユニットの攻撃で敵を撃破した時、このユニットを未行動にする。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var card = CardFactory.CreateCard(121, player);//50
        player.FrontField.AddCard(card);
        var support1 = CardFactory.CreateCard(2, player);//30支援
        player.Deck.AddCard(support1);

        // 敌方配置
        var rivalCard = CardFactory.CreateCard(1, rival);//70战斗力
        rival.FrontField.AddCard(rivalCard);
        var rivalSupport = CardFactory.CreateCard(4, rival);//10支援
        rival.Deck.AddCard(rivalSupport);

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Request.SetNextResult(); //选择Induction

        Game.DoBattle(card, rivalCard);

        Assert.IsTrue(card.IsHorizontal == false);
    }

    [Test]
    public void Skill2Test()
    {
        /// <summary>
        /// スキル2
        /// 『手製の手槍』〖转职技〗【起】[翻面1]ターン終了まで、すべての<飛行>の味方に<槍>と射程１-２が追加される。（はこのユニットがクラスチェンジしていなければ使用できない）
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var card = CardFactory.CreateCard(121, player);
        player.FrontField.AddCard(card);
        var hand = CardFactory.CreateCard(121, player);
        player.Hand.AddCard(hand);
        var unit = CardFactory.CreateCard(2, player);
        player.FrontField.AddCard(unit);
        var bond = CardFactory.CreateCard(2, player);
        player.Bond.AddCard(bond);
        var deck = CardFactory.CreateCard(2, player);
        player.Deck.AddCard(deck);

        Game.DoLevelUp(hand, true);
        Request.SetNextResult(); //翻面1
        Game.DoActionSkill(hand.GetUsableActionSkills()[0]);

        Assert.IsTrue(hand.HasRange(RangeEnum.OnetoTwo));
        Assert.IsTrue(unit.HasRange(RangeEnum.OnetoTwo));
    }
}
