using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// 『飛行特効』【常】このユニットが<飛行>を攻撃している場合、このユニットの戦闘力は＋３０される。
/// </summary>
public class Wingslayer : PermanentSkill
{
    public override bool CanTarget(Card card)
    {
        return card == Owner
            && Game.AttackingUnit == card
            && Game.DefendingUnit.HasType(TypeEnum.Flight);
    }

    public override void SetItemToApply()
    {
        ItemsToApply.Add(new PowerBuff(this, 30));
    }
}

/// <summary>
/// 【常】このユニットが<竜>を攻撃している場合、このユニットの戦闘力は＋２０される。
/// </summary>
public class Dragonslayer : PermanentSkill
{
    public override bool CanTarget(Card card)
    {
        return card == Owner
            && Game.AttackingUnit == card
            && Game.DefendingUnit.HasType(TypeEnum.Dragon);
    }

    public override void SetItemToApply()
    {
        ItemsToApply.Add(new PowerBuff(this, 20));
    }
}

/// <summary>
/// 『天空の運び手』【起】[横置]他の味方を１体選び、移動させる。
/// </summary>
public class WingedDeliverer : ActionSkill
{
    public override bool CheckConditions()
    {
        return true;
    }

    public override Cost DefineCost()
    {
        return Cost.ActionSelf(this);
    }

    public override async Task Do()
    {
        var choices = Controller.Field.Cards;
        choices.Remove(Owner);
        if (choices.Count > 0)
        {
            await Controller.ChooseMove(choices, 1, 1, this);
        }
    }
}

/// <summary>
/// 『重装の心得』【常】このユニットが<魔法>以外に攻撃されている場合、このユニットの戦闘力は＋２０される。
/// </summary>
public class ArmorExpertise : PermanentSkill
{
    public override bool CanTarget(Card card)
    {
        return card == Owner
            && Game.DefendingUnit == card
            && !Game.AttackingUnit.HasWeapon(WeaponEnum.Magic);
    }

    public override void SetItemToApply()
    {
        ItemsToApply.Add(new PowerBuff(this, 20));
    }
}

/// <summary>
/// 『鍵開け』【起】[横置]相手のデッキの１番上のカードを公開させる。そのカードの出撃コストが３以上の場合、あなたは翻面1してもよい。そうしたなら、カードを１枚引く。
/// </summary>
public class Unlock : ActionSkill
{
    public override bool CheckConditions()
    {
        return true;
    }

    public override Cost DefineCost()
    {
        return Cost.ActionSelf(this);
    }

    public override async Task Do()
    {
        var target = Opponent.Deck.Top;
        Opponent.ShowCard(target, this);
        if (target.DeployCost >= 3)
        {
            var choices = Controller.GetReversableBonds(this);
            if (choices.Count > 0)
            {
                if (await Request.AskIfReverseBond(1, this, Controller))
                {
                    await Controller.ChooseReverseBond(choices, 1, 1, this, false);
                    Controller.DrawCard(1, this);
                }
            }
        }
    }
}

/// <summary>
/// 『ライブ』【起】[横置，翻面2]自分の退避エリアから『……』以外のカードを１枚選び、手札に加える。
/// 需指定除……以外的单位名
/// </summary>
public class Heal : ActionSkill
{
    public string ExceptName { get; protected set; }

    public override bool CheckConditions()
    {
        return true;
    }

    public override Cost DefineCost()
    {
        return Cost.ActionSelf(this) + Cost.ReverseBond(this, 2);
    }

    public override async Task Do()
    {
        await Controller.ChooseAddToHand(Controller.Retreat.Filter(unit => !unit.HasUnitNameOf(ExceptName)), 1, 1, this);
    }
}

/// <summary>
///【起】〖1回合1次〗[翻面1]ターン終了まで、このユニットの戦闘力は＋１０される。
/// </summary>
public class ReverseBondToAdd10 : ActionSkill
{
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
///【起】〖1回合1次〗[翻面1]ターン終了まで、このユニットの戦闘力は＋２０される。
/// </summary>
public class ReverseBondToAdd20 : ActionSkill
{
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
        Controller.AttachItem(new PowerBuff(this, 20, LastingTypeEnum.UntilTurnEnds), Owner);
        return Task.CompletedTask;
    }
}

/// <summary>
/// 『天空を翔ける者』【起】〖1回合1次〗このユニットを移動させる。このスキルはこのユニットが未行動でなければ使用できない。
/// </summary>
public class AngelicFlight : ActionSkill
{
    public override bool CheckConditions()
    {
        return !Owner.IsHorizontal;
    }

    public override Cost DefineCost()
    {
        return Cost.Null;
    }

    public override Task Do()
    {
        Controller.Move(Owner, this);
        return Task.CompletedTask;
    }
}

/// <summary>
/// 【常】相手の手札が４枚以下の場合、このユニットの戦闘力は＋１０される。
/// </summary>
public class Hand4OrLessAdd10 : PermanentSkill
{
    public override bool CanTarget(Card card)
    {
        return card == Owner && Opponent.Hand.Count <= 4;
    }

    public override void SetItemToApply()
    {
        ItemsToApply.Add(new PowerBuff(this, 10));
    }
}