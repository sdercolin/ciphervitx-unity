using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

public static class JsonUtils
{
    public static string ToJson(object any)
    {
        return JsonConvert.SerializeObject(any);
    }

    public static object FromJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}
