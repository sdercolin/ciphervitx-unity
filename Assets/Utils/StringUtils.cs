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
            string[] splited = json.Trim(new char[] { '{', '}' }).SplitProtectingWrappers(", ", StringSplitOptions.RemoveEmptyEntries, "[]", "{}", "<>");
            string guid = null;
            foreach (var item in splited)
            {
                if (item.Contains("\"guid\": \""))
                {
                    guid = item.Replace("\"guid\": \"", "").Trim('\"', ' ');
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
        string[] splited = json.Trim(new char[] { '{', '}' }).SplitProtectingWrappers(", ", StringSplitOptions.RemoveEmptyEntries, "[]", "{}", "<>");
        string typename = null;
        foreach (var item in splited)
        {
            if (item.Contains("\"type\": \""))
            {
                typename = item.Replace("\"type\": \"", "").Trim('\"', ' ');
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
            if (item.Contains("\"type\": \""))
            {
                continue;
            }
            string[] splited2 = item.SplitOnce(": ");
            string name = splited2[0].Trim(new char[] { '\"', ' ' });
            object value = ParseAny(splited2[1]);
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

public static class StringExtensions
{
    public static string[] SplitOnce(this string content, string separator)
    {
        var index = content.IndexOf(separator, StringComparison.Ordinal);
        var key = content.Substring(0, index);
        var value = content.Substring(index+ separator.Length);
        return new string[] { key, value };
    }

    public static string[] SplitProtectingWrappers(this string content, string separator, StringSplitOptions stringSplitOptions, params string[] wrapperPairs)
    {
        var wrappedDict = new Dictionary<string, string>();
        int count = 1;
        if (wrapperPairs.Length > 0)
        {
            while (true)
            {
                int? firstStart = null;
                int? lastEnd = null;
                foreach (var wrapperPair in wrapperPairs)
                {
                    if (wrapperPair.Length != 2)
                    {
                        continue;
                    }
                    var start = content.IndexOf(wrapperPair[0]);
                    var end = content.LastIndexOf(wrapperPair[1]);
                    if (start >= 0 && end >= 0)
                    {
                        if (firstStart == null || (start < firstStart && end > lastEnd))
                        {
                            firstStart = start;
                            lastEnd = end;
                        }
                    }
                }
                if (firstStart == null || lastEnd == null)
                {
                    break;
                }
                var sub = content.Substring((int)firstStart, (int)lastEnd - (int)firstStart + 1);
                wrappedDict.Add("@#" + count.ToString(), sub);
                content = content.Substring(0, (int)firstStart) + content.Substring((int)lastEnd + 1);
                count++;
            }
        }
        var splited = content.Split(new string[] { separator }, stringSplitOptions);
        var result = new List<string>();
        foreach (var part in splited)
        {
            var partResult = part;
            foreach (var wrapped in wrappedDict)
            {
                if (part.Contains(wrapped.Key))
                {
                    partResult = partResult.Replace(wrapped.Key, wrapped.Value);
                }
            }
            result.Add(partResult);
        }
        return result.ToArray();
    }
}