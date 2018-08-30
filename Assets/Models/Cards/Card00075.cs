using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-029 風の魔道士 マリク
/// </summary>
public class Card00075 : Card00022
{
    public Card00075(User controller) : base(controller)
    {
        Serial = "00075";
        Pack = "B01";
        CardNum = "B01-029";
        Title = "风之魔法师";
        UnitName = "玛利克";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
