﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class DictionaryUtils
{
    public static Dictionary<T1, T2> Clone<T1, T2>(Dictionary<T1, T2> dictionary)
    {
        if (dictionary == null)
        {
            return null;
        }
        Dictionary<T1, T2> clone = new Dictionary<T1, T2>();
        foreach (var key in dictionary.Keys)
        {
            clone.Add(key, dictionary[key]);
        }
        return clone;
    }

    public static string ToString<T1, T2>(Dictionary<T1, T2> dictionary)
    {
        throw new NotImplementedException();
    }

    public static Dictionary<T1, T2> FromString<T1, T2>(string json)
    {
        throw new NotImplementedException();
    }
}