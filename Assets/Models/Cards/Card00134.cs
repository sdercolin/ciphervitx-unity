using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-088 異界より来た男の子 マーク（男）
/// </summary>
public class Card00134 : Card
{
    public Card00134(User controller) : base(controller)
    {
        Serial = "00134";
        Pack = "B01";
        CardNum = "B01-088";
        Title = "来自异界的男孩子";
        UnitName = "玛克（男）";
        power = 40;
        support = 20;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『軍師の系譜』【自】[翻面2]このユニットが出撃した時、自分のオーブの数が相手より少ない場合、コストを支払うなら、デッキの１番上のカードをオーブに追加する。このスキルは味方に『ルフレ（女）』がいなければ発動しない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "军师的系谱";
            Description = "『军师的系谱』【自】[翻面2]这名单位出击时，自己的宝玉数量比对手少的场合，你可以支付费用。如果支付，则将卡组最上方的1张卡追加到宝玉区。这个能力仅限我方战场上存在「路弗雷（女）」时才能发动。";
            Optional = true;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions(Induction induction)
        {
            return Controller.Orb.Count < Opponent.Orb.Count && Controller.Field.Filter(unit => unit.HasUnitNameOf("路弗雷（女）")).Count > 0;
        }

        public override Induction CheckInduceConditions(Message message)
        {
            var deployMessage = message as DeployMessage;
            if (deployMessage != null)
            {
                if (deployMessage.Targets.Contains(Owner))
                {
                    return new Induction();
                }
            }
            return null;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 2);
        }

        public override Task Do(Induction induction)
        {
            Controller.AddToOrb(Controller.Deck.Top, this);
            return Task.CompletedTask;
        }
    }
}
