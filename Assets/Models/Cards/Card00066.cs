using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-020 タリスの斧使い マジ
/// </summary>
public class Card00066 : Card00017
{
    public Card00066(User controller) : base(controller)
    {
        Serial = "00066";
        Pack = "B01";
        CardNum = "B01-020";
        Title = "塔利斯的斧手";
        UnitName = "玛基";
        power = 30;
        support = 10;
        deployCost = 1;
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
