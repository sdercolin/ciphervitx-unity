using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-013 雌伏の王子 マルス
/// </summary>
public class Card00158 : Card
{
    public Card00158(User controller) : base(controller)
    {
        Serial = "00158";
        Pack = "P01";
        CardNum = "P01-013";
        Title = "雌伏的王子";
        UnitName = "马尔斯";
        power = 60;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
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
    /// 『勇敢なる王子』【起】[翻面2]敵を１体選び、移動させる。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "勇敢的王子";
            Description = "『勇敢的王子』【起】[翻面2]选择1名敌方单位，将其移动。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 2);
        }

        public override async Task Do()
        {
            await Controller.ChooseMove(Opponent.Field.Cards, 1, 1, this);
        }
    }

    /// <summary>
    /// スキル2
    /// 『レイピア』【常】このユニットが<獣馬>か<アーマー>を攻撃している場合、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "突刺剑";
            Description = "『突刺剑』【常】这名单位攻击<兽马>或<重甲>属性单位的期间，这名单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.AttackingUnit == card
                && (Game.DefendingUnit.HasType(TypeEnum.Beast) || Game.DefendingUnit.HasType(TypeEnum.Armor));
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10));
        }
    }
}
