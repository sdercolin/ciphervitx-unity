using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-026 辺境の聖女 レナ
/// </summary>
public class Card00072 : Card00020
{
    public Card00072(User controller) : base(controller)
    {
        Serial = "00072";
        Pack = "B01";
        CardNum = "B01-026";
        Title = "边境的圣女";
        UnitName = "蕾娜";
        power = 20;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Staff);
        ranges.Add(RangeEnum.None);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
        sk3 = new Sk3();
        Attach(sk3);
    }
}
