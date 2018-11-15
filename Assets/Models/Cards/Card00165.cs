using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B02) B02-066 イノセント・キラー ピエリ
/// </summary>
public class Card00165 : Card
{
    public Card00165(User controller) : base(controller)
    {
        Serial = "00165";
        Pack = "B02";
        CardNum = "B02-066";
        Title = "纯洁杀手";
        UnitName = "皮耶莉";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Black);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Armor);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『返り血いっぱいなの！』【自】ユニットが撃破されるたび、ターン終了まで、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "溅起了好多血呢！";
            Description = "『溅起了好多血呢！』【自】每次有单位被击破时，直到回合结束为止，这名单位的战斗力+10。";
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
                if (destroyMessage.DestroyedUnits.Count > 0)
                {
                    return new MyInduction()
                    {
                        Targets = destroyMessage.DestroyedUnits
                    };
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
            var targets = ((MyInduction)induction).Targets;
            targets.ForEach(unit =>
            {
                Controller.AttachItem(new PowerBuff(this, 10, LastingTypeEnum.UntilTurnEnds), Owner);
            });
            return Task.CompletedTask;
        }

        public class MyInduction : Induction
        {
            public List<Card> Targets;
        }
    }

    /// <summary>
    /// スキル2
    /// 『ピエリの小槍』〖转职技〗【起】[翻面1]ターン終了まで、このユニットに射程１-２が追加される。（はこのユニットがクラスチェンジしていなければ使用できない）
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ReverseBondToAddRange1to2
    {
        public Sk2()
        {
            Number = 2;
            Name = "皮耶莉的小枪";
            Description = "〖转职技〗『皮耶莉的小枪』【起】[翻面1]直到回合结束为止，这名单位追加射程1-2。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.CCS;
        }
    }
}
