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
    public static void SetNextResult(dynamic result)
    {
        NextResults.Enqueue(result);
    }
    public static void ClearNextResults()
    {
        NextResults.Clear();
    }
    #endregion

    public static async Task<T> ChooseUpToOne<T>(List<T> choices, User targetUser, RequestFlags flags = RequestFlags.Null)
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

    public static async Task<T> ChooseOne<T>(List<T> choices, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        return (await Choose(choices, 1, 1, targetUser))[0];
    }

    public static async Task<List<T>> ChooseUpTo<T>(List<T> choices, int max, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        return await Choose(choices, 0, max, targetUser);
    }

    public static async Task<List<T>> Choose<T>(List<T> choices, int number, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        return await Choose(choices, number, number, targetUser);
    }

    public static async Task<List<T>> Choose<T>(List<T> choices, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        return await Choose(choices, 0, choices.Count, targetUser);
    }

    public static async Task<List<T>> Choose<T>(List<T> choices, int min, int max, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        Debug.Log("Requesting Choose: " + Environment.NewLine
            + "choices = " + ListUtils.ToString(choices) + Environment.NewLine
            + "min = " + min + Environment.NewLine
            + "max = " + max);
        if (NextResults.Count > 0)
        {
            var result = NextResults.Dequeue();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
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
            var result = NextResults.Dequeue();
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
            var result = NextResults.Dequeue();
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
            var result = NextResults.Dequeue();
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
            var result = NextResults.Dequeue();
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
        return await AskIfSendToRetreat(new List<Card>() { target }, targetUser);
    }

    public static async Task<bool> AskIfSendToRetreat(List<Card> targets, User targetUser, RequestFlags flags = RequestFlags.Null)
    {
        Debug.Log("Requesting AskIfReverseBond: " + Environment.NewLine
            + "targets = " + StringUtils.CreateFromAny(targets));
        if (NextResults.Count > 0)
        {
            var result = NextResults.Dequeue();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            // TO DO
            return false;
        }
    }

    public static async Task<bool> AskIfDeployToFrontField(Card target)
    {
        Debug.Log("Requesting AskIfDeployToFrontField: " + Environment.NewLine
            + "target = " + StringUtils.CreateFromAny(target));
        if (NextResults.Count > 0)
        {
            var result = NextResults.Dequeue();
            Debug.Log("<<<<" + StringUtils.CreateFromAny(result) + Environment.NewLine);
            return result;
        }
        else
        {
            // TO DO
            return true;
        }
    }

    [Flags]
    public enum RequestFlags
    {
        Null = 0,
        DoNotAllowSameName = 1
    }
}

