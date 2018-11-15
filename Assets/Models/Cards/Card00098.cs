using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-052 英雄王の末裔 クロム
/// </summary>
public class Card00098 : Card
{
    public Card00098(User controller) : base(controller)
    {
        Serial = "00098";
        Pack = "B01";
        CardNum = "B01-052";
        Title = "英雄王的末裔";
        UnitName = "库洛姆";
        power = 50;
        support = 20;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『勇敢なる王子』【起】[翻面2]敵を１体選び、移動させる。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1()
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
}
