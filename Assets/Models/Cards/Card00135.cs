using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-089 飛翔せし漆黒の翼 ジェローム
/// </summary>
public class Card00135 : Card
{
    public Card00135(User controller) : base(controller)
    {
        Serial = "00135";
        Pack = "B01";
        CardNum = "B01-089";
        Title = "飞翔的漆黑之翼";
        UnitName = "杰罗姆";
        power = 60;
        support = 30;
        deployCost = 4;
        classChangeCost = 3;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Axe);
        types.Add(TypeEnum.Flight);
        types.Add(TypeEnum.Dragon);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『剣殺し』【常】このユニットが<剣>と戦闘している場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "弑剑";
            Description = "『弑剑』【常】这名单位与<剑>武器单位战斗的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && ((Game.AttackingUnit == card && Game.DefendingUnit.HasWeapon(WeaponEnum.Sword)) || Game.AttackingUnit.HasWeapon(WeaponEnum.Sword) && Game.DefendingUnit == card);
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 20));
        }
    }

    /// <summary>
    /// スキル2
    /// 『ショートアクス』〖转职技〗【起】〖1回合1次〗[翻面1]ターン終了まで、このユニットの戦闘力は＋１０され、このユニットに射程１-２が追加される。（はこのユニットがクラスチェンジしていなければ使用できない）
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ActionSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "短斧";
            Description = "〖转职技〗『短斧』【起】〖1回合1次〗[翻面1]直到回合结束为止，这名单位的战斗力+10，这名单位追加射程1-2。（〖转职技〗仅限这名单位经过转职后才能使用）";
            OncePerTurn = true;
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.CCS;
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
            Controller.AttachItem(new PowerBuff(this, 10, LastingTypeEnum.UntilTurnEnds), Owner);
            Controller.AttachItem(new RangeBuff(this, true, RangeEnum.OnetoTwo, LastingTypeEnum.UntilTurnEnds), Owner);
            return Task.CompletedTask;
        }
    }
}
