using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-001 白夜王国の王子 カムイ（男）
/// </summary>
public class Card00147 : Card
{
    public Card00147(User controller) : base(controller)
    {
        Serial = "00147";
        Pack = "P01";
        CardNum = "P01-001";
        Title = "白夜王国的王子";
        UnitName = "神威（男）";
        power = 40;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.White);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
        sk3 = new Sk3();
        Attach(sk3);
    }

    /// <summary>
    /// スキル1
    /// 『暗夜の心』【常】このユニットは<暗夜>としてもあつかう。（【常】はこのカードがユニットとして戦場にいる間だけ有効になる）
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "暗夜之心";
            Description = "『暗夜之心』【常】这名单位也当做<暗夜>势力。（【常】仅限这张卡作为单位存在于战场上的期间才有效）";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new SymbolBuff(this, true, SymbolEnum.Black));
        }
    }

    /// <summary>
    /// スキル2
    /// 『夜刀神・空夜』【常】自分のターン中、自分の<白夜>の絆カードが２枚以上の場合、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "夜刀神·空夜";
            Description = "『夜刀神·空夜』【常】自己的回合中，自己的<白夜>势力的羁绊卡有2张以上的场合，这名单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.TurnPlayer == Controller
                && Controller.Bond.Filter(bond => bond.HasSymbol(SymbolEnum.White)).Count >= 2;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10));
        }
    }

    /// <summary>
    /// スキル3
    /// 〖攻击型〗『英雄の紋章』【支】自分の攻撃ユニットが<白夜>の場合、戦闘終了まで、そのユニットが攻撃で破壊するオーブは２つになる。
    /// </summary>
    public Sk3 sk3;
    public class Sk3 : HeroEmblem
    {
        public Sk3() : base()
        {
            Number = 3;
            Name = "英雄之纹章";
            Description = "〖攻击型〗『英雄之纹章』【支】我方的攻击单位是<白夜>势力的场合，直到战斗结束为止，那名单位的攻击所将破坏的宝玉变为2颗。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
            Symbol = SymbolEnum.White;
        }
    }
}
