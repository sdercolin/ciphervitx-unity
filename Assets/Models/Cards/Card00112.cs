using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-066 碧眼の黒豹 ソール
/// </summary>
public class Card00112 : Card00040
{
    public Card00112(User controller) : base(controller)
    {
        Serial = "00112";
        Pack = "B01";
        CardNum = "B01-066";
        Title = "碧眼的黑豹";
        UnitName = "索尔";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
