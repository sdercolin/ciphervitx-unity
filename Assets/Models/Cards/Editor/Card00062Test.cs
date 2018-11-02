using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class Card00062Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// スキル1
        /// 『戦場の息吹』【起】[他の味方を１体行動済みにする]ターン終了まで、このユニットの戦闘力は＋１０される。
        /// </summary>
        /// 
        /// <summary>
        /// スキル2
        /// 『闘神の一撃』【自】このユニットが攻撃した時、このユニットの戦闘力が１００以上の場合、ターン終了まで、このユニットが攻撃で破壊するオーブは２つになる。
        /// </summary>
        Game.Initialize();
        Game.SetTestMode();
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = player;

        var card = CardFactory.CreateCard(62, player); //70
        var card1 = CardFactory.CreateCard(1, player);
        var card2 = CardFactory.CreateCard(2, player);
        var card3 = CardFactory.CreateCard(3, player);
        var support = CardFactory.CreateCard(62, player);//同名0支援

        player.FrontField.AddCard(card);
        player.FrontField.AddCard(card1);
        player.FrontField.AddCard(card2);
        player.FrontField.AddCard(card3);
        player.Deck.AddCard(support);

        var card4 = CardFactory.CreateCard(1, rival);//70
        card4.IsHero = true;
        var rivalOrb1 = CardFactory.CreateCard(6, rival);
        var rivalOrb2 = CardFactory.CreateCard(6, rival);
        var rivalSupport = CardFactory.CreateCard(2, rival);//30支援

        rival.FrontField.AddCard(card4);
        rival.Deck.AddCard(rivalSupport);
        rival.Orb.AddCard(rivalOrb1);
        rival.Orb.AddCard(rivalOrb2);

        Request.SetNextResult();//横置
        Game.DoActionSkill(card.GetUsableActionSkills()[0]);//起
        Request.SetNextResult();//横置
        Game.DoActionSkill(card.GetUsableActionSkills()[0]);//起
        Request.SetNextResult();//横置
        Game.DoActionSkill(card.GetUsableActionSkills()[0]);//起
        Request.SetNextResult(); //选择被诱发的Skill
        Request.SetNextResult(false); //不必杀
        Request.SetNextResult(false); //不回避
        Request.SetNextResult(); //拿一个宝玉
        Request.SetNextResult(); //拿一个宝玉
        Game.DoBattle(card, card4);
        
        Assert.IsTrue(rival.Orb.Count == 0);
    }
}
