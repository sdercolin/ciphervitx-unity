using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-059 イーリス聖王国の王女 リズ
/// </summary>
public class Card00033 : Card
{
    public Card00033(User controller) : base(controller)
    {
        Serial = "00033";
        Pack = "S02";
        CardNum = "B01-059";
        Title = "伊利斯圣王国的王女";
        UnitName = "莉兹";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Axe);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『闘う修道女』〖转职技〗【自】このユニットの攻撃で敵を撃破した時、自分の退避エリアから『リズ』以外で出撃コストが３以下のカードを１枚選び、手札に加える。（はこのユニットがクラスチェンジしていなければ発動しない）
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "战斗的修道女";
            Description = "〖转职技〗『战斗的修道女』【自】这名单位的攻击击破敌方单位时，从自己的退避区选择1张「莉兹」以外的出击费用3以下的卡，将其加入手牌。（〖转职技〗仅限这名单位经过转职后才能使用）";
            Optional = false;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.CCS;
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

        public override async Task Do(Induction induction)
        {
            await Controller.ChooseAddToHand(Controller.Retreat.Filter(unit => !unit.HasUnitNameOf("莉兹") && unit.DeployCost <= 3), 1, 1, this);
        }
    }
}
