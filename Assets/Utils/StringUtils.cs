using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class StringUtils
{
    public static string ToString(dynamic item)
    {
        if (item is System.Collections.IList)
        {
            return ListUtils.ToString(item);
        }
        if (item is System.Collections.IDictionary)
        {
            return DictionaryUtils.ToString(item);
        }
        else if (item is bool)
        {
            return BooleanUtils.ToString(item);
        }
        else
        {
            return item.ToString();
        }
    }
}

