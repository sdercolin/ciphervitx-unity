using System.Collections.Generic;
/// <summary>
/// (S01) S01-004 剛剣の使い手 オグマ
/// </summary>
public class Card00004 : Card
{
    public Card00004(User controller) : base(controller)
    {
        Serial = "00004";
        Pack = "S01";
        CardNum = "S01-004";
        Title = "刚剑之剑手";
        UnitName = "奥古玛";
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
    /// 『タリス王国軍の隊長』【自】出撃コストが２以下の味方が出撃するたび、ターン終了まで、このユニットとその味方の戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "塔利斯王国军的队长";
            Description = "『塔利斯王国军的队长』【自】每次出击费用2以下的我方单位出击时，直到回合结束为止，这名单位与那名我方单位的战斗力+10。";
            Optional = false;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        List<Card> Targets;

        public override bool CheckConditions()
        {
            return true;
        }

        public override bool CheckInduceConditions(Message message)
        {
            var deployMessage = message as DeployMessage;
            if (deployMessage != null)
            {
                Targets = deployMessage.Filter(deployMessage.Targets, card => card.Controller == Controller && card.DeployCost <= 2);
                return Targets.Count > 0;
            }
            return false;
        }

        public override void Do()
        {
            Targets.ForEach(unit =>
            {
                unit.Attach(new PowerBuff(Owner, this, 10, LastingTypeEnum.UntilTurnEnds));
                Owner.Attach(new PowerBuff(Owner, this, 10, LastingTypeEnum.UntilTurnEnds));
            });
        }

        public override Cost DefineCost()
        {
            return Cost.Null;
        }
    }

    /// <summary>
    /// スキル2
    /// 『サンダーソード』【起】[翻面1]ターン終了まで、このユニットの戦闘力は-１０され、このユニットに<魔法>と射程１-２が追加される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ActionSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "雷之剑";
            Description = "『雷之剑』【起】[翻面1]直到回合结束为止，这名单位的战斗力-10，这名单位追加<魔法>武器与射程1-2。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.UseBondCost(this, 1);
        }

        public override void Do()
        {
            Owner.Attach(new PowerBuff(Owner, this, -10, LastingTypeEnum.UntilTurnEnds));
            Owner.Attach(new WeaponBuff(Owner, this, true, WeaponEnum.Magic, LastingTypeEnum.UntilTurnEnds));
            Owner.Attach(new RangeBuff(Owner, this, true, RangeEnum.OnetoTwo, LastingTypeEnum.UntilTurnEnds));
        }
    }
}
