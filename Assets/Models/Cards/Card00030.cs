using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-055 聖王の血を引く者 ルキナ
/// </summary>
public class Card00030 : Card
{
    public Card00030(User controller) : base(controller)
    {
        Serial = "00030";
        Pack = "S02";
        CardNum = "B01-055";
        Title = "圣王之血的继承者";
        UnitName = "露琪娜";
        power = 50;
        support = 20;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『聖なる血脈』【起】〖1回合1次〗[翻面1]ターン終了まで、このユニットと味方の『クロム』の戦闘力は＋１０される。このスキルは味方に『クロム』がいなければ使用できない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "神圣的血脉";
            Description = "『神圣的血脉』【起】〖1回合1次〗[翻面1]直到回合结束为止，这名单位和我方的「库洛姆」的战斗力+10。这个能力仅限我方战场上存在「库洛姆」时才能使用。";
            OncePerTurn = true;
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return Controller.Field.SearchCard("库洛姆").Count > 0;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 1);
        }

        public override Task Do()
        {
            Owner.Attach(new PowerBuff(this, 10, LastingTypeEnum.UntilTurnEnds));
            var targets = Owner.Controller.Field.SearchCard("库洛姆");
            targets.ForEach(unit =>
            {
                unit.Attach(new PowerBuff(this, 10, LastingTypeEnum.UntilTurnEnds));
            });              
            return Task.CompletedTask;
        }
    }
}