using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardListBond : CardListBase
{

    protected override void Arrange()
    {
        int count = m_ListItems.Count;
        if (count == 0)
        {
            return;
        }

        //the center of list area
        float x0 = transform.position.x;
        float y0 = transform.position.y;
        float z0 = transform.position.z;
        //the width of list area
        float width = ((RectTransform)transform).rect.width;
        //the width of card
        float cardHeight = m_Prototype.height;

        float interval;
        if ((cardHeight + (count - 1) * m_Interval) <= width)
        {
            interval = m_Interval;
        }
        else
        {
            interval = (width - cardHeight) / (count - 1);
        }

        for (int i = 0; i < count; i++)
        {
            CardListItem item = m_ListItems[i];
            float x = x0 - width / 2 + cardHeight / 2 + i * interval;
            item.transform.position = new Vector3(x, y0, z0);
        }
    }
}
