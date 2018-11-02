using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B02) B02-003 白夜王国の王子 カムイ（男）
/// </summary>
public class Card00162 : Card00147
{
    public Card00162(User controller) : base(controller)
    {
        Serial = "00162";
        Pack = "B02";
        CardNum = "B02-003";
        Title = "白夜王国的王子";
        UnitName = "神威（男）";
        power = 40;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.White);
        genders.Add(GenderEnum.Male);
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
