using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-012 護りの騎士 ドーガ
/// </summary>
public class Card00058 : Card00012
{
    public Card00058(User controller) : base(controller)
    {
        Serial = "00058";
        Pack = "B01";
        CardNum = "B01-012";
        Title = "护卫骑士";
        UnitName = "杜卡";
        power = 30;
        support = 10;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Armor);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
