using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-048 アリティアの王女 エリス
/// </summary>
public class Card00094 : Card
{
    public Card00094(User controller) : base(controller)
    {
        Serial = "00094";
        Pack = "B01";
        CardNum = "B01-048";
        Title = "阿利缇亚的王女";
        UnitName = "爱丽丝";
        power = 40;
        support = 20;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Staff);
        ranges.Add(RangeEnum.None);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『オーム』【起】[横置，翻面1]自分の退避エリアから出撃コストが２以下の<光の剣>のカードを１枚選び、出撃させる。そうしたなら、ゲーム終了まで、すべての味方は『オーム』を使用できない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "复活之杖";
            Description = "『复活之杖』【起】[横置，翻面1]从自己的退避区中选择1张出击费用2以下的<光之剑>势力的卡，将其出击。如果这样做了，则直到游戏结束为止，所有我方单位不能使用『复活之杖』。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ActionSelf(this) + Cost.ReverseBond(this, 1);
        }

        public override async Task Do()
        {
            var targets = Controller.Retreat.Filter(card => card.DeployCost <= 2 && card.HasSymbol(SymbolEnum.Red));
            if (targets.Count > 0)
            {
                await Controller.ChooseDeploy(targets, 1, 1, null, null, this);
                Controller.AttachItem(new UserForbidAumStaff(this, LastingTypeEnum.Forever), Owner);
            }

        }
    }

    /// <summary>
    /// スキル2
    /// 『エリスの想い』【常】味方の『マルス』と『マリク』の戦闘力は＋１０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "爱丽丝的思绪";
            Description = "『爱丽丝的思绪』【常】我方的「马尔斯」和「玛利克」的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card.Controller == Controller && (card.HasUnitNameOf("马尔斯") || card.HasUnitNameOf("玛利克"));
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10));
        }
    }
    
    public class UserForbidAumStaff : SubSkill, IUserForbidActionSkill
    {
        public UserForbidAumStaff(Skill origin, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType) { }

        public string ForbiddenSkillName => "复活之杖";
    }
}