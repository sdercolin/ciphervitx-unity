using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00063Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『宿命の好敵手』【常】『シーダ』か『ナバール』以外のカードはこのユニットの支援に失敗する。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(63, player); //70
        var support1 = CardFactory.CreateCard(1, player);//20支援

        player.FrontField.AddCard(card);
        player.Deck.AddCard(support1);

        var rivalcard = CardFactory.CreateCard(3, rival);//70
        var rivalSupport1 = CardFactory.CreateCard(1, rival);//20支援

        rival.FrontField.AddCard(rivalcard);
        rival.Deck.AddCard(rivalSupport1);

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Game.DoBattle(card, rivalcard);
        
        Assert.IsTrue(rivalcard.IsOnField);

        var support2 = CardFactory.CreateCard(2, player);//希达
        player.Deck.AddCard(support2);

        var rivalSupport2 = CardFactory.CreateCard(2, rival);//30支援
        rival.Deck.AddCard(rivalSupport2);

        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Game.DoBattle(card, rivalcard);

        Assert.IsFalse(rivalcard.IsOnField);
    }
}
