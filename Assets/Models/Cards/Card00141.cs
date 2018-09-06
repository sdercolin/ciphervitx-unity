using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-095 秘めた憧憬 セレナ
/// </summary>
public class Card00141 : Card
{
    public Card00141(User controller) : base(controller)
    {
        Serial = "00141";
        Pack = "B01";
        CardNum = "B01-095";
        Title = "暗藏的憧憬";
        UnitName = "赛蕾娜";
        power = 40;
        support = 10;
        deployCost = 1;
        classChangeCost = 0;
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
    /// 『裏腹な心』【常】味方に『ティアモ』がいる場合、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "口是心非";
            Description = "『口是心非』【常】我方战场上存在「缇雅莫」的场合，这名单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Controller.Field.Filter(unit => unit.HasUnitNameOf("缇雅莫")).Count > 0;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10));
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『攻撃の紋章』【支】戦闘終了まで、自分の攻撃ユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : AttackEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "攻击之纹章";
            Description = "〖攻击型〗『攻击之纹章』【支】直到战斗结束为止，我方的攻击单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
