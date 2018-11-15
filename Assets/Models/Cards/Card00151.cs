using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (P01) P01-005 正義を貫く王子 クロム
/// </summary>
public class Card00151 : Card
{
    public Card00151(User controller) : base(controller)
    {
        Serial = "00151";
        Pack = "P01";
        CardNum = "P01-005";
        Title = "贯彻正义的王子";
        UnitName = "库洛姆";
        power = 60;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.Blue);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Sword);
        ranges.Add(RangeEnum.One);
        sk1 = new Sk1();
        Attach(sk1);
    }

    /// <summary>
    /// スキル1
    /// 『白銀の君主』【自】このユニットにクラスチェンジした時、敵を１体選び、移動させてもよい。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1()
        {
            Number = 1;
            Name = "白银君主";
            Description = "『白银君主』【自】转职为这名单位时，你可以选择1名敌方单位，将其移动。";
            Optional = true;
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
            if (levelupMessage != null)
            {
                if (levelupMessage.IsClassChange && levelupMessage.Target == Owner)
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
            await Controller.ChooseMove(Opponent.Field.Cards, 0, 1, this);
        }
    }
}
