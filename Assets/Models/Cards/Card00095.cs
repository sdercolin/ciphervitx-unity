using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-049 異邦の剣士 アテナ
/// </summary>
public class Card00095 : Card
{
    public Card00095(User controller) : base(controller)
    {
        Serial = "00095";
        Pack = "B01";
        CardNum = "B01-049";
        Title = "异邦的剑士";
        UnitName = "雅典娜";
        power = 60;
        support = 10;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『少女剣士の恩返し』【特】このカードは自分の退避エリアにカードが５枚以上なければ出撃できない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "少女剑士的报恩";
            Description = "『少女剑士的报恩』【特】这张卡仅限自己的退避区中的卡有5张以上时才能出击。";
            TypeSymbols.Add(SkillTypeSymbol.Special);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner && Controller.Retreat.Count < 5;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new CanNotDeploy(this));
        }
    }

    /// <summary>
    /// スキル2
    /// 『サンダーソード』【起】[翻面1]ターン終了まで、このユニットの戦闘力は-１０され、このユニットに<魔法>と射程１-２が追加される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ActionSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "雷之剑";
            Description = "『雷之剑』【起】[翻面1]直到回合结束为止，这名单位的战斗力-10，这名单位追加<魔法>武器与射程1-2。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 1);
        }

        public override Task Do()
        {
            Controller.AttachItem(new PowerBuff(this, -10, LastingTypeEnum.UntilTurnEnds), Owner);
            Controller.AttachItem(new WeaponBuff(this, true, WeaponEnum.Magic, LastingTypeEnum.UntilTurnEnds), Owner);
            Controller.AttachItem(new RangeBuff(this, true, RangeEnum.OnetoTwo, LastingTypeEnum.UntilTurnEnds), Owner);
            return Task.CompletedTask;
        }
    }
}
