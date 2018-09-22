using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// (B02) B02-014 爆炎使い サイゾウ
/// </summary>
public class Card00164 : Card
{
    public Card00164(User controller) : base(controller)
    {
        Serial = "00164";
        Pack = "B02";
        CardNum = "B02-014";
        Title = "爆炎操控者";
        UnitName = "才藏";
        power = 50;
        support = 20;
        deployCost = 3;
        classChangeCost = 2;
        symbols.Add(SymbolEnum.White);
        genders.Add(GenderEnum.Male);
        weapons.Add(WeaponEnum.Knife);
        ranges.Add(RangeEnum.OnetoTwo);
        sk1 = new Sk1();
        Attach(sk1);
        sk2 = new Sk2();
        Attach(sk2);
    }

    /// <summary>
    /// スキル1
    /// 『炎魔の陣』【起】〖1回合1次〗[他の味方を２体行動済みにする]ターン終了まで、このユニットの戦闘力は＋２０される。
    /// </summary>
    public Sk1 sk1;
    public class Sk1 : ActionSkill
    {
        public Sk1() : base()
        {
            Number = 1;
            Name = "炎魔之阵";
            Description = "『炎魔之阵』【起】〖1回合1次〗[将2名其他我方单位转为已行动状态]直到回合结束为止，这名单位的战斗力+20。";
            OncePerTurn = true;
            TypeSymbols.Add(SkillTypeSymbol.Action);
            Keyword = SkillKeyword.Null;
            OncePerTurn = true;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Cost DefineCost()
        {
            return Cost.ActionOthers(this, 2);
        }

        public override Task Do()
        {
            Controller.AttachItem(new PowerBuff(this, 20, LastingTypeEnum.UntilTurnEnds), Owner);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// スキル2
    /// 『強行偵察』【自】このユニットの攻撃で敵を撃破した時、このユニットがこのターンに『炎魔の陣』を使用しているなら、相手のデッキの１番上のカードを公開させる。あなたはそのカードを退避エリアに置かせてもよい。
    /// </summary>
    public Sk2 sk2;
    public class Sk2 : AutoSkill
    {
        public Sk2() : base()
        {
            Number = 2;
            Name = "强行侦察";
            Description = "『强行侦察』【自】这名单位的攻击击破敌方单位时，如果这名单位在这个回合中使用过『炎魔之阵』，公开对手卡组最上方的1张卡。你可以将那张卡放置到退避区。";
            Optional = false;
            TypeSymbols.Add(SkillTypeSymbol.Auto);
            Keyword = SkillKeyword.Null;
        }

        public override bool CheckConditions(Induction induction)
        {
            return ((Card00164)Owner).sk1.UsedInThisTurn;
        }

        public override Induction CheckInduceConditions(Message message)
        {
            var destroyMessage = message as DestroyMessage;
            if (destroyMessage != null)
            {
                foreach (var unit in destroyMessage.DestroyedUnits)
                {
                    if (destroyMessage.AttackingUnit == Owner && unit.Controller == Opponent)
                    {
                        return new Induction();
                    }
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
            var target = Opponent.Deck.Top;
            Controller.ShowCard(target, this);
            if (await Request.AskIfSendToRetreat(target, Controller))
            {
                Controller.SendToRetreat(target, this);
            }
        }
    }
}
