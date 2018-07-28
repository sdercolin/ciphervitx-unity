using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class StringUtils
{
    public static string CreateFromAny(dynamic item)
    {
        if (item == null)
        {
            return "\"null\"";
        }
        else if (item is int)
        {
            return item.ToString();
        }
        else if (item is bool)
        {
            return BooleanUtils.ToString(item);
        }
        else if (item is System.Collections.IList)
        {
            return ListUtils.ToString(item);
        }
        else if (item is Card || item is Area || item is User || item is IAttachable)
        {
            return item.ToString();
        }
        else
        {
            // is Enum or string
            return "\"" + item.ToString() + "\"";
        }
    }

    public static object ParseAny(string json)
    {
        if (json == null || json == "" || json == "\"null\"")
        {
            return null;
        }
        if (int.TryParse(json, out int integer))
        {
            // is integer
            return integer;
        }
        else if (BooleanUtils.TryParse(json, out bool boolean))
        {
            // is boolean
            return boolean;
        }
        else if (json.Length > 2 && json.First() == '"' && json.Last() == '"')
        {
            if (EnumUtils.TryParse(json, out Type enumType, out object enumValue))
            {
                // is enum
                return enumValue;
            }
            else
            {
                // is string
                return json.Substring(1, json.Length - 2);
            }
        }
        else if (json.Length > 2 && json.First() == '[' && json.Last() == ']')
        {
            // is list
            return ListUtils.FromString(json);
        }
        else if (json.Length > 2 && json.First() == '{' && json.Last() == '}')
        {
            // is object: User, Area, Card, Skill, Buff
            string[] splited = json.Trim(new char[] { '{', '}' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            string guid = null;
            foreach (var item in splited)
            {
                if (item.Contains("\"guid\": \""))
                {
                    guid = item.Replace("\"guid\": \"", "").Trim('\"');
                    break;
                }
            }
            if (guid == null)
            {
                return null;
            }
            object gameObject = Game.GetObject(guid);
            if (gameObject != null)
            {
                return gameObject;
            }
            // TO DO: is new object (Buff or SubSkill)
        }
        return null;
    }
}

