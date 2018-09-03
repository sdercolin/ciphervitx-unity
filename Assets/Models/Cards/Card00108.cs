using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-062 ロザンヌの侯爵 ヴィオール
/// </summary>
public class Card00108 : Card00036
{
    public Card00108(User controller) : base(controller)
    {
        Serial = "00108";
        Pack = "B01";
        CardNum = "B01-062";
        Title = "罗赞努的侯爵";
        UnitName = "维奥尔";
        power = 50;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Bow);
        ranges.Add(RangeEnum.Two);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
