using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-013 同盟軍の弓騎士 ゴードン
/// </summary>
public class Card00059 : Card00013
{
    public Card00059(User controller) : base(controller)
    {
        Serial = "00059";
        Pack = "B01";
        CardNum = "B01-013";
        Title = "同盟军的弓骑士";
        UnitName = "哥顿";
        power = 50;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Bow);
        ranges.Add(RangeEnum.Two);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
