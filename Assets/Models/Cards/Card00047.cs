using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-001 救国の英雄 マルス
/// </summary>
public class Card00047 : Card
{
    public Card00047(User controller) : base(controller)
    {
        Serial = "00047";
        Pack = "B01";
        CardNum = "B01-001";
        Title = "救国的英雄";
        UnitName = "马尔斯";
        power = 70;
        support = 20;
        deployCost = 5;
        classChangeCost = 4;
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
    /// 『英雄の凱歌』【起】[翻面3，自分の手札から『マルス』を１枚退避エリアに置く]次の相手のターン終了まで、すべての味方の戦闘力は＋３０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "英雄的凯歌";
            Description = "『英雄的凯歌』【起】[翻面3，从自己的手牌将1张「马尔斯」放置到退避区]直到下个对手的回合结束为止，所有我方单位的战斗力+30。";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 3) + Cost.DiscardHand(this, 1, card => card.HasUnitNameOf(Strings.Get("card_text_unitname_マルス")));
        }

        public override Task Do()
        {
            Controller.Field.ForEachCard(unit =>
                {
                    Controller.AttachItem(new PowerBuff(this, 30, LastingTypeEnum.UntilNextOpponentTurnEnds), unit);
                });
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『ファルシオン』【常】このユニットが<竜>を攻撃している場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : Dragonslayer
    {
        public Sk2()
        {
            Number = 2;
            Name = "法尔西昂";
            Description = "『法尔西昂』【常】这名单位攻击<龙>属性单位的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }
    }
}
