using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-018 タリスの傭兵 オグマ
/// </summary>
public class Card00015 : Card
{
    public Card00015(User controller) : base(controller)
    {
        Serial = "00015";
        Pack = "S01";
        CardNum = "B01-018";
        Title = "塔利斯的佣兵";
        UnitName = "奥古玛";
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
    /// 『傭兵隊長』【常】自分のターン中、出撃コストが２以下の味方が他に２体以上いる場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "佣兵队长";
            Description = "『佣兵队长』【常】自己的回合中，我方战场上有2名以上其他出击费用2以下的单位的场合，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.TurnPlayer == Owner.Controller
                && card.Controller.Field.Filter(unit => unit.DeployCost <= 2 && unit != Owner).Count >= 2;
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
