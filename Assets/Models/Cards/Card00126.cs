using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-080 愛と執念の闇使い サーリャ
/// </summary>
public class Card00126 : Card
{
    public Card00126(User controller) : base(controller)
    {
        Serial = "00126";
        Pack = "B01";
        CardNum = "B01-080";
        Title = "爱与执念的暗魔法师";
        UnitName = "萨莉雅";
        power = 60;
        support = 20;
        deployCost = 4;
        classChangeCost = 3;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『ルイン』【起】〖1回合1次〗[翻面3，自分の手札から『サーリャ』を１枚退避エリアに置く]相手は自分の手札を２枚選び、退避エリアに置く。このユニットがクラスチェンジしている場合、代わりに相手は自分の手札を３枚選び、退避エリアに置く。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "破灭";
            Description = "『破灭』【起】〖1回合1次〗[翻面3，从自己的手牌将1张「萨莉雅」放置到退避区]对手选择他自己的2张手牌，放置到退避区。这个单位经过转职后的场合，改为对手选择他自己的3张手牌，放置到退避区。";
            OncePerTurn = true;
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 3) + Cost.DiscardHand(this, 1, card => card.HasUnitNameOf(Strings.Get("card_text_unitname_サーリャ")));
        }

        public override async Task Do()
        {
            if (!Owner.IsClassChanged)
            {
                await Opponent.ChooseDiscardHand(Opponent.Hand.Cards, 2, 2, false, this);
            }
            else
            {
                await Opponent.ChooseDiscardHand(Opponent.Hand.Cards, 3, 3, false, this);
            }
        }
    }

    /// <summary>
    /// スキル2
    /// 『禁断の呪い』【常】相手の手札が１枚もない場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "禁断的诅咒";
            Description = "『禁断的诅咒』【常】对手没有手牌的场合，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner && Opponent.Hand.Count == 0;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 20));
        }
    }
}
