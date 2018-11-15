using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-077 不敵なる傭兵 グレゴ
/// </summary>
public class Card00123 : Card
{
    public Card00123(User controller) : base(controller)
    {
        Serial = "00123";
        Pack = "B01";
        CardNum = "B01-077";
        Title = "无畏的佣兵";
        UnitName = "古雷格";
        power = 40;
        support = 10;
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
    /// 『グレゴと幼き竜』【常】このユニットが敵の攻撃で撃破された場合、このユニットを退避エリアに置く代わりに自分の絆エリアに置く。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "古雷格与幼龙";
            Description = "『古雷格与幼龙』【常】这名单位被敌方单位的攻击击破的场合，不将这名单位放置到退避区，改为将其放置到自己的羁绊区。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new SetDestroyedCardToBondInsteadOfRetreat(this));
        }
    }

    public class SetDestroyedCardToBondInsteadOfRetreat : SubSkill
    {
        public SetDestroyedCardToBondInsteadOfRetreat(Skill origin, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType) { }

        public override bool Try(Message message, ref Message substitute)
        {
            var sendToRetreatDestructionProcessMessage = message as SendToRetreatDestructionProcessMessage;
            if (sendToRetreatDestructionProcessMessage != null)
            {
                if (sendToRetreatDestructionProcessMessage.Targets.Contains(Owner) && Owner.DestructionReasonTag == DestructionReasonTag.ByBattle)
                {
                    sendToRetreatDestructionProcessMessage.Targets.Remove(Owner);
                    if (sendToRetreatDestructionProcessMessage.Targets.Count == 0)
                    {
                        sendToRetreatDestructionProcessMessage = null;
                    }
                    substitute = sendToRetreatDestructionProcessMessage + new ToBondMessage()
                    {
                        Targets = new List<Card>() { Owner },
                        TargetFrontShown = true,
                        Reason = this
                    };
                    return false;
                }
            }
            return true;
        }
    }
}
