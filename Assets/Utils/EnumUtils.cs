using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

public static class EnumUtils
{
    public static string ToString<T>(T item)
    {
        if (typeof(T).IsEnum)
        {
            return "\"" + typeof(T).Name + "#" + item.ToString() + "\"";
        }
        else
        {
            return null;
        }
    }

    public static bool TryParse(string json, out Type type, out object value)
    {
        try
        {
            string[] splited = json.Trim(new char[] { '"' }).SplitOnce("#");
            type = Assembly.GetExecutingAssembly().GetType(splited.First());
            if (type != null)
            {
                value = Enum.ToObject(type, splited.Last());
                return true;
            }
        }
        catch { }
        type = null;
        value = null;
        return false;
    }
}

