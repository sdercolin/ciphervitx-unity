using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-093 力を封印せし者 ウード
/// </summary>
public class Card00139 : Card
{
    public Card00139(User controller) : base(controller)
    {
        Serial = "00139";
        Pack = "B01";
        CardNum = "B01-093";
        Title = "封印力量者";
        UnitName = "伍德";
        power = 40;
        support = 10;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
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
    /// 『古の魔剣？　ミステルトィン』【起】[翻面1]ターン終了まで、このユニットの戦闘力は-１０され、このユニットが攻撃で破壊するオーブは２つになる。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "古代魔剑？ 米斯特尔通";
            Description = "『古代魔剑？ 米斯特尔通』[翻面1]直到回合结束为止，这名单位的战斗力-10，这名单位的攻击所将破坏的宝玉变为2颗。";
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
            Controller.AttachItem(new PowerBuff(this, -10, LastingTypeEnum.UntilTurnEnds), Owner);
            Controller.AttachItem(new DestroyTwoOrbs(this, LastingTypeEnum.UntilTurnEnds), Owner);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『攻撃の紋章』【支】戦闘終了まで、自分の攻撃ユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : AttackEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "攻击之纹章";
            Description = "〖攻击型〗『攻击之纹章』【支】直到战斗结束为止，我方的攻击单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
