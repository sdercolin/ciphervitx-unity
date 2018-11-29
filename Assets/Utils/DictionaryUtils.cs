using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class DictionaryUtils
{
    public static Dictionary<T1, T2> Clone<T1, T2>(this Dictionary<T1, T2> dictionary)
    {
        if (dictionary == null)
        {
            return null;
        }
        var clone = new Dictionary<T1, T2>();
        foreach (var key in dictionary.Keys)
        {
            clone.Add(key, dictionary[key]);
        }
        return clone;
    }

    public static string Serialize<T1, T2>(this Dictionary<T1, T2> dictionary)
    {
        string json = String.Empty;
        foreach (var pair in dictionary)
        {
            if (!String.IsNullOrEmpty(json))
            {
                json += ", ";
            }
            json += SerializationUtils.SerializeAny(pair.Key) + ": " + SerializationUtils.SerializeAny(pair.Value);
        }
        return "<" + json + ">";
    }

    public static dynamic Deserialize(string json)
    {
        string[] splited = json.UnWrap().SplitProtectingWrappers(", ", StringSplitOptions.RemoveEmptyEntries, "[]", "{}", "<>");
        var firstKey = splited[0].SplitOnce(": ")[0];
        var firstValue = splited[0].SplitOnce(": ")[1];
        var keyType = SerializationUtils.Deserialize(firstKey).GetType().GetBaseTypeOverObject();
        var valueType = SerializationUtils.Deserialize(firstValue).GetType().GetBaseTypeOverObject();
        Type[] typeArgs = { keyType, valueType };
        var constructed = typeof(Dictionary<,>).MakeGenericType(typeArgs);

        object result = Activator.CreateInstance(constructed);
        var methodInfo = constructed.GetMethod("Add");
        foreach (var item in splited)
        {
            var key = item.SplitOnce(": ")[0];
            var value = item.SplitOnce(": ")[1];
            object[] parametersArray = { SerializationUtils.Deserialize(key), SerializationUtils.Deserialize(value) };
            methodInfo.Invoke(result, parametersArray);
        }
        return result;
    }
}