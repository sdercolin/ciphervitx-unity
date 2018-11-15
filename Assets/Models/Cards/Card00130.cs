using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-084 ロザンヌの守護者 セルジュ
/// </summary>
public class Card00130 : Card
{
    public Card00130(User controller) : base(controller)
    {
        Serial = "00130";
        Pack = "B01";
        CardNum = "B01-084";
        Title = "罗赞努的守护者";
        UnitName = "塞尔吉";
        power = 50;
        support = 30;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Axe);
        types.Add(TypeEnum.Flight);
        types.Add(TypeEnum.Dragon);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『飛竜の叫び』〖转职技〗【常】他のすべての<飛行>の味方の戦闘力は＋１０される。（はこのユニットがクラスチェンジしていなければ有効にならない）
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "飞龙之呐喊";
            Description = "〖转职技〗『飞龙之呐喊』【常】其他所有<飞行>属性的我方单位的战斗力+10。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.CCS;
        }

        public override bool CanTarget(Card card)
        {
            return card != Owner
                && card.Controller == Controller
                && card.IsOnField
                && card.HasType(TypeEnum.Flight);
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10));
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
