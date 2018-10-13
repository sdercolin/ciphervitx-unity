using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIListBase : UIBehaviour
{
    public delegate void OnItemClick(UIListItem item);

    [SerializeField]
    protected UIListItem prototype;

    [SerializeField]
    protected float interval = 10F;

    [SerializeField]
    protected int itemsPerLine = 1; //useless now

    [SerializeField]
    protected bool vertical = true;

    protected List<UIListItem> m_ListItems = new List<UIListItem>();
    protected OnItemClick onItemClick;

    public void SetClick(OnItemClick action)
    {
        onItemClick = action;
    }

    protected override void Awake()
    {
        Init();
    }

    private bool inited = false;
    public void Init()
    {
        if (inited)
        {
            return;
        }
        inited = true;

        if (prototype != null)
        {
            prototype.gameObject.SetActive(false);
        }
    }

    public void OnListItemClick(UIListItem item)
    {
        if (item != null && onItemClick != null)
        {
            onItemClick.Invoke(item);
        }
    }

    protected virtual void Arrange()
    {
        int count = m_ListItems.Count;
        if (count == 0)
        {
            return;
        }

        if (vertical)
        {
            //the center of list area
            float x0 = transform.position.x;
            float y0 = transform.position.y;
            float z0 = transform.position.z;
            //the height of list area
            float height = ((RectTransform)transform).rect.height;
            //the height of list item
            float cardHeight = prototype.Height;

            float realInterval;
            if ((cardHeight + (count - 1) * interval) <= height)
            {
                realInterval = interval;
            }
            else
            {
                realInterval = (height - cardHeight) / (count - 1);
            }
            for (int i = 0; i < count; i++)
            {
                UIListItem item = m_ListItems[i];
                float y = y0 + height / 2 - cardHeight / 2 - i * realInterval;
                item.transform.position = new Vector3(x0, y, z0);
            }
        }
        else
        {
            //the center of list area
            float x0 = transform.position.x;
            float y0 = transform.position.y;
            float z0 = transform.position.z;
            //the width of list area
            float width = ((RectTransform)transform).rect.width;
            //the width of card
            float cardWidth = prototype.Width;

            if ((count * cardWidth + (count - 1) * interval) <= width)
            {
                for (int i = 0; i < count; i++)
                {
                    UIListItem item = m_ListItems[i];
                    float x = x0 - (((count - 1) / 2 - i) * (cardWidth + interval));
                    item.transform.position = new Vector3(x, y0, z0);
                }
            }
            else
            {
                float interval = (count * cardWidth - width) / (count - 1);
                for (int i = 0; i < count; i++)
                {
                    UIListItem item = m_ListItems[i];
                    float x = x0 - width / 2 + cardWidth / 2 + i * (cardWidth - interval);
                    item.transform.position = new Vector3(x, y0, z0);
                }
            }
        }
    }

    public UIListItem AddListItem()
    {
        if (prototype == null)
        {
            return null;
        }

        var go = Instantiate(prototype.gameObject);
        UIListItem item = go.GetComponent<UIListItem>();

        item.transform.SetParent(transform, false);
        item.gameObject.SetActive(true);
        item.List = this;
        m_ListItems.Add(item);

        item.Init();
        Arrange();

        return item;
    }

    public UIListItem GetItemByIndex(int index)
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
            UIListItem item = m_ListItems[index];
            m_ListItems.RemoveAt(index);
            Destroy(item.gameObject);
            Arrange();
        }
    }

    public void RemoveItem(UIListItem listItem)
    {
        if (listItem.List != this)
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
                UIListItem tmp = m_ListItems[randomIdx];
                m_ListItems[randomIdx] = m_ListItems[i];
                m_ListItems[i] = tmp;
            }
        }
        Arrange();
    }

    public void Clear()
    {
        int n = m_ListItems.Count;
        for (int i = n - 1; i >= 0; i--)
        {
            UIListItem item = m_ListItems[i];
            m_ListItems.RemoveAt(i);
            Destroy(item.gameObject);
        }
        Arrange();
    }

    public int IndexOf(UIListItem item)
    {
        return m_ListItems.IndexOf(item);
    }

    public List<int> GetSelectedItemsIndex()
    {
        List<int> ret = new List<int>();
        int n = m_ListItems.Count;
        for (int i = 0; i < n; i++)
        {
            UIListItem item = m_ListItems[i];
            if (item.Selected)
            {
                ret.Add(i);
            }
        }
        return ret;
    }
}
