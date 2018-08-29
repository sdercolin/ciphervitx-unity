using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-004 愛を説く翼 シーダ
/// </summary>
public class Card00050 : Card
{
    public Card00050(User controller) : base(controller)
    {
        Serial = "00050";
        Pack = "B01";
        CardNum = "B01-004";
        Title = "倡爱之翼";
        UnitName = "希达";
        power = 60;
        support = 30;
        deployCost = 4;
        classChangeCost = 3;
        symbols.Add(SymbolEnum.Red);
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
    /// 『説得』【自】[翻面1]このユニットの攻撃で敵を撃破した時、コストを支払うなら、自分のデッキから『シーダ』以外で<光の剣>のカードを１枚選び、公開してから手札に加える。その後、デッキをシャッフルする。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "说得";
            Description = "『说得』【自】[翻面1]这名单位的攻击击破敌方单位时，你可以支付费用。如果支付，则从自己的卡组中选择1张「希达」以外的<光之剑>势力的卡，将其公开并加入手牌。这之后，切洗卡组。";
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
            await Controller.ChooseAddToHand(Controller.Deck.Filter(unit => !unit.HasUnitNameOf("希达") && unit.HasSymbol(SymbolEnum.Red)), 1, 1, this);
            //TODO
            Controller.ShuffleDeck(this);
        }
    }

    /// <summary>
    /// スキル2
    /// 『天空を翔ける者』【起】〖1回合1次〗このユニットを移動させる。このスキルはこのユニットが未行動でなければ使用できない。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ActionSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "翱翔天空者";
            Description = "『翱翔天空者』【起】〖1回合1次〗将这名单位移动。这个能力只能在这名单位处于未行动状态时使用。";
            OncePerTurn = true;
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return !Owner.IsHorizontal;
        }

        public override Cost DefineCost()
        {
            return Cost.Null;
        }

        public override Task Do()
        {
            Controller.Move(Owner, this);
            return Task.CompletedTask;
        }
    }
}
