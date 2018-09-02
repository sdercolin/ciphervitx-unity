using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-055 聖王の血を引く者 ルキナ
/// </summary>
public class Card00101 : Card00030
{
    public Card00101(User controller) : base(controller)
    {
        Serial = "00101";
        Pack = "B01";
        CardNum = "B01-055";
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
