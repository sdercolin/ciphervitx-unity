using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardListItem : UIBehaviour,
    IPointerClickHandler{

    [SerializeField]
    public CardListBase m_List;

    [SerializeField]
    private bool m_KeepFrontInfo = true; //if false, the default status of card will be back

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_List != null)
        {
            m_List.OnListItemClick(this);
        }
    }

    public float Width { get { return ((RectTransform)transform).rect.width; } }
    public float Height { get { return ((RectTransform)transform).rect.height; } }

    private Card m_Card;
    public Card Card
    {
        get { return m_Card; }
        set
        {
            m_Card = value;
            SetImage(value.Serial);
        }
    }

    private bool m_IsInited = false;
    public void Init()
    {
        if (m_IsInited)
        {
            return;
        }
        m_IsInited = true;

        m_Img = gameObject.GetComponent<Image>();

        if (!m_KeepFrontInfo)
        {
            m_Backed = true;
            m_Img.sprite = ResourceManager.CardBack;
        }
    }

    #region tap/untap
    private static Quaternion TAPPED = Quaternion.Euler(new Vector3(0, 0, 90));
    private static Quaternion UNTAPPED = Quaternion.Euler(new Vector3(0, 0, 0));

    private bool m_Tapped = false;

    public bool Tap()
    {
        if (m_Tapped)
        {
            return false;
        }
        m_Tapped = true;
        transform.rotation = TAPPED;
        return true;
    }

    public bool Untap()
    {
        if (!m_Tapped)
        {
            return false;
        }
        m_Tapped = false;
        transform.rotation = UNTAPPED;
        return true;
    }
    #endregion


    #region front/back
    private Sprite m_FrontSprite;
    private bool m_Backed = false;
    private Image m_Img;

    public void TurnBack()
    {
        if (m_Backed)
        {
            return;
        }

        if (m_KeepFrontInfo)
        {
            m_FrontSprite = m_Img.sprite;
        }
        m_Img.sprite = ResourceManager.CardBack;
        m_Backed = true;
    }

    public void SetImage(string index)
    {
        m_Img.sprite = ResourceManager.GetSprite(index);
    }

    public void TurnFront(Sprite front = null)
    {
        if (!m_Backed)
        {
            return;
        }

        if (!m_KeepFrontInfo && front == null)
        {
            return;
        }
        else
        {
            m_Img.sprite = front == null ? m_FrontSprite : front;
        }

        m_Backed = false;
    }

    public void TurnFront(string index)
    {
        TurnFront(ResourceManager.GetSprite(index));
    }
    #endregion
}
