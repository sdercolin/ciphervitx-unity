using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-053 イーリス聖王国の王子 クロム
/// </summary>
public class Card00029 : Card
{
    public Card00029(User controller) : base(controller)
    {
        Serial = "00029";
        Pack = "S02";
        CardNum = "B01-053";
        Title = "伊利斯圣王国的王子";
        UnitName = "库洛姆";
        power = 40;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
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
    /// 『聖痕の輝き』【常】クラスチェンジしている他の味方１体につき、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "圣痕的光辉";
            Description = "『圣痕的光辉』【常】我方战场上每有1名经过转职后的单位，这名单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10 * Owner.Controller.Field.Filter(unit => unit.IsClassChanged).Count));
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『英雄の紋章』【支】自分の攻撃ユニットが<聖痕>の場合、戦闘終了まで、そのユニットが攻撃で破壊するオーブは２つになる。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : HeroEmblem
    {
        public Sk2()
        {
            Number = 2;
            Name = "英雄之纹章";
            Description = "〖攻击型〗『英雄之纹章』【支】我方的攻击单位是<圣痕>势力的场合，直到战斗结束为止，那名单位的攻击所将破坏的宝玉变为2颗。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
            Symbol = SymbolEnum.Blue;
        }
    }
}
