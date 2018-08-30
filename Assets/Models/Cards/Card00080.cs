using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-034 大陸一の弓騎士 ジョルジュ
/// </summary>
public class Card00080 : Card
{
    public Card00080(User controller) : base(controller)
    {
        Serial = "00080";
        Pack = "B01";
        CardNum = "B01-034";
        Title = "大陆第一的弓骑士";
        UnitName = "乔治";
        power = 50;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Bow);
        ranges.Add(RangeEnum.Two);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『無双の射手』【起】[出撃コストが２以下の味方を１体行動済みにする]ターン終了まで、このユニットが<飛行>を攻撃している場合、このユニットの攻撃は神速回避されない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "无双的射手";
            Description = "『无双的射手』【起】[将1名出击费用2以下的我方单位转为已行动状态]直到回合结束为止，这名单位攻击<飞行>属性的单位的期间，这名单位的攻击不能被神速回避。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ActionUnits(this, 1, unit => unit.DeployCost<=2);
        }

        public override Task Do()
        {
            //TODO
            Controller.AttachItem(new CanNotBeAvoidedAttackWing(this, LastingTypeEnum.UntilTurnEnds), Owner);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『飛行特効』【常】このユニットが<飛行>を攻撃している場合、このユニットの戦闘力は＋３０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : Wingslayer
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "飞行特效";
            Description = "『飞行特效』【常】这名单位攻击<飞行>属性单位的期间，这名单位的战斗力+30。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }
    }
}
