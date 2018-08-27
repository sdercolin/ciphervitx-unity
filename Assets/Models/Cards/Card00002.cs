using System.Threading.Tasks;

/// <summary>
/// (S01) S01-002 戦場を翔ける王女 シーダ
/// </summary>
public class Card00002 : Card
{
    public Card00002(User controller) : base(controller)
    {
        Serial = "00002";
        Pack = "S01";
        CardNum = "S01-002";
        Title = "翱翔战场的王女";
        UnitName = "希达";
        power = 50;
        support = 30;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Flight);
        types.Add(TypeEnum.Dragon);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『飛竜の翼』【自】出撃コストが２以下の味方が出撃するたび、味方を好きな数だけ選び、移動させてもよい。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "飞龙之翼";
            Description = "『飞龙之翼』【自】每次出击费用2以下的我方单位出击时，你可以选择任意名我方单位，将其移动。";
            Optional = true;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions(Induction induction)
        {
            return true;
        }

        public override Induction CheckInduceConditions(Message message)
        {
            var deployMessage = message as DeployMessage;
            if (deployMessage != null)
            {
                if (deployMessage.TrueForAny(deployMessage.Targets, card => card.Controller == Controller && card.DeployCost <= 2))
                {
                    return new Induction();
                }
            }
            return null;
        }

        public override Cost DefineCost()
        {
            return Cost.Null;
        }

        public override async Task Do(Induction induction)
        {
            var choices = Controller.Field.Cards;
            await Controller.ChooseMove(choices, 0, choices.Count);
        }
    }

    /// <summary>
    /// スキル2
    /// 『手槍』【起】[翻面1]ターン終了まで、このユニットに射程１-２が追加される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ActionSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "投枪";
            Description = "『投枪』【起】[翻面1]直到回合结束为止，这名单位追加射程1-2。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
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
            Owner.Attach(new RangeBuff(this, true, RangeEnum.OnetoTwo, LastingTypeEnum.UntilTurnEnds));
            return Task.CompletedTask;
        }
    }
}
