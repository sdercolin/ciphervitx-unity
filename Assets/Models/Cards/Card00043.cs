using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-070 孤高の刃 ロンクー
/// </summary>
public class Card00043 : Card
{
    public Card00043(User controller) : base(controller)
    {
        Serial = "00043";
        Pack = "S02";
        CardNum = "B01-070";
        Title = "孤高之刃";
        UnitName = "隆库";
        power = 50;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『剣の達人』〖转职技〗【常】他の<剣>の味方１体につき、このユニットの戦闘力は＋１０される。（はこのユニットがクラスチェンジしていなければ有効にならない）
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "剑之达人";
            Description = "〖转职技〗『剑之达人』【常】我方战场上每有1名其他<剑>武器的单位，这名单位的战斗力+10。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.CCS;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner && card.IsClassChanged;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10 * Controller.Field.Filter(unit => unit.HasWeapon(WeaponEnum.Sword) && unit != Owner).Count));
        }
    }

    /// <summary>
    /// スキル2
    /// 『キルソード』【起】[翻面3]ターン終了まで、このユニットの攻撃は神速回避されない。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ActionSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "必杀刃";
            Description = "『必杀刃』【起】[翻面3]直到回合结束为止，这名单位的攻击不能被神速回避。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 3);
        }

        public override Task Do()
        {
            Owner.Attach(new CanNotBeAvoided(this, LastingTypeEnum.UntilTurnEnds));
            return Task.CompletedTask;
        }
    }
}
