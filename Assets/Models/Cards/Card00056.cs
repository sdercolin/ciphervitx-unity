using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-010 緑の騎士 アベル
/// </summary>
public class Card00056 : Card00011
{
    public Card00056(User controller) : base(controller)
    {
        Serial = "00056";
        Pack = "B01";
        CardNum = "B01-010";
        Title = "绿色骑士";
        UnitName = "阿贝尔";
        power = 40;
        support = 10;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
