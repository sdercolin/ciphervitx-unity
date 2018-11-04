using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00095Test
{

    [Test]
    public void Skill1Test()
    {
        /// <summary>
        /// スキル1
        /// 『少女剣士の恩返し』【特】このカードは自分の退避エリアにカードが５枚以上なければ出撃できない。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(95, player);
        player.Hand.AddCard(card);

        var retreat1 = CardFactory.CreateCard(95, player);
        var retreat2 = CardFactory.CreateCard(95, player);
        var retreat3 = CardFactory.CreateCard(95, player);
        var retreat4 = CardFactory.CreateCard(95, player);
        var retreat5 = CardFactory.CreateCard(95, player);
        player.Retreat.AddCard(retreat1);
        player.Retreat.AddCard(retreat2);
        player.Retreat.AddCard(retreat3);
        player.Retreat.AddCard(retreat4);

        var bond1 = CardFactory.CreateCard(95, player);
        var bond2 = CardFactory.CreateCard(95, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);

        //4张
        Game.TryDoMessage(new EmptyMessage());
        Assert.IsFalse(card.CheckDeployment());

        //5 张
        player.Retreat.AddCard(retreat5);
        Game.TryDoMessage(new EmptyMessage());
        Assert.IsTrue(card.CheckDeployment());
    }
}
