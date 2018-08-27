using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-008 赤の騎士 カイン
/// </summary>
public class Card00054 : Card00009
{
    public Card00054(User controller) : base(controller)
    {
        Serial = "00054";
        Pack = "B01";
        CardNum = "B01-008";
        Title = "赤色骑士";
        UnitName = "卡因";
        power = 40;
        support = 10;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
