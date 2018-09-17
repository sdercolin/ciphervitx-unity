using System;
using System.Collections.Generic;

/// <summary>
/// 所有区域的基类
/// </summary>
public abstract class Area
{
    public Area(User Controller)
    {
        list = new List<Card>();
        this.Controller = Controller;
        Guid = System.Guid.NewGuid().ToString();
    }

    public string Guid;
    public override string ToString()
    {
        return "{\"guid\": \"" + Guid + "\" }";
    }

    /// <summary>
    /// 该区域的卡片列表
    /// </summary>
    protected List<Card> list;

    /// <summary>
    /// 该区域的卡片列表的浅表拷贝
    /// </summary>
    public virtual List<Card> Cards => ListUtils.Clone(list);

    /// <summary>
    /// 是否包含某张卡
    /// </summary>
    /// <param name="card">卡</param>
    /// <returns>是否包含某张卡</returns>
    public bool Contains(Card card)
    {
        return Cards.Contains(card);
    }

    public int Count => Cards.Count;

    /// <summary>
    /// 控制者
    /// </summary>
    public User Controller;

    public virtual void ProcessCardIn(Card card, Area fromArea) { }
    public virtual void ProcessCardOut(Card card, Area toArea) { }

    /// <summary>
    /// 查找卡的位置
    /// </summary>
    /// <param name="card">要查找的卡</param>
    /// <returns>该卡的索引，未找到则为-1</returns>
    public int IndexOf(Card card)
    {
        return list.IndexOf(card);
    }

    /// <summary>
    /// 加入卡
    /// </summary>
    /// <param name="card">要加入的卡</param>
    public void AddCard(Card card)
    {
        AddCard(card, Count);
    }

    /// <summary>
    /// 在特定卡前加入卡
    /// </summary>
    /// <param name="card">要加入的卡</param>
    /// <param name="pos">参考的卡</param>
    public void AddCard(Card card, Card referenceCard)
    {
        if (Contains(referenceCard))
        {
            AddCard(card, list.IndexOf(referenceCard));
        }
    }

    /// <summary>
    /// 在特定位置加入卡
    /// </summary>
    /// <param name="card">要加入的卡</param>
    /// <param name="pos">位置</param>
    public void AddCard(Card card, int pos)
    {
        list.Insert(pos, card);
        ProcessCardIn(card, card.BelongedRegion);
    }

    /// <summary>
    /// 移除卡
    /// </summary>
    /// <param name="card">要移除的卡</param>
    public void RemoveCard(Card card)
    {
        ProcessCardOut(card, card.BelongedRegion);
        list.Remove(card);
    }

    /// <summary>
    /// 获取一个打乱的排序
    /// </summary>
    public List<int> GetShuffledOrder()
    {
        int N = list.Count;
        List<int> newOrder = new List<int>();
        for (int i = 0; i < N; i++)
        {
            newOrder.Add(i);
        }
        Random rnd = new Random();
        for (int j = 0; j < N; j++)
        {
            int pos = rnd.Next(j, N);
            int temp = newOrder[pos];
            newOrder[pos] = newOrder[j];
            newOrder[j] = temp;
        }
        return newOrder;
    }

    public void ApplyOrder(List<int> order)
    {
        var newList = new List<Card>();
        foreach (var number in order)
        {
            newList.Add(list[number]);
        }
        list = newList;
    }

    /// <summary>
    /// 查找具备某个单位名的卡
    /// </summary>
    /// <param name="UnitName">单位名</param>
    /// <returns>具备该单位名的卡的列表</returns>
    public List<Card> SearchCard(string UnitName)
    {
        List<Card> result = new List<Card>();
        Cards.ForEach(card =>
        {
            if (card.HasUnitNameOf(UnitName))
            {
                result.Add(card);
            }
        });
        return result;
    }

    public void ForEachCard(Action<Card> action)
    {
        Cards.ForEach(action);
    }

    public bool TrueForAllCard(Predicate<Card> predicate)
    {
        return Cards.TrueForAll(predicate);
    }

    public List<Card> Filter(Predicate<Card> match)
    {
        return Cards.FindAll(match);
    }
}

/// <summary>
/// 卡组
/// </summary>
public class Deck : Area
{
    public Deck(User Controller) : base(Controller)
    {
        list = new List<Card>();
        this.Controller = Controller;
    }

    public Card Top => list[0];

    public List<Card> GetTopCards(int number)
    {
        return list.GetRange(0, number);
    }

    public void ImportCard(Card card)
    {
        list.Add(card);
    }

    public override void ProcessCardIn(Card card, Area fromArea)
    {
        card.Reset();
        card.FrontShown = false;
        card.Visible = false;
    }

    public override void ProcessCardOut(Card card, Area toArea)
    {
        if (list.Count == 0)
        {
            Controller.Retreat.ForEachCard(retreatCard => retreatCard.MoveTo(this));
            Controller.ShuffleDeck(null);
        }
    }
}

/// <summary>
/// 手牌
/// </summary>
public class Hand : Area
{
    public Hand(User Controller) : base(Controller)
    {
        list = new List<Card>();
        this.Controller = Controller;
    }

    public override void ProcessCardIn(Card card, Area fromArea)
    {
        card.Reset();
        card.Visible = false;
    }
}

/// <summary>
/// 退避区
/// </summary>
public class Retreat : Area
{
    public Retreat(User Controller) : base(Controller)
    {
        list = new List<Card>();
        this.Controller = Controller;
    }

    public override void ProcessCardIn(Card card, Area fromArea)
    {
        card.Reset();
    }
}

/// <summary>
/// 支援区
/// </summary>
public class Support : Area
{
    public Card SupportCard => Count > 0 ? list[0] : null;

    public Support(User Controller) : base(Controller)
    {
        list = new List<Card>();
        this.Controller = Controller;
    }

    public bool SupportedBy(string unitName)
    {
        if (SupportCard == null)
        {
            return false;
        }
        else
        {
            return SupportCard.HasUnitNameOf(unitName);
        }
    }

    public override void ProcessCardIn(Card card, Area fromArea)
    {
        card.Reset();
    }
}

/// <summary>
/// 羁绊区
/// </summary>
public class Bond : Area
{
    public Bond(User Controller) : base(Controller)
    {
        list = new List<Card>();
        this.Controller = Controller;
    }

    /// <summary>
    /// 未翻面的羁绊卡数量
    /// </summary>
    public int UnusedBondsCount => UnusedBonds.Count;

    /// <summary>
    /// 未翻面的羁绊卡列表
    /// </summary>
    public List<Card> UnusedBonds => list.FindAll(bond => bond.FrontShown);

    /// <summary>
    /// 是否包含具备某势力的卡
    /// </summary>
    /// <param name="symbol">要查找的势力</param>
    /// <returns></returns>
    public bool HasSymbolOf(SymbolEnum symbol)
    {
        return !list.TrueForAll(bondcard =>
        {
            return !(bondcard.FrontShown && bondcard.HasSymbol(symbol));
        });
    }

    public override void ProcessCardIn(Card card, Area fromArea)
    {
        card.Reset();
        card.IsHorizontal = true;
    }
}

/// <summary>
/// 宝玉区
/// </summary>
public class Orb : Area
{
    public Orb(User Controller) : base(Controller)
    {
        list = new List<Card>();
        this.Controller = Controller;
    }

    public override void ProcessCardIn(Card card, Area fromArea)
    {
        card.Reset();
        card.FrontShown = false;
        card.Visible = false;
    }
}

/// <summary>
/// 战场（不维护卡的列表）
/// </summary>
public class Field : Area
{
    public Field(User Controller) : base(Controller)
    {
        this.Controller = Controller;
    }

    public override List<Card> Cards => ListUtils.Combine(Controller.BackField.Cards, Controller.FrontField.Cards);

    public bool HasSameNameCardWith(Card card)
    {
        return !TrueForAllCard(x => !x.HasSameUnitNameWith(card));
    }
}

/// <summary>
/// 前卫区
/// </summary>
public class FrontField : Area
{
    public FrontField(User Controller) : base(Controller)
    {
        list = new List<Card>();
        this.Controller = Controller;
    }

    public override void ProcessCardIn(Card card, Area fromArea)
    {
        if (!(fromArea is BackField))
        {
            card.Reset();
        }
    }
}

/// <summary>
/// 后卫区
/// </summary>
public class BackField : Area
{
    public BackField(User Controller) : base(Controller)
    {
        list = new List<Card>();
        this.Controller = Controller;
    }

    public override void ProcessCardIn(Card card, Area fromArea)
    {
        if (!(fromArea is FrontField))
        {
            card.Reset();
        }
    }
}

/// <summary>
/// 叠放区（被叠放在单位下方的卡均位于这个区域）
/// </summary>
public class Overlay : Area
{
    public Overlay(User Controller) : base(Controller)
    {
        list = new List<Card>();
        this.Controller = Controller;
    }

    public override void ProcessCardIn(Card card, Area fromArea)
    {
        card.Reset();
    }
}