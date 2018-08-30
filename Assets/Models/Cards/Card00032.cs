using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-058 記憶を失くした軍師 ルフレ（女）
/// </summary>
public class Card00032 : Card
{
    public Card00032(User controller) : base(controller)
    {
        Serial = "00032";
        Pack = "S02";
        CardNum = "B01-058";
        Title = "失去记忆的军师";
        UnitName = "路弗雷（女）";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.Magic);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『軍師の才』【常】自分のオーブの数が相手より少ない場合、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "军师之才";
            Description = "『军师之才』【常】自己的宝玉数量比对手少的场合，这名单位的战斗力+10。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Controller.Orb.Count < Opponent.Orb.Count;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10));
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『計略の紋章』【支】自分の攻撃ユニットが<聖痕>の場合、相手の防御ユニット以外の敵を１体選ぶ。その敵を移動させてもよい。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : TacticalEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "计略之纹章";
            Description = "〖攻击型〗『计略之纹章』【支】自己的攻击单位是<圣痕>势力的场合，选择对手的防御单位以外的1名敌方单位。你可以移动那名敌方单位。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
            Symbol = SymbolEnum.Blue;
        }
    }
}
