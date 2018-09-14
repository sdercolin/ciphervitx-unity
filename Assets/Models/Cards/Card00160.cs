using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-012 亡国の王子 マルス
/// </summary>
public class Card00160 : Card00048
{
    public Card00160(User controller) : base(controller)
    {
        Serial = "00160";
        Pack = "P01";
        CardNum = "P01-012";
        Title = "亡国的王子";
        UnitName = "马尔斯";
        power = 50;
        support = 20;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }
}
