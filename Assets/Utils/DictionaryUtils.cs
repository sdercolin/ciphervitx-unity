using System;
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
        string json = String.Empty;
        foreach (var pair in dictionary)
        {
            if (!String.IsNullOrEmpty(json))
            {
                json += ", ";
            }
            json += StringUtils.CreateFromAny(pair.Key) + ": " + StringUtils.CreateFromAny(pair.Value);
        }
        return "<" + json + ">";
    }

    public static Dictionary<object, object> FromString(string json)
    {
        var result = new Dictionary<object, object>();
        string[] splited = json.Trim(new char[] { '<', '>' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in splited)
        {
            var key = item.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[0];
            var value = item.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[1];
            result.Add(StringUtils.ParseAny(key), StringUtils.ParseAny(value));
        }
        return result;
    }
}