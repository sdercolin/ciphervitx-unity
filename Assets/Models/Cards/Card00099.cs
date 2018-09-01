using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-053 イーリス聖王国の王子 クロム
/// </summary>
public class Card00099 : Card00029
{
    public Card00099(User controller) : base(controller)
    {
        Serial = "00099";
        Pack = "B01";
        CardNum = "B01-053";
        Title = "伊利斯圣王国的王子";
        UnitName = "库洛姆";
        power = 40;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
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
