using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ResourceManager
{
    private static Dictionary<string, Sprite> s_Sprites = new Dictionary<string, Sprite>();
    public static Sprite GetSprite(string index)
    {
        Sprite ret;
        if (!s_Sprites.TryGetValue(index, out ret))
        {
            ret = Resources.Load(index, typeof(Sprite)) as Sprite;
            s_Sprites.Add(index, ret);
        }
        return ret;
    }

    public static void ClearSpriteCache()
    {
        s_Sprites.Clear();
    }

    private static Sprite s_CardBack;
    public static Sprite CardBack
    {
        get
        {
            if (s_CardBack == null)
            {
                s_CardBack = GetSprite("back");
            }
            return s_CardBack;
        }
    }
}
