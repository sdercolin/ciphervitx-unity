using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-005 心癒やす翼 シーダ
/// </summary>
public class Card00051 : Card
{
    public Card00051(User controller) : base(controller)
    {
        Serial = "00051";
        Pack = "B01";
        CardNum = "B01-005";
        Title = "愈心之翼";
        UnitName = "希达";
        power = 40;
        support = 30;
        deployCost = 2;
        classChangeCost = 0;
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
    /// 『ウイングスピア』【常】このユニットが<獣馬>か<アーマー>を攻撃している場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "翼枪";
            Description = "『翼枪』【常】这名单位攻击<兽马>或<重甲>属性单位的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.AttackingUnit == card
                && (Game.DefendingUnit.HasType(TypeEnum.Beast)|| Game.DefendingUnit.HasType(TypeEnum.Armor));
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 20));
        }
    }

    /// <summary>
    /// スキル2
    /// 『天空の運び手』【起】[横置]他の味方を１体選び、移動させる。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : WingedDeliverer
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "天空的运送者";
            Description = "『天空的运送者』【起】[横置]选择1名其他我方单位，将其移动。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }
    }
}
