using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardListV : CardListBase {

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
        //the height of list area
        float height = ((RectTransform)transform).rect.height;
        //the height of card
        float cardHeight = m_Prototype.height;

        float interval;
        if ((cardHeight + (count - 1) * m_Interval) <= height)
        {
            interval = m_Interval;
        }
        else
        {
            interval = (height - cardHeight) / (count - 1);
        }
        for (int i = 0; i < count; i++)
        {
            CardListItem item = m_ListItems[i];
            float y = y0 + height / 2 - cardHeight / 2 - i * interval;
            item.transform.position = new Vector3(x0, y, z0);
        }
    }
}
