using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-006 タリスの王女 シーダ
/// </summary>
public class Card00007 : Card
{
    public Card00007(User controller) : base(controller)
    {
        Serial = "00007";
        Pack = "S01";
        CardNum = "B01-006";
        Title = "塔利斯的王女";
        UnitName = "希达";
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
    /// 『王女のカリスマ』【起】[横置，他の味方を１体行動済みにする]他の味方を１体選ぶ。ターン終了まで、その味方の戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "王女的领袖魅力";
            Description = "『王女的领袖魅力』【起】[横置，将1名其他我方单位转为已行动状态]选择1名其他我方单位。直到回合结束为止，那名我方单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ActionSelf(this) + Cost.ActionOthers(this, 1);
        }

        public override async Task Do()
        {
            var choices = Controller.Field.Cards;
            choices.Remove(Owner);
            if (choices.Count > 0)
            {
                var target = await Request.ChooseOne(choices, Controller);
                Controller.AttachItem(new PowerBuff(this, 10, LastingTypeEnum.UntilTurnEnds), target);
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
