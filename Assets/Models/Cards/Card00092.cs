using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-046 マムクート・プリンセス チキ
/// </summary>
public class Card00092 : Card
{
    public Card00092(User controller) : base(controller)
    {
        Serial = "00092";
        Pack = "B01";
        CardNum = "B01-046";
        Title = "龙人公主";
        UnitName = "芝琪";
        power = 60;
        support = 20;
        deployCost = 5;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.DragonStone);
        types.Add(TypeEnum.Dragon);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
        sk3 = new Sk3();
        Attach(sk3);
    }

    /// <summary>
    /// スキル1
    /// 『竜姫の微笑み』【常】自分のターン中、このユニットの支援に成功したカードが退避エリアに置かれる場合、代わりに自分の絆エリアに置いてもよい。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "龙姬的微笑";
            Description = "『龙姬的微笑』【常】自己的回合中，成功支援这名单位的卡被放置到退避区的场合，你可以改为将其放置到自己的羁绊区。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner && Game.TurnPlayer == Controller;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new SetSupportCardToBondInsteadOfRetreat(this));
        }
    }

    /// <summary>
    /// スキル2
    /// 『長寿な竜一族』【常】自分の絆カードが８枚以上の場合、このユニットの戦闘力は＋３０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "长寿的龙一族";
            Description = "『长寿的龙一族』【常】自己的羁绊卡有8张以上的场合，这名单位的战斗力+30。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner && Controller.Bond.Count >= 8;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 30));
        }
    }

    /// <summary>
    /// スキル3
    /// 『神竜石』【常】このユニットが<竜>を攻撃している場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk3 sk3;
    public class Sk3 : Dragonslayer
    {
        public Sk3() : base()
        {
            Number = 3;
            Name = "神龙石";
            Description = "『神龙石』【常】这名单位攻击<龙>属性单位的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }
    }

    public class SetSupportCardToBondInsteadOfRetreat : SubSkill
    {
        public SetSupportCardToBondInsteadOfRetreat(Skill origin, LastingTypeEnum lastingType = LastingTypeEnum.Forever) : base(origin, lastingType) { }

        public override bool Try(Message message, ref Message substitute)
        {
            var removeSupportMessage = message as RemoveSupportMessage;
            if (removeSupportMessage != null)
            {
                if (Game.AttackingUnit == Owner)
                {
                    substitute = new ToBondMessage()
                    {
                        Targets = removeSupportMessage.Targets,
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
