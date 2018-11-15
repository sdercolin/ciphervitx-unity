using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-074 お菓子好き盗賊 ガイア
/// </summary>
public class Card00120 : Card
{
    public Card00120(User controller) : base(controller)
    {
        Serial = "00120";
        Pack = "B01";
        CardNum = "B01-074";
        Title = "喜欢甜食的盗贼";
        UnitName = "盖亚";
        power = 30;
        support = 10;
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
    /// 『鍵開け』【起】[横置]相手のデッキの１番上のカードを公開させる。そのカードの出撃コストが３以上の場合、あなたは翻面1してもよい。そうしたなら、カードを１枚引く。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : Unlock
    {
        public Sk1()
        {
            Number = 1;
            Name = "开锁";
            Description = "『开锁』【起】[横置]公开对手卡组最上方的1张卡。那张卡的出击费用在3以上的场合，你可以[翻面1]。如果这样做了，则抽1张卡。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『盗賊の紋章』【支】相手のデッキの１番上のカードを公開させる。あなたはそのカードを退避エリアに置かせてもよい。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ThiefEmblem
    {
        public Sk2()
        {
            Number = 2;
            Name = "盗贼之纹章";
            Description = "〖攻击型〗『盗贼之纹章』【支】公开对手卡组最上方的1张卡。你可以将那张卡放置到退避区。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
