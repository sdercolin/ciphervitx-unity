using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-097 花の似合う男 アズール
/// </summary>
public class Card00143 : Card
{
    public Card00143(User controller) : base(controller)
    {
        Serial = "00143";
        Pack = "B01";
        CardNum = "B01-097";
        Title = "与花相配的男子";
        UnitName = "阿兹尔";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
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
    /// 『太陽』【自】このユニットの攻撃で敵を撃破した時、次の相手のターン終了まで、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "太阳";
            Description = "『太阳』【自】这名单位的攻击击破敌方单位时，直到下一次对手的回合结束为止，这名单位的战斗力+20。";
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
            Controller.AttachItem(new PowerBuff(this, 20, LastingTypeEnum.UntilNextOpponentTurnEnds), Owner);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『運命の出会い』〖转职技〗【常】このユニットを支援しているカードが<女>の場合、このユニットの戦闘力は＋１０される。（はこのユニットがクラスチェンジしていなければ有効にならない）
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "命运的邂逅";
            Description = "〖转职技〗『命运的邂逅』【常】这名单位被<女性>单位支援的期间，这名单位的战斗力+10。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.CCS;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.BattlingUnits.Contains(Owner)
                && Controller.Support.Filter(supportcard => supportcard.HasGender(GenderEnum.Female)).Count > 0;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new SupportBuff(this, 10));
        }
    }
}
