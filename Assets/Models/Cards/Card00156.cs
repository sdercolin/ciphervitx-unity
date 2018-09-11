using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-010 物陰の闇使い サーリャ
/// </summary>
public class Card00156 : Card00128
{
    public Card00156(User controller) : base(controller)
    {
        Serial = "00156";
        Pack = "P01";
        CardNum = "P01-010";
        Title = "藏身黑暗的暗魔法师";
        UnitName = "萨莉雅";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }
}
