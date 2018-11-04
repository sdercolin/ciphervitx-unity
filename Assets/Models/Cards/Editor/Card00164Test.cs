using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00164Test
{
    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『炎魔の陣』【起】〖1回合1次〗[他の味方を２体行動済みにする]ターン終了まで、このユニットの戦闘力は＋２０される。
        /// </summary>
        /// 
        /// <summary>
        /// スキル2
        /// 『強行偵察』【自】このユニットの攻撃で敵を撃破した時、このユニットがこのターンに『炎魔の陣』を使用しているなら、相手のデッキの１番上のカードを公開させる。あなたはそのカードを退避エリアに置かせてもよい。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        // 己方配置
        var caizang = CardFactory.CreateCard(164, player); //战斗力50
        player.FrontField.AddCard(caizang);
        var cost1 = CardFactory.CreateCard(2, player);
        player.FrontField.AddCard(cost1);
        var cost2 = CardFactory.CreateCard(3, player);
        player.FrontField.AddCard(cost2);
        var support = CardFactory.CreateCard(2, player); //支援力30
        player.Deck.AddCard(support);

        // 敌方配置
        var rivalUnit = CardFactory.CreateCard(1, rival); //战斗力70
        rival.FrontField.AddCard(rivalUnit);
        var rivalSupport = CardFactory.CreateCard(2, rival); //支援力30
        rival.Deck.AddCard(rivalSupport);
        var rivalTop = CardFactory.CreateCard(1, rival);
        rival.Deck.AddCard(rivalTop);

        Request.SetNextResult(); //设置横置的单位
        Game.DoActionSkill(caizang.GetUsableActionSkills()[0]).Wait();

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Request.SetNextResult(); //选择诱发
        Request.SetNextResult(true); //选择丢弃
        Game.DoBattle(caizang, rivalUnit).Wait();

        Assert.IsTrue(rival.Retreat.Contains(rivalTop));
    }
}
