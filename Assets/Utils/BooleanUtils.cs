using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class BooleanUtils
{
    public static string ToString(bool item)
    {
        return item ? "\"true\"" : "\"false\"";
    }
    public static bool? FromString(string json)
    {
        if (json == "\"true\"")
        {
            return true;
        }
        if (json == "\"false\"")
        {
            return false;
        }
        return null;
    }

    public static bool TryParse(string json, out bool value)
    {
        if (json == "\"true\"")
        {
            value = true;
            return true;
        }
        if (json == "\"false\"")
        {
            value = false;
            return true;
        }
        value = false;
        return false;
    }
}

