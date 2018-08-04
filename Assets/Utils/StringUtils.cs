using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        int integer;
        bool boolean;
        Type enumType;
        object enumValue;
        if (int.TryParse(json, out integer))
        {
            // is integer
            return integer;
        }
        else if (BooleanUtils.TryParse(json, out boolean))
        {
            // is boolean
            return boolean;
        }
        else if (json.Length > 2 && json.First() == '"' && json.Last() == '"')
        {
            if (EnumUtils.TryParse(json, out enumType, out enumValue))
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
            // is new object (Buff or SubSkill)
            gameObject = ParseToCreate(json);
            return gameObject;
        }
        return null;
    }

    public static object ParseToCreate(string json)
    {
        string[] splited = json.Trim(new char[] { '{', '}' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        string typename = null;
        foreach (var item in splited)
        {
            if (item.Contains("\"type\": \""))
            {
                typename = item.Replace("\"type\": \"", "").Trim('\"');
                break;
            }
        }
        if (typename == null)
        {
            return null;
        }
        Type type = Assembly.GetExecutingAssembly().GetType(typename);
        List<object> parameters = new List<object>();
        List<string> parameterNames = new List<string>();
        Dictionary<string, dynamic> otherFields = new Dictionary<string, dynamic>();
        var constructorInfo = type.GetConstructors()[0];
        foreach (var param in constructorInfo.GetParameters())
        {
            parameterNames.Add(param.Name);
            parameters.Add(null);
        }

        foreach (var item in splited)
        {
            if (item.Contains("\"type\": \""))
            {
                continue;
            }
            string[] splited2 = item.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
            string name = splited2[0].Trim(new char[] { '\"' });
            object value = StringUtils.ParseAny(splited2[1]);
            if (parameterNames.Contains(name))
            {
                parameters[parameterNames.IndexOf(name)] = value;
            }
            else
            {
                otherFields.Add(name, value);
            }
        }
        var newObject = Activator.CreateInstance(type, parameters);
        foreach (var pair in otherFields)
        {
            bool isSet = false;
            while (type != typeof(object))
            {
                var field = type.GetField(pair.Key, BindingFlags.Instance);
                if (field == null)
                {
                    field = type.GetField(pair.Key, BindingFlags.NonPublic | BindingFlags.Instance);
                }
                if (field != null)
                {
                    field.SetValue(newObject, pair.Value);
                    isSet = true;

                }
                type = type.BaseType;
            }
            if (!isSet)
            {
                throw new DeserializeFailureException(json, pair.Key, "Field \"" + pair.Key + "\"is not found when deserializing Json: " + Environment.NewLine + json);
            }
        }
        return newObject;
    }
}

public class DeserializeFailureException : Exception
{
    public string FailedJson;
    public string FailedFieldName;

    public DeserializeFailureException(string failedJson, string failedFieldName, string message) : base(message)
    {
        FailedJson = failedJson;
        FailedFieldName = failedFieldName;
    }
}