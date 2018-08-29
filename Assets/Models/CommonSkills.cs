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
        return Cost.Action(this);
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
        return Cost.Action(this);
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