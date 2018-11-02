using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-022 紅の死神 ナバール
/// </summary>
public class Card00068 : Card
{
    public Card00068(User controller) : base(controller)
    {
        Serial = "00068";
        Pack = "B01";
        CardNum = "B01-022";
        Title = "红色死神";
        UnitName = "那巴尔";
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
    /// 『告死の一閃』【起】[自分の手札から『ナバール』を１枚退避エリアに置く]ターン終了まで、このユニットの攻撃は神速回避されない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "告死的一闪";
            Description = "『告死的一闪』【起】[从自己的手牌将1张「那巴尔」放置到退避区]直到回合结束为止，这名单位的攻击不能被神速回避。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.DiscardHand(this, 1, unit => unit.HasUnitNameOf(Strings.Get("card_text_unitname_ナバール")));
        }

        public override Task Do()
        {
            Controller.AttachItem(new CanNotBeAvoided(this, LastingTypeEnum.UntilTurnEnds), Owner);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『二刀流』【自】[翻面1]このユニットの攻撃で敵を撃破した時、コストを支払うなら、主人公以外で出撃コストが２以下の敵を１体選び、撃破する。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : AutoSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "二刀流";
            Description = "『二刀流』【自】[翻面1]这名单位的攻击击破敌方单位时，你可以支付费用。如果支付，选择1名主人公以外的出击费用2以下的敌方单位，将其击破。";
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
            return Cost.ReverseBond(this, 1);
        }

        public override async Task Do(Induction induction)
        {
            var choices = Opponent.Field.Filter(unit => unit.DeployCost <= 2 && !unit.IsHero);
            await Controller.ChooseDestroy(choices, 0, 1, this, false);
        }
    }
}
