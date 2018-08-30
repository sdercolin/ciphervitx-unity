using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-018 タリスの傭兵 オグマ
/// </summary>
public class Card00064 : Card00015
{
    public Card00064(User controller) : base(controller)
    {
        Serial = "00064";
        Pack = "B01";
        CardNum = "B01-018";
        Title = "塔利斯的佣兵";
        UnitName = "奥古玛";
        power = 40;
        support = 10;
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
