using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Request
{
    #region Testing
    static Queue<dynamic> NextResults = new Queue<dynamic>();
    static Queue<bool> NextResultsAreIndexes = new Queue<bool>();

    /// <summary>
    /// 设置下一个请求的结果
    /// </summary>
    /// <param name="result">结果，若为空则为默认结果</param>
    /// <param name="isIndex">（仅针对选择）若为true，则对应传入需选择项的Index列表</param>
    public static void SetNextResult(dynamic result = null, bool isIndex = false)
    {
        NextResults.Enqueue(result);
        NextResultsAreIndexes.Enqueue(isIndex);
    }
    public static void ClearNextResults()
    {
        NextResults.Clear();
        NextResultsAreIndexes.Clear();
    }
    public static List<T> GetNextChooseResult<T>(List<T> choices, int min, int max)
    {
        dynamic nextResult = NextResults.Dequeue();
        bool isDefault = nextResult == null;
        bool isIndex = NextResultsAreIndexes.Dequeue();
        if (isDefault)
        {
            return choices.GetRange(0, max);
        }
        if (isIndex)
        {
            var result = new List<T>();
            var indexList = nextResult as List<int>;
            foreach (var index in indexList)
            {
                result.Add(choices[index]);
            }
            return result;
        }
        return nextResult;
    }
    public static bool GetNextAskResult()
    {
        dynamic nextResult = NextResults.Dequeue();
        bool isDefault = nextResult == null;
        NextResultsAreIndexes.Dequeue();
        return isDefault ? false : (bool)nextResult;
    }
    #endregion

    public static async Task<T> ChooseUpToOne<T>(List<T> choices, User targetUser, RequestFlags flags = RequestFlags.Null, string description = "")
    {
        var results = await Choose(choices, 0, 1, targetUser);
        return results.Count == 0 ? default(T) : results[0];
    }

    public static async Task<T> ChooseOne<T>(List<T> choices, User targetUser, RequestFlags flags = RequestFlags.Null, string description = "")
    {
        return (await Choose(choices, 1, 1, targetUser, flags, description))[0];
    }

    public static async Task<List<T>> ChooseUpTo<T>(List<T> choices, int max, User targetUser, RequestFlags flags = RequestFlags.Null, string description = "")
    {
        return await Choose(choices, 0, max, targetUser, flags, description);
    }

    public static async Task<List<T>> Choose<T>(List<T> choices, int number, User targetUser, RequestFlags flags = RequestFlags.Null, string description = "")
    {
        return await Choose(choices, number, number, targetUser, flags, description);
    }

    public static async Task<List<T>> Choose<T>(List<T> choices, User targetUser, RequestFlags flags = RequestFlags.Null, string description = "")
    {
        return await Choose(choices, 0, choices.Count, targetUser, flags, description);
    }

    public static async Task<List<T>> Choose<T>(List<T> choices, int min, int max, User targetUser, RequestFlags flags = RequestFlags.Null, string description = "")
    {
        LogUtils.Log("Requesting Choose: " + Environment.NewLine
            + "choices = " + ListUtils.ToString(choices) + Environment.NewLine
            + "min = " + min + Environment.NewLine
            + "max = " + max);
        max = Math.Min(max, choices.Count);
        min = Math.Min(min, max);
        if (NextResults.Count > 0)
        {
            var result = GetNextChooseResult(choices, min, max);
            LogUtils.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            if (targetUser is Player && Config.GetValue("apply_default_choices") == "true")
            {
                if (min == choices.Count)
                {
                    return choices;
                }
            }
            if (targetUser is Player)
            {
                // request UI
            }
            else
            {
                // request server
                var remoteRequest = new ChooseRemoteRequest<T>
                {
                    Choices = choices,
                    Min = min,
                    Max = max,
                    Flags = flags,
                    Description = description
                };
                var returnedRequest = await Game.MessageManager?.Request(remoteRequest) as ChooseRemoteRequest<T>;
                return returnedRequest?.Response as List<T>;
            }
            return null;
        }
    }

    public static async Task<bool> AskIfUse<T>(T target, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        LogUtils.Log("Requesting AskIfUse: " + Environment.NewLine
            + "target = " + StringUtils.CreateFromAny(target));
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            LogUtils.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            // TO DO
            return false;
        }
    }

    public static async Task<bool> AskIfReverseBond(int number, Skill reason, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        LogUtils.Log("Requesting AskIfReverseBond: " + Environment.NewLine
            + "number = " + StringUtils.CreateFromAny(number) + Environment.NewLine
            + "reason = " + StringUtils.CreateFromAny(reason));
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            LogUtils.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            // TO DO
            return false;
        }
    }

    public static async Task<bool> AskIfCriticalAttack(User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        LogUtils.Log("Requesting AskIfCriticalAttack");
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            LogUtils.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            // TO DO
            return false;
        }
    }

    public static async Task<bool> AskIfAvoid(User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        LogUtils.Log("Requesting AskIfAvoid");
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            LogUtils.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            // TO DO
            return false;
        }
    }

    public static async Task<bool> AskIfSendToRetreat(Card target, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        return await AskIfSendToRetreat(new List<Card>() { target }, targetUser, flags);
    }

    public static async Task<bool> AskIfSendToRetreat(List<Card> targets, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        LogUtils.Log("Requesting AskIfSendToRetreat: " + Environment.NewLine
            + "targets = " + StringUtils.CreateFromAny(targets));
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            LogUtils.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            // TO DO
            return false;
        }
    }

    public static async Task<bool> AskIfDeployToFrontField(Card target, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        LogUtils.Log("Requesting AskIfDeployToFrontField: " + Environment.NewLine
            + "target = " + StringUtils.CreateFromAny(target));
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            LogUtils.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            // TO DO
            return true;
        }
    }

    public static async Task<int> ChooseRPS(User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        // TO DO:
        // 石头：0，剪刀：1，布：2
        return 0;
    }

    public static async Task<bool> AskIfChangeFirstHand(User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        // TO DO
        return false;
    }

    [Flags]
    public enum RequestFlags
    {
        Null = 0,
        DoNotAllowSameName = 1,
        ShowOrder = 2
    }
}

public abstract class RemoteRequest
{
    static readonly int fieldNumber = 7;
    protected dynamic field1;
    protected dynamic field2;
    protected dynamic field3;
    protected dynamic field4;
    protected dynamic field5;
    protected dynamic field6;
    protected dynamic field7;

    public virtual dynamic Response { get; set; }
    public string Guid = System.Guid.NewGuid().ToString();

    public override string ToString()
    {
        string json = "\"type\": \"" + GetType().Name + "\", \"response\": " + StringUtils.CreateFromAny(Response) + ", \"requestId\": \"" + Guid + "\"";
        if (GetType().IsGenericType)
        {
            Type innerType = GetType().GenericTypeArguments[0];
            json += ", \"innerType\": \"" + innerType.Name + "\"";
        }
        for (int i = 0; i < fieldNumber; i++)
        {
            dynamic field = GetType().GetField("field" + (i + 1).ToString(), BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            if (field != null)
            {
                json += ", \"field" + (i + 1).ToString() + "\": " + StringUtils.CreateFromAny(field);
            }
        }
        return "{" + json + "}";
    }

    public static RemoteRequest FromString(string json)
    {
        string[] splited = json.Trim(new char[] { '{', '}' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        string typename = null;
        dynamic response = null;
        string guid = null;
        string innerTypename = null;
        foreach (var item in splited)
        {
            if (item.Contains("\"type\": \""))
            {
                typename = item.Replace("\"type\": \"", "").Trim('\"', ' ');
            }
            if (item.Contains("\"response\": \""))
            {
                response = StringUtils.CreateFromAny(item.Replace("\"response\": \"", "").Trim('\"', ' '));
            }
            if (item.Contains("\"requestId\": \"") )
            {
                guid = StringUtils.CreateFromAny(item.Replace("\"requestId\": \"", "").Trim('\"', ' '));
            }
            if (item.Contains("\"innerType\": \""))
            {
                innerTypename = item.Replace("\"innerType\": \"", "").Trim('\"', ' ');
            }
        }
        if (typename == null)
        {
            return null;
        }
        var requestType = Assembly.GetExecutingAssembly().GetType(typename);
        try
        {
            RemoteRequest newRequest;
            if (requestType.ContainsGenericParameters)
            {
                Type innerType = Assembly.GetExecutingAssembly().GetType(innerTypename);
                while (!innerType.BaseType.Equals(typeof(object)))
                {
                    innerType = innerType.BaseType;
                }
                Type[] typeArgs = { innerType };
                requestType = requestType.MakeGenericType(typeArgs);
                newRequest = Activator.CreateInstance(requestType) as RemoteRequest;
            }
            else
            {
                newRequest = Activator.CreateInstance(requestType) as RemoteRequest;
            }
            newRequest.Response = response;
            newRequest.Guid = guid;
            foreach (var item in splited)
            {
                if (item.Contains("\"type\": \""))
                {
                    continue;
                }
                if (item.Contains("\"response\": \""))
                {
                    continue;
                }
                if (item.Contains("\"requestId\": \""))
                {
                    continue;
                }
                if (item.Contains("\"innerType\": \""))
                {
                    continue;
                }
                var index = item.IndexOf(": ", StringComparison.Ordinal);
                object value = StringUtils.ParseAny(item.Substring(index + 2));
                requestType.GetField(item.Substring(0, index).Trim(new char[] { '\"' }), BindingFlags.NonPublic | BindingFlags.Instance).SetValue(newRequest, value);
            }
            return newRequest;
        }
        catch (Exception e)
        {
            LogUtils.Log(e.ToString());
            return null;
        }
    }

    public abstract Task Do();
}

public class ChooseRemoteRequest<T> : RemoteRequest
{
    public List<T> Choices { get { return field1; } set { field1 = value; } }
    public int Min { get { return field2; } set { field2 = value; } }
    public int Max { get { return field3; } set { field3 = value; } }
    public Request.RequestFlags Flags { get { return field4; } set { field4 = value; } }
    public string Description { get { return field5; } set { field5 = value; } }

    public async override Task Do()
    {
        await Request.Choose(Choices, Min, Max, Game.Player, Flags, Description);
    }
}