using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIListItem : UIBehaviour, IPointerClickHandler
{
    public UIListBase List;

    private bool inited = false;
    public virtual void Init()
    {
        if (inited)
        {
            return;
        }
        inited = true;

        if (isToggle)
        {
            if (select != null)
            {
                select.gameObject.SetActive(false);
            }
            if (unselect != null)
            {
                unselect.gameObject.SetActive(true);
            }
        }
    }

    public float Width { get { return ((RectTransform)transform).rect.width; } }
    public float Height { get { return ((RectTransform)transform).rect.height; } }
    public int Index { get { return List.IndexOf(this); } }

    [SerializeField]
    public bool isToggle = false;

    [SerializeField]
    private Image select;

    [SerializeField]
    private Image unselect;

    public bool Selected { get; private set; } = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        UILogger.LogYellow("click item of index: " + Index.ToString());
        if (isToggle)
        {
            Selected = !Selected;
            if (select != null)
            {
                select.gameObject.SetActive(Selected);
            }
            if (unselect != null)
            {
                unselect.gameObject.SetActive(!Selected);
            }
        }

        List.OnListItemClick(this);
    }
}
