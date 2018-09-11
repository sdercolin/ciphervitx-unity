using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleCardPicture : UIBehaviour
{
    protected Image m_Image;

    protected bool m_IsInited = false;
    public virtual void Init()
    {
        if (m_IsInited)
        {
            return;
        }
        m_IsInited = true;

        m_Image = transform.Find("Card").GetComponent<Image>();
    }

    public void SetCardImage(string index)
    {
        Sprite sprite = ResourceManager.GetSprite(index);
        m_Image.sprite = sprite;
    }

    public void ShowCard(bool bShow)
    {
        m_Image.gameObject.SetActive(bShow);
    }
}
