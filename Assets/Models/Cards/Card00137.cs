using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-091 選ばれし希望の戦士 ウード
/// </summary>
public class Card00137 : Card
{
    public Card00137(User controller) : base(controller)
    {
        Serial = "00137";
        Pack = "B01";
        CardNum = "B01-091";
        Title = "天选的希望战士";
        UnitName = "伍德";
        power = 70;
        support = 10;
        deployCost = 4;
        classChangeCost = 3;
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
    /// 『聖魔剣 ホーリーデビルソード』【起】[自分の手札から『ウード』を１枚退避エリアに置く]ターン終了まで、このユニットの戦闘力は２倍になる。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "圣魔剑·Holy Devil Sword";
            Description = "『圣魔剑·Holy Devil Sword』【起】[从自己的手牌将1张「伍德」放置到退避区]直到回合结束为止，这名单位的战斗力变为2倍。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.DiscardHand(this, 1, card => card.HasUnitNameOf(Strings.Get("card_text_unitname_ウード")));
        }

        public override Task Do()
        {
            Controller.AttachItem(new PowerBuff(this, Owner.Power, LastingTypeEnum.UntilTurnEnds), Owner);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『古の魔剣 ミストルティン』〖转职技〗【常】このユニットは必殺攻撃できず、このユニットが攻撃で破壊するオーブは２つになる。（はこのユニットがクラスチェンジしていなければ有効にならない）
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2()
        {
            Number = 2;
            Name = "古代魔剑 米斯托尔汀";
            Description = "〖转职技〗『古代魔剑 米斯托尔汀』【常】这名单位不能进行必杀攻击，这名单位的攻击所将破坏的宝玉变为2颗。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.CCS;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new CanNotCriticalAttack(this));
            ItemsToApply.Add(new DestroyTwoOrbs(this));
        }
    }
}
