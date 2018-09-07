using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-008 マケドニアの王女 ミネルバ
/// </summary>
public class Card00154 : Card00077
{
    public Card00154(User controller) : base(controller)
    {
        Serial = "00154";
        Pack = "P01";
        CardNum = "P01-008";
        Title = "马凯多尼亚的王女";
        UnitName = "密涅瓦";
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
