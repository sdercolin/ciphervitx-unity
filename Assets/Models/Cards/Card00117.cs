using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-071 女嫌いの剣士 ロンクー
/// </summary>
public class Card00117 : Card00044
{
    public Card00117(User controller) : base(controller)
    {
        Serial = "00117";
        Pack = "B01";
        CardNum = "B01-071";
        Title = "讨厌女人的剑士";
        UnitName = "隆库";
        power = 50;
        support = 10;
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
