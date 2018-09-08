using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-011 追憶の神竜族 チキ
/// </summary>
public class Card00157 : Card00145
{
    public Card00157(User controller) : base(controller)
    {
        Serial = "00157";
        Pack = "P01";
        CardNum = "P01-011";
        Title = "追忆的神龙族";
        UnitName = "芝琪";
        power = 30;
        support = 20;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.DragonStone);
        types.Add(TypeEnum.Dragon);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
