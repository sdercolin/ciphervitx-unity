using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-003 アリティアの王子 マルス
/// </summary>
public class Card00049 : Card00006
{
    public Card00049(User controller) : base(controller)
    {
        Serial = "00049";
        Pack = "B01";
        CardNum = "B01-003";
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
