using System.Collections.Generic;

public static class ListUtils<T>
{
    public static List<T> Clone(List<T> list)
    {
        List<T> clone = new List<T>();
        clone.AddRange(list);
        return clone;
    }
}
