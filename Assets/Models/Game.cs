﻿using System;
using System.Collections.Generic;

public class Game
{
    public Game()
    {
        Player = new Player(this);
        Rival = new Rival(this);
    }

    public Player Player;
    public Rival Rival;

    //诱发效果处理用
    public List<string> SkillUsedInThisGame = new List<string>();
    public List<List<Skill>> InducedSkillSetList = new List<List<Skill>>();
    public List<Skill> InducedSkillSet = new List<Skill>();
    public int InducedSkillProcessCount = 0;

    //回合信息
    public bool IsMyTurn;
    public int TurnCount = 0;
    public int DeploymentCount = 0;
    public Phase CurrentPhase;

    //战斗用
    public Card AttackingUnit = null; //攻击单位
    public Card DefencingUnit = null; //防御单位
    public List<Card> BattlingUnits { get { return new List<Card> { AttackingUnit, DefencingUnit }; } } //战斗单位
    public int PowerUpByCritical = 0; //必杀攻击增加的战斗力
    public int PowerUpBySupport = 0; //支援增加的战斗力

    //等待延时用
    public bool DoNotWait = false;
    public bool WaitingFlag = false;
    public int WaitingTime = 3000;

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
        Player.ForEachCard(action);
        Rival.ForEachCard(action);
    }

    public bool TrueForAllCard(Predicate<Card> predicate)
    {
        return Player.TrueForAllCard(predicate) && Rival.TrueForAllCard(predicate);
    }

    /// <summary>
    /// 搜索卡片
    /// </summary>
    /// <param name="guid">卡的id</param>
    /// <returns>符合条件的卡</returns>
    public Card GetCardByGuid(string guid)
    {
        foreach (Card card in AllCards)
        {
            if (card.Guid == guid)
            {
                return card;
            }
        }
        return null;
    }
}

public enum Phase
{
    BeginningPhase,
    BondPhase,
    DeploymentPhase,
    ActionPhase,
    EndPhase
}
