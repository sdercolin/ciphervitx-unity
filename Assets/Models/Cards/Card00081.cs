using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-035 オーラの継承者 リンダ
/// </summary>
public class Card00081 : Card
{
    public Card00081(User controller) : base(controller)
    {
        Serial = "00081";
        Pack = "B01";
        CardNum = "B01-035";
        Title = "圣光的继承者";
        UnitName = "琳达";
        power = 50;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『叡智の泉』【自】〖1回合1次〗[翻面1]出撃コストが２以下の味方が出撃した時、コストを支払うなら、カードを１枚引く。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "睿智之泉";
            Description = "『睿智之泉』【自】〖1回合1次〗[翻面1]出击费用2以下的我方单位出击时，你可以支付费用。如果支付，则抽1张卡。";
            OncePerTurn = true;
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
            return Cost.ReverseBond(this, 1);
        }

        public override Task Do(Induction induction)
        {
            Controller.DrawCard(1, this);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『オーラ』【起】〖1回合1次〗[翻面1]ターン終了まで、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ReverseBondToAdd20
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "圣光";
            Description = "『圣光』【起】〖1回合1次〗[翻面1]直到回合结束为止，这名单位的战斗力+20。";
            OncePerTurn = true;
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
            OncePerTurn = true;
        }
    }
}
