using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card00040Test
{

    [Test]
    public void SkillTest()
    {
        /// <summary>
        /// ������1
        /// ���}�ܡ���תְ����������<��>��<ħ��>��<�oʯ>�����l�Δ��Ϲ��ĤǤ��ʤ������Ϥ��Υ�˥åȤ����饹�����󥸤��Ƥ��ʤ�����Є��ˤʤ�ʤ���
        /// </summary>
        Game.Initialize();
        Game.LosingProcessDisabled = true;
        var player = Game.Player;
        var rival = Game.Rival;
        Game.TurnPlayer = rival;

        var card1 = CardFactory.CreateCard(40, player);
        player.FrontField.AddCard(card1);
        var card2 = CardFactory.CreateCard(13, rival);//��
        rival.BackField.AddCard(card2);
        var card3 = CardFactory.CreateCard(21, rival);//ħ��
        rival.BackField.AddCard(card3);
        var card4 = CardFactory.CreateCard(157, rival);//�oʯ
        rival.BackField.AddCard(card4);


        Assert.IsTrue(card2.CheckAttack());
        Assert.IsTrue(card3.CheckAttack());
        Assert.IsTrue(card4.CheckAttack());

        var card5 = CardFactory.CreateCard(40, player);
        player.Hand.AddCard(card5);
        Game.DoLevelUp(card5, true);

        Assert.IsFalse(card2.CheckAttack());
        Assert.IsFalse(card3.CheckAttack());
        Assert.IsFalse(card4.CheckAttack());
    }
}
