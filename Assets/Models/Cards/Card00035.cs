using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-061 穏和な騎士団長 フレデリク
/// </summary>
public class Card00035 : Card
{
    public Card00035(User controller) : base(controller)
    {
        Serial = "00035";
        Pack = "S02";
        CardNum = "B01-061";
        Title = "温和的骑士团长";
        UnitName = "弗雷德里克";
        power = 70;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Axe);
        types.Add(TypeEnum.Armor);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『戦場の教育役』【特】このカードは絆エリアに置くことができない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "战场上的教育家";
            Description = "『战场上的教育家』【特】这张卡不能放置到羁绊区。";
            TypeSymbols.Add(SkillTypeSymbol.Special);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new CanNotBePlacedInBond(this));
        }
    }
}
