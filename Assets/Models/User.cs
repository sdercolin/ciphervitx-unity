using System;
using System.Collections.Generic;

/// <summary>
/// 玩家类
/// </summary>
public abstract class User
{
    public User()
    {
        Deck = new Deck(this);
        Hand = new Hand(this);
        Retreat = new Retreat(this);
        Support = new Support(this);
        Bond = new Bond(this);
        Orb = new Orb(this);
        Field = new Field(this);
        FrontField = new FrontField(this);
        BackField = new BackField(this);
        Overlay = new Overlay(this);
        Guid = System.Guid.NewGuid().ToString();
    }

    public string Guid;
    public override string ToString()
    {
        return "{\"guid\": \"" + Guid + "\" }";
    }

    public List<Area> AllAreas { get => new List<Area> { Deck, Hand, Retreat, Support, Bond, Orb, FrontField, BackField, Overlay }; }
    public Deck Deck;
    public Hand Hand;
    public Retreat Retreat;
    public Support Support;
    public Bond Bond;
    public Orb Orb;
    public Field Field;
    public FrontField FrontField;
    public BackField BackField;
    public Overlay Overlay;
    public abstract User Opponent { get; }
    public Card Hero { get => AllCards.Find(card => card.IsHero); }

    /// <summary>
    /// 待处理的转职奖励计数
    /// </summary>
    public int CCBonusInducedCount = 0;

    /// <summary>
    /// 败北标志
    /// </summary>
    public bool LoseFlag = false;

    /// <summary>
    /// 本回合中已经出击（升级）过的费用数
    /// </summary>
    public int DeployAndCCCostCount = 0;

    /// <summary>
    /// 行动阶段结束标志
    /// </summary>
    public bool ActionPhaseEnded = false;

    /// <summary>
    /// 附加能力列表
    /// </summary>
    public List<SubSkill> SubSkillList = new List<SubSkill>();

    public List<Card> AllCards
    {
        get
        {
            List<Card> allCards = new List<Card>();
            ForEachCard(card => allCards.Add(card));
            return allCards;
        }
    }

    public void ForEachCard(Action<Card> action)
    {
        AllAreas.ForEach(area => area.ForEachCard(action));
    }

    public bool TrueForAllCard(Predicate<Card> predicate)
    {
        return AllAreas.TrueForAll(area => area.TrueForAllCard(predicate));
    }

    #region 动作
    public void Move(Card target, Skill reason)
    {
        if (target != null)
        {
            List<Card> targets = new List<Card> { target };
            Move(targets, reason);
        }
    }

    public void Move(List<Card> targets, Skill reason)
    {
        Game.DoMessage(new MoveMessage()
        {
            Targets = targets,
            Reason = reason
        });
    }

    public void UseBond(Card target, Skill reason)
    {
        if (target != null)
        {
            List<Card> targets = new List<Card> { target };
            UseBond(targets, reason);
        }
    }

    public void UseBond(List<Card> targets, Skill reason)
    {
        if (targets.Count > 0)
        {
            Game.DoMessage(new UseBondMessage()
            {
                Targets = targets,
                Reason = reason
            });
        }
    }

    public void StartTurn()
    {
        Game.DoMessage(new StartTurnMessage()
        {
            TurnPlayer = this
        });
    }

    public void GoToBondPhase()
    {
        Game.DoMessage(new GoToBondPhaseMessage()
        {
            TurnPlayer = this
        });
    }

    public void GoToDeploymentPhase()
    {
        Game.DoMessage(new GoToDeploymentPhaseMessage()
        {
            TurnPlayer = this
        });
    }

    public void GoToActionPhase()
    {
        Game.DoMessage(new GoToActionPhaseMessage()
        {
            TurnPlayer = this
        });
    }

    public void EndTurn()
    {
        Game.DoMessage(new EndTurnMessage()
        {
            TurnPlayer = this
        });
    }

    public void ClearStatusEndingTurn()
    {
        Game.DoMessage(new ClearStatusEndingTurnMessage()
        {
            TurnPlayer = this
        });
    }

    public void SwitchTurn()
    {
        Game.DoMessage(new SwitchTurnMessage(){

        });
    }
    public void SetToBond(Card target, bool frontShown, Skill reason = null)
    {
        if (target != null)
        {
            List<Card> targets = new List<Card> { target };
            SetToBond(targets, frontShown, reason);
        }
    }

    public void SetToBond(List<Card> targets, bool frontShown, Skill reason = null)
    {
        if (targets.Count > 0)
        {
            Game.DoMessage(new ToBondMessage
            {
                Targets = targets,
                TargetFrontShown = frontShown,
                Reason = reason
            });
        }
    }

    public void ChooseSetToBond(List<Card> targets, bool frontShown, int min, int max, Skill reason = null)
    {
        if (targets.Count > 0)
        {
            ToBondMessage readyToBondMessage = Game.TryMessage(new ToBondMessage
            {
                Targets = targets,
                TargetFrontShown = frontShown
            }) as ToBondMessage;
            if (readyToBondMessage != null && readyToBondMessage.Targets.Count > 0)
            {
                SetToBond(Request.Choose(readyToBondMessage.Targets, min, max, this), readyToBondMessage.TargetFrontShown, readyToBondMessage.Reason);
            }
        }
    }

    public void RefreshUnit(Card target, Skill reason)
    {
        if (target != null)
        {
            RefreshUnit(new List<Card>() { target }, reason);
        }
    }

    public void RefreshUnit(List<Card> targets, Skill reason)
    {
        if (targets.Count > 0)
        {
            Game.DoMessage(new RefreshUnitMessage()
            {
                Targets = targets,
                Reason = reason
            });
        }
    }

    public void DrawCard(int number, Skill reason = null)
    {
        Game.DoMessage(new DrawCardMessage()
        {
            Player = this,
            Number = number,
            Reason = reason
        });
    }

    public List<Card> ChooseDiscardedCardsSameNameProcess(List<Card> units, string name)
    {
        ReadyForSameNameProcessPartialMessage readyForSameNameProcessMessage = Game.TryMessage(new ReadyForSameNameProcessPartialMessage()
        {
            Targets = units,
            Name = name
        }) as ReadyForSameNameProcessPartialMessage;
        if (readyForSameNameProcessMessage != null && readyForSameNameProcessMessage.Targets.Count > 1)
        {
            Card savedUnit;
            if (readyForSameNameProcessMessage.Targets.Contains(Hero))
            {
                savedUnit = Hero;
            }
            else
            {
                savedUnit = Request.ChooseOne(readyForSameNameProcessMessage.Targets, this);
            }
            List<Card> confirmedTarget = ListUtils.Clone(readyForSameNameProcessMessage.Targets);
            confirmedTarget.Remove(savedUnit);
            return confirmedTarget;
        }
        else
        {
            return new List<Card>();
        }
    }
    #endregion
}

/// <summary>
/// 自己
/// </summary>
public class Player : User
{
    public Player() : base() { }

    public override User Opponent { get => Game.Rival; }
}

/// <summary>
/// 对手
/// </summary>
public class Rival : User
{
    public Rival() : base() { }

    public override User Opponent { get => Game.Player; }
}