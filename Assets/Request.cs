<<<<<<< HEAD:Assets/Models/Request.cs
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Request
{
    #region Testing
    private static Queue<dynamic> NextResults = new Queue<dynamic>();
    private static Queue<bool> NextResultsAreIndexes = new Queue<bool>();
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
        else
        {
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
            else
            {
                return nextResult;
            }
        }
    }
    public static bool GetNextAskResult()
    {
        dynamic nextResult = NextResults.Dequeue();
        bool isDefault = nextResult == null;
        bool isIndex = NextResultsAreIndexes.Dequeue();
        if (isDefault)
        {
            return false;
        }
        else
        {
            return nextResult;
        }
    }
    #endregion

    public static async Task<T> ChooseUpToOne<T>(List<T> choices, User targetUser, RequestFlags flags = RequestFlags.Null, string description = "")
    {
        List<T> results = await Choose(choices, 0, 1, targetUser);
        if (results.Count == 0)
        {
            return default(T);
        }
        else
        {
            return results[0];
        }
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
        Debug.Log("Requesting Choose: " + Environment.NewLine
            + "choices = " + ListUtils.ToString(choices) + Environment.NewLine
            + "min = " + min + Environment.NewLine
            + "max = " + max);
        max = Math.Max(max, choices.Count);
        min = Math.Min(min, max);
        if (NextResults.Count > 0)
        {
            var result = GetNextChooseResult<T>(choices, min, max);
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            return await UIMainController.GetUIMainController().RequestChoose<T>(choices, min, max, description);
            // TO DO
            //return null;
        }
    }

    public static async Task<bool> AskIfUse<T>(T target, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        Debug.Log("Requesting AskIfUse: " + Environment.NewLine
            + "target = " + StringUtils.CreateFromAny(target));
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            return await UIMainController.GetUIMainController().RequestChooseBool(StringUtils.CreateFromAny(target));
            // TO DO
            //return false;
        }
    }

    public static async Task<bool> AskIfReverseBond(int number, Skill reason, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        Debug.Log("Requesting AskIfReverseBond: " + Environment.NewLine
            + "number = " + StringUtils.CreateFromAny(number) + Environment.NewLine
            + "reason = " + StringUtils.CreateFromAny(reason));
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            return await UIMainController.GetUIMainController().RequestChooseBool(StringUtils.CreateFromAny(reason));
            // TO DO
            //return false;
        }
    }

    public static async Task<bool> AskIfCriticalAttack(User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        Debug.Log("Requesting AskIfCriticalAttack");
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            return await UIMainController.GetUIMainController().RequestChooseBool("Critical Attack");
            // TO DO
            //return false;
        }
    }

    public static async Task<bool> AskIfAvoid(User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        Debug.Log("Requesting AskIfAvoid");
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            return await UIMainController.GetUIMainController().RequestChooseBool("Avoid");
            // TO DO
            //return false;
        }
    }

    public static async Task<bool> AskIfSendToRetreat(Card target, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        return await AskIfSendToRetreat(new List<Card>() { target }, targetUser,flags);
    }

    public static async Task<bool> AskIfSendToRetreat(List<Card> targets, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        Debug.Log("Requesting AskIfReverseBond: " + Environment.NewLine
            + "targets = " + StringUtils.CreateFromAny(targets));
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            return await UIMainController.GetUIMainController().RequestChooseBool(StringUtils.CreateFromAny(targets));
            // TO DO
            //return false;
        }
    }

    public static async Task<bool> AskIfDeployToFrontField(Card target, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        Debug.Log("Requesting AskIfDeployToFrontField: " + Environment.NewLine
            + "target = " + StringUtils.CreateFromAny(target));
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            return await UIMainController.GetUIMainController().RequestChooseBool(StringUtils.CreateFromAny(target));
            // TO DO
            //return true;
        }
    }
    
    public static async Task<int> ChooseRPS(User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        // TO DO:
        // 石头：0，剪刀：1，布：2
        //return 0;
        List<int> rps = new List<int>()
        {
            0,
            1,
            2
        };
        return (await UIMainController.GetUIMainController().RequestChoose<int>(rps, 1, 1, "Rogue Paladin or Shaman"))[0];
    }

    public static async Task<bool> AskIfChangeFirstHand(User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        // TO DO
        //return false;
        return await UIMainController.GetUIMainController().RequestChooseBool("Mulligan");
    }

    [Flags]
    public enum RequestFlags
    {
        Null = 0,
        DoNotAllowSameName = 1,
        ShowOrder = 2
    }
}

=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Request
{
    #region Testing
    private static Queue<dynamic> NextResults = new Queue<dynamic>();
    private static Queue<bool> NextResultsAreIndexes = new Queue<bool>();
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
        else
        {
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
            else
            {
                return nextResult;
            }
        }
    }
    public static bool GetNextAskResult()
    {
        dynamic nextResult = NextResults.Dequeue();
        bool isDefault = nextResult == null;
        NextResultsAreIndexes.Dequeue();
        if (isDefault)
        {
            return false;
        }
        else
        {
            return nextResult;
        }
    }
    #endregion

    public static async Task<T> ChooseUpToOne<T>(List<T> choices, User targetUser, RequestFlags flags = RequestFlags.Null, string description = "")
    {
        List<T> results = await Choose(choices, 0, 1, targetUser);
        if (results.Count == 0)
        {
            return default(T);
        }
        else
        {
            return results[0];
        }
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
        Debug.Log("Requesting Choose: " + Environment.NewLine
            + "choices = " + ListUtils.ToString(choices) + Environment.NewLine
            + "min = " + min + Environment.NewLine
            + "max = " + max);
        max = Math.Min(max, choices.Count);
        min = Math.Min(min, max);
        if (NextResults.Count > 0)
        {
            var result = GetNextChooseResult<T>(choices, min, max);
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
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
            // TO DO
            return null;
        }
    }

    public static async Task<bool> AskIfUse<T>(T target, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        Debug.Log("Requesting AskIfUse: " + Environment.NewLine
            + "target = " + StringUtils.CreateFromAny(target));
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
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
        Debug.Log("Requesting AskIfReverseBond: " + Environment.NewLine
            + "number = " + StringUtils.CreateFromAny(number) + Environment.NewLine
            + "reason = " + StringUtils.CreateFromAny(reason));
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
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
        Debug.Log("Requesting AskIfCriticalAttack");
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
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
        Debug.Log("Requesting AskIfAvoid");
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
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
        Debug.Log("Requesting AskIfSendToRetreat: " + Environment.NewLine
            + "targets = " + StringUtils.CreateFromAny(targets));
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
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
        Debug.Log("Requesting AskIfDeployToFrontField: " + Environment.NewLine
            + "target = " + StringUtils.CreateFromAny(target));
        if (NextResults.Count > 0)
        {
            var result = GetNextAskResult();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
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

>>>>>>> master:Assets/Request.cs
