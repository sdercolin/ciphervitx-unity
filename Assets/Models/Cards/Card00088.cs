using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-042 天馬三姉妹の末っ子 エスト
/// </summary>
public class Card00088 : Card
{
    public Card00088(User controller) : base(controller)
    {
        Serial = "00088";
        Pack = "B01";
        CardNum = "B01-042";
        Title = "天马三姐妹的末女";
        UnitName = "爱丝特";
        power = 50;
        support = 30;
        deployCost = 3;
        classChangeCost = 2;
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
    /// 『トライアングルアタック』〖阵型技〗【自】[味方の『パオラ』と『カチュア』を行動済みにする]このユニットが攻撃した時、コストを支払うなら、戦闘終了まで、このユニットの戦闘力は＋５０され、攻撃は神速回避されない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "三角攻击";
            Description = "〖阵型技〗『三角攻击』【自】[将我方的「帕奥拉」和「卡秋雅」转为已行动状态]这名单位攻击时，你可以支付费用。如果支付，则直到战斗结束为止，这名单位的战斗力+50，攻击不能被神速回避。";
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
            return Cost.ActionOthers(this, 1, card => card.HasUnitNameOf(Strings.Get("card_text_unitname_パオラ"))) + Cost.ActionOthers(this, 1, card => card.HasUnitNameOf(Strings.Get("card_text_unitname_カチュア")));
        }

        public override Task Do(Induction induction)
        {
            Controller.AttachItem(new PowerBuff(this, 50, LastingTypeEnum.UntilBattleEnds), Owner);
            Controller.AttachItem(new CanNotBeAvoided(this, LastingTypeEnum.UntilBattleEnds), Owner);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『三姉妹の絆』【常】このユニットが『パオラ』か『カチュア』に支援されている場合、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "三姐妹的羁绊";
            Description = "『三姐妹的羁绊』【常】这名单位被「帕奥拉」或「卡秋雅」支援的期间，这名单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.BattlingUnits.Contains(card)
                && (card.Controller.Support.SupportedBy(Strings.Get("card_text_unitname_パオラ")) || card.Controller.Support.SupportedBy(Strings.Get("card_text_unitname_カチュア")));
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10));
        }
    }
}
