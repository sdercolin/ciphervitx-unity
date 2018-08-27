using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Request
{
    public static dynamic NextResult = null; // Only for test

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
        var defaultResult = NextResult;
        if (defaultResult != null)
        {
            NextResult = null;
            return defaultResult;
        }
        else
        {
            // TO DO
            throw new NotImplementedException();
        }
    }

    public static async Task<bool> AskIfUse<T>(T target, User targetUser)
    {
        var defaultResult = NextResult;
        if (defaultResult != null)
        {
            NextResult = null;
            return defaultResult;
        }
        else
        {
            // TO DO
            throw new NotImplementedException();
        }
    }

    public static async Task<bool> AskIfCriticalAttack(User targetUser)
    {
        var defaultResult = NextResult;
        if (defaultResult != null)
        {
            NextResult = null;
            return defaultResult;
        }
        else
        {
            // TO DO
            throw new NotImplementedException();
        }
    }

    public static async Task<bool> AskIfAvoid(User targetUser)
    {
        var defaultResult = NextResult;
        if (defaultResult != null)
        {
            NextResult = null;
            return defaultResult;
        }
        else
        {
            // TO DO
            throw new NotImplementedException();
        }
    }

    public static async Task<bool> AskIfSendToRetreat(List<Card> targets, User targetUser)
    {
        var defaultResult = NextResult;
        if (defaultResult != null)
        {
            NextResult = null;
            return defaultResult;
        }
        else
        {
            // TO DO
            throw new NotImplementedException();
        }
    }
}

