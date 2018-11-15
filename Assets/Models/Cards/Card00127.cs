using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-081 ペレジア王国の呪術師 サーリャ
/// </summary>
public class Card00127 : Card
{
    public Card00127(User controller) : base(controller)
    {
        Serial = "00127";
        Pack = "B01";
        CardNum = "B01-081";
        Title = "佩雷吉亚王国的咒术师";
        UnitName = "萨莉雅";
        power = 40;
        support = 20;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『ミィル』【起】[横置，翻面2]相手は自分の手札を１枚選び、退避エリアに置く。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "熔流";
            Description = "『熔流』【起】[横置，翻面2]对手选择他自己的1张手牌，放置到退避区。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ActionSelf(this) + Cost.ReverseBond(this, 2);
        }

        public override async Task Do()
        {
            await Opponent.ChooseDiscardHand(Opponent.Hand.Cards, 1, 1, false, this);
        }
    }
}
