using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B02) B02-053 暗夜王国の王女 カムイ（女）
/// </summary>
public class Card00163 : Card00148
{
    public Card00163(User controller) : base(controller)
    {
        Serial = "00163";
        Pack = "B02";
        CardNum = "B02-053";
        Title = "暗夜王国的王女";
        UnitName = "神威（女）";
        power = 40;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Black);
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
