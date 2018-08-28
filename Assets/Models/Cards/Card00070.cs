using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-024 紅の剣士 ナバール
/// </summary>
public class Card00070 : Card00019
{
    public Card00070(User controller) : base(controller)
    {
        Serial = "00070";
        Pack = "B01";
        CardNum = "B01-024";
        Title = "红色剑士";
        UnitName = "那巴尔";
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
