using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00083Test
{
    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『愛の双刃』【自】[味方の『アストリア』を行動済みにする]このユニットが攻撃した時、コストを支払うなら、戦闘終了まで、このユニットの戦闘力は＋４０される。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(83, player);
        player.FrontField.AddCard(card);
        var card1 = CardFactory.CreateCard(90, player);
        var support = CardFactory.CreateCard(83, player);//自己
        player.FrontField.AddCard(card1);
        player.Deck.AddCard(support);

        var rivalUnit = CardFactory.CreateCard(1, rival);
        rival.FrontField.AddCard(rivalUnit);
        var rivalSupport = CardFactory.CreateCard(2, rival);//30支援
        rival.Deck.AddCard(rivalSupport);

        Request.SetNextResult();//选择技能
        Request.SetNextResult(true);//选择发动
        Request.SetNextResult();//横置
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Game.DoBattle(card, rivalUnit).Wait();
        Assert.IsFalse(rivalUnit.IsOnField); //击破
    }

}
