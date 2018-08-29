using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S01) B01-036 大司祭ミロアの娘 リンダ
/// </summary>
public class Card00023 : Card
{
    public Card00023(User controller) : base(controller)
    {
        Serial = "00023";
        Pack = "S01";
        CardNum = "B01-036";
        Title = "大司祭米罗亚的女儿";
        UnitName = "琳达";
        power = 30;
        support = 20;
        deployCost = 1;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Red);
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
    /// 『サンダー』【起】〖1回合1次〗[翻面1]ターン終了まで、このユニットの戦闘力は＋１０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "闪电";
            Description = "『闪电』【起】〖1回合1次〗[翻面1]直到回合结束为止，这名单位的战斗力+10。";
            OncePerTurn = true;
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 1);
        }

        public override Task Do()
        {
            Controller.AttachItem(new PowerBuff(this, 10, LastingTypeEnum.UntilTurnEnds), Owner);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 〖攻击型〗『魔術の紋章』【支】カードを１枚引く。自分の手札を１枚選び、退避エリアに置く。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : MagicEmblem
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "魔术之纹章";
            Description = "〖攻击型〗『魔术之纹章』【支】抽1张卡。选择自己的1张手牌，放置到退避区。";
            TypeSymbols.Add(SkillTypeSymbol.Support);
            Keyword = SkillKeyword.Null;
        }
    }
}
