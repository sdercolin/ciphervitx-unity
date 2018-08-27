using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-011 アリティアの盾 ドーガ
/// </summary>
public class Card00057 : Card
{
    public Card00057(User controller) : base(controller)
    {
        Serial = "00057";
        Pack = "B01";
        CardNum = "B01-011";
        Title = "阿利缇亚之盾";
        UnitName = "杜卡";
        power = 50;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Armor);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『鉄壁の盾』【自】[翻面1]他の味方が攻撃された時、コストを支払うなら、このユニットはその味方の代わりに攻撃される。このスキルはこのユニットが前衛でなければ発動しない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "铁壁之盾";
            Description = "『铁壁之盾』【自】[翻面1]其他我方单位被攻击时，你可以支付费用。如果支付，则这名单位代替那名我方单位被攻击。这个能力仅限这名单位位于前卫区时才能发动。";
            Optional = true;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions(Induction induction)
        {
            return Owner.BelongedRegion == Controller.FrontField;
        }

        public override Induction CheckInduceConditions(Message message)
        {
            var attackMessage = message as AttackMessage;
            if (attackMessage != null)
            {
                if (attackMessage.DefendingUnit.Controller == Controller && attackMessage.DefendingUnit != Owner)
                {
                    return new Induction();
                }
            }
            return null;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 1);
        }

        public override Task Do(Induction induction)
        {
            //TODO
            Owner.Attach(new ReplaceAsDeffender(this));
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『重装の心得』【常】このユニットが<魔法>以外に攻撃されている場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ArmorExpertise
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "重装的心得";
            Description = "『重装的心得』【常】这名单位被<魔法>以外的武器攻击的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }
    }
}
