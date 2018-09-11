using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-064 紅瞳の猛牛 ソワレ
/// </summary>
public class Card00110 : Card00038
{
    public Card00110(User controller) : base(controller)
    {
        Serial = "00110";
        Pack = "B01";
        CardNum = "B01-064";
        Title = "红瞳的猛牛";
        UnitName = "索瓦蕾";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
