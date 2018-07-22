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

    public static bool FromString(string json)
    {
        if(json== "\"true\"")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

