using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class Request<T>
{
    public static T ChooseUpToOne(List<T> choices)
    {
        List<T> results = Choose(choices, 0, 1);
        if (results.Count == 0)
        {
            return default(T);
        }
        else
        {
            return results[0];
        }
    }

    public static T ChooseOne(List<T> choices)
    {
        return Choose(choices, 1, 1)[0];
    }

    public static List<T> ChooseUpTo(List<T> choices, int max)
    {
        return Choose(choices, 0, max);
    }

    public static List<T> Choose(List<T> choices, int number)
    {
        return Choose(choices, number, number);
    }

    public static List<T> Choose(List<T> choices, int min, int max)
    {
        throw new NotImplementedException();
    }
    
    public static bool AskIfUse(T target)
    {
        throw new NotImplementedException();
    }
}

