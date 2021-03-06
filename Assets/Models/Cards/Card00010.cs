using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-009 黒豹と呼ばれし騎士 アベル
/// </summary>
public class Card00010 : Card
{
    public Card00010(User controller) : base(controller)
    {
        Serial = "00010";
        Pack = "S01";
        CardNum = "B01-009";
        Title = "人称黑豹的骑士";
        UnitName = "阿贝尔";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『聖騎士の加護』【常】後衛の敵はこのユニットと出撃コストが２以下の味方を攻撃できない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "圣骑士的加护";
            Description = "『圣骑士的加护』【常】敌方后卫区上的单位不能攻击这名单位与出击费用2以下的我方单位。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                || (card.Controller == Controller && card.IsOnField && card.DeployCost <= 2);
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new WillNotBeAttackedFromBackField(this));
        }
    }

    /// <summary>
    /// スキル2
    /// 『緑赤の双撃』【自】[味方の『カイン』を行動済みにする]このユニットが攻撃した時、コストを支払うなら、戦闘終了まで、このユニットの戦闘力は＋４０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : AutoSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "绿赤之双击";
            Description = "『绿赤之双击』【自】[将我方的「卡因」转为已行动状态]这名单位攻击时，你可以支付费用。如果支付，则直到战斗结束为止，这名单位的战斗力+40。";
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
            return Cost.ActionOthers(this, 1, card => card.HasUnitNameOf(Strings.Get("card_text_unitname_カイン")));
        }

        public override Task Do(Induction induction)
        {
            Controller.AttachItem(new PowerBuff(this, 40, LastingTypeEnum.UntilBattleEnds), Owner);
            return Task.CompletedTask;
        }
    }
}
