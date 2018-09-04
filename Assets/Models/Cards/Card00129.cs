using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-083 照れ屋の踊り子 オリヴィエ
/// </summary>
public class Card00129 : Card
{
    public Card00129(User controller) : base(controller)
    {
        Serial = "00129";
        Pack = "B01";
        CardNum = "B01-083";
        Title = "害羞的舞者";
        UnitName = "奥莉薇";
        power = 20;
        support = 10;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『踊り』【起】[横置，翻面2]このターンに攻撃した味方を１体選び、未行動にする。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "舞蹈";
            Description = "『舞蹈』【起】[横置，翻面2]选择1名这个回合中攻击过的我方单位，将其转为未行动状态。";
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
            Controller.RefreshUnit(await Request.Choose(Controller.Field.Cards, 1, 1, Controller), this);
        }
    }
}
