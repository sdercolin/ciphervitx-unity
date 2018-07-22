﻿using System;
using System.Collections.Generic;

public static class ListUtils
{
    public static List<T> Clone<T>(List<T> list)
    {
        if (list == null)
        {
            return null;
        }
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
                json += StringUtils.ToString(item);
            }
            else
            {
                json += ", " + StringUtils.ToString(item);
            }
        }
        return "[" + json + "]";
    }

    public static List<T> FromString<T>(string json)
    {
        throw new NotImplementedException();
    }
}