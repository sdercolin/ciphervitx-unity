using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static async Task<T> ChooseUpToOne<T>(List<T> choices, User targetUser)
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

    public static async Task<T> ChooseOne<T>(List<T> choices, User targetUser)
    {
        return (await Choose(choices, 1, 1, targetUser))[0];
    }

    public static async Task<List<T>> ChooseUpTo<T>(List<T> choices, int max, User targetUser)
    {
        return await Choose(choices, 0, max, targetUser);
    }

    public static async Task<List<T>> Choose<T>(List<T> choices, int number, User targetUser)
    {
        return await Choose(choices, number, number, targetUser);
    }

    public static async Task<List<T>> Choose<T>(List<T> choices, User targetUser)
    {
        return await Choose(choices, 0, choices.Count, targetUser);
    }

    public static async Task<List<T>> Choose<T>(List<T> choices, int min, int max, User targetUser)
    {
        if (NextResults.Count > 0)
        {
            return NextResults.Dequeue();
        }
        else
        {
            // TO DO
            return null;
        }
    }

    public static async Task<bool> AskIfUse<T>(T target, User targetUser)
    {
        if (NextResults.Count > 0)
        {
            return NextResults.Dequeue();
        }
        else
        {
            // TO DO
            return false;
        }
    }

    public static async Task<bool> AskIfReverseBond(int number, Skill reason, User targetUser)
    {
        if (NextResults.Count > 0)
        {
            return NextResults.Dequeue();
        }
        else
        {
            // TO DO
            return false;
        }
    }

    public static async Task<bool> AskIfCriticalAttack(User targetUser)
    {
        if (NextResults.Count > 0)
        {
            return NextResults.Dequeue();
        }
        else
        {
            // TO DO
            return false;
        }
    }

    public static async Task<bool> AskIfAvoid(User targetUser)
    {
        if (NextResults.Count > 0)
        {
            return NextResults.Dequeue();
        }
        else
        {
            // TO DO
            return false;
        }
    }

    public static async Task<bool> AskIfSendToRetreat(List<Card> targets, User targetUser)
    {
        if (NextResults.Count > 0)
        {
            return NextResults.Dequeue();
        }
        else
        {
            // TO DO
            return false;
        }
    }
}

