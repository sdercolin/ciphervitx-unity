using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-051 新たなる聖王 クロム
/// </summary>
public class Card00097 : Card
{
    public Card00097(User controller) : base(controller)
    {
        Serial = "00097";
        Pack = "B01";
        CardNum = "B01-051";
        Title = "新的圣王";
        UnitName = "库洛姆";
        power = 70;
        support = 20;
        deployCost = 5;
        classChangeCost = 4;
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
    /// 『聖王の威光』【起】[翻面3，自分の手札から『クロム』を１枚退避エリアに置く]敵を好きな数だけ選び、移動させる。ターン終了まで、すべての味方の戦闘力は＋３０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "圣王的威光";
            Description = "『圣王的威光』【起】[翻面3，从自己的手牌将1张「库洛姆」放置到退避区]选择任意名敌方单位，将其移动。直到回合结束为止，所有我方单位的战斗力+30。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 3) + Cost.DiscardHand(this, 1, card => card.HasUnitNameOf("库洛姆"));
        }

        public override async Task Do()
        {
            var choices = Opponent.Field.Cards;
            await Controller.ChooseMove(choices, 0, choices.Count, this);
            Controller.Field.ForEachCard(unit =>
            {
                Controller.AttachItem(new PowerBuff(this, 30, LastingTypeEnum.UntilTurnEnds), unit);
            });
        }
    }

    /// <summary>
    /// スキル2
    /// 『神剣 ファルシオン』〖转职技〗【常】このユニットが<竜>を攻撃している場合、このユニットの戦闘力は＋４０される。（はこのユニットがクラスチェンジしていなければ有効にならない）
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "神剑 法尔西昂";
            Description = "〖转职技〗『神剑 法尔西昂』【常】这名单位攻击<龙>属性单位的期间，这名单位的战斗力+40。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.CCS;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.AttackingUnit == card
                && Game.DefendingUnit.HasType(TypeEnum.Dragon);
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 40));
        }
    }
}
