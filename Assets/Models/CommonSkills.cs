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