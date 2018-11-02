using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-092 伝説の継承者 ウード
/// </summary>
public class Card00138 : Card
{
    public Card00138(User controller) : base(controller)
    {
        Serial = "00138";
        Pack = "B01";
        CardNum = "B01-092";
        Title = "传说的继承者";
        UnitName = "伍德";
        power = 50;
        support = 10;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『蒼炎剣 ブルーフレイムソード』【起】〖1回合1次〗[翻面1]ターン終了まで、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ReverseBondToAdd10
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "苍炎剑·Blue Flame Sword";
            Description = "『苍炎剑·Blue Flame Sword』【起】〖1回合1次〗[翻面1]直到回合结束为止，这名单位的战斗力+10。";
            OncePerTurn = true;
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }
    }

    /// <summary>
    /// スキル2
    /// 『受け継がれし聖痕』【常】このユニットが『リズ』に支援されている場合、このユニットが攻撃で破壊するオーブは２つになる。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "继承的圣痕";
            Description = "『继承的圣痕』【常】这名单位被「莉兹」支援的期间，这名单位的攻击所将破坏的宝玉变为2颗。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.BattlingUnits.Contains(card)
                && card.Controller.Support.SupportedBy(Strings.Get("card_text_unitname_リズ"));
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new DestroyTwoOrbs(this));
        }
    }
}
