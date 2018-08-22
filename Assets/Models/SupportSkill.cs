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
    public abstract SupportSkillType Type { get; }

    public abstract bool Optional { get; }

    /// <summary>
    /// 能力解决，在流程中被调用
    /// </summary>
    /// <param name="AttackingUnit">攻击单位</param>
    /// <param name="AttackedUnit">被攻击单位</param>
    public async Task Solve(Card AttackingUnit, Card AttackedUnit)
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

    public bool Check()
    {
        if (!Available)
        {
            return false;
        }
        if (Type == SupportSkillType.Attacking && (Game.TurnPlayer != Controller))
        {
            return false;
        }
        if (Type == SupportSkillType.Defending && (Game.TurnPlayer == Controller))
        {
            return false;
        }
        Cost = DefineCost();
        return CheckConditions(Game.AttackingUnit, Game.DefendingUnit) && Cost.Check();
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

/// <summary>
/// 英雄之纹章
/// 需指定Symbol
/// </summary>
public class HeroEmblem : SupportSkill
{
    public SymbolEnum Symbol;
    public override bool Optional => false;
    public override SupportSkillType Type => SupportSkillType.Attacking;

    public override bool CheckConditions(Card AttackingUnit, Card AttackedUnit)
    {
        return AttackingUnit.HasSymbol(Symbol);
    }

    public override Cost DefineCost()
    {
        return Cost.Null;
    }

    public override Task Do(Card AttackingUnit, Card AttackedUnit)
    {
        AttackingUnit.Attach(new DestroyTwoOrbs(this, LastingTypeEnum.UntilBattleEnds));
        return Task.CompletedTask;
    }
}

/// <summary>
/// 天空之纹章
/// </summary>
public class FlyingEmblem : SupportSkill
{
    public override bool Optional => false;
    public override SupportSkillType Type => SupportSkillType.Attacking;

    public override bool CheckConditions(Card AttackingUnit, Card AttackedUnit)
    {
        return true;
    }

    public override Cost DefineCost()
    {
        return Cost.Null;
    }

    public override async Task Do(Card AttackingUnit, Card AttackedUnit)
    {
        var targets = Controller.Field.Cards;
        targets.Remove(Game.AttackingUnit);
        await Controller.ChooseMove(targets, 0, 1, this);
    }
}

/// <summary>
/// 攻击之纹章
/// </summary>
public class AttackEmblem : SupportSkill
{
    public override bool Optional => false;
    public override SupportSkillType Type => SupportSkillType.Attacking;

    public override bool CheckConditions(Card AttackingUnit, Card AttackedUnit)
    {
        return true;
    }

    public override Cost DefineCost()
    {
        return Cost.Null;
    }

    public override Task Do(Card AttackingUnit, Card AttackedUnit)
    {
        AttackingUnit.Attach(new PowerBuff(this, 20, LastingTypeEnum.UntilBattleEnds));
        return Task.CompletedTask;
    }
}

/// <summary>
/// 防御之纹章
/// </summary>
public class DefenceEmblem : SupportSkill
{
    public override bool Optional => false;
    public override SupportSkillType Type => SupportSkillType.Defending;

    public override bool CheckConditions(Card AttackingUnit, Card AttackedUnit)
    {
        return true;
    }

    public override Cost DefineCost()
    {
        return Cost.Null;
    }

    public override Task Do(Card AttackingUnit, Card AttackedUnit)
    {
        AttackedUnit.Attach(new PowerBuff(this, 20, LastingTypeEnum.UntilBattleEnds));
        return Task.CompletedTask;
    }
}

/// <summary>
/// 魔术之纹章
/// </summary>
public class MagicEmblem : SupportSkill
{
    public override bool Optional => false;
    public override SupportSkillType Type => SupportSkillType.Attacking;

    public override bool CheckConditions(Card AttackingUnit, Card AttackedUnit)
    {
        return true;
    }

    public override Cost DefineCost()
    {
        return Cost.Null;
    }

    public override async Task Do(Card AttackingUnit, Card AttackedUnit)
    {
        AttackingUnit.Controller.DrawCard(1, this);
        await Controller.ChooseDiscardHand(Controller.Hand.Cards, 1, 1, false, this);
    }
}

/// <summary>
/// 龙人之纹章
/// 需指定Symbol
/// </summary>
public class DragonEmblem : SupportSkill
{
    public SymbolEnum Symbol;
    public override bool Optional => false;
    public override SupportSkillType Type => SupportSkillType.Attacking;

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

/// <summary>
/// 计略之纹章
/// 需指定Symbol
/// </summary>
public class TacticalEmblem : SupportSkill
{
    public SymbolEnum Symbol;
    public override bool Optional => false;
    public override SupportSkillType Type => SupportSkillType.Attacking;

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

/// <summary>
/// 祈祷之纹章
/// </summary>
public class MiracleEmblem : SupportSkill
{
    public override bool Optional => false;
    public override SupportSkillType Type => SupportSkillType.Defending;

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

/// <summary>
/// 盗贼之纹章
/// </summary>
public class ThiefEmblem : SupportSkill
{
    public override bool Optional => false;
    public override SupportSkillType Type => SupportSkillType.Attacking;

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

/// <summary>
/// 黑暗之纹章
/// </summary>
public class DarkEmblem : SupportSkill
{
    public override bool Optional => false;
    public override SupportSkillType Type => SupportSkillType.Attacking;

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