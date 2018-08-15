using System.Threading.Tasks;

/// <summary>
/// (S01) S01-001 スターロード マルス
/// </summary>
public class Card00001 : Card
{
    public Card00001(User controller) : base(controller)
    {
        Serial = "00001";
        Pack = "S01";
        CardNum = "S01-001";
        Title = "星领主";
        UnitName = "马尔斯";
        power = 70;
        support = 20;
        deployCost = 4;
        classChangeCost = 3;
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
    /// 『光の王子』【自】〖1回合1次〗出撃コストが２以下の味方が出撃した時、後衛の敵を１体選び、移動させてもよい。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "光之王子";
            Description = "『光之王子』〖1回合1次〗【自】出击费用2以下的我方单位出击时，你可以选择1名敌方后卫区上的单位，将其移动。";
            OncePerTurn = true;
            Optional = true;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override bool CheckInduceConditions(Message message)
        {
            var deployMessage = message as DeployMessage;
            if (deployMessage != null)
            {
                return deployMessage.TrueForAny(deployMessage.Targets, card => card.Controller == Controller && card.DeployCost <= 2);
            }
            return false;
        }

        public override Cost DefineCost()
        {
            return Cost.Null;
        }

        public override async Task Do()
        {
            var choices = Opponent.BackField.Cards;
            var target = await Request.ChooseUpToOne(choices, Controller);
            Controller.Move(target, this);
        }
    }

    /// <summary>
    /// スキル2
    /// 『ファルシオン』【常】このユニットが<竜>を攻撃している場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : PermanentSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "法尔西昂";
            Description = "『法尔西昂』【常】这名单位攻击<龙>属性单位的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }

        public override bool CanTarget(Card card)
        {
            return card == Owner
                && Game.AttackingUnit == card
                && Game.DefencingUnit.HasType(TypeEnum.Dragon);
        }

        public override void SetItemToApply()
        {
            ItemsToApply.Add(new PowerBuff(this, 20));
        }
    }
}
