using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-026 辺境の聖女 レナ
/// </summary>
public class Card00020 : Card
{
    public Card00020(User controller) : base(controller)
    {
        Serial = "00020";
        Pack = "S01";
        CardNum = "B01-026";
        Title = "边境的圣女";
        UnitName = "蕾娜";
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
    /// 『ライブ』【起】[横置，翻面2]自分の退避エリアから『レナ』以外のカードを１枚選び、手札に加える。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : Heal
    {
        public Sk1()
        {
            Number = 1;
            Name = "回复之杖";
            Description = "『回复之杖』【起】[横置，翻面2]从自己的退避区中选择1张「蕾娜」以外的卡，将其加入手牌。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
            ExceptName = Strings.Get("card_text_unitname_レナ");
        }
    }

    /// <summary>
    /// スキル2
    /// 『ジュリアンとの絆』【常】味方の『ジュリアン』の戦闘力は＋１０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2()
        {
            Number = 2;
            Name = "与朱利安的羁绊";
            Description = "『与朱利安的羁绊』【常】我方的「朱利安」的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card.Controller == Controller
                && card.IsOnField
                && card.HasUnitNameOf(Strings.Get("card_text_unitname_ジュリアン"));
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
        public Sk3()
        {
            Number = 3;
            Name = "祈祷之纹章";
            Description = "〖防御型〗『祈祷之纹章』【支】直到战斗结束为止，对手的攻击单位不能进行必杀攻击。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
