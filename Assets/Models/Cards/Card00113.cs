using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-067 お人好しな碧騎士 ソール
/// </summary>
public class Card00113 : Card00041
{
    public Card00113(User controller) : base(controller)
    {
        Serial = "00113";
        Pack = "B01";
        CardNum = "B01-067";
        Title = "忠厚的碧骑士";
        UnitName = "索尔";
        power = 40;
        support = 10;
        deployCost = 1;
        classChangeCost = 0;
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
