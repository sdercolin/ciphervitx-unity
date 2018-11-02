using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00092Test
{
    /// <summary>
    /// スキル1
    /// 『竜姫の微笑み』【常】自分のターン中、このユニットの支援に成功したカードが退避エリアに置かれる場合、代わりに自分の絆エリアに置いてもよい。
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
        var tiki = CardFactory.CreateCard(92, player);
        player.FrontField.AddCard(tiki);
        var support1 = CardFactory.CreateCard(92, player);//支援失败
        player.Deck.AddCard(support1);
        var support2 = CardFactory.CreateCard(2, player);//支援成功
        player.Deck.AddCard(support2);

        // 敌方配置
        var rivalCard = CardFactory.CreateCard(1, rival);//70战斗力
        rival.FrontField.AddCard(rivalCard);
        var rivalSupport1 = CardFactory.CreateCard(2, rival);//30支援
        rival.Deck.AddCard(rivalSupport1);
        var rivalSupport2 = CardFactory.CreateCard(2, rival);//30支援
        rival.Deck.AddCard(rivalSupport2);

        //支援失败
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Game.DoBattle(tiki, rivalCard);

        Assert.IsTrue(player.Bond.Count == 0);

        //支援成功
        tiki.IsHorizontal = false;

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Game.DoBattle(tiki, rivalCard);

        Assert.IsTrue(player.Bond.Count == 1);
    }

    /// <summary>
    /// スキル2
    /// 『長寿な竜一族』【常】自分の絆カードが８枚以上の場合、このユニットの戦闘力は＋３０される。
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
        var tiki = CardFactory.CreateCard(92, player);
        player.FrontField.AddCard(tiki);
        var bond1 = CardFactory.CreateCard(1, player);
        var bond2 = CardFactory.CreateCard(1, player);
        var bond3 = CardFactory.CreateCard(1, player);
        var bond4 = CardFactory.CreateCard(1, player);
        var bond5 = CardFactory.CreateCard(1, player);
        var bond6 = CardFactory.CreateCard(1, player);
        var bond7 = CardFactory.CreateCard(1, player);
        var bond8 = CardFactory.CreateCard(1, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);
        player.Bond.AddCard(bond3);
        player.Bond.AddCard(bond4);
        player.Bond.AddCard(bond5);
        player.Bond.AddCard(bond6);
        player.Bond.AddCard(bond7);
        player.Bond.AddCard(bond8);

        Game.DoMessage(new EmptyMessage());
        Assert.IsTrue(tiki.Power == 90);
    }
}
