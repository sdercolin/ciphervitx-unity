using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-071 女嫌いの剣士 ロンクー
/// </summary>
public class Card00044 : Card
{
    public Card00044(User controller) : base(controller)
    {
        Serial = "00044";
        Pack = "S02";
        CardNum = "B01-071";
        Title = "讨厌女人的剑士";
        UnitName = "隆库";
        power = 50;
        support = 10;
        deployCost = 1;
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
    /// 『女は近づくな』【常】<女>のカードはこのユニットの支援に失敗する。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "女人不要靠近我";
            Description = "『女人不要靠近我』【常】<女性>卡片对这名单位的支援会失败。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card.HasGender(GenderEnum.Female)
                && Game.BattlingUnits.Contains(Owner)
                && card.BelongedRegion == Controller.Support;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new FailToSupport(this));
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『攻撃の紋章』【支】戦闘終了まで、自分の攻撃ユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : AttackEmblem
    {
        public Sk2()
        {
            Number = 2;
            Name = "攻击之纹章";
            Description = "〖攻击型〗『攻击之纹章』【支】直到战斗结束为止，我方的攻击单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
