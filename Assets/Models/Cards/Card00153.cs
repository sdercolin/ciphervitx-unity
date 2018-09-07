using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-007 癒やしの勇者 リフ
/// </summary>
public class Card00153 : Card
{
    public Card00153(User controller) : base(controller)
    {
        Serial = "00153";
        Pack = "P01";
        CardNum = "P01-007";
        Title = "治愈的勇者";
        UnitName = "里弗";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Axe);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『きずぐすり』【自】[横置，このユニットを撃破する]他の味方が攻撃された時、コストを支払うなら、戦闘終了まで、その防御ユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "伤药";
            Description = "『伤药』【自】[横置，将这名单位击破]其他我方单位被攻击时，你可以支付费用。如果支付，则直到战斗结束为止，那名防御单位的战斗力+20。";
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
            var attackMessage = message as AttackMessage;
            if (attackMessage != null)
            {
                var target = attackMessage.DefendingUnit;
                if (target.Controller == Controller && target != Owner)
                {
                    return new MyInduction()
                    {
                        Target = target
                    };
                }
            }
            return null;
        }

        public override Cost DefineCost()
        {
            return Cost.ActionSelf(this) + Cost.DestroySelf(this);
        }

        public override Task Do(Induction induction)
        {
            var target = ((MyInduction)induction).Target;
            Controller.AttachItem(new PowerBuff(this, 20, LastingTypeEnum.UntilBattleEnds), target);
            return Task.CompletedTask;
        }

        public class MyInduction : Induction
        {
            public Card Target;
        }
    }
}
