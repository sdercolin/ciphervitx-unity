using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-030 プリンセス ミネルバ
/// </summary>
public class Card00076 : Card
{
    public Card00076(User controller) : base(controller)
    {
        Serial = "00076";
        Pack = "B01";
        CardNum = "B01-030";
        Title = "公主";
        UnitName = "密涅瓦";
        power = 50;
        support = 30;
        deployCost = 4;
        classChangeCost = 3;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Female);
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
    /// 『飛竜の鞭』【常】他の<飛行>の味方１体につき、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "飞龙之鞭";
            Description = "『飞龙之鞭』【常】我方战场上每有1名其他<飞行>属性的单位，这名单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10 * Controller.Field.Filter(unit => unit.HasType(TypeEnum.Flight) && unit != Owner).Count));
        }
    }

    /// <summary>
    /// スキル2
    /// 『アイオテの盾』【常】すべての敵は『飛行特効』を失い、新たに得ることもできない。（【常】はこのカードがユニットとして戦場にいる間だけ有効になる）
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "艾奥提之盾";
            Description = "『艾奥提之盾』【常】所有敌方单位失去『飞行特效』，且不能重新获得。（【常】仅限这张卡作为单位存在于战场上的期间才有效）";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card.Controller == Opponent;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new DisableSkill(this)
            {
                TargetName = "飞行特效"
            });
            //TODO
            ItemsToApply.Add(new CanNotGetSkill(this)
            {
                Name = "飞行特效"
            });
        }
    }
}
