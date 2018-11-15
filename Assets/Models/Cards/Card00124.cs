using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-078 永遠の幼子 ノノ
/// </summary>
public class Card00124 : Card
{
    public Card00124(User controller) : base(controller)
    {
        Serial = "00124";
        Pack = "B01";
        CardNum = "B01-078";
        Title = "永远的幼子";
        UnitName = "诺诺";
        power = 50;
        support = 20;
        deployCost = 3;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Female);
        weapons.Add(WeaponEnum.DragonStone);
        types.Add(TypeEnum.Dragon);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『幼き竜』【自】自分のターン終了時、自分の絆カードを１枚選び、手札に加える。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "幼龙";
            Description = "『幼龙』【自】自己的回合结束时，选择自己的1张羁绊卡，将其加入手牌。";
            Optional = false;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions(Induction induction)
        {
            return true;
        }

        public override Induction CheckInduceConditions(Message message)
        {
            var endTurnMessage = message as EndTurnMessage;
            if (endTurnMessage != null)
            {
                if (Game.TurnPlayer == Controller)
                {
                    return new Induction();
                }
            }
            return null;
        }

        public override Cost DefineCost()
        {
            return Cost.Null;
        }

        public override async Task Do(Induction induction)
        {
            await Controller.ChooseAddToHand(Controller.Bond.Cards, 1, 1, this);
        }
    }

    /// <summary>
    /// スキル2
    /// 『長寿な竜一族』【常】自分の絆カードが６枚以上の場合、このユニットの戦闘力は＋３０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2()
        {
            Number = 2;
            Name = "长寿的龙一族";
            Description = "『长寿的龙一族』【常】自己的羁绊卡有6张以上的场合，这名单位的战斗力+30。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner && Controller.Bond.Count >= 6;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 30));
        }
    }
}
