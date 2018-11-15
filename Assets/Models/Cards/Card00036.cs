using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-062 ロザンヌの侯爵 ヴィオール
/// </summary>
public class Card00036 : Card
{
    public Card00036(User controller) : base(controller)
    {
        Serial = "00036";
        Pack = "S02";
        CardNum = "B01-062";
        Title = "罗赞努的侯爵";
        UnitName = "维奥尔";
        power = 50;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Bow);
        ranges.Add(RangeEnum.Two);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『弓の達人』〖转职技〗【常】他の<弓>の味方１体につき、このユニットの戦闘力は＋１０される。（はこのユニットがクラスチェンジしていなければ有効にならない）
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "弓之达人";
            Description = "〖转职技〗『弓之达人』【常】我方战场上每有1名其他<弓>武器的单位，这名单位的战斗力+10。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.CCS;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10 * Controller.Field.Filter(unit => unit.HasWeapon(WeaponEnum.Bow) && unit != Owner).Count));
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
