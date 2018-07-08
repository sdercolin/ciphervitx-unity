using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class DictionaryUtils<T1, T2>
{
    public static Dictionary<T1, T2> Clone(Dictionary<T1, T2> dictionary)
    {
        Dictionary<T1, T2> clone = new Dictionary<T1, T2>();
        foreach (var key in dictionary.Keys)
        {
            clone.Add(key, dictionary[key]);
        }
        return clone;
    }
}