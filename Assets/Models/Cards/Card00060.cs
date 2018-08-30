using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-014 解放軍の弓兵 ゴードン
/// </summary>
public class Card00060 : Card00014
{
    public Card00060(User controller) : base(controller)
    {
        Serial = "00060";
        Pack = "B01";
        CardNum = "B01-014";
        Title = "解放军的弓兵";
        UnitName = "哥顿";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
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
