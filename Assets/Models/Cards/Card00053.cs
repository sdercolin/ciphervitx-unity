using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-007 猛牛と呼ばれし騎士 カイン
/// </summary>
public class Card00053 : Card00008
{
    public Card00053(User controller) : base(controller)
    {
        Serial = "00053";
        Pack = "B01";
        CardNum = "B01-007";
        Title = "人称猛牛的骑士";
        UnitName = "卡因";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
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
