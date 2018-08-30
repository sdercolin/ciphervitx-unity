using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-032 天真爛漫な司祭 マリア
/// </summary>
public class Card00078 : Card
{
    public Card00078(User controller) : base(controller)
    {
        Serial = "00078";
        Pack = "B01";
        CardNum = "B01-032";
        Title = "天真烂漫的司祭";
        UnitName = "玛莉娅";
        power = 50;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『リザーブ』【起】[横置，翻面3]自分の退避エリアから『マリア』以外でユニット名が異なるカードを２枚まで選び、手札に加える。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "生命之杖";
            Description = "『生命之杖』【起】[横置，翻面3]从自己的退避区中选择至多2张「玛莉娅」以外的单位名不同的卡，将其加入手牌。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.Action(this) + Cost.ReverseBond(this, 3);
        }

        public override async Task Do()
        {
            //TODO
            await Controller.ChooseAddToHand(Controller.Retreat.Filter(unit => !unit.HasUnitNameOf("玛莉娅")), 0, 2, this);
        }
    }
}
