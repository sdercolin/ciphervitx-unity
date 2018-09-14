using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-072 罵詈雑言の令嬢 マリアベル
/// </summary>
public class Card00045 : Card
{
    public Card00045(User controller) : base(controller)
    {
        Serial = "00045";
        Pack = "S02";
        CardNum = "B01-072";
        Title = "出言不逊的大小姐";
        UnitName = "玛丽亚贝尔";
        power = 20;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Staff);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.None);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
        sk3 = new Sk3();
        Attach(sk3);
    }

    /// <summary>
    /// スキル1
    /// 『ライブ』【起】[横置，翻面2]自分の退避エリアから『マリアベル』以外のカードを１枚選び、手札に加える。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : Heal
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "回复之杖";
            Description = "『回复之杖』【起】[横置，翻面2]从自己的退避区中选择1张「玛丽亚贝尔」以外的卡，将其加入手牌。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
            ExceptName = "玛丽亚贝尔";
        }
    }

    /// <summary>
    /// スキル2
    /// 『令嬢の嗜み』【常】自分のターン中、味方の『リズ』の戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "大小姐的用心";
            Description = "『大小姐的用心』【常】自己的回合中，我方的「莉兹」的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return Game.TurnPlayer == Controller
                && card.Controller == Controller
                && card.IsOnField
                && card.HasUnitNameOf("莉兹");
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 20));
        }
    }

    /// <summary>
    /// スキル3
    /// 〖防御型〗『祈りの紋章』【支】戦闘終了まで、相手の攻撃ユニットは必殺攻撃できない。
    /// </summary>
    public Sk3 sk3;
    public class Sk3 : MiracleEmblem
    {
        public Sk3() : base()
        {
            Number = 3;
            Name = "祈祷之纹章";
            Description = "〖防御型〗『祈祷之纹章』【支】直到战斗结束为止，对手的攻击单位不能进行必杀攻击。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
