using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00073Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『鍵開け』【起】[横置]相手のデッキの１番上のカードを公開させる。そのカードの出撃コストが３以上の場合、あなたは翻面1してもよい。そうしたなら、カードを１枚引く。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card1 = CardFactory.CreateCard(73, player);
        var bond = CardFactory.CreateCard(2, player);
        var support1 = CardFactory.CreateCard(1, player);

        player.FrontField.AddCard(card1);
        player.Bond.AddCard(bond);
        player.Deck.AddCard(support1);

        var rivalSupport1 = CardFactory.CreateCard(1, rival);//4C

        rival.Deck.AddCard(rivalSupport1);

        Request.SetNextResult(true);//选择翻面
        Request.SetNextResult();//翻面1

        Game.DoActionSkill(card1.GetUsableActionSkills()[0]);
        Assert.IsTrue(support1.BelongedRegion == player.Hand);//抽进手牌 
    }

    [Test]
    public void Skill2Test()
    {
        /// <summary>
        /// スキル2
        /// 『財宝奪取』【起】[翻面1]相手のデッキの１番上のカードを退避エリアに置かせる。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card1 = CardFactory.CreateCard(73, player);
        var bond = CardFactory.CreateCard(2, player);

        player.FrontField.AddCard(card1);
        player.Bond.AddCard(bond);

        var rivalSupport1 = CardFactory.CreateCard(1, rival);

        rival.Deck.AddCard(rivalSupport1);

        Request.SetNextResult();//翻面1

        Game.DoActionSkill(card1.GetUsableActionSkills()[1]);
        Assert.IsTrue(rivalSupport1.BelongedRegion == rival.Retreat);//送进退避区 
    }
}
