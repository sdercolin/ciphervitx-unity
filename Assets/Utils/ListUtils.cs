using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ListUtils
{
    public static List<T> Clone<T>(List<T> list)
    {
        if (list == null)
        {
            return null;
        }
        var clone = new List<T>();
        clone.AddRange(list);
        return clone;
    }

    public static List<T> Combine<T>(List<T> listA, List<T> listB)
    {
        var combined = new List<T>();
        combined.AddRange(listA);
        combined.AddRange(listB);
        return combined;
    }

    public static List<T2> UpdateParallel<T1, T2>(List<T1> newKey, List<T1> oldKey, List<T2> oldValue)
    {
        var newValue = new List<T2>();
        foreach (var item in newKey)
        {
            int index = oldKey.IndexOf(item);
            if (index > 0)
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
        foreach (var item in list)
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

    public static dynamic FromString(string json)
    {
        string[] splited = json.Trim(new char[] { '[', ']' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        Type type = StringUtils.ParseAny(splited[0]).GetType();
        while (!type.BaseType.Equals(typeof(object)))
        {
            type = type.BaseType;
        }
        Type[] typeArgs = { type };
        Type constructed = typeof(List<>).MakeGenericType(typeArgs);
        object result = Activator.CreateInstance(constructed);
        MethodInfo methodInfo = constructed.GetMethod("Add");
        foreach (var item in splited)
        {
            object[] parametersArray = new object[] { StringUtils.ParseAny(item) };
            methodInfo.Invoke(result, parametersArray);
        }
        return result;
    }
}