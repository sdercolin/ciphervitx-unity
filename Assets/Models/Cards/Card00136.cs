using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B01) B01-090 鉄仮面の竜騎士 ジェローム
/// </summary>
public class Card00136 : Card
{
    public Card00136(User controller) : base(controller)
    {
        Serial = "00136";
        Pack = "B01";
        CardNum = "B01-090";
        Title = "铁面具龙骑士";
        UnitName = "杰罗姆";
        power = 40;
        support = 30;
        deployCost = 2;
        classChangeCost = 0;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Axe);
        types.Add(TypeEnum.Flight);
        types.Add(TypeEnum.Dragon);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『天翔ける双竜』【常】自分のターン中、このユニットと味方の『セルジュ』の戦闘力は＋１０される。このスキルは味方に『セルジュ』がいなければ有効にならない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "天翔的双龙";
            Description = "『天翔的双龙』【常】自己的回合中，这名单位与我方的「塞尔吉」的战斗力+10。这个能力仅限我方战场上存在「塞尔吉」时才有效。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return (card == Owner || card.HasUnitNameOf(Strings.Get("card_text_unitname_セルジュ")))
                && Game.TurnPlayer == Controller
                && card.IsOnField
                && card.Controller == Controller
                && Controller.Field.Filter(unit => unit.HasUnitNameOf(Strings.Get("card_text_unitname_セルジュ"))).Count > 0;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 10));
        }
    }
}
