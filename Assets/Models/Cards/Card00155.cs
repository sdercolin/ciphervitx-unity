using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-009 恋心を秘めた騎士 ティアモ
/// </summary>
public class Card00155 : Card
{
    public Card00155(User controller) : base(controller)
    {
        Serial = "00155";
        Pack = "P01";
        CardNum = "P01-009";
        Title = "暗藏恋心的骑士";
        UnitName = "缇雅莫";
        power = 40;
        support = 30;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Flight);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『リフレッシュ』【自】お互いのターン開始時、このユニットと同じエリアに味方が他に１体もいない場合、ターン終了まで、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "振奋";
            Description = "『振奋』【自】双方的回合开始时，这名单位所在区域上没有其他我方单位的场合，直到回合结束为止，这名单位的战斗力+10。";
            Optional = false;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions(Induction induction)
        {
            return Owner.BelongedRegion.TrueForAllCard(unit => unit == Owner);
        }

        public override Induction CheckInduceConditions(Message message)
        {
            var startTurnMessage = message as StartTurnMessage;
            if (startTurnMessage != null)
            {
                return new Induction();
            }
            return null;
        }

        public override Cost DefineCost()
        {
            return Cost.Null;
        }

        public override Task Do(Induction induction)
        {
            Controller.AttachItem(new PowerBuff(this, 10, LastingTypeEnum.UntilTurnEnds), Owner);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『手槍』【起】[翻面1]ターン終了まで、このユニットに射程１-２が追加される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ReverseBondToAddRange1to2
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "投枪";
            Description = "『投枪』【起】[翻面1]直到回合结束为止，这名单位追加射程1-2。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }
    }
}
