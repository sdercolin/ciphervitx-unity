using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-087 壊れた心の呪術師 ヘンリー
/// </summary>
public class Card00133 : Card
{
    public Card00133(User controller) : base(controller)
    {
        Serial = "00133";
        Pack = "B01";
        CardNum = "B01-087";
        Title = "心灵崩坏的咒术师";
        UnitName = "亨利";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『赤の呪い』【常】相手の手札が４枚以下の場合、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : Hand4OrLessAdd10
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "赤色诅咒";
            Description = "『赤色诅咒』【常】对手的手牌在4张以下的场合，这名单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『暗闇の紋章』【支】相手の手札が５枚以上の場合、相手は自分の手札を１枚選び、退避エリアに置く。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : DarkEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "黑暗之纹章";
            Description = "〖攻击型〗『黑暗之纹章』【支】对手的手牌有5张以上的场合，对手选择他自己的1张手牌，放置到退避区。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
