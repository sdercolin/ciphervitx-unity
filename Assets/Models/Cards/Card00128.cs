using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-082 物陰の闇使い サーリャ
/// </summary>
public class Card00128 : Card
{
    public Card00128(User controller) : base(controller)
    {
        Serial = "00128";
        Pack = "B01";
        CardNum = "B01-082";
        Title = "藏身黑暗的暗魔法师";
        UnitName = "萨莉雅";
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
    /// 『呪い』【常】自分のターン中、自分の手札の枚数が相手より多い場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "诅咒";
            Description = "『诅咒』【常】自己的回合中，自己的手牌数量比对手多的场合，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner && Game.TurnPlayer == Controller && Controller.Hand.Count > Opponent.Hand.Count;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 20));
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『暗闇の紋章』【支】相手の手札が５枚以上の場合、相手は自分の手札を１枚選び、退避エリアに置く。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : DarkEmblem
    {
        public Sk2()
        {
            Number = 2;
            Name = "黑暗之纹章";
            Description = "〖攻击型〗『黑暗之纹章』【支】对手的手牌有5张以上的场合，对手选择他自己的1张手牌，放置到退避区。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
