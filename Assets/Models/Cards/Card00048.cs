using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-002 亡国の王子 マルス
/// </summary>
public class Card00048 : Card
{
    public Card00048(User controller) : base(controller)
    {
        Serial = "00048";
        Pack = "B01";
        CardNum = "B01-002";
        Title = "亡国的王子";
        UnitName = "马尔斯";
        power = 50;
        support = 20;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『英雄の資質』【常】<光の剣>の味方が他に２体以上いる場合、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "英雄的资质";
            Description = "『英雄的资质』【常】我方战场上有2名以上其他<光之剑>势力的单位的场合，这名单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Controller.Field.Filter(unit => unit.HasSymbol(SymbolEnum.Red) && unit != Owner).Count >= 2;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10));
        }
    }
}
