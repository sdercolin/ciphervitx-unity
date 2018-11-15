using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-016 タリスの闘神 オグマ
/// </summary>
public class Card00062 : Card
{
    public Card00062(User controller) : base(controller)
    {
        Serial = "00062";
        Pack = "B01";
        CardNum = "B01-016";
        Title = "塔利斯的斗神";
        UnitName = "奥古玛";
        power = 70;
        support = 10;
        deployCost = 4;
        classChangeCost = 3;
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
    /// 『戦場の息吹』【起】[他の味方を１体行動済みにする]ターン終了まで、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "战场的气息";
            Description = "『战场的气息』【起】[将1名其他我方单位转为已行动状态]直到回合结束为止，这名单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ActionOthers(this, 1);
        }

        public override Task Do()
        {
            Controller.AttachItem((new PowerBuff(this, 10, LastingTypeEnum.UntilTurnEnds)), Owner);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『闘神の一撃』【自】このユニットが攻撃した時、このユニットの戦闘力が１００以上の場合、ターン終了まで、このユニットが攻撃で破壊するオーブは２つになる。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : AutoSkill
    {
        public Sk2()
        {
            Number = 2;
            Name = "斗神的一击";
            Description = "『斗神的一击』【自】这名单位攻击时，这名单位的战斗力在100以上的场合，直到回合结束为止，这名单位的攻击所将破坏的宝玉变为2颗。";
            Optional = false;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions(Induction induction)
        {
            return Owner.Power >= 100;
        }

        public override Induction CheckInduceConditions(Message message)
        {
            var attackMessage = message as AttackMessage;
            if (attackMessage != null)
            {
                if (attackMessage.AttackingUnit == Owner)
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

        public override Task Do(Induction induction)
        {
            Controller.AttachItem(new DestroyTwoOrbs(this, LastingTypeEnum.UntilTurnEnds), Owner);
            return Task.CompletedTask;
        }
    }
}
