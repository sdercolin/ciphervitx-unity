using System.Collections.Generic;

public static class ListUtils
{
    public static List<T> Clone<T>(List<T> list)
    {
        List<T> clone = new List<T>();
        clone.AddRange(list);
        return clone;
    }
}
