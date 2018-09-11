using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-063 貴族的な弓使い ヴィオール
/// </summary>
public class Card00109 : Card00037
{
    public Card00109(User controller) : base(controller)
    {
        Serial = "00109";
        Pack = "B01";
        CardNum = "B01-063";
        Title = "贵族弓箭手";
        UnitName = "维奥尔";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Bow);
        ranges.Add(RangeEnum.Two);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
        sk3 = new Sk3();
        Attach(sk3);
    }
}
