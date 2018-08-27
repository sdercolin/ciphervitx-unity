using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (S02) B01-057 聖王の神軍師 ルフレ（女）
/// </summary>
public class Card00031 : Card
{
    public Card00031(User controller) : base(controller)
    {
        Serial = "00031";
        Pack = "S02";
        CardNum = "B01-057";
        Title = "圣王的神军师";
        UnitName = "路弗雷（女）";
        power = 60;
        support = 20;
        deployCost = 4;
        classChangeCost = 3;
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
    /// 『神軍師の采配』【自】他の味方がクラスチェンジするたび、敵を１体選び、移動させてもよい。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : AutoSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "神军师的指挥";
            Description = "『神军师的指挥』【自】每次其它我方单位转职时，你可以选择1名敌方单位，将其移动。";
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
            if (levelupMessage != null && levelupMessage.IsClassChange)
            {
                var target = levelupMessage.Target;
                if (target.Controller == Controller && target != Owner)
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

    /// <summary>
    /// スキル2
    /// 『これも、策のうちです』〖转职技〗【起】[翻面3]自分のオーブの数が相手より少ない場合、デッキの１番上のカードをオーブに追加する。（はこのユニットがクラスチェンジしていなければ使用できない）
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : ActionSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "这也在计策当中";
            Description = "〖转职技〗『这也在计策当中』【起】[翻面3]自己的宝玉数量比对手少的场合，将卡组最上方的1张卡追加到宝玉区。（〖转职技〗仅限这名单位经过转职后才能使用）";
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.CCS;
        }

        public override bool CheckConditions()
        {
            return Owner.IsClassChanged;
        }

        public override Cost DefineCost()
        {
            return Cost.ReverseBond(this, 3);
        }

        public override Task Do()
        {
            if (Controller.Orb.Cards.Count < Opponent.Orb.Cards.Count)
            {
                //TODO
                Controller.AddToOrb(Controller.Deck.Top, this);
            }
            return Task.CompletedTask;
        }
    }
}
