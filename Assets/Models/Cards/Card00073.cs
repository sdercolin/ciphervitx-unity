using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-027 辺境の盗賊 ジュリアン
/// </summary>
public class Card00073 : Card
{
    public Card00073(User controller) : base(controller)
    {
        Serial = "00073";
        Pack = "B01";
        CardNum = "B01-027";
        Title = "边境的盗贼";
        UnitName = "朱利安";
        power = 50;
        support = 10;
        deployCost = 2;
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
    /// 『財宝奪取』【起】[翻面1]相手のデッキの１番上のカードを退避エリアに置かせる。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ActionSkill
    {
        public Sk2()
        {
            Number = 2;
            Name = "财宝夺取";
            Description = "『财宝夺取』【起】[翻面1]将对手卡组最上方的1张卡放置到退避区。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 1);
        }

        public override Task Do()
        {
            Controller.SendToRetreat(Opponent.Deck.Top, this);
            return Task.CompletedTask;
        }
    }
}
