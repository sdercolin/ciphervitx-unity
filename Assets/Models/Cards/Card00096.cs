using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-050 ブラックナイツ カミュ
/// </summary>
public class Card00096 : Card
{
    public Card00096(User controller) : base(controller)
    {
        Serial = "00096";
        Pack = "B01";
        CardNum = "B01-050";
        Title = "黑骑士";
        UnitName = "卡缪";
        power = 70;
        support = 10;
        deployCost = 5;
        classChangeCost = 4;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『神槍 グラディウス』【起】[翻面4]出撃コストが２以下のすべての敵を撃破する。ターン終了まで、このスキルで撃破した敵１体につき、このユニットの戦闘力は＋１０され、このユニットに射程１-２が追加される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "神枪 古拉迪乌斯";
            Description = "『神枪 古拉迪乌斯』【起】[翻面4]将出击费用在2以下的所有敌方单位击破。直到回合结束为止，由于这个能力被击破的敌方单位每有1名，这名单位的战斗力+10，这名单位追加射程1-2。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 4);
        }

        public override Task Do()
        {
            var targets = Opponent.Field.Filter(unit => unit.DeployCost <= 2);
            Controller.Destroy(targets, this, false);
            Controller.AttachItem(new PowerBuff(this, 10 * targets.FindAll(unit => unit.DestroyedCount > 0).Count, LastingTypeEnum.UntilTurnEnds), Owner);
            Controller.AttachItem(new RangeBuff(this, true, RangeEnum.OnetoTwo, LastingTypeEnum.UntilTurnEnds), Owner);
            return Task.CompletedTask;
        }
    }
}
