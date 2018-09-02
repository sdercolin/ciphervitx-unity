using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-060 飛び跳ねシスター リズ
/// </summary>
public class Card00106 : Card00034
{
    public Card00106(User controller) : base(controller)
    {
        Serial = "00106";
        Pack = "B01";
        CardNum = "B01-060";
        Title = "活蹦乱跳的修女";
        UnitName = "莉兹";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Staff);
        ranges.Add(RangeEnum.None);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
