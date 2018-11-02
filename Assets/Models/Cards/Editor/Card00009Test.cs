using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00009Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『赤と緑の絆』【常】このユニットが『アベル』に支援されている場合、このユニットの戦闘力は＋３０される。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;
    
        var kayin = CardFactory.CreateCard(9, player);
        player.FrontField.AddCard(kayin);
        var support = CardFactory.CreateCard(11, player);
        player.Deck.AddCard(support);

        var enemy = CardFactory.CreateCard(1, rival);//70
        rival.FrontField.AddCard(enemy);
        var en_support = CardFactory.CreateCard(2, rival);//30
        rival.Deck.AddCard(en_support);

        Request.SetNextResult(false);//不必杀
        Request.SetNextResult(false);//不回避

        Game.DoBattle(kayin, enemy);
        Assert.IsTrue(rival.Retreat.Contains(enemy));
    }
}
