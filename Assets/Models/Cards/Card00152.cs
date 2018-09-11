using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-006 忠義の古強者 ジェイガン
/// </summary>
public class Card00152 : Card00003
{
    public Card00152(User controller) : base(controller)
    {
        Serial = "00152";
        Pack = "P01";
        CardNum = "P01-006";
        Title = "忠义的老战士";
        UnitName = "杰刚";
        power = 70;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }
}
