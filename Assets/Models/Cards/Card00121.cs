using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-075 完璧なる天馬騎士 ティアモ
/// </summary>
public class Card00121 : Card
{
    public Card00121(User controller) : base(controller)
    {
        Serial = "00121";
        Pack = "B01";
        CardNum = "B01-075";
        Title = "完美的天马骑士";
        UnitName = "缇雅莫";
        power = 50;
        support = 30;
        deployCost = 3;
        classChangeCost = 2;
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
    /// 『疾風迅雷』【自】〖1回合1次〗このユニットの攻撃で敵を撃破した時、このユニットを未行動にする。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "疾风迅雷";
            Description = "『疾风迅雷』【自】〖1回合1次〗这名单位的攻击击破敌方单位时，将这名单位转为未行动状态。";
            OncePerTurn = true;
            Optional = false;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions(Induction induction)
        {
            return true;
        }

        public override Induction CheckInduceConditions(Message message)
        {
            var destroyMessage = message as DestroyMessage;
            if (destroyMessage != null)
            {
                foreach (var unit in destroyMessage.DestroyedUnits)
                {
                    if (destroyMessage.AttackingUnit == Owner && unit.Controller == Opponent)
                    {
                        return new Induction();
                    }
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
            Controller.Untap(Owner, this);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『手製の手槍』〖转职技〗【起】[翻面1]ターン終了まで、すべての<飛行>の味方に<槍>と射程１-２が追加される。（はこのユニットがクラスチェンジしていなければ使用できない）
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ActionSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "自制的投枪";
            Description = "〖转职技〗『自制的投枪』【起】[翻面1]直到回合结束为止，所有<飞行>属性的我方单位追加<枪>武器与射程1-2。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.CCS;
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
            var targets = Controller.Field.Filter(unit => unit.HasType(TypeEnum.Flight));
            targets.ForEach(unit =>
                {
                    Controller.AttachItem(new WeaponBuff(this, true, WeaponEnum.Lance, LastingTypeEnum.UntilTurnEnds), unit);
                    Controller.AttachItem(new RangeBuff(this, true, RangeEnum.OnetoTwo, LastingTypeEnum.UntilTurnEnds), unit);
                });
            return Task.CompletedTask;
        }
    }
}
