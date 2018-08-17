using System;
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

    public static List<T> Combine<T>(List<T> listA, List<T> listB)
    {
        List<T> combined = new List<T>();
        combined.AddRange(listA);
        combined.AddRange(listB);
        return combined;
    }

    public static List<T2> UpdateParallel<T1,T2>(List<T1> newKey, List<T1> oldKey, List<T2> oldValue)
    {
        List<T2> newValue = new List<T2>();
        foreach (var item in newKey)
        {
            int index = oldKey.IndexOf(item);
            if(index>0)
            {
                newValue.Add(oldValue[index]);
            }
            else
            {
                throw new Exception();
            }
        }
        return newValue;
    } 

    public static string ToString<T>(List<T> list)
    {
        string json = String.Empty;
        foreach (T item in list)
        {
            if (String.IsNullOrEmpty(json))
            {
                json += StringUtils.CreateFromAny(item);
            }
            else
            {
                json += ", " + StringUtils.CreateFromAny(item);
            }
        }
        return "[" + json + "]";
    }

    public static List<object> FromString(string json)
    {
        var result = new List<object>();
        string[] splited = json.Trim(new char[] { '[', ']' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in splited)
        {
            result.Add(StringUtils.ParseAny(item));
        }
        return result;
    }
}