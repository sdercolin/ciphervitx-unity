using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-008 赤の騎士 カイン
/// </summary>
public class Card00009 : Card
{
    public Card00009(User controller) : base(controller)
    {
        Serial = "00009";
        Pack = "S01";
        CardNum = "B01-008";
        Title = "赤色骑士";
        UnitName = "卡因";
        power = 40;
        support = 10;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『赤と緑の絆』【常】このユニットが『アベル』に支援されている場合、このユニットの戦闘力は＋３０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "赤与绿的羁绊";
            Description = "『赤与绿的羁绊』【常】这名单位被「阿贝尔」支援的期间，这名单位的战斗力+30。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.BattlingUnits.Contains(card)
                && card.Controller.Support.SupportCard.HasUnitNameOf("阿贝尔");
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 30));
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『攻撃の紋章』【支】戦闘終了まで、自分の攻撃ユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : AttackEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "攻击之纹章";
            Description = "〖攻击型〗『攻击之纹章』【支】直到战斗结束为止，我方的攻击单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
