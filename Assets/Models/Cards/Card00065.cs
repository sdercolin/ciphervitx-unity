using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-019 タリスの戦士 サジ
/// </summary>
public class Card00065 : Card00016
{
    public Card00065(User controller) : base(controller)
    {
        Serial = "00065";
        Pack = "B01";
        CardNum = "B01-019";
        Title = "塔利斯的战士";
        UnitName = "萨基";
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
