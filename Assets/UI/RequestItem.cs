using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RequestItem : UIListItem
{
    [SerializeField]
    private Text textDesc;

    public void SetText(string text)
    {
        textDesc.text = text;
    }
}
