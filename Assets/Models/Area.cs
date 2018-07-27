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
        return "{\"Guid\": \"" + Guid + "\" }";
    }

    /// <summary>
    /// 该区域的卡片列表
    /// </summary>
    protected List<Card> list;

    /// <summary>
    /// 该区域的卡片列表的浅表拷贝
    /// </summary>
    public List<Card> Cards
    {
        get
        {
            return ListUtils.Clone(list);
        }
    }

    /// <summary>
    /// 是否包含某张卡
    /// </summary>
    /// <param name="card">卡</param>
    /// <returns>是否包含某张卡</returns>
    public bool Contains(Card card)
    {
        return list.Contains(card);
    }

    public int Count
    {
        get
        {
            return list.Count;
        }
    }

    /// <summary>
    /// 控制者
    /// </summary>
    public User Controller;

    public virtual void ProcessCardIn(Card card, Area fromArea)
    {
        card.AttachableList.ForEach(item =>
        {
            if(!item.AvailableAreas.Contains(this))
            {
                item.Detach();
            }
        });
    }
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
    /// 切洗该区域的卡
    /// </summary>
    public void Shuffle()
    {
        int N = list.Count;
        int[] array = new int[N];
        for (int i = 0; i < N; i++)
        {
            array[i] = i;
        }
        Random rnd = new Random();
        for (int j = 0; j < N; j++)
        {
            int pos = rnd.Next(j, N);
            int temp = array[pos];
            array[pos] = array[j];
            array[j] = temp;
        }
        List<Card> CardList_temp = new List<Card>();
        for (int i = 0; i < N; i++)
        {
            CardList_temp.Add(list[array[i]]);
        }
        list = CardList_temp;
    }

    /// <summary>
    /// 查找具备某个单位名的卡
    /// </summary>
    /// <param name="UnitName">单位名</param>
    /// <returns>具备该单位名的卡的列表</returns>
    public List<Card> SearchCard(string UnitName)
    {
        List<Card> result = new List<Card>();
        list.ForEach(card =>
        {
            if (card.HasUnitNameOf(UnitName))
            {
                result.Add(card);
            }
        });
        return result;
    }

    public virtual void ForEachCard(Action<Card> action)
    {
        list.ForEach(action);
    }

    public virtual bool TrueForAllCard(Predicate<Card> predicate)
    {
        return list.TrueForAll(predicate);
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

    public void ImportCard(Card card)
    {
        list.Add(card);
    }

    public override void ProcessCardIn(Card card, Area fromArea)
    {
        card.FrontShown = false;
        card.IsHorizontal = false;
        card.Visible = false;
        base.ProcessCardIn(card, fromArea);
    }

    public override void ProcessCardOut(Card card, Area toArea)
    {
        if (list.Count == 0)
        {
            //割り込み処理：补充卡组
        }
        base.ProcessCardOut(card, toArea);
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
        card.FrontShown = true;
        card.IsHorizontal = false;
        card.Visible = false;
        base.ProcessCardIn(card, fromArea);
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
        card.FrontShown = true;
        card.IsHorizontal = false;
        card.Visible = true;
        base.ProcessCardIn(card, fromArea);
    }
}

/// <summary>
/// 支援区
/// </summary>
public class Support : Area
{
    public Support(User Controller) : base(Controller)
    {
        list = new List<Card>();
        this.Controller = Controller;
    }

    public override void ProcessCardIn(Card card, Area fromArea)
    {
        card.FrontShown = true;
        card.IsHorizontal = false;
        card.Visible = true;
        base.ProcessCardIn(card, fromArea);
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
    public int UnusedBondsCount
    {
        get
        {
            int count = 0;
            foreach (Card bond in list)
            {
                if (bond.FrontShown)
                {
                    count++;
                }
            }
            return count;
        }
    }

    /// <summary>
    /// 未翻面的羁绊卡列表
    /// </summary>
    public List<Card> UnusedBonds
    {
        get
        {
            List<Card> result = new List<Card>();
            foreach (Card bond in base.list)
            {
                if (bond.FrontShown)
                {
                    result.Add(bond);
                }
            }
            return result;
        }
    }

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
        card.FrontShown = true;
        card.IsHorizontal = true;
        card.Visible = true;
        base.ProcessCardIn(card, fromArea);
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
        card.FrontShown = false;
        card.IsHorizontal = false;
        card.Visible = false;
        base.ProcessCardIn(card, fromArea);
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

    public override void ForEachCard(Action<Card> action)
    {
        Controller.FrontField.ForEachCard(action);
        Controller.BackField.ForEachCard(action);
    }

    public override bool TrueForAllCard(Predicate<Card> predicate)
    {
        return Controller.FrontField.TrueForAllCard(predicate) && Controller.BackField.TrueForAllCard(predicate);
    }

    public bool HasSameNameCardWith(Card card)
    {
        return !TrueForAllCard(x => !x.HasSameUnitNameWith(card));
    }

    public override void ProcessCardIn(Card card, Area fromArea) { }
    public override void ProcessCardOut(Card card, Area toArea) { }
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
            card.IsHorizontal = false;
        }
        card.FrontShown = true;
        card.Visible = true;
        base.ProcessCardIn(card, fromArea);
    }

    public override void ProcessCardOut(Card card, Area toArea)
    {
        if (!(toArea is BackField))
        {
            //离场
        }
        base.ProcessCardOut(card, toArea);
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
            card.IsHorizontal = false;
        }
        card.FrontShown = true;
        card.Visible = true;
        base.ProcessCardIn(card, fromArea);
    }

    public override void ProcessCardOut(Card card, Area toArea)
    {
        if (!(toArea is FrontField))
        {
            //离场
        }
        base.ProcessCardOut(card, toArea);
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
        card.FrontShown = true;
        card.IsHorizontal = false;
        card.Visible = true;
        base.ProcessCardIn(card, fromArea);
    }
}