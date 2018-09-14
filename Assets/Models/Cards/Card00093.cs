using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-047 神竜族の王女 チキ
/// </summary>
public class Card00093 : Card
{
    public Card00093(User controller) : base(controller)
    {
        Serial = "00093";
        Pack = "B01";
        CardNum = "B01-047";
        Title = "神龙族的王女";
        UnitName = "芝琪";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.DragonStone);
        types.Add(TypeEnum.Dragon);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『長寿な竜一族』【常】自分の絆カードが４枚以上の場合、このユニットの戦闘力は＋３０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "长寿的龙一族";
            Description = "『长寿的龙一族』【常】自己的羁绊卡有4张以上的场合，这名单位的战斗力+30。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner && Controller.Bond.Count >= 4;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 30));
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『竜人の紋章』【支】自分の攻撃ユニットが<光の剣>の場合、自分の手札を１枚選ぶ。そのカードを絆エリアに置いてもよい。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : DragonEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "龙人之纹章";
            Description = "〖攻击型〗『龙人之纹章』【支】自己的攻击单位是<光之剑>势力的场合，选择自己的1张手牌。你可以将那张卡放置到羁绊区。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
            Symbol = SymbolEnum.Red;
        }
    }
}
