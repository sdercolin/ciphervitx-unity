using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

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

    public bool DeckLoaded = false;

    /// <summary>
    /// 本回合中已经出击（升级）过的费用数
    /// </summary>
    public int DeployAndCCCostCount = 0;

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
    public async Task<bool> SetDeck(string filename, bool usePresetHero)
    {
        int heroIndex = -1;
        string[] lines = File.ReadAllLines(filename);
        string errorText = String.Empty;
        List<Card> cardlistTemp = new List<Card>();
        Dictionary<string, int> SameNameCountDictionary = new Dictionary<string, int>();

        foreach (string serial in lines)
        {
            int serialInt;
            bool success = false;
            if (int.TryParse(serial.Trim('*', '+'), out serialInt))
            {
                Card newcard = CardFactory.CreateCard(serialInt, this);
                if (newcard != null)
                {
                    cardlistTemp.Add(newcard);
                    if (newcard.SkillList.FindAll(skill => skill is AllowOverFourInDeck).Count == 0)
                    {
                        if (SameNameCountDictionary.ContainsKey(newcard.CardName))
                        {
                            SameNameCountDictionary[newcard.CardName]++;
                        }
                        else
                        {
                            SameNameCountDictionary.Add(newcard.CardName, 1);
                        }
                    }
                    success = true;
                }
                if (serial.Contains("*") && heroIndex == -1)
                {
                    heroIndex = cardlistTemp.IndexOf(newcard);
                }
            }
            if (!success)
            {
                errorText += "> " + Strings.Get("load_deck_card_not_found", serial) + Environment.NewLine;
            }
        }

        if (cardlistTemp.Count < 50)
        {
            errorText += "> " + Strings.Get("load_deck_total_not_enough") + Environment.NewLine;
        }

        foreach (var cardname in SameNameCountDictionary.Keys)
        {
            if (SameNameCountDictionary[cardname] > 4)
            {
                errorText += "> " + Strings.Get("load_deck_same_card_over_limit", cardname) + Environment.NewLine;
            }
        }

        if (usePresetHero && heroIndex >= 0)
        {
            Card hero = cardlistTemp[heroIndex];
            if (hero.DeployCost != 1)
            {
                errorText += "> " + Strings.Get("load_deck_hero_preset_illegal", hero.CardName) + Environment.NewLine;
            }
            else
            {
                hero.IsHero = true;
            }
        }
        if (errorText == String.Empty)
        {
            if (cardlistTemp.FindAll(card => card.IsHero).Count == 0)
            {
                (await Request.ChooseOne(cardlistTemp.FindAll(card => card.DeployCost == 1), this, Request.RequestFlags.Null, Strings.Get("load_deck_request_choose_hero")))
                    .IsHero = true;
            }
            Dictionary<string, int> cardDict = new Dictionary<string, int>();
            foreach (var card in cardlistTemp)
            {
                cardDict.Add(card.Guid, int.Parse(card.Serial.TrimStart('0')));
            }
            Game.DoMessage(new SetDeckMessage()
            {
                User = this,
                CardDict = cardDict,
                HeroGuid = cardlistTemp.Find(card => card.IsHero).Guid
            });
            return true;
        }
        else
        {
            //提示错误
            return false;
        }
    }

    public void SetHeroUnit()
    {
        Game.DoMessage(new SetHeroUnitMessage()
        {
            User = this
        });
    }

    public void SetFirstHand()
    {
        Game.DoMessage(new SetFirstHandMessage()
        {
            User = this
        });
    }

    public void PutBackFirstHand()
    {
        Game.DoMessage(new PutBackFirstHandMessage()
        {
            User = this
        });
    }

    public void SetFirstOrbs()
    {
        Game.DoMessage(new SetFirstOrbsMessage()
        {
            User = this
        });
    }

    public void Move(Card target, Skill reason)
    {
        if (target != null)
        {
            List<Card> targets = new List<Card> { target };
            Move(targets, reason);
        }
    }

    public void ShuffleDeck(Skill reason)
    {
        var order = Deck.GetShuffledOrder();
        Game.TryDoMessage(new ShuffleDeckMessage()
        {
            User = this,
            Order = order,
            Reason = reason
        });
    }

    public void Move(List<Card> targets, Skill reason)
    {
        if (targets.Count > 0)
        {
            Game.TryDoMessage(new MoveMessage()
            {
                Targets = targets,
                Reason = reason
            });
        }
    }

    public async Task ChooseMove(List<Card> targets, int min, int max, Skill reason = null)
    {
        Move(await Request.Choose(targets, min, max, this), reason);
    }

    public void ReverseBond(Card target, Skill reason, bool asCost = true)
    {
        if (target != null)
        {
            List<Card> targets = new List<Card> { target };
            ReverseBond(targets, reason, asCost);
        }
    }

    public void ReverseBond(List<Card> targets, Skill reason, bool asCost = true)
    {
        if (targets.Count > 0)
        {
            Game.TryDoMessage(new ReverseBondMessage()
            {
                Targets = targets,
                Reason = reason,
                AsCost = asCost
            });
        }
    }

    public List<Card> GetReversableBonds(Skill reason = null)
    {
        return Bond.UnusedBonds.FindAll(card => card.CheckReverseBond(reason));
    }

    public async Task ChooseReverseBond(List<Card> targets, int min, int max, Skill reason, bool asCost = true)
    {
        ReverseBond(await Request.Choose(targets, min, max, this), reason, asCost);
    }


    public void Attack(Card unit, Card target)
    {
        if (unit != null && target != null)
        {
            Game.TryDoMessage(new AttackMessage()
            {
                AttackingUnit = unit,
                DefendingUnit = target
            });
        }
    }

    public void SetSupportCard()
    {
        if (Deck.Count > 0)
        {
            Game.TryDoMessage(new SetSupportMessage()
            {
                User = this
            });
        }
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

    public void RemoveSupportCard()
    {
        if (Support.SupportCard != null)
        {
            Game.TryDoMessage(new RemoveSupportMessage()
            {
                Targets = new List<Card>() { Support.SupportCard },
                Reason = null,
                AsCost = false
            });
        }
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
        SetToBond(await Request.Choose(GetPossibleCardsToSetToBond(targets, frontShown, reason), min, max, this), frontShown, reason);
    }

    public void DiscardHand(Card target, bool asCost, Skill reason = null)
    {
        if (target != null)
        {
            List<Card> targets = new List<Card> { target };
            DiscardHand(targets, asCost, reason);
        }
    }

    public void DiscardHand(List<Card> targets, bool asCost, Skill reason = null)
    {
        if (targets.Count > 0)
        {
            Game.TryDoMessage(new DiscardHandMessage
            {
                Targets = targets,
                AsCost = asCost,
                Reason = reason
            });
        }
    }

    public async Task ChooseDiscardHand(List<Card> targets, int min, int max, bool asCost, Skill reason = null)
    {
        DiscardHand(await Request.Choose(targets, min, max, this), asCost, reason);
    }

    public void AddToOrb(Card target, Skill reason = null)
    {
        if (target != null)
        {
            Game.TryDoMessage(new AddToOrbMessage
            {
                Target = target,
                Reason = reason
            });
        }
    }

    public async Task ChooseAddToOrb(List<Card> targets, bool optional, Skill reason = null)
    {
        if (optional)
        {
            var result = await Request.ChooseUpToOne(targets, this);
            if (result != null)
            {
                AddToOrb(result, reason);
            }
        }
        else
        {
            AddToOrb(await Request.ChooseOne(targets, this), reason);
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

    public void AttachItem(IAttachable item, Card target)
    {
        if (item == null || target == null)
        {
            return;
        }
        var powerBuff = item as PowerBuff;
        if (powerBuff != null)
        {
            if (powerBuff.Value == 0)
            {
                return;
            }
        }
        var supportBuff = item as SupportBuff;
        if (supportBuff != null)
        {
            if (supportBuff.Value == 0)
            {
                return;
            }
        }
        Game.TryDoMessage(new AttachItemMessage()
        {
            Item = item,
            Target = target
        });
    }

    public void GrantSkill(IAttachable item, Card target)
    {
        if (item != null && target != null)
        {
            Game.TryDoMessage(new GrantSkillMessage()
            {
                Item = item,
                Target = target
            });
        }
    }

    public List<Card> GetDeployableHands(bool actioned = false, Skill reason = null)
    {
        return Hand.Filter(card => card.CheckDeployment(actioned, reason));
    }

    public List<Card> GetDeployableCards(List<Card> targets, ref Dictionary<Card, bool> toFrontFieldDict, ref Dictionary<Card, bool> actionedDict, Skill reason = null)
    {
        var targets_new = ListUtils.Clone(targets);
        foreach (var card in targets)
        {
            Area area = null;
            if (toFrontFieldDict.ContainsKey(card))
            {
                if (toFrontFieldDict[card])
                {
                    area = card.Controller.FrontField;
                }
                else
                {
                    area = card.Controller.BackField;
                }
            }
            bool actioned = actionedDict.ContainsKey(card) ? actionedDict[card] : false;
            bool canDeploy;
            if (area == null)
            {
                canDeploy = card.CheckDeployment(card.Controller.FrontField, actioned, reason)
                    && card.CheckDeployment(card.Controller.BackField, actioned, reason);
            }
            else
            {
                canDeploy = card.CheckDeployment(area, actioned, reason);
            }
            if (!canDeploy)
            {
                targets_new.Remove(card);
                toFrontFieldDict.Remove(card);
                actionedDict.Remove(card);
            }
        }
        return targets_new;
    }

    /// <summary>
    /// 执行出击动作
    /// </summary>
    public void Deploy(Card target, bool toFrontField, bool actioned = false, Skill reason = null)
    {
        Deploy(new List<Card>() { target }, new List<bool> { toFrontField }, new List<bool> { actioned }, reason);
    }

    /// <summary>
    /// 执行出击动作
    /// </summary>
    public void Deploy(List<Card> targets, List<bool> toFrontFieldList, List<bool> actionedList, Skill reason = null)
    {
        if (targets.Count > 0)
        {
            Game.TryDoMessage(new DeployMessage()
            {
                Targets = targets,
                ToFrontFieldList = toFrontFieldList,
                ActionedList = actionedList,
                Reason = reason
            });
        }
    }

    /// <summary>
    /// 出击（询问出击位置）
    /// </summary>
    public async Task DeployChooseArea(Card target, Dictionary<Card, bool> actionedDict = null, Skill reason = null)
    {
        await ChooseDeploy(new List<Card>() { target }, 1, 1, null, actionedDict, reason);
    }

    /// <summary>
    /// 出击（询问出击位置）
    /// </summary>
    public async Task DeployChooseArea(List<Card> targets, Dictionary<Card, bool> actionedDict = null, Skill reason = null)
    {
        await ChooseDeploy(targets, targets.Count, targets.Count, null, actionedDict, reason);
    }

    /// <summary>
    /// 选择出击
    /// </summary>
    /// <param name="choices">选择项</param>
    /// <param name="min">最少选择数量</param>
    /// <param name="max">最多选择数量</param>
    /// <param name="toFrontFieldDict">对选择项的出击位置的规定</param>
    /// <param name="actionedDict">对选择项的出击后的行动状态的规定</param>
    /// <param name="reason">引发本次出击的能力</param>
    /// <returns></returns>
    public async Task ChooseDeploy(List<Card> choices, int min, int max, Dictionary<Card, bool> toFrontFieldDict = null, Dictionary<Card, bool> actionedDict = null, Skill reason = null)
    {
        if (toFrontFieldDict == null)
        {
            toFrontFieldDict = new Dictionary<Card, bool>();
        }
        if (actionedDict == null)
        {
            actionedDict = new Dictionary<Card, bool>();
        }
        List<Card> chosen = await Request.Choose(GetDeployableCards(choices, ref toFrontFieldDict, ref actionedDict, reason), min, max, this);
        var toFrontFieldList = new List<bool>();
        var actionedList = new List<bool>();
        foreach (var card in chosen)
        {
            if (toFrontFieldDict.ContainsKey(card))
            {
                toFrontFieldList.Add(toFrontFieldDict[card]);
            }
            else
            {
                toFrontFieldList.Add(await Request.AskIfDeployToFrontField(card, this));
            }
            if (actionedDict.ContainsKey(card))
            {
                actionedList.Add(actionedDict[card]);
            }
            else
            {
                actionedList.Add(false);
            }
        }
        Deploy(chosen, toFrontFieldList, actionedList, reason);
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
        if (target != null && baseUnit != null)
        {
            Game.TryDoMessage(new LevelUpMessage()
            {
                Target = target,
                BaseUnit = baseUnit,
                Reason = reason
            });
        }
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
        if (supportCard != null)
        {
            AttachItem(new PowerBuff(null, supportCard.Support, LastingTypeEnum.UntilBattleEnds), unit);
        }
    }

    public async Task<bool> CriticalAttack()
    {
        if (await Request.AskIfCriticalAttack(this))
        {
            if (Game.AttackingUnit.CheckCriticalAttack())
            {
                List<Card> costs = Game.AttackingUnit.GetCostsForCriticalAttack();
                Card cost = await Request.ChooseUpToOne(costs, this);
                if (cost != null)
                {
                    Game.TryDoMessage(new CriticalAttackMessage()
                    {
                        AttackingUnit = Game.AttackingUnit,
                        DefendingUnit = Game.DefendingUnit,
                        Cost = cost
                    });
                    return Game.CriticalFlag;
                }
            }
        }
        return false;
    }

    public async Task<bool> Avoid()
    {
        if (await Request.AskIfAvoid(this))
        {
            if (Game.DefendingUnit.CheckAvoid())
            {
                List<Card> costs = Game.DefendingUnit.GetCostsForAvoid();
                Card cost = await Request.ChooseUpToOne(costs, this);
                if (cost != null)
                {
                    Game.TryDoMessage(new AvoidMessage()
                    {
                        AttackingUnit = Game.AttackingUnit,
                        DefendingUnit = Game.DefendingUnit,
                        Cost = cost
                    });
                    return Game.AvoidFlag;
                }
            }
        }
        return false;
    }

    public void EndBattle()
    {
        Game.TryDoMessage(new EndBattleMessage()
        {
            AttackingUnit = Game.AttackingUnit,
            DefendingUnit = Game.DefendingUnit
        });
    }

    public void ClearStatusEndingBattle()
    {
        Game.TryDoMessage(new ClearStatusEndingBattleMessage()
        {
            AttackingUnit = Game.AttackingUnit,
            DefendingUnit = Game.DefendingUnit
        });
    }

    public void SetActioned(List<Card> targets, Skill reason, bool asCost = true)
    {
        if (targets.Count > 0)
        {
            Game.TryDoMessage(new SetActionedMessage()
            {
                Targets = targets,
                Reason = reason,
                AsCost = asCost
            });
        }
    }

    public async Task ChooseSetActioned(List<Card> targets, int min, int max, Skill reason, bool asCost = true)
    {
        SetActioned(await Request.Choose(targets, min, max, this), reason, asCost);
    }

    public void Destroy(Card target, Skill reason, bool asCost)
    {
        if (target != null)
        {
            Destroy(new List<Card>() { target }, reason, asCost);
        }
    }

    public void Destroy(List<Card> targets, Skill reason, bool asCost)
    {
        if (targets.Count > 0)
        {
            Game.TryDoMessage(new DestroyMessage()
            {
                DestroyedUnits = targets,
                Count = 1,
                ReasonTag = asCost ? DestructionReasonTag.BySkillCost : DestructionReasonTag.BySkill,
                Reason = reason,
                AttackingUnit = null
            });
        }
    }

    public async Task ChooseDestroy(List<Card> targets, int min, int max, Skill reason, bool asCost)
    {
        Destroy(await Request.Choose(targets, min, max, this), reason, asCost);
    }

    public void AddToHand(List<Card> targets, Skill reason, bool show = true)
    {
        if (targets.Count > 0)
        {
            Game.TryDoMessage(new AddToHandMessage()
            {
                Targets = targets,
                Reason = reason,
                Show = show
            });
        }
    }

    public async Task ChooseAddToHand(List<Card> targets, int min, int max, Skill reason, bool show = true, Request.RequestFlags flags = Request.RequestFlags.Null)
    {
        AddToHand(await Request.Choose(targets, min, max, this, flags), reason, show);
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

    public void ShowCard(Card target, Skill reason)
    {
        if (target != null)
        {
            List<Card> targets = new List<Card> { target };
            ShowCard(targets, reason);
        }
    }

    public void ShowCard(List<Card> targets, Skill reason)
    {
        if (targets.Count > 0)
        {
            Game.TryDoMessage(new ShowCardsMessage()
            {
                Targets = targets,
                Reason = reason
            });
        }
    }

    public void SendToRetreat(Card target, Skill reason, bool asCost = false)
    {
        if (target != null)
        {
            SendToRetreat(new List<Card>() { target }, reason, asCost);
        }
    }

    public void SendToRetreat(List<Card> targets, Skill reason, bool asCost = false)
    {
        if (targets.Count > 0)
        {
            Game.TryDoMessage(new SendToRetreatMessage()
            {
                Targets = targets,
                Reason = reason,
                AsCost = asCost
            });
        }
    }

    public void ChangeDefendingUnit(Card toUnit, Skill reason)
    {
        if (toUnit != null)
        {
            Game.TryDoMessage(new ChangeDefendingUnitMessage()
            {
                FromUnit = Game.DefendingUnit,
                ToUnit = toUnit,
                Reason = reason
            });
        }
    }

    public async Task ChooseSetToDeckTop(List<Card> targets, int min, int max, Skill reason, Request.RequestFlags flags = Request.RequestFlags.Null)
    {
        if (targets.Count > 0)
        {
            Game.TryDoMessage(new SetToDeckTopMessage()
            {
                Targets = await Request.Choose(targets, min, max, this, flags),
                Reason = reason
            });
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

    public bool Synchronized = false;
}