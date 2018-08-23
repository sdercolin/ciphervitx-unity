using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-013 同盟軍の弓騎士 ゴードン
/// </summary>
public class Card00013 : Card
{
    public Card00013(User controller) : base(controller)
    {
        Serial = "00013";
        Pack = "S01";
        CardNum = "B01-013";
        Title = "同盟军的弓骑士";
        UnitName = "哥顿";
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
    /// 『牽制射撃』【自】出撃コストが２以下の味方が出撃するたび、<飛行>の敵を１体選び、移動させてもよい。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "牵制射击";
            Description = "『牵制射击』【自】每次出击费用2以下的我方单位出击时，你可以选择1名<飞行>属性的敌方单位，将其移动。";
            Optional = true;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override bool CheckInduceConditions(Message message)
        {
            var deployMessage = message as DeployMessage;
            if (deployMessage != null)
            {
                return deployMessage.TrueForAny(deployMessage.Targets, card => card.Controller == Controller && card.DeployCost <= 2);
            }
            return false;
        }

        public override Cost DefineCost()
        {
            return Cost.Null;
        }

        public override async Task Do()
        {
            await Controller.ChooseMove(Opponent.Field.Filter(unit => unit.HasType(TypeEnum.Flight)), 0, 1, this);
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
