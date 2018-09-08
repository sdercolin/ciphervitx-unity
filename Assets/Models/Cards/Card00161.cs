using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-014 聖王の血を引く者 ルキナ
/// </summary>
public class Card00161 : Card00030
{
    public Card00161(User controller) : base(controller)
    {
        Serial = "00161";
        Pack = "P01";
        CardNum = "P01-014";
        Title = "圣王之血的继承者";
        UnitName = "露琪娜";
        power = 50;
        support = 20;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }
}
