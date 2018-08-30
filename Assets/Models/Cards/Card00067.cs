using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-021 タリスの義勇兵 バーツ
/// </summary>
public class Card00067 : Card00018
{
    public Card00067(User controller) : base(controller)
    {
        Serial = "00067";
        Pack = "B01";
        CardNum = "B01-021";
        Title = "塔利斯的义勇兵";
        UnitName = "巴兹";
        power = 40;
        support = 10;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Axe);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
