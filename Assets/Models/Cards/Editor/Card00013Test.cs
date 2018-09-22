using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00013Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『牽制射撃』【自】出撃コストが２以下の味方が出撃するたび、<飛行>の敵を１体選び、移動させてもよい。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var gedun = CardFactory.CreateCard(13, player);
        player.BackField.AddCard(gedun);
        var myUnit1 = CardFactory.CreateCard(9, player);//1C
        var myUnit2 = CardFactory.CreateCard(11, player);//1C
        var myUnit3 = CardFactory.CreateCard(3, player);//3C
        player.Hand.AddCard(myUnit1);
        player.Hand.AddCard(myUnit2);
        player.Hand.AddCard(myUnit3);

        var hisUnit1 = CardFactory.CreateCard(2, rival);//xida
        var hisUnit2 = CardFactory.CreateCard(42, rival);//simiya
        var hisUint3 = CardFactory.CreateCard(1, rival);
        rival.FrontField.AddCard(hisUnit1);
        rival.BackField.AddCard(hisUnit2);
        rival.FrontField.AddCard(hisUint3);

        Game.DoDeployment(myUnit3, true); //什么都没发生

        // 移动前场的希达至后场
        Request.SetNextResult(); //默认选择第一个Induction
        Request.SetNextResult(true); //选择使用
        Request.SetNextResult(new List<Card>() { hisUnit1 }); //选择对象
        Game.DoDeployment(myUnit1, true);
        Assert.IsTrue(rival.BackField.Contains(hisUnit1));

        // 移动后场的斯米亚到前场
        Request.SetNextResult(); //默认选择第一个Induction
        Request.SetNextResult(true); //选择使用
        Request.SetNextResult(new List<Card>() { hisUnit2 }); //选择对象
        Game.DoDeployment(myUnit2, true);
        Assert.IsTrue(rival.FrontField.Contains(hisUnit2));
    }
}
