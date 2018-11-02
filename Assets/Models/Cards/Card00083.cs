using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-037 愛深き騎士 ミディア
/// </summary>
public class Card00083 : Card
{
    public Card00083(User controller) : base(controller)
    {
        Serial = "00083";
        Pack = "B01";
        CardNum = "B01-037";
        Title = "深情的骑士";
        UnitName = "米迪娅";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『愛の双刃』【自】[味方の『アストリア』を行動済みにする]このユニットが攻撃した時、コストを支払うなら、戦闘終了まで、このユニットの戦闘力は＋４０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "爱之双刃";
            Description = "『爱之双刃』【自】[将我方的「阿斯特利亚」转为已行动状态]这名单位攻击时，你可以支付费用。如果支付，则直到战斗结束为止，这名单位的战斗力+40。";
            Optional = true;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions(Induction induction)
        {
            return true;
        }

        public override Induction CheckInduceConditions(Message message)
        {
            var attackMessage = message as AttackMessage;
            if (attackMessage != null)
            {
                if (attackMessage.AttackingUnit == Owner)
                {
                    return new Induction();
                }
            }
            return null;
        }

        public override Cost DefineCost()
        {
            return Cost.ActionOthers(this, 1, card => card.HasUnitNameOf(Strings.Get("card_text_unitname_アストリア")));
        }

        public override Task Do(Induction induction)
        {
            Controller.AttachItem(new PowerBuff(this, 40, LastingTypeEnum.UntilBattleEnds), Owner);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『ドラゴンランス』【常】このユニットが<竜>を攻撃している場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : Dragonslayer
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "杀龙枪";
            Description = "『杀龙枪』【常】这名单位攻击<龙>属性单位的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }
    }
}
