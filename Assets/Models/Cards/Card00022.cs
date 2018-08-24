using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-029 風の魔道士 マリク
/// </summary>
public class Card00022 : Card
{
    public Card00022(User controller) : base(controller)
    {
        Serial = "00022";
        Pack = "S01";
        CardNum = "B01-029";
        Title = "风之魔法师";
        UnitName = "玛利克";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
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
    /// 〖攻击型〗『魔術の紋章』【支】カードを１枚引く。自分の手札を１枚選び、退避エリアに置く。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : MagicEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "魔术之纹章";
            Description = "〖攻击型〗『魔术之纹章』【支】抽1张卡。选择自己的1张手牌，放置到退避区。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
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
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
            Available = false;
        }
    }
}