using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILogger
{
    private static readonly bool enabled = true;

    public static void LogError(string content)
    {
        if (enabled)
        {
            Debug.LogError(content);
        }
    }
}