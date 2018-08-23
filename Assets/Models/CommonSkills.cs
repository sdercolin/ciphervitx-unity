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