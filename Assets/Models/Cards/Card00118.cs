using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-072 罵詈雑言の令嬢 マリアベル
/// </summary>
public class Card00118 : Card00045
{
    public Card00118(User controller) : base(controller)
    {
        Serial = "00118";
        Pack = "B01";
        CardNum = "B01-072";
        Title = "出言不逊的大小姐";
        UnitName = "玛丽亚贝尔";
        power = 20;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Staff);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.None);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
        sk3 = new Sk3();
        Attach(sk3);
    }
}
