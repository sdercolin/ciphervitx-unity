using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleCardPictureWithNum : SingleCardPicture
{
    private Text m_Text;

    public override void Init()
    {
        if (m_IsInited)
        {
            return;
        }
        base.Init();

        m_Text = transform.Find("Number").GetComponent<Text>();
    }

    public void SetText(string text)
    {
        m_Text.text = text;
    }
}
