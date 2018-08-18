using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    public List<Area> AllAreas => new List<Area> { Deck, Hand, Retreat, Support, Bond, Orb, FrontField, BackField, Overlay };
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
    public Card Hero => AllCards.Find(card => card.IsHero);

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
        Game.TryDoMessage(new MoveMessage()
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
            Game.TryDoMessage(new UseBondMessage()
            {
                Targets = targets,
                Reason = reason
            });
        }
    }

    public void Attack(Card unit, Card target)
    {
        Game.TryDoMessage(new AttackMessage()
        {
            AttackingUnit = unit,
            DefendingUnit = target
        });
    }

    public void SetSupportCard()
    {
        Game.TryDoMessage(new SetSupportMessage()
        {
            User = this
        });
    }

    public void ConfirmSupportCard()
    {
        Card battlingUnit = Game.BattlingUnits.Find(unit => unit.Controller == this);
        bool isSuccessful = !(Support.SupportCard == null || battlingUnit.HasSameUnitNameWith(Support.SupportCard));
        Game.TryDoMessage(new ConfirmSupportMessage()
        {
            Unit = battlingUnit,
            SupportCard = Support.SupportCard,
            IsSuccessful = isSuccessful
        });
    }

    public void StartTurn()
    {
        Game.TryDoMessage(new StartTurnMessage()
        {
            TurnPlayer = this
        });
    }

    public void GoToBondPhase()
    {
        Game.TryDoMessage(new GoToBondPhaseMessage()
        {
            TurnPlayer = this
        });
    }

    public void GoToDeploymentPhase()
    {
        Game.TryDoMessage(new GoToDeploymentPhaseMessage()
        {
            TurnPlayer = this
        });
    }

    public void GoToActionPhase()
    {
        Game.TryDoMessage(new GoToActionPhaseMessage()
        {
            TurnPlayer = this
        });
    }

    public void EndTurn()
    {
        Game.TryDoMessage(new EndTurnMessage()
        {
            TurnPlayer = this
        });
    }

    public void ClearStatusEndingTurn()
    {
        Game.TryDoMessage(new ClearStatusEndingTurnMessage()
        {
            TurnPlayer = this
        });
    }

    public void SwitchTurn()
    {
        Game.TryDoMessage(new SwitchTurnMessage()
        {
            NextTurnPlayer = Opponent
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
            Game.TryDoMessage(new ToBondMessage
            {
                Targets = targets,
                TargetFrontShown = frontShown,
                Reason = reason
            });
        }
    }

    public List<Card> GetPossibleCardsToSetToBond(List<Card> targets, bool frontShown = true, Skill reason = null)
    {
        return targets.FindAll(card => card.CheckSetToBond(frontShown, reason));
    }

    public async Task ChooseSetToBond(List<Card> targets, int min, int max, bool frontShown = true, Skill reason = null)
    {
        if (targets.Count > 0)
        {
            SetToBond(await Request.Choose(GetPossibleCardsToSetToBond(targets, frontShown, reason), min, max, this), frontShown, reason);
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
            Game.TryDoMessage(new RefreshUnitMessage()
            {
                Targets = targets,
                Reason = reason
            });
        }
    }

    public void DrawCard(int number, Skill reason = null)
    {
        Game.TryDoMessage(new DrawCardMessage()
        {
            Player = this,
            Number = number,
            Reason = reason
        });
    }

    public List<Card> GetDeployableHands(bool actioned = false, Skill reason = null)
    {
        return Hand.Filter(card => card.CheckDeployment(actioned, reason));
    }

    public List<Card> GetDeployableCards(List<Card> targets, ref List<bool> toFrontField, ref List<bool> actioned, Skill reason = null)
    {
        List<Card> targets_new = new List<Card>();
        List<bool> toFrontField_new = new List<bool>();
        List<bool> actioned_new = new List<bool>();
        foreach (var card in targets)
        {
            int index = targets.IndexOf(card);
            Area area;
            if (toFrontField[index])
            {
                area = card.Controller.FrontField;
            }
            else
            {
                area = card.Controller.BackField;
            }
            if (card.CheckDeployment(area, actioned[index], reason))
            {
                targets_new.Add(card);
                toFrontField_new.Add(toFrontField[index]);
                actioned_new.Add(actioned[index]);
            }
        }
        toFrontField = toFrontField_new;
        actioned = actioned_new;
        return targets_new;
    }

    public void Deploy(Card target, bool toFrontField, bool actioned = false, Skill reason = null)
    {
        Deploy(new List<Card>() { target }, new List<bool> { toFrontField }, new List<bool> { actioned }, reason);
    }

    public void Deploy(List<Card> targets, List<bool> toFrontField, List<bool> actioned, Skill reason = null)
    {
        Game.TryDoMessage(new DeployMessage()
        {
            Targets = targets,
            ToFrontField = toFrontField,
            Actioned = actioned,
            Reason = reason
        });
    }
    public async Task ChooseDeploy(List<Card> targets, int min, int max, List<bool> toFrontField, List<bool> actioned, Skill reason = null)
    {
        if (targets.Count > 0)
        {
            var chosen = await Request.Choose(GetDeployableCards(targets, ref toFrontField, ref actioned, reason), min, max, this);
            Deploy(chosen, ListUtils.UpdateParallel(chosen, targets, toFrontField), ListUtils.UpdateParallel(chosen, targets, actioned));
        }
    }

    public List<Card> GetLevelUpableHands(Skill reason = null)
    {
        return Hand.Filter(card => card.CheckLevelUp(reason));
    }

    public async Task LevelUp(Card target, Skill reason = null)
    {
        List<Card> baseUnits = target.GetLevelUpableUnits(reason);
        Card baseUnit;
        if (baseUnits.Count < 1)
        {
            return;
        }
        else if (baseUnits.Count == 1)
        {
            baseUnit = baseUnits[0];
        }
        else
        {
            baseUnit = await Request.ChooseOne(baseUnits, this);
        }
        Game.TryDoMessage(new LevelUpMessage()
        {
            Target = target,
            BaseUnit = baseUnit,
            Reason = reason
        });
    }

    public List<Card> GetPossibleCardsToUseActionSkill()
    {
        return AllCards.FindAll(card => card.CheckUsingActionSkill());
    }

    public List<ActionSkill> GetUsableActionSkills(Card card)
    {
        return card.GetUsableActionSkills();
    }

    public async Task UseActionSkill(ActionSkill skill)
    {
        await skill.Solve();
    }

    public async Task SolveSupportSkills()
    {
        var supportCard = Support.SupportCard;
        if (supportCard == null)
        {
            return;
        }
        if (!supportCard.CheckUseSupportSkill())
        {
            return;
        }
        //TO DO: 复数支援能力
        await supportCard.GetUsableSupportSkills()[0].Solve(Game.AttackingUnit, Game.DefendingUnit);
    }

    public void AddSupportToPower(Card unit)
    {
        var supportCard = Support.SupportCard;
        if(supportCard!=null)
        {
            unit.Attach(new PowerBuff(null, supportCard.Support, LastingTypeEnum.UntilBattleEnds));
        }
    }

    public async Task<List<Card>> ChooseDiscardedCardsSameNameProcess(List<Card> units, string name)
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
                savedUnit = await Request.ChooseOne(readyForSameNameProcessMessage.Targets, this);
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

    public override User Opponent => Game.Rival;
}

/// <summary>
/// 对手
/// </summary>
public class Rival : User
{
    public Rival() : base() { }

    public override User Opponent => Game.Player;
}