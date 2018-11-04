using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00050Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『説得』【自】[翻面1]このユニットの攻撃で敵を撃破した時、コストを支払うなら、自分のデッキから『シーダ』以外で<光の剣>のカードを１枚選び、公開してから手札に加える。その後、デッキをシャッフルする。
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(50, player);//60         
        var support1 = CardFactory.CreateCard(1, player);//20支援
        var support2 = CardFactory.CreateCard(1, player);

        player.FrontField.AddCard(card);
        player.Deck.AddCard(support1);
        player.Deck.AddCard(support2);
        player.Bond.AddCard(support2);

        var card1 = CardFactory.CreateCard(2, rival);//50
        var rivalSupport1 = CardFactory.CreateCard(1, rival);//20支援

        rival.FrontField.AddCard(card1);
        rival.Deck.AddCard(rivalSupport1);

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Request.SetNextResult();//选择
        Request.SetNextResult(true); //选择使用
        Request.SetNextResult();//翻面
        Request.SetNextResult();//选择
        Game.DoBattle(card, card1).Wait();

        Assert.IsTrue(player.Hand.Count == 1);
    }

    [Test]
    public void Skill2Test()
    {
        /// <summary>
        /// スキル2
        /// 『天空を翔ける者』【起】〖1回合1次〗このユニットを移動させる。このスキルはこのユニットが未行動でなければ使用できない。
        /// </summary> 
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        Game.TurnPlayer = player;

        var card1 = CardFactory.CreateCard(50, player);
        var card2 = CardFactory.CreateCard(1, player);

        player.FrontField.AddCard(card1);
        player.FrontField.AddCard(card2);

        Game.DoActionSkill(card1.GetUsableActionSkills()[0]).Wait();

        Assert.IsTrue(card1.BelongedRegion == player.BackField);
    }
}
