using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-006 タリスの王女 シーダ
/// </summary>
public class Card00052 : Card00007
{
    public Card00052(User controller) : base(controller)
    {
        Serial = "00052";
        Pack = "B01";
        CardNum = "B01-006";
        Title = "塔利斯的王女";
        UnitName = "希达";
        power = 30;
        support = 30;
        deployCost = 1;
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
}
