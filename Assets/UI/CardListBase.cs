using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class CardListBase : UIBehaviour {

    public delegate void OnItemClick(CardListItem item);

    [SerializeField]
    protected CardListItem m_Prototype;

    [SerializeField]
    protected float m_Interval = 10F;

    [SerializeField]
    protected bool m_DefaultTap = false;

    protected List<CardListItem> m_ListItems = new List<CardListItem>();
    protected OnItemClick m_OnItemClick;

    public void SetClick(OnItemClick action)
    {
        if (action != null)
        {
            m_OnItemClick = action;
        }
    }

    protected override void Awake()
    {
        Init();
    }

    private bool m_IsInited = false;
    public void Init()
    {
        if (m_IsInited)
        {
            return;
        }
        m_IsInited = true;

        if (m_Prototype != null)
        {
            m_Prototype.gameObject.SetActive(false);
        }
    }

    public void OnListItemClick(CardListItem item)
    {
        if (item != null && m_OnItemClick != null)
        {
            m_OnItemClick.Invoke(item);
        }
    }

    public CardListItem AddListItem()
    {
        if (m_Prototype == null)
        {
            return null;
        }

        var go = Instantiate(m_Prototype.gameObject);
        CardListItem item = go.GetComponent<CardListItem>();

        item.transform.SetParent(transform, false);
        item.gameObject.SetActive(true);
        item.m_List = this;
        m_ListItems.Add(item);

        item.Init();
        Arrange();

        if (m_DefaultTap)
        {
            item.Tap();
        }
        return item;
    }

    protected abstract void Arrange();

    public CardListItem GetItemByIndex(int index)
    {
        if (index >= 0 && index < m_ListItems.Count)
        {
            return m_ListItems[index];
        }
        return null;
    }

    public void RemoveItemByIndex(int index)
    {
        if (index >= 0 && index < m_ListItems.Count)
        {
            CardListItem item = m_ListItems[index];
            m_ListItems.RemoveAt(index);
            Destroy(item.gameObject);
            Arrange();
        }
    }

    public void RemoveItem(CardListItem listItem)
    {
        if (listItem.m_List != this)
        {
            return;
        }

        RemoveItemByIndex(m_ListItems.IndexOf(listItem));
    }

    public void Shuffle()
    {
        int n = m_ListItems.Count;
        for (int i = 0; i < n - 1; i++)
        {
            int randomIdx = Random.Range(i, n);
            if (randomIdx != i)
            {
                CardListItem tmp = m_ListItems[randomIdx];
                m_ListItems[randomIdx] = m_ListItems[i];
                m_ListItems[i] = tmp;
            }
        }
        Arrange();
    }

    public CardListItem SearchCard(Card card)
    {
        foreach (CardListItem listItem in m_ListItems)
        {
            if (listItem.Card == card)
            {
                return listItem;
            }
        }
        return null;
    }
}
