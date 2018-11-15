using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class SerializationUtils
{
    public static string SerializeAny(dynamic item)
    {
        if (item == null)
        {
            return "\"null\"";
        }
        if (item is int)
        {
            return item.ToString();
        }
        if (item is bool)
        {
            return ((bool)item).Serialize();
        }
        if (item is System.Collections.IList)
        {
            return ListUtils.Serialize(item);
        }
        if (item is ISerializable)
        {
            return (item as ISerializable).Serialize();
        }
        if (item is System.Collections.IDictionary)
        {
            return DictionaryUtils.Serialize(item);
        }
        if (item.GetType().IsEnum)
        {
            return EnumUtils.Serialize(item);
        }
        // is string
        return "\"" + item + "\"";
    }

    public static object Deserialize(string json)
    {
        if (string.IsNullOrEmpty(json) || json == "\"null\"")
        {
            return null;
        }
        int integer;
        bool boolean;
        Type enumType;
        object enumValue;
        if (int.TryParse(json, out integer))
        {
            return integer;
        }
        if (BooleanUtils.TryParse(json, out boolean))
        {
            return boolean;
        }
        if (json.Length >= 2 && json.First() == '"' && json.Last() == '"')
        {
            if (json == "\"\"")
            {
                return "";
            }
            if (EnumUtils.TryParse(json, out enumType, out enumValue))
            {
                return enumValue;
            }
            return json.Substring(1, json.Length - 2);
        }
        if (json.Length > 2 && json.First() == '[' && json.Last() == ']')
        {
            return ListUtils.Deserialize(json);
        }
        if (json.Length > 2 && json.First() == '<' && json.Last() == '>')
        {
            return DictionaryUtils.Deserialize(json);
        }
        if (json.Length > 2 && json.First() == '{' && json.Last() == '}')
        {
            // is ISerializable: User, Area, Card, Skill, Buff, Induction
            string[] splited = json.UnWrap().SplitProtectingWrappers(", ", StringSplitOptions.RemoveEmptyEntries, "[]", "{}", "<>");
            string guid = null;
            foreach (var item in splited)
            {
                if (item.SplitOnce(": ")[0].UnWrap() == "guid")
                {
                    guid = item.SplitOnce(": ")[1].UnWrap();
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
            // is new object (Buff, SubSkill, Inducion)
            gameObject = DeserializeCreate(json);
            return gameObject;
        }
        return null;
    }

    static ISerializable DeserializeCreate(string json)
    {
        string[] splited = json.UnWrap().SplitProtectingWrappers(", ", StringSplitOptions.RemoveEmptyEntries, "[]", "{}", "<>");
        string typename = null;
        foreach (var item in splited)
        {
            if (item.SplitOnce(": ")[0].UnWrap() == "type")
            {
                typename = item.SplitOnce(": ")[1].UnWrap();
                break;
            }
        }
        if (typename == null)
        {
            return null;
        }
        var type = Assembly.GetExecutingAssembly().GetType(typename);
        var parameters = new List<object>();
        var parameterNames = new List<string>();
        var otherFields = new Dictionary<string, dynamic>();
        var constructorInfo = type.GetConstructors()[0];
        foreach (var param in constructorInfo.GetParameters())
        {
            parameterNames.Add(param.Name);
            parameters.Add(null);
        }

        foreach (var item in splited)
        {
            string[] splited2 = item.SplitOnce(": ");
            string name = splited2[0].UnWrap();
            if (name == "type")
            {
                continue;
            }
            object value = Deserialize(splited2[1]);
            if (parameterNames.Contains(name))
            {
                parameters[parameterNames.IndexOf(name)] = value;
            }
            else
            {
                otherFields.Add(name, value);
            }
        }
        var newObject = Activator.CreateInstance(type, parameters.ToArray());
        foreach (var pair in otherFields)
        {
            var nowType = type;
            bool isSet = false;
            while (nowType != typeof(object))
            {
                var field = nowType.GetField(pair.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (field == null)
                {
                    var property = nowType.GetProperty(pair.Key.ToUpperCapital(), BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (property != null)
                    {
                        property.SetValue(newObject, pair.Value);
                        isSet = true;
                    }
                }
                else
                {
                    field.SetValue(newObject, pair.Value);
                    isSet = true;
                }
                nowType = nowType.BaseType;
            }
            if (!isSet)
            {
                throw new DeserializeFailureException(json, pair.Key, "Field \"" + pair.Key + "\"is not found when deserializing Json: " + Environment.NewLine + json);
            }
        }
        return newObject as ISerializable;
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