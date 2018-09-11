using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-033 マケドニアの妹姫 マリア
/// </summary>
public class Card00079 : Card
{
    public Card00079(User controller) : base(controller)
    {
        Serial = "00079";
        Pack = "B01";
        CardNum = "B01-033";
        Title = "马凯多尼亚的小公主";
        UnitName = "玛莉娅";
        power = 20;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Staff);
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
    /// 『ライブ』【起】[横置，翻面2]自分の退避エリアから『マリア』以外のカードを１枚選び、手札に加える。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : Heal
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "回复之杖";
            Description = "『回复之杖』【起】[横置，翻面2]从自己的退避区中选择1张「玛莉娅」以外的卡，将其加入手牌。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
            ExceptName = "玛莉娅";
        }
    }

    /// <summary>
    /// スキル2
    /// 『マリアの想い』【常】味方の『ミネルバ』と『ミシェイル』の戦闘力は＋１０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "玛利娅的心绪";
            Description = "『玛利娅的心绪』【常】我方的「密涅瓦」和「米歇尔」的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card.Controller == Controller
                && card.IsOnField
                && (card.HasUnitNameOf("密涅瓦") || card.HasUnitNameOf("米歇尔"));
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10));
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
