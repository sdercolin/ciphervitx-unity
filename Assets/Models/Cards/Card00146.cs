using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-100 秘密の行商人 アンナ
/// </summary>
public class Card00146 : Card
{
    public Card00146(User controller) : base(controller)
    {
        Serial = "00146";
        Pack = "B01";
        CardNum = "B01-100";
        Title = "秘密的旅行商人";
        UnitName = "安娜";
        power = 20;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
        sk3 = new Sk3();
        Attach(sk3);
        sk4 = new Sk4();
        Attach(sk4);
    }

    /// <summary>
    /// スキル1
    /// 『アンナ姉妹』【常】他の味方の『アンナ』１体につき、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "安娜姐妹";
            Description = "『安娜姐妹』【常】战场上每有1名其他我方的「安娜」，这名单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10 * Controller.Field.Filter(unit => unit.HasUnitNameOf("安娜") && unit != Owner).Count));
        }
    }

    /// <summary>
    /// スキル2
    /// 『１００人のアンナ』【特】このカードは味方に『アンナ』がいても出撃させることができ、『アンナ』が２体以上味方にいてもよい。【特】このカードと同じカード名のカードをデッキに５枚以上入れてもよい。【常】『アンナ』のカードはこのユニットの支援に成功する。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : Annas
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "100名安娜";
            Description = "『100名安娜』【特】即便我方战场上存在「安娜」，这名单位也可以出击，我方战场上可以存在2名以上的「安娜」。";
            TypeSymbols.Add(SkillTypeSymbol.Special);
            Keyword = SkillKeyword.Null;
        }
    }

    /// <summary>
    /// スキル3
    /// </summary>
    public Sk3 sk3;
    public class Sk3 : AllowOverFourInDeck
    {
        public Sk3() : base()
        {
            Number = 3;
            Name = "100名安娜";
            Description = "『100名安娜』【特】你可以在卡组中加入5张以上的与这张卡的卡名相同的卡。";
            TypeSymbols.Add(SkillTypeSymbol.Special);
            Keyword = SkillKeyword.Null;
        }
    }

    /// <summary>
    /// スキル4
    /// </summary>
    public Sk4 sk4;
    public class Sk4 : PermanentSkill
    {
        public Sk4() : base()
        {
            Number = 4;
            Name = "100名安娜";
            Description = "『100名安娜』【常】「安娜」卡对这名单位的支援将会成功。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new CanBeSupportedBy(this)
            {
                UnitName = "安娜";
            });
        }
    }
}