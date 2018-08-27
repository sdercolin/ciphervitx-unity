using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-012 護りの騎士 ドーガ
/// </summary>
public class Card00012 : Card
{
    public Card00012(User controller) : base(controller)
    {
        Serial = "00012";
        Pack = "S01";
        CardNum = "B01-012";
        Title = "护卫骑士";
        UnitName = "杜卡";
        power = 30;
        support = 10;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Armor);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『重装の心得』【常】このユニットが<魔法>以外に攻撃されている場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ArmorExpertise
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "重装的心得";
            Description = "『重装的心得』【常】这名单位被<魔法>以外的武器攻击的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }
    }

    /// <summary>
    /// スキル2
    /// 〖防御型〗『防御の紋章』【支】戦闘終了まで、自分の防御ユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : DefenceEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "防御之纹章";
            Description = "〖防御型〗『防御之纹章』【支】直到战斗结束为止，自己的防御单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
