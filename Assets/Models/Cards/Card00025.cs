using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) S02-002 忘れられた重騎士 カラム
/// </summary>
public class Card00025 : Card
{
    public Card00025(User controller) : base(controller)
    {
        Serial = "00025";
        Pack = "S02";
        CardNum = "S02-002";
        Title = "被忘却的重骑士";
        UnitName = "卡拉姆";
        power = 30;
        support = 10;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Armor);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『重装の心得』【常】このユニットが<魔法>以外に攻撃されている場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "重装的心得";
            Description = "『重装的心得』【常】这名单位被<魔法>以外的武器攻击的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.DefendingUnit == card
                && !Game.AttackingUnit.HasWeapon(WeaponEnum.Magic);
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 20));
        }
    }

    /// <summary>
    /// スキル2
    /// 〖防御型〗『防御の紋章』【支】戦闘終了まで、自分の防御ユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : DefenceEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "防御之纹章";
            Description = "〖防御型〗『防御之纹章』【支】直到战斗结束为止，自己的防御单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
