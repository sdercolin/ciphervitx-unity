using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) S02-001 聖王国の守護者 クロム
/// </summary>
public class Card00024 : Card
{
    public Card00024(User controller) : base(controller)
    {
        Serial = "00024";
        Pack = "S02";
        CardNum = "S02-001";
        Title = "圣王国的守护者";
        UnitName = "库洛姆";
        power = 70;
        support = 20;
        deployCost = 4;
        classChangeCost = 3;
        symbols.Add(SymbolEnum.Blue);
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
    /// 『クロム自警団』【自】〖1回合1次〗他の味方がクラスチェンジした時、ターン終了まで、その味方の戦闘力は＋２０され、その味方が攻撃で破壊するオーブは２つになる。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "库洛姆警卫队";
            Description = "『库洛姆警卫队』【自】〖1回合1次〗其他我方单位转职时，直到回合结束为止，那名我方单位的战斗力+20，那名我方单位的攻击所将破坏的宝玉变为2颗。";
            OncePerTurn = true;
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
            var levelupMessage = message as LevelUpMessage;
            if (levelupMessage != null && levelupMessage.IsClassChange)
            {
                var target = levelupMessage.Target;
                if (target.Controller == Controller && target != Owner)
                {
                    return new MyInduction()
                    {
                        Target = target
                    };
                }
            }
            return null;
        }

        public override Cost DefineCost()
        {
            return Cost.Null;
        }

        public override Task Do(Induction induction)
        {
            var target = ((MyInduction)induction).Target;
            Controller.AttachItem(new PowerBuff(this, 20, LastingTypeEnum.UntilTurnEnds), target);
            Controller.AttachItem(new DestroyTwoOrbs(this, LastingTypeEnum.UntilTurnEnds), target);
            return Task.CompletedTask;
        }

        public class MyInduction : Induction
        {
            public Card Target;
        }
    }

    /// <summary>
    /// スキル2
    /// 『封剣 ファルシオン』【常】このユニットが<竜>を攻撃している場合、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : Dragonslayer
    {
        public Sk2()
        {
            Number = 2;
            Name = "封剑 法尔西昂";
            Description = "『封剑 法尔西昂』【常】这名单位攻击<龙>属性单位的期间，这名单位的战斗力+20。";
            TypeSymbols.Add(SkillTypeSymbol.Permanent);
            Keyword = SkillKeyword.Null;
        }
    }
}
