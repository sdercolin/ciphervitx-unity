using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-056 マルスを名乗る剣士 ルキナ
/// </summary>
public class Card00102 : Card
{
    public Card00102(User controller) : base(controller)
    {
        Serial = "00102";
        Pack = "B01";
        CardNum = "B01-056";
        Title = "自称马尔斯的剑士";
        UnitName = "露琪娜";
        power = 40;
        support = 20;
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
        sk3 = new Sk3();
        Attach(sk3);
    }

    /// <summary>
    /// スキル1
    /// 『英雄王の名』【特】このカードのユニット名は『マルス』としてもあつかう。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "英雄王之名";
            Description = "『英雄王之名』【特】这张卡的单位名也当作「马尔斯」。";
            TypeSymbols.Add(SkillTypeSymbol.Special);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new UnitNameBuff(this, true, Strings.Get("card_text_unitname_マルス")));
        }
    }

    /// <summary>
    /// スキル2
    /// 『裏剣 ファルシオン』【常】このユニットが<竜>を攻撃している場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : Dragonslayer
    {
        public Sk2()
        {
            Number = 2;
            Name = "里剑 法尔西昂";
            Description = "『里剑 法尔西昂』【常】这名单位攻击<龙>属性单位的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }
    }

    /// <summary>
    /// スキル3
    /// 〖攻击型〗『英雄の紋章』【支】自分の攻撃ユニットが<聖痕>の場合、戦闘終了まで、そのユニットが攻撃で破壊するオーブは２つになる。
    /// </summary>
    public Sk3 sk3;
    public class Sk3 : HeroEmblem
    {
        public Sk3()
        {
            Number = 3;
            Name = "英雄之纹章";
            Description = "〖攻击型〗『英雄之纹章』【支】我方的攻击单位是<圣痕>势力的场合，直到战斗结束为止，那名单位的攻击所将破坏的宝玉变为2颗。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
            Symbol = SymbolEnum.Blue;

        }
    }
}
