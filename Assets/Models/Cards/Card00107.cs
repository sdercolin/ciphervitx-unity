using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-061 穏和な騎士団長 フレデリク
/// </summary>
public class Card00107 : Card00035
{
    public Card00107(User controller) : base(controller)
    {
        Serial = "00107";
        Pack = "B01";
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
}
