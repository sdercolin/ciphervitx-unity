using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-028 疾風の賢者 マリク
/// </summary>
public class Card00021 : Card
{
    public Card00021(User controller) : base(controller)
    {
        Serial = "00021";
        Pack = "S01";
        CardNum = "B01-028";
        Title = "疾风之贤者";
        UnitName = "玛利克";
        power = 60;
        support = 20;
        deployCost = 4;
        classChangeCost = 3;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
        sk3 = new Sk3();
        Attach(sk3);
    }

    /// <summary>
    /// スキル1
    /// 『エクスカリバー』【起】〖1回合1次〗[翻面1]ターン終了まで、このユニットは『飛行特効』を得る。（『飛行特効』【常】このユニットが<飛行>を攻撃している場合、このユニットの戦闘力は＋３０される。）
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "圣风刃";
            Description = "『圣风刃』【起】〖1回合1次〗[翻面1]直到回合结束为止，这名单位获得『飞行特效』。（『飞行特效』【常】这名单位攻击<飞行>属性单位的期间，这名单位的战斗力+30。）";
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
            return Cost.ReverseBond(this, 1);
        }

        public override Task Do()
        {
            Owner.Attach(new EnableSkill(this, LastingTypeEnum.UntilTurnEnds)
            {
                Target = Owner.SkillList.Find(item => item.Name == "飞行特效")
            });
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『風の超魔法』【自】このユニットの攻撃で敵を撃破した時、このユニットがこのターンに『エクスカリバー』を使用しているなら、カードを１枚引く。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : AutoSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "风之超魔法";
            Description = "『风之超魔法』【自】这名单位的攻击击破敌方单位时，如果这名单位在这个回合中使用过『圣风刃』，抽1张卡。";
            Optional = false;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return ((Card00021)Owner).sk1.UsedInThisTurn;
        }

        public override bool CheckInduceConditions(Message message)
        {
            var destroyMessage = message as DestroyMessage;
            if (destroyMessage != null)
            {
                return destroyMessage.AttackingUnit == Owner;
            }
            return false;
        }

        public override Cost DefineCost()
        {
            return Cost.Null;
        }

        public override Task Do()
        {
            Owner.Controller.DrawCard(1, this);
            return Task.CompletedTask;
        }
    }

    public Sk3 sk3;
    public class Sk3 : Wingslayer
    {
        public Sk3() : base()
        {
            Number = 3;
            Name = "飞行特效";
            Description = "『飞行特效』【常】这名单位攻击<飞行>属性单位的期间，这名单位的战斗力+30。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
            Available = false;
        }
    }
}
