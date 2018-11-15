using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-025 慈愛の聖女 レナ
/// </summary>
public class Card00071 : Card
{
    public Card00071(User controller) : base(controller)
    {
        Serial = "00071";
        Pack = "B01";
        CardNum = "B01-025";
        Title = "慈爱的圣女";
        UnitName = "蕾娜";
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
    }

    /// <summary>
    /// スキル1
    /// 『聖者の祝福』【自】〖1回合1次〗[翻面1]出撃コストが２以下の味方が出撃した時、コストを支払うなら、自分の退避エリアから『レナ』以外で出撃コストが１のカードを１枚選び、手札に加える。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "圣者的祝福";
            Description = "『圣者的祝福』【自】〖1回合1次〗[翻面1]出击费用2以下的我方单位出击时，你可以支付费用。如果支付，从自己的退避区中选择1张「蕾娜」以外的出击费用1的卡，将其加入手牌。";
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

        public override async Task Do(Induction induction)
        {
            await Controller.ChooseAddToHand(Controller.Retreat.Filter(card => card.DeployCost == 1 && !card.HasUnitNameOf(Strings.Get("card_text_unitname_レナ"))), 0, 1, this);
        }
    }
}
