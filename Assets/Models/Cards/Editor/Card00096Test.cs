using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00096Test
{
    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『神槍 グラディウス』【起】[翻面4]出撃コストが２以下のすべての敵を撃破する。ターン終了まで、このスキルで撃破した敵１体につき、このユニットの戦闘力は＋１０され、このユニットに射程１-２が追加される。
        /// </summary>
        /// 
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(96, player);
        player.FrontField.AddCard(card);
        var bond1 = CardFactory.CreateCard(90, player);
        var bond2 = CardFactory.CreateCard(90, player);
        var bond3 = CardFactory.CreateCard(90, player);
        var bond4 = CardFactory.CreateCard(90, player);
        player.Bond.AddCard(bond1);
        player.Bond.AddCard(bond2);
        player.Bond.AddCard(bond3);
        player.Bond.AddCard(bond4);

        var rivalUnit1 = CardFactory.CreateCard(6, rival);//1C
        var rivalUnit2 = CardFactory.CreateCard(7, rival);//1C
        var rivalUnit3 = CardFactory.CreateCard(3, rival);//3C
        rival.FrontField.AddCard(rivalUnit1);
        rival.FrontField.AddCard(rivalUnit2);
        rival.FrontField.AddCard(rivalUnit3);

        Request.SetNextResult(); //翻面
        Game.DoActionSkill(card.GetUsableActionSkills()[0]);

        Assert.IsTrue(rival.FrontField.Count == 1);
        Assert.IsTrue(card.Power == 90);
        Assert.IsTrue(card.HasRange(RangeEnum.OnetoTwo));
    }

}
