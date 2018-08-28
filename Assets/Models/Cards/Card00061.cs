using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-015 癒やしの僧 リフ
/// </summary>
public class Card00061 : Card
{
    public Card00061(User controller) : base(controller)
    {
        Serial = "00061";
        Pack = "B01";
        CardNum = "B01-015";
        Title = "治愈的僧侣";
        UnitName = "里弗";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Staff);
        ranges.Add(RangeEnum.None);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
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
            return Cost.Action(this) + Cost.Destroy(Owner, this);
        }

        public override Task Do(Induction induction)
        {
            var target = ((MyInduction)induction).Target;
            target.Attach(new PowerBuff(this, 20, LastingTypeEnum.UntilBattleEnds));
            return Task.CompletedTask;
        }

        public class MyInduction : Induction
        {
            public Card Target;
        }
    }

    /// <summary>
    /// スキル2
    /// 〖防御型〗『祈りの紋章』【支】戦闘終了まで、相手の攻撃ユニットは必殺攻撃できない。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : MiracleEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "祈祷之纹章";
            Description = "〖防御型〗『祈祷之纹章』【支】直到战斗结束为止，对手的攻击单位不能进行必杀攻击。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
