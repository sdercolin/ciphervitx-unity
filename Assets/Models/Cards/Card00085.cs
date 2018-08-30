using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-039 天馬を駆る姉騎士 パオラ
/// </summary>
public class Card00085 : Card
{
    public Card00085(User controller) : base(controller)
    {
        Serial = "00085";
        Pack = "B01";
        CardNum = "B01-039";
        Title = "驾驭天马的姊骑士";
        UnitName = "帕奥拉";
        power = 30;
        support = 30;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Flight);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『ペガサス三姉妹』【起】[横置，翻面2]自分のデッキから出撃コストが２以下の『カチュア』か『エスト』を１枚選び、出撃させる。その後、デッキをシャッフルする。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "天马三姐妹";
            Description = "『天马三姐妹』【起】[横置，翻面2]从自己的卡组中选择1张出击费用2以下的「卡秋雅」或「爱丝特」，将其出击。这之后，切洗卡组。";
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
            //TODO
            await Controller.ChooseDeploy(Controller.Deck.Filter(card => card.DeployCost <= 2 && (card.HasUnitNameOf("卡秋雅") || card.HasUnitNameOf("爱丝特"))), 0, 1, this);
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『天空の紋章』【支】自分の攻撃ユニット以外の味方を１体選ぶ。その味方を移動させてもよい。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : FlyingEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "天空之纹章";
            Description = "〖攻击型〗『天空之纹章』【支】选择1名自己的攻击单位以外的我方单位。你可以移动那名我方单位。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
