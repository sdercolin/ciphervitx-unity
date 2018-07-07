using System;
using System.Collections.Generic;

public class Game
{
    static Game ThisGame;
    public Game(Player Player, Rival Rival)
    {
        this.Player = Player;
        this.Rival = Rival;
    }

    public Player Player;
    public Rival Rival;
    public bool IsMyTurn;
    public int TurnCount = 0;
    public Phase CurrentPhase;

    //诱发效果处理用
    public List<string> SkillUsedInThisGame = new List<string>();
    public List<List<Skill>> InducedSkillSetList = new List<List<Skill>>();
    public List<Skill> InducedSkillSet = new List<Skill>();
    public int InducedSkillProcessCount = 0;

    //战斗用
    public List<Card> UnitsInBattle = new List<Card>(); // 0为发起攻击的单位，1为被攻击的单位
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
}

public enum Phase
{
    BeginningPhase,
    BondPhase,
    DeploymentPhase,
    ActionPhase,
    EndPhase
}
