using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-094 憧れを追う刃 セレナ
/// </summary>
public class Card00140 : Card
{
    public Card00140(User controller) : base(controller)
    {
        Serial = "00140";
        Pack = "B01";
        CardNum = "B01-094";
        Title = "追逐憧憬之刃";
        UnitName = "赛蕾娜";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『斧殺し』【常】このユニットが<斧>と戦闘している場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "弑斧";
            Description = "『弑斧』【常】这名单位与<斧>武器单位战斗的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && ((Game.AttackingUnit == card && Game.DefendingUnit.HasWeapon(WeaponEnum.Axe)) || Game.AttackingUnit.HasWeapon(WeaponEnum.Axe) && Game.DefendingUnit == card);
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 20));
        }
    }

    /// <summary>
    /// スキル2
    /// 『あざとい一面』〖转职技〗【常】このユニットを支援しているカードが<男>の場合、そのカードの支援力は＋１０される。（はこのユニットがクラスチェンジしていなければ有効にならない）
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "诱惑的一面";
            Description = "〖转职技〗『诱惑的一面』【常】这名单位被<男性>单位支援的期间，那名单位的支援力+10。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.CCS;
        }

        public override bool CanTarget(Card card)
        {
            return card.HasGender(GenderEnum.Male)
                && Game.BattlingUnits.Contains(Owner)
                && card.BelongedRegion == Controller.Support;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new SupportBuff(this, 10));
        }
    }
}
