using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Request
{
    public static T ChooseUpToOne<T>(List<T> choices, User targetUser)
    {
        List<T> results = Choose(choices, 0, 1, targetUser);
        if (results.Count == 0)
        {
            return default(T);
        }
        else
        {
            return results[0];
        }
    }

    public static T ChooseOne<T>(List<T> choices, User targetUser)
    {
        return Choose(choices, 1, 1, targetUser)[0];
    }

    public static List<T> ChooseUpTo<T>(List<T> choices, int max, User targetUser)
    {
        return Choose(choices, 0, max, targetUser);
    }

    public static List<T> Choose<T>(List<T> choices, int number, User targetUser)
    {
        return Choose(choices, number, number, targetUser);
    }

    public static List<T> Choose<T>(List<T> choices, User targetUser)
    {
        return Choose(choices, 0, choices.Count, targetUser);
    }

    public static List<T> Choose<T>(List<T> choices, int min, int max, User targetUser)
    {
        throw new NotImplementedException();
    }

    public static bool AskIfUse<T>(T target, User targetUser)
    {
        throw new NotImplementedException();
    }
}

