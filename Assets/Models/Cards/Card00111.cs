using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-065 勇敢な紅騎士 ソワレ
/// </summary>
public class Card00111 : Card00039
{
    public Card00111(User controller) : base(controller)
    {
        Serial = "00111";
        Pack = "B01";
        CardNum = "B01-065";
        Title = "勇敢的红骑士";
        UnitName = "索瓦蕾";
        power = 40;
        support = 10;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
