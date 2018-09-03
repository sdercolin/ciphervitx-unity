using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-058 記憶を失くした軍師 ルフレ（女）
/// </summary>
public class Card00104 : Card00032
{
    public Card00104(User controller) : base(controller)
    {
        Serial = "00104";
        Pack = "B01";
        CardNum = "B01-058";
        Title = "失去记忆的军师";
        UnitName = "路弗雷（女）";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
