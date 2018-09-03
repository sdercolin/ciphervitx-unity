using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-070 孤高の刃 ロンクー
/// </summary>
public class Card00116 : Card00043
{
    public Card00116(User controller) : base(controller)
    {
        Serial = "00116";
        Pack = "B01";
        CardNum = "B01-070";
        Title = "孤高之刃";
        UnitName = "隆库";
        power = 50;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
