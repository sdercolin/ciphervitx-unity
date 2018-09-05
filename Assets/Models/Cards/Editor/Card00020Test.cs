using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00020Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『ライブ』【起】[横置，翻面2]自分の退避エリアから『レナ』以外のカードを１枚選び、手札に加える。
        /// </summary>
        /// 
        /// <summary>
        /// スキル2
        /// 『ジュリアンとの絆』【常】味方の『ジュリアン』の戦闘力は＋１０される。
        /// </summary>
        Game.Initialize();
        var player = Game.Player;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(20, player);
        var card2 = CardFactory.CreateCard(1, player);

        player.FrontField.AddCard(card);
        player.Retreat.AddCard(card2);
        //player.Retreat.AddCard(card);

        var bond1 = CardFactory.CreateCard(1, player);
        var bond2 = CardFactory.CreateCard(1, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);

        Assert.IsTrue(player.Hand.Count == 0);
        
        Request.SetNextResult(new List<Card>() { bond1 });
        Request.SetNextResult(new List<Card>() { bond2 });
        Request.SetNextResult(new List<Card>() { card2 });

        //ActionSkill actionSkill = card.GetUsableActionSkills()[0];
        Game.DoActionSkill(card.GetUsableActionSkills()[0]);

        Assert.IsTrue(player.Hand.Count == 1);
    }
}
