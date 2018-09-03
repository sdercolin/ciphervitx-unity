using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-059 イーリス聖王国の王女 リズ
/// </summary>
public class Card00105 : Card00033
{
    public Card00105(User controller) : base(controller)
    {
        Serial = "00105";
        Pack = "B01";
        CardNum = "B01-059";
        Title = "伊利斯圣王国的王女";
        UnitName = "莉兹";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Axe);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }
}
