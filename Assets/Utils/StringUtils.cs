using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class StringExtensions
{
    public static string[] SplitOnce(this string content, string separator)
    {
        var index = content.IndexOf(separator, StringComparison.Ordinal);
        var key = content.Substring(0, index);
        var value = content.Substring(index + separator.Length);
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
                    if (start < 0)
                    {
                        continue;
                    }
                    var layer = 1;
                    var end = start + 1;
                    while (layer > 0)
                    {
                        var nextPos = content.IndexOfAny(new char[] { wrapperPair[0], wrapperPair[1] }, end);
                        if (nextPos < 0)
                        {
                            break;
                        }
                        if (content[nextPos] == wrapperPair[0])
                        {
                            layer++;
                        }
                        else
                        {
                            layer--;
                        }
                        end = nextPos;
                    }
                    if (layer == 0)
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
                var key = "@#" + (count++).ToString();
                wrappedDict.Add(key, sub);
                content = content.Substring(0, (int)firstStart) + key + content.Substring((int)lastEnd + 1);
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

    public static string UnWrap(this string content)
    {
        content = content.Trim(' ');
        return content.Substring(1, content.Length - 2);
    }

    public static string ToUpperCapital(this string content)
    {
        if (content.Length > 0)
        {
            content = content.Substring(0, 1).ToUpper() + content.Substring(1);
        }
        return content;
    }
}