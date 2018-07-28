using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class BooleanUtils
{
    public static string ToString(bool item)
    {
        if (item)
        {
            return "\"true\"";
        }
        else
        {
            return "\"false\"";
        }
    }
    public static bool? FromString(string json)
    {
        if (json == "\"true\"")
        {
            return true;
        }
        else if (json == "\"false\"")
        {
            return false;
        }
        else
        {
            return null;
        }
    }

    public static bool TryParse(string json, out bool value)
    {
        if (json == "\"true\"")
        {
            value = true;
            return true;
        }
        else if (json == "\"false\"")
        {
            value = false;
            return true;
        }
        else
        {
            value = false;
            return false;
        }
    }
}

