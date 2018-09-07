using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-004 マルスを名乗る剣士 ルキナ
/// </summary>
public class Card00150 : Card00102
{
    public Card00150(User controller) : base(controller)
    {
        Serial = "00150";
        Pack = "P01";
        CardNum = "P01-004";
        Title = "自称马尔斯的剑士";
        UnitName = "露琪娜";
        power = 40;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
        sk3 = new Sk3();
        Attach(sk3);
    }
}
