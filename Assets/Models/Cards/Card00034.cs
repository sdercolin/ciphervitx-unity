using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-060 飛び跳ねシスター リズ
/// </summary>
public class Card00034 : Card
{
    public Card00034(User controller) : base(controller)
    {
        Serial = "00034";
        Pack = "S02";
        CardNum = "B01-060";
        Title = "活蹦乱跳的修女";
        UnitName = "莉兹";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Staff);
        ranges.Add(RangeEnum.None);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『ライブ』【起】[横置，翻面2]自分の退避エリアから『リズ』以外のカードを１枚選び、手札に加える。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : Heal
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "回复之杖";
            Description = "『回复之杖』【起】[横置，翻面2]从自己的退避区中选择1张「莉兹」以外的卡，将其加入手牌。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
            ExceptName = "莉兹";
        }
    }

    /// <summary>
    /// スキル2
    /// 〖防御型〗『祈りの紋章』【支】戦闘終了まで、相手の攻撃ユニットは必殺攻撃できない。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : MiracleEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "祈祷之纹章";
            Description = "〖防御型〗『祈祷之纹章』【支】直到战斗结束为止，对手的攻击单位不能进行必杀攻击。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
