using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-003 アリティアの王子 マルス
/// </summary>
public class Card00006 : Card
{
    public Card00006(User controller) : base(controller)
    {
        Serial = "00006";
        Pack = "S01";
        CardNum = "B01-003";
        Title = "阿利缇亚的王子";
        UnitName = "马尔斯";
        power = 40;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
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
    /// 『若き英雄』【起】[横置，他の味方を１体行動済みにする]敵を１体選び、移動させる。このスキルはこのユニットが前衛でなければ使用できない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "年轻的英雄";
            Description = "『年轻的英雄』【起】[横置，将1名其他我方单位转为已行动状态]选择1名敌方单位，将其移动。这个能力仅限这名单位位于前卫区时才可以使用。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return Owner.BelongedRegion is FrontField;
        }

        public override Cost DefineCost()
        {
            return Cost.ActionSelf(this) + Cost.ActionOthers(this, 1);
        }

        public override async Task Do()
        {
            await Controller.ChooseMove(Opponent.Field.Cards, 1, 1, this);
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『英雄の紋章』【支】自分の攻撃ユニットが<光の剣>の場合、戦闘終了まで、そのユニットが攻撃で破壊するオーブは２つになる。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : HeroEmblem
    {
        public Sk2()
        {
            Number = 2;
            Name = "英雄之纹章";
            Description = "〖攻击型〗『英雄之纹章』【支】我方的攻击单位是<光之剑>势力的场合，直到战斗结束为止，那名单位的攻击所将破坏的宝玉变为2颗。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
            Symbol = SymbolEnum.Red;
        }
    }
}
