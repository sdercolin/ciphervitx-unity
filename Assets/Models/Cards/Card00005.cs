using System.Threading.Tasks;
/// <summary>
/// (S01) S01-005 必殺剣の使い手 ナバール
/// </summary>
public class Card00005 : Card
{
    public Card00005(User controller) : base(controller)
    {
        Serial = "00005";
        Pack = "S01";
        CardNum = "S01-005";
        Title = "必杀剑之剑手";
        UnitName = "那巴尔";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
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
    /// 『キルソード』【起】[翻面3]ターン終了まで、このユニットの攻撃は神速回避されない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "必杀刃";
            Description = "『必杀刃』【起】[翻面3]直到回合结束为止，这名单位的攻击不能被神速回避。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.UseBond(this, 3);
        }

        public override Task Do()
        {
            Owner.Attach(new CanNotBeAvoided(this, LastingTypeEnum.UntilTurnEnds));
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『孤高の剣士』【常】このユニットと自分の主人公の他に味方が１体もいない場合、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "孤高的剑士";
            Description = "『孤高的剑士』【常】我方战场上除这名单位和主人公以外没有其他单位的场合，这名单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && card.Controller.Field.TrueForAllCard(unit => unit == Owner || unit.IsHero);
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10));
        }
    }
}
