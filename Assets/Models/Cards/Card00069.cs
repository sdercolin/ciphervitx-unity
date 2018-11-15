using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-023 凄腕の剣客 ナバール
/// </summary>
public class Card00069 : Card
{
    public Card00069(User controller) : base(controller)
    {
        Serial = "00069";
        Pack = "B01";
        CardNum = "B01-023";
        Title = "卓越的剑客";
        UnitName = "那巴尔";
        power = 70;
        support = 10;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『宿命の好敵手』【常】『シーダ』か『オグマ』以外のカードはこのユニットの支援に失敗する。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "宿命的好敌手";
            Description = "『宿命的好敌手』【常】「希达」和「奥古玛」以外的卡对这名单位的支援将会失败。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return !(card.HasUnitNameOf(Strings.Get("card_text_unitname_シーダ")) || card.HasUnitNameOf(Strings.Get("card_text_unitname_オグマ")))
                && Game.BattlingUnits.Contains(Owner)
                && card.BelongedRegion == Controller.Support;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new FailToSupport(this));
        }
    }
}
