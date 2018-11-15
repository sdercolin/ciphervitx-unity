using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-085 竜好きの竜騎士 セルジュ
/// </summary>
public class Card00131 : Card
{
    public Card00131(User controller) : base(controller)
    {
        Serial = "00131";
        Pack = "B01";
        CardNum = "B01-085";
        Title = "喜欢龙的龙骑士";
        UnitName = "塞尔吉";
        power = 30;
        support = 30;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Axe);
        types.Add(TypeEnum.Flight);
        types.Add(TypeEnum.Dragon);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『天空の運び手』【起】[横置]他の味方を１体選び、移動させる。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : WingedDeliverer
    {
        public Sk1()
        {
            Number = 1;
            Name = "天空的运送者";
            Description = "『天空的运送者』【起】[横置]选择1名其他我方单位，将其移动。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『天空の紋章』【支】自分の攻撃ユニット以外の味方を１体選ぶ。その味方を移動させてもよい。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : FlyingEmblem
    {
        public Sk2()
        {
            Number = 2;
            Name = "天空之纹章";
            Description = "〖攻击型〗『天空之纹章』【支】选择1名自己的攻击单位以外的我方单位。你可以移动那名我方单位。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
