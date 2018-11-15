using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-096 二つの心を持つ弓兵 ノワール
/// </summary>
public class Card00142 : Card
{
    public Card00142(User controller) : base(controller)
    {
        Serial = "00142";
        Pack = "B01";
        CardNum = "B01-096";
        Title = "拥有二重心灵的弓兵";
        UnitName = "诺瓦尔";
        power = 40;
        support = 20;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Bow);
        ranges.Add(RangeEnum.Two);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『母に習った呪い』【常】自分のターン中、自分の手札の枚数が相手より多い場合、このユニットの戦闘力は＋２０される。このスキルは味方に『サーリャ』がいなければ有効にならない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "学自母亲的诅咒";
            Description = "『学自母亲的诅咒』【常】自己的回合中，自己的手牌张数比对手多的场合，这名单位的战斗力+20。这个能力仅限我方战场上存在「萨莉雅」时才有效。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.TurnPlayer == Controller
                && Controller.Hand.Count > Opponent.Hand.Count
                && Controller.Field.Filter(unit => unit.HasUnitNameOf(Strings.Get("card_text_unitname_サーリャ"))).Count > 0;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 20));
        }
    }

    /// <summary>
    /// スキル2
    /// 『飛行特効』【常】このユニットが<飛行>を攻撃している場合、このユニットの戦闘力は＋３０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : Wingslayer
    {
        public Sk2()
        {
            Number = 2;
            Name = "飞行特效";
            Description = "『飞行特效』【常】这名单位攻击<飞行>属性单位的期间，这名单位的战斗力+30。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }
    }
}
