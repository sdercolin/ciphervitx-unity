using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-028 疾風の賢者 マリク
/// </summary>
public class Card00074 : Card00021
{
    public Card00074(User controller) : base(controller)
    {
        Serial = "00074";
        Pack = "B01";
        CardNum = "B01-028";
        Title = "疾风之贤者";
        UnitName = "玛利克";
        power = 60;
        support = 20;
        deployCost = 4;
        classChangeCost = 3;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
