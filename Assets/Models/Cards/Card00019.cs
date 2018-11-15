using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-024 紅の剣士 ナバール
/// </summary>
public class Card00019 : Card
{
    public Card00019(User controller) : base(controller)
    {
        Serial = "00019";
        Pack = "S01";
        CardNum = "B01-024";
        Title = "红色剑士";
        UnitName = "那巴尔";
        power = 40;
        support = 10;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『無頼の剣』【常】自分のターン中、このユニットと自分の主人公の他に味方が１体もいない場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "独行之剑";
            Description = "『独行之剑』【常】自己的回合中，我方战场上除这名单位和主人公以外没有其他单位的场合，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.TurnPlayer == card.Controller
                && card.Controller.Field.TrueForAllCard(unit => unit == Owner || unit.IsHero);
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 20));
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『攻撃の紋章』【支】戦闘終了まで、自分の攻撃ユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : AttackEmblem
    {
        public Sk2()
        {
            Number = 2;
            Name = "攻击之纹章";
            Description = "〖攻击型〗『攻击之纹章』【支】直到战斗结束为止，我方的攻击单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
