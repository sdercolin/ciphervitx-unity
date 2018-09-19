using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-021 タリスの義勇兵 バーツ
/// </summary>
public class Card00018 : Card
{
    public Card00018(User controller) : base(controller)
    {
        Serial = "00018";
        Pack = "S01";
        CardNum = "B01-021";
        Title = "塔利斯的义勇兵";
        UnitName = "巴兹";
        power = 40;
        support = 10;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Axe);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『サジマジバーツ』〖阵型技〗【自】[味方の『サジ』と『マジ』を行動済みにする]このユニットが攻撃した時、コストを支払うなら、戦闘終了まで、このユニットの戦闘力は＋５０され、このユニットが攻撃で破壊するオーブは２つになる。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "萨基玛基巴兹";
            Description = "〖阵型技〗『萨基玛基巴兹』【自】[将我方的「萨基」和「玛基」转为已行动状态]这名单位攻击时，你可以支付费用。如果支付，则直到战斗结束为止，这名单位的战斗力+50，这名单位的攻击所将破坏的宝玉变为2颗。";
            Optional = true;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.FS;
        }

        public override bool CheckConditions(Induction induction)
        {
            return true;
        }

        public override Induction CheckInduceConditions(Message message)
        {
            var attackMessage = message as AttackMessage;
            if (attackMessage != null)
            {
                if (attackMessage.AttackingUnit == Owner)
                {
                    return new Induction();
                }
            }
            return null;
        }

        public override Cost DefineCost()
        {
            return Cost.ActionOthers(this, 1, card => card.HasUnitNameOf(Strings.Get("card_text_unitname_サジ"))) + Cost.ActionOthers(this, 1, card => card.HasUnitNameOf(Strings.Get("card_text_unitname_マジ")));
        }

        public override Task Do(Induction induction)
        {
            Controller.AttachItem(new PowerBuff(this, 50, LastingTypeEnum.UntilBattleEnds), Owner);
            Controller.AttachItem(new DestroyTwoOrbs(this, LastingTypeEnum.UntilBattleEnds), Owner);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『戦士の心得』【常】自分のターン中、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "战士的心得";
            Description = "『战士的心得』【常】自己的回合中，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.TurnPlayer == Owner.Controller;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 20));
        }
    }
}
