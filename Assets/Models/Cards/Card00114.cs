using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-068 心優しき自警団の花 スミア
/// </summary>
public class Card00114 : Card
{
    public Card00114(User controller) : base(controller)
    {
        Serial = "00114";
        Pack = "B01";
        CardNum = "B01-068";
        Title = "温柔的警卫队之花";
        UnitName = "丝米娅";
        power = 50;
        support = 30;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
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
    /// 『天馬の叫び』〖转职技〗【常】他の味方を支援しているカードが<飛行>の場合、そのカードの支援力は＋１０される。（はこのユニットがクラスチェンジしていなければ有効にならない）
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "天马之呐喊";
            Description = "〖转职技〗『天马之呐喊』【常】支援其他我方单位的卡是<飞行>属性的场合，那张卡的支援力+10。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.CCS;
        }

        public override bool CanTarget(Card card)
        {
            return card.HasType(TypeEnum.Flight)
                && !Game.BattlingUnits.Contains(Owner)
                && card.BelongedRegion == Controller.Support;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new SupportBuff(this, 10));
        }
    }

    /// <summary>
    /// スキル2
    /// 『天空を翔ける者』【起】〖1回合1次〗このユニットを移動させる。このスキルはこのユニットが未行動でなければ使用できない。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : AngelicFlight
    {
        public Sk2()
        {
            Number = 2;
            Name = "翱翔天空者";
            Description = "『翱翔天空者』【起】〖1回合1次〗将这名单位移动。这个能力只能在这名单位处于未行动状态时使用。";
            OncePerTurn = true;
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
            OncePerTurn = true;
        }
    }
}
