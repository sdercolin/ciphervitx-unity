using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-015 謎多き戦術師 ルフレ（男）
/// </summary>
public class Card00159 : Card
{
    public Card00159(User controller) : base(controller)
    {
        Serial = "00159";
        Pack = "P01";
        CardNum = "P01-015";
        Title = "谜团重重的战术师";
        UnitName = "路弗雷（男）";
        power = 50;
        support = 20;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『戦知識』【自】[翻面2]このユニットが出撃した時、自分のオーブの数が相手より少ない場合、コストを支払うなら、デッキの１番上のカードをオーブに追加する。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "战斗知识";
            Description = "『战斗知识』【自】[翻面2]这名单位出击时，自己的宝玉数量比对手少的场合，你可以支付费用。如果支付，则从卡组最上方将1张卡放置到宝玉区。";
            Optional = true;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions(Induction induction)
        {
            return Controller.Orb.Count < Opponent.Orb.Count;
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
