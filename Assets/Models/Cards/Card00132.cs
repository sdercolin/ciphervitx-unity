using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-086 笑顔のソーサラー ヘンリー
/// </summary>
public class Card00132 : Card
{
    public Card00132(User controller) : base(controller)
    {
        Serial = "00132";
        Pack = "B01";
        CardNum = "B01-086";
        Title = "笑颜巫师";
        UnitName = "亨利";
        power = 50;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『スライム』【自】他の味方がクラスチェンジするたび、あなたと相手はそれぞれ自分の手札を１枚選び、退避エリアに置く。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "黏液";
            Description = "『黏液』【自】每次其它我方单位转职时，你和对手分别选择自己的1张手牌，放置到退避区。";
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
            var levelupMessage = message as LevelUpMessage;
            if (levelupMessage != null && levelupMessage.IsClassChange)
            {
                if (levelupMessage.Target.Controller == Controller && levelupMessage.Target != Owner)
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

        public override async Task Do(Induction induction)
        {
            await Controller.ChooseDiscardHand(Controller.Hand.Cards, 1, 1, false, this);
            await Opponent.ChooseDiscardHand(Opponent.Hand.Cards, 1, 1, false, this);
        }
    }

    /// <summary>
    /// スキル2
    /// 『死の呪い』〖转职技〗【起】[横置，翻面1]このユニットを撃破する。相手は自分の手札を２枚選び、退避エリアに置く。（はこのユニットがクラスチェンジしていなければ使用できない）
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ActionSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "死之诅咒";
            Description = "〖转职技〗『死之诅咒』【起】[横置，翻面1]将这名单位击破。对手选择他自己的2张手牌，放置到退避区。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.CCS;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ActionSelf(this) + Cost.ReverseBond(this, 1);
        }

        public override async Task Do()
        {
            Controller.Destroy(Owner, this, false);
            await Opponent.ChooseDiscardHand(Opponent.Hand.Cards, 2, 2, false, this);
        }
    }
}
