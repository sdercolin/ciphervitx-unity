using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-066 碧眼の黒豹 ソール
/// </summary>
public class Card00040 : Card
{
    public Card00040(User controller) : base(controller)
    {
        Serial = "00040";
        Pack = "S02";
        CardNum = "B01-066";
        Title = "碧眼的黑豹";
        UnitName = "索尔";
        power = 60;
        support = 10;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『聖盾』〖转职技〗【常】<弓>か<魔法>か<竜石>の後衛の敵は攻撃できない。（はこのユニットがクラスチェンジしていなければ有効にならない）
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "圣盾";
            Description = "〖转职技〗『圣盾』【常】敌方后卫区上的<弓>和<魔法>和<龙石>武器的单位不能攻击。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.CCS;
        }

        public override bool CanTarget(Card card)
        {
            return card.BelongedRegion == Opponent.BackField
                && (card.HasWeapon(WeaponEnum.Bow) || card.HasWeapon(WeaponEnum.Magic) || card.HasWeapon(WeaponEnum.DragonStone))
                && Owner.IsClassChanged;
        }

        public override void SetItemToApply()
        {
            //TODO
            ItemsToApply.Add(new CanNotAttack(this));
        }
    }

    /// <summary>
    /// スキル2
    /// 『碧と紅の絆』【常】このユニットが『ソワレ』に支援されている場合、このユニットの戦闘力は＋３０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "碧与红的羁绊";
            Description = "『碧与红的羁绊』【常】这名单位被「索瓦蕾」支援的期间，这名单位的战斗力+30。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.BattlingUnits.Contains(card)
                && card.Controller.Support.Filter(supportCard => supportCard.HasUnitNameOf("索瓦蕾")).Count > 0;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 30));
        }
    }
}
