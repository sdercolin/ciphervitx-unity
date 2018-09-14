using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-076 若き天才騎士 ティアモ
/// </summary>
public class Card00122 : Card00046
{
    public Card00122(User controller) : base(controller)
    {
        Serial = "00122";
        Pack = "B01";
        CardNum = "B01-076";
        Title = "年轻的天才骑士";
        UnitName = "缇雅莫";
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
