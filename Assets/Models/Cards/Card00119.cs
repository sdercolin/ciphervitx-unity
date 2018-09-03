using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-073 スイーテストアサシン ガイア
/// </summary>
public class Card00119 : Card
{
    public Card00119(User controller) : base(controller)
    {
        Serial = "00119";
        Pack = "B01";
        CardNum = "B01-073";
        Title = "甜蜜刺客";
        UnitName = "盖亚";
        power = 50;
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
    /// 『暗殺』【起】[横置]相手のデッキの１番上のカードを公開させる。そのカードの出撃コストが３以上の場合、あなたは翻面2してもよい。そうしたなら、敵を１体選び、撃破する。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "暗杀";
            Description = "『暗杀』【起】[横置]公开对手卡组最上方的1张卡。那张卡的出击费用在3以上的场合，你可以[翻面2]。如果这样做了，则选择1名敌方单位，将其击破。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ActionSelf(this);
        }

        public override async Task Do()
        {
            var target = Opponent.Deck.Top;
            Opponent.ShowCard(target, this);
            if (target.DeployCost >= 3)
            {
                var choices = Controller.GetReversableBonds(this);
                if (choices.Count >= 2)
                {
                    if (await Request.AskIfReverseBond(2, this, Controller))
                    {
                        await Controller.ChooseReverseBond(choices, 2, 2, this, false);
                        await Controller.ChooseDestroy(Opponent.Field.Cards, 1, 1, this, false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// スキル2
    /// 『報酬はスイーツ』〖转职技〗【自】このユニットの『暗殺』で敵を撃破した時、カードを１枚引く。（はこのユニットがクラスチェンジしていなければ発動しない）
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : AutoSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "报酬是甜食";
            Description = "〖转职技〗『报酬是甜食』【自】这名单位的『暗杀』击破敌方单位时，抽1张卡。（〖转职技〗仅限这名单位经过转职后才能使用）";
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
                if (destroyMessage.Reason == ((Card00119)Owner).sk1)
                {
                    return new Induction();
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
            Owner.Controller.DrawCard(1, this);
            return Task.CompletedTask;
        }
    }
}
