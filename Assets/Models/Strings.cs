using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading.Tasks;

public static class Strings
{
    public static string CurrentLanguage = "";
    const string path = @"Resouces/Strings/Strings{0}.xml";
    private static readonly Dictionary<string, string> stringsDict = new Dictionary<string, string>();

    public static void Load(string language)
    {
        string complete_path = (language == "") ? String.Format(path, "") : String.Format(path, "_" + language);
        XmlDocument xml = new XmlDocument();
        xml.Load(complete_path);
        stringsDict.Clear();
        foreach (var node in xml.DocumentElement.ChildNodes)
        {
            var element = node as XmlElement;
            if (element != null)
            {
                stringsDict.Add(element.GetAttribute("key"), element.GetAttribute("value"));
            }
        }
        CurrentLanguage = language;
    }

    public static string Get(string key, params string[] parameters)
    {
        return String.Format(stringsDict[key], parameters);
    }
}
