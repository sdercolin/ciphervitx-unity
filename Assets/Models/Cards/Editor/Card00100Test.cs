using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00100Test
{
    [Test]
    public void Skill1Test1()
    {
        /// <summary>
        /// スキル1
        /// 『運命に抗う者』【起】〖1回合1次〗[翻面1，自分の手札から『ルキナ』を１枚退避エリアに置く]カードを２枚引き、自分の手札を１枚選び、デッキの１番上に置く。このユニットがクラスチェンジしている場合、代わりにカードを３枚引き、自分の手札を２枚選び、デッキの１番上に好きな順番で置く。
        /// </summary>
        /// 没有转职
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(100, player);
        player.FrontField.AddCard(card);
        var bond1 = CardFactory.CreateCard(90, player);
        player.Bond.AddCard(bond1);
        var hand1 = CardFactory.CreateCard(100, player);
        player.Hand.AddCard(hand1);
        var deck1 = CardFactory.CreateCard(100, player);
        var deck2 = CardFactory.CreateCard(100, player);
        var deck3 = CardFactory.CreateCard(100, player);
        player.Deck.AddCard(deck1);
        player.Deck.AddCard(deck2);
        player.Deck.AddCard(deck3);

        Request.SetNextResult(); //翻面
        Request.SetNextResult();//选择丢弃
        Request.SetNextResult();//放回卡组
        Game.DoActionSkill(card.GetUsableActionSkills()[0]).Wait();

        Assert.IsTrue(player.Hand.Count == 1);
        Assert.IsTrue(player.Deck.Count == 2);
    }

    [Test]
    public void Skill1Test2()
    {
        /// <summary>
        /// スキル1
        /// 『運命に抗う者』【起】〖1回合1次〗[翻面1，自分の手札から『ルキナ』を１枚退避エリアに置く]カードを２枚引き、自分の手札を１枚選び、デッキの１番上に置く。このユニットがクラスチェンジしている場合、代わりにカードを３枚引き、自分の手札を２枚選び、デッキの１番上に好きな順番で置く。
        /// </summary>
        /// 转职
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(100, player);
        player.FrontField.AddCard(card);
        var bond1 = CardFactory.CreateCard(90, player);
        var bond2 = CardFactory.CreateCard(90, player);
        var bond3 = CardFactory.CreateCard(100, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);
        player.Bond.AddCard(bond3);
        var hand1 = CardFactory.CreateCard(100, player);
        var hand2 = CardFactory.CreateCard(100, player);
        player.Hand.AddCard(hand1);
        player.Hand.AddCard(hand2);
        var deck1 = CardFactory.CreateCard(100, player);
        var deck2 = CardFactory.CreateCard(100, player);
        var deck3 = CardFactory.CreateCard(100, player);
        var deck4 = CardFactory.CreateCard(100, player);
        var deck5 = CardFactory.CreateCard(100, player);
        player.Deck.AddCard(deck1);
        player.Deck.AddCard(deck2);
        player.Deck.AddCard(deck3);
        player.Deck.AddCard(deck4);
        player.Deck.AddCard(deck5);

        Game.DoLevelUp(hand1, true).Wait();//转职
        Assert.IsTrue(player.Hand.Count == 2);
        Assert.IsTrue(player.Deck.Count == 4);

        Request.SetNextResult(); //翻面
        Request.SetNextResult();//选择丢弃
        Request.SetNextResult();//放回卡组
        Game.DoActionSkill(hand1.GetUsableActionSkills()[0]).Wait();

        Assert.IsTrue(player.Hand.Count == 2);
        Assert.IsTrue(player.Deck.Count == 3);
    }
}
