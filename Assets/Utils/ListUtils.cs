using System;
using System.Collections.Generic;

public static class ListUtils
{
    public static List<T> Clone<T>(List<T> list)
    {
        List<T> clone = new List<T>();
        clone.AddRange(list);
        return clone;
    }

    public static string ToString<T>(List<T> list)
    {
        string json = String.Empty;
        foreach (T item in list)
        {
            if (String.IsNullOrEmpty(json))
            {
                json += item.ToString();
            }
            else
            {
                json += String.Format(", {0}", item);
            }
        }
        return "[" + json + "]";
    }

    public static List<T> FromString<T>(string json)
    {
        throw new NotImplementedException();
    }
}