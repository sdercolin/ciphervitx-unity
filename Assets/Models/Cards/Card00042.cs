using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-069 花占いの乙女 スミア
/// </summary>
public class Card00042 : Card
{
    public Card00042(User controller) : base(controller)
    {
        Serial = "00042";
        Pack = "S02";
        CardNum = "B01-069";
        Title = "花占卜少女";
        UnitName = "丝米娅";
        power = 30;
        support = 30;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
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
    /// 『天空の運び手』【起】[横置]他の味方を１体選び、移動させる。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "天空的运送者";
            Description = "『天空的运送者』【起】[横置]选择1名其他我方单位，将其移动。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.Action(this);
        }

        public override async Task Do()
        {
            var choices = Controller.Field.Cards;
            choices.Remove(Owner);
            if (choices.Count > 0)
            {
                await Controller.ChooseMove(choices, 1, 1, this);
            }
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
