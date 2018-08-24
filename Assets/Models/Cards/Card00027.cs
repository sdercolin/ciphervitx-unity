using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) S02-004 理知の瞳 ミリエル
/// </summary>
public class Card00027 : Card
{
    public Card00027(User controller) : base(controller)
    {
        Serial = "00027";
        Pack = "S02";
        CardNum = "S02-004";
        Title = "理智之瞳";
        UnitName = "米莉艾尔";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『魔道研究』【起】[横置，自分の手札を１枚退避エリアに置く]カードを１枚引く。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "魔道研究";
            Description = "『魔道研究』【起】[横置，将自己的1张手牌放置到退避区]抽1张卡。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.Action(this) + Cost.DiscardHand(this, 1);
        }

        public override Task Do()
        {
            Owner.Controller.DrawCard(1, this);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『魔術の紋章』【支】カードを１枚引く。自分の手札を１枚選び、退避エリアに置く。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : MagicEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "魔术之纹章";
            Description = "〖攻击型〗『魔术之纹章』【支】抽1张卡。选择自己的1张手牌，放置到退避区。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
