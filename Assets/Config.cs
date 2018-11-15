using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using System.Linq;

public static class Config
{
    const string path = @"Assets/config.xml";
    static readonly Dictionary<string, string> config = new Dictionary<string, string>();

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetValue(string key)
    {
        string strReturn = null;
        if (config.ContainsKey(key))
        {
            strReturn = config[key];
        }
        return strReturn;
    }

    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetValue(string key, string value)
    {
        if (config.ContainsKey(key))
        {
            config[key] = value;
        }
        else
        {
            config.Add(key, value);
        }
        SaveUpdates();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="key"></param>
    public static void DelValue(string key)
    {
        if (config.ContainsKey(key))
        {
            config.Remove(key);
            SaveUpdates();
        }
    }

    public static void Load()
    {
        config.Clear();
        var doc = XDocument.Load(path);
        foreach (var el in doc.Root.Elements())
        {
            config.Add(el.Attributes().Single(attr => attr.Name == "key").Value, el.Attributes().Single(attr => attr.Name == "value").Value);
        }
    }

    static void SaveUpdates()
    {
        var doc = XDocument.Parse("<?xml version=\"1.0\" encoding=\"utf-8\"?><root></root>");
        doc.Root.Add(config.Select(kv =>
        {
            var el = new XElement("config");
            el.SetAttributeValue("key", kv.Key);
            el.SetAttributeValue("value", kv.Value);
            return el;
        }));
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        doc.Save(path, SaveOptions.None);
    }
}
