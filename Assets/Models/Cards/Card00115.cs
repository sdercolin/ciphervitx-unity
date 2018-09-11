using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-069 花占いの乙女 スミア
/// </summary>
public class Card00115 : Card00042
{
    public Card00115(User controller) : base(controller)
    {
        Serial = "00115";
        Pack = "B01";
        CardNum = "B01-069";
        Title = "花占卜少女";
        UnitName = "丝米娅";
        power = 30;
        support = 30;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
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
