using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-003 アリティアの王子 マルス
/// </summary>
public class Card00149 : Card00006
{
    public Card00149(User controller) : base(controller)
    {
        Serial = "00149";
        Pack = "P01";
        CardNum = "P01-003";
        Title = "阿利缇亚的王子";
        UnitName = "马尔斯";
        power = 40;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
