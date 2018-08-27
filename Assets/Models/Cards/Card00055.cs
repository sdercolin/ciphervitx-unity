using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-009 黒豹と呼ばれし騎士 アベル
/// </summary>
public class Card00055 : Card00010
{
    public Card00055(User controller) : base(controller)
    {
        Serial = "00055";
        Pack = "B01";
        CardNum = "B01-009";
        Title = "人称黑豹的骑士";
        UnitName = "阿贝尔";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
