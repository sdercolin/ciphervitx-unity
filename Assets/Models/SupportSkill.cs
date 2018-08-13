using System;
using System.Threading.Tasks;
/// <summary>
/// 支援型能力
/// </summary>
public abstract class SupportSkill : Skill
{
    public SupportSkill() : base() { }

    /// <summary>
    /// 支援能力种类
    /// </summary>
    public SupportSkillType Type;

    public bool Optional = false;

    /// <summary>
    /// 能力解决，在流程中被调用
    /// </summary>
    /// <param name="AttackingUnit">攻击单位</param>
    /// <param name="AttackedUnit">被攻击单位</param>
    public async void Solve(Card AttackingUnit, Card AttackedUnit)
    {
        Cost = DefineCost();
        if (CheckConditions(AttackingUnit, AttackedUnit) && Cost.Check())
        {
            if (Optional)
            {
                if (!await Request.AskIfUse(this, Controller))
                {
                    return;
                }
            }
            //Owner.Controller.Broadcast(new Message(MessageType.UseSkill, new System.Collections.ArrayList { this }));
            await Cost.Pay();
            await Do(AttackingUnit, AttackedUnit);
        }
    }

    /// <summary>
    /// 判断是否符合发动条件
    /// </summary>
    /// <param name="AttackingUnit">攻击单位</param>
    /// <param name="AttackedUnit">被攻击单位</param>
    /// <returns>若符合，则返回true</returns>
    public abstract bool CheckConditions(Card AttackingUnit, Card AttackedUnit);

    /// <summary>
    /// 实行能力
    /// </summary>
    /// <param name="AttackingUnit">攻击单位</param>
    /// <param name="AttackedUnit">被攻击单位</param>
    public abstract Task Do(Card AttackingUnit, Card AttackedUnit);

    /// <summary>
    /// 定义费用
    /// </summary>
    public abstract Cost DefineCost();
    public Cost Cost;
}

/// <summary>
/// 支援能力种类
/// </summary>
public enum SupportSkillType
{
    Attacking, //攻击型
    Defending, //防御型
    AttackingDefending //攻防型
}

public class HeroEmblem : SupportSkill
{
    public SymbolEnum Symbol;

    public override bool CheckConditions(Card AttackingUnit, Card AttackedUnit)
    {
        throw new NotImplementedException();
    }

    public override Cost DefineCost()
    {
        throw new NotImplementedException();
    }

    public override Task Do(Card AttackingUnit, Card AttackedUnit)
    {
        throw new NotImplementedException();
    }
}

public class FlyingEmblem : SupportSkill
{
    public override bool CheckConditions(Card AttackingUnit, Card AttackedUnit)
    {
        throw new NotImplementedException();
    }

    public override Cost DefineCost()
    {
        throw new NotImplementedException();
    }

    public override Task Do(Card AttackingUnit, Card AttackedUnit)
    {
        throw new NotImplementedException();
    }
}

public class AttackEmblem : SupportSkill
{
    public override bool CheckConditions(Card AttackingUnit, Card AttackedUnit)
    {
        throw new NotImplementedException();
    }

    public override Cost DefineCost()
    {
        throw new NotImplementedException();
    }

    public override Task Do(Card AttackingUnit, Card AttackedUnit)
    {
        throw new NotImplementedException();
    }
}

public class DefenceEmblem : SupportSkill
{
    public override bool CheckConditions(Card AttackingUnit, Card AttackedUnit)
    {
        throw new NotImplementedException();
    }

    public override Cost DefineCost()
    {
        throw new NotImplementedException();
    }

    public override Task Do(Card AttackingUnit, Card AttackedUnit)
    {
        throw new NotImplementedException();
    }
}