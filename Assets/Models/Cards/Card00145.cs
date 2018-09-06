using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-099 追憶の神竜族 チキ
/// </summary>
public class Card00145 : Card
{
    public Card00145(User controller) : base(controller)
    {
        Serial = "00145";
        Pack = "B01";
        CardNum = "B01-099";
        Title = "追忆的神龙族";
        UnitName = "芝琪";
        power = 30;
        support = 20;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.DragonStone);
        types.Add(TypeEnum.Dragon);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『神竜の巫女』【起】〖1回合1次〗[翻面2]自分の手札を１枚選び、絆エリアに置く。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "神龙的巫女";
            Description = "『神龙的巫女』【起】〖1回合1次〗[翻面2]选择自己的1张手牌，放置到羁绊区。";
            OncePerTurn = true;
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 2);
        }

        public override async Task Do()
        {
            await Controller.ChooseSetToBond(Controller.Hand.Cards, 1, 1, true, this);
        }
    }

    /// <summary>
    /// スキル2
    /// 『長寿な竜一族』【常】自分の絆カードが５枚以上の場合、このユニットの戦闘力は＋３０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "长寿的龙一族";
            Description = "『长寿的龙一族』【常】自己的羁绊卡有5张以上的场合，这名单位的战斗力+30。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner && Controller.Bond.Count >= 5;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 30));
        }
    }
}
