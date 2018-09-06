using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-054 未来を知る者 ルキナ
/// </summary>
public class Card00100 : Card
{
    public Card00100(User controller) : base(controller)
    {
        Serial = "00100";
        Pack = "B01";
        CardNum = "B01-054";
        Title = "知晓未来者";
        UnitName = "露琪娜";
        power = 70;
        support = 20;
        deployCost = 4;
        classChangeCost = 3;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『運命に抗う者』【起】〖1回合1次〗[翻面1，自分の手札から『ルキナ』を１枚退避エリアに置く]カードを２枚引き、自分の手札を１枚選び、デッキの１番上に置く。このユニットがクラスチェンジしている場合、代わりにカードを３枚引き、自分の手札を２枚選び、デッキの１番上に好きな順番で置く。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "抗争命运者";
            Description = "『抗争命运者』【起】〖1回合1次〗[翻面1，从自己的手牌将1张「露琪娜」放置到退避区]抽2张卡，选择自己的1张手牌，放置到卡组最上方。这名单位经过转职后的场合，改为抽卡3张，选择自己的2张手牌，按任意顺序放置到卡组最上方。";
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
            return Cost.ReverseBond(this, 1) + Cost.DiscardHand(this, 1, card => card.HasUnitNameOf("露琪娜"));
        }

        public override async Task Do()
        {
            if (!Owner.IsClassChanged)
            {
                Controller.DrawCard(2, this);
                await Controller.ChooseSetToDeckTop(Controller.Hand.Cards, 1, 1, this);
            }
            else
            {
                Controller.DrawCard(3, this);
                await Controller.ChooseSetToDeckTop(Controller.Hand.Cards, 2, 2, this, Request.RequestFlags.ShowOrder);
            }
        }
    }

    /// <summary>
    /// スキル2
    /// 『裏剣 ファルシオン』【常】このユニットが<竜>を攻撃している場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : Dragonslayer
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "里剑 法尔西昂";
            Description = "『里剑 法尔西昂』【常】这名单位攻击<龙>属性单位的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }
    }
}
