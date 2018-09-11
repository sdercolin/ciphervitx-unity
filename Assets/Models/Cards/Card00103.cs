using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-057 聖王の神軍師 ルフレ（女）
/// </summary>
public class Card00103 : Card00031
{
    public Card00103(User controller) : base(controller)
    {
        Serial = "00103";
        Pack = "B01";
        CardNum = "B01-057";
        Title = "圣王的神军师";
        UnitName = "路弗雷（女）";
        power = 60;
        support = 20;
        deployCost = 4;
        classChangeCost = 3;
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
