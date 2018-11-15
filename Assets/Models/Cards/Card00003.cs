/// <summary>
/// (S01) S01-003 忠義の古強者 ジェイガン
/// </summary>
public class Card00003 : Card
{
    public Card00003(User controller) : base(controller)
    {
        Serial = "00003";
        Pack = "S01";
        CardNum = "S01-003";
        Title = "忠义的老战士";
        UnitName = "杰刚";
        power = 70;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Red);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Lance);
        types.Add(TypeEnum.Beast);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『戦場の教育役』【特】このカードは絆エリアに置くことができない。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : PermanentSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "战场上的教育家";
            Description = "『战场上的教育家』【特】这张卡不能放置到羁绊区。";
            TypeSymbols.Add(SkillTypeSymbol.Special);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner;
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new CanNotBePlacedInBond(this));
        }
    }
}
