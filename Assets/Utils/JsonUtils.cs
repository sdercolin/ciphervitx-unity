using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class JsonUtils
{
    public static JsonSerializerSettings settings = new JsonSerializerSettings();

    static JsonUtils()
    {
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
        settings.PreserveReferencesHandling = PreserveReferencesHandling.All;
        settings.TypeNameHandling = TypeNameHandling.All;
        settings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
    }

    public static string ToJson(object any)
    {
        return JsonConvert.SerializeObject(any, settings);
    }

    public static object FromJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, settings);
    }
}