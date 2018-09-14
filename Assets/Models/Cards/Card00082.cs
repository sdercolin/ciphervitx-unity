using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-036 大司祭ミロアの娘 リンダ
/// </summary>
public class Card00082 : Card00023
{
    public Card00082(User controller) : base(controller)
    {
        Serial = "00082";
        Pack = "B01";
        CardNum = "B01-036";
        Title = "大司祭米罗亚的女儿";
        UnitName = "琳达";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
