using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00085Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『ペガサス三姉妹』【起】[横置，翻面2]自分のデッキから出撃コストが２以下の『カチュア』か『エスト』を１枚選び、出撃させる。その後、デッキをシャッフルする。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(85, player);
        var bond1 = CardFactory.CreateCard(9, player);
        var bond2 = CardFactory.CreateCard(9, player);
        var deck1 = CardFactory.CreateCard(87, player);
        var deck2 = CardFactory.CreateCard(89, player);
        player.FrontField.AddCard(card);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);
        player.Deck.AddCard(deck1);
        player.Deck.AddCard(deck2);

        Request.SetNextResult(); //翻面
        Request.SetNextResult(); //选择出击
        Request.SetNextResult(true); //选择前场
        Game.DoActionSkill(card.GetUsableActionSkills()[0]).Wait();

        Assert.IsTrue(player.Deck.Filter(card0 => card0.DeployCost <= 2 && (card0.HasUnitNameOf(Strings.Get("card_text_unitname_カチュア")) || card0.HasUnitNameOf(Strings.Get("card_text_unitname_エスト")))).Count == 1);
        Assert.IsTrue(player.FrontField.Count == 2);
    }
}
