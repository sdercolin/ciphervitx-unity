using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00021Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『エクスカリバー』【起】〖1回合1次〗[翻面1]ターン終了まで、このユニットは『飛行特効』を得る。（『飛行特効』【常】このユニットが<飛行>を攻撃している場合、このユニットの戦闘力は＋３０される。）
        /// </summary>
         /// <summary>
        /// スキル2
        /// 『風の超魔法』【自】このユニットの攻撃で敵を撃破した時、このユニットがこのターンに『エクスカリバー』を使用しているなら、カードを１枚引く。
        /// </summary>
        Game.Initialize();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var malike = CardFactory.CreateCard(21, player);
        player.BackField.AddCard(malike);
        var support = CardFactory.CreateCard(21, player);//支援失败
        player.Deck.AddCard(support);
        var draw = CardFactory.CreateCard(1, player);
        player.Deck.AddCard(draw);
        var bond = CardFactory.CreateCard(1, player);
        player.Bond.AddCard(bond);

        var xida = CardFactory.CreateCard(50, rival);//43希达
        rival.FrontField.AddCard(xida);
        xida.IsHero = true;
        var support2 = CardFactory.CreateCard(77, rival);//30
        rival.Deck.AddCard(support2);
        var orb = CardFactory.CreateCard(1, rival);
        rival.Orb.AddCard(orb);

        // 翻面
        Request.SetNextResult(new List<Card>() { bond });
        Game.DoActionSkill(malike.GetUsableActionSkills()[0]);

        // 攻击
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Request.SetNextResult(); //拿走一个宝玉
        Request.SetNextResult(); //默认选择第一个Induction
        Request.SetNextResult(true);// draw one card
        Game.DoBattle(malike, xida);
        Assert.IsTrue(rival.Orb.Count == 0); //应该被击破了

        Assert.IsTrue(player.Hand.Count == 1);//draw one card
        Assert.IsTrue(player.Hand.Contains(draw));

    }
}
