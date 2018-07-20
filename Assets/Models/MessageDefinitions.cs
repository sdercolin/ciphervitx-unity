using System;
using System.Collections.Generic;

/// <summary>
/// MessageDefinitions.cs 定义各个Message派生类所包含的属性，且实现序列化，反序列化与复制方法
/// 本文件由程序生成，勿手动修改
/// </summary>

#region 反序列化工厂方法
public partial class Message
{
    public static Message FromString(string json)
    {
        //反序列化
        throw new NotImplementedException();
    }
}
#endregion

#region 派生类定义
public partial class EmptyMessage
{
    //由于没有添加属性，Clone()无需重写
    //由于没有添加属性，ToString()无需重写
}

public partial class DeployMessage : Message
{
    //增加的属性
    public struct MetaData
    {
        public bool ToFrontField;
        public bool Actioned;
    }
    public Dictionary<Card, MetaData> MetaDict = new Dictionary<Card, MetaData>();

    public override Message Clone()
    {
        //由于增加了属性，需要重写实现Clone()
        DeployMessage clone = base.Clone() as DeployMessage; //调用基类的Clone()
        clone.MetaDict = DictionaryUtils.Clone(MetaDict); //补充该派生类所添加的属性
        return clone;
    }

    public override string ToString()
    {
        //每个Message的派生类分别重写实现序列化
        string baseJson = base.ToString(); //获得基本的json字符串
        string json = baseJson.Substring(1, baseJson.Length - 2); //去掉最外层的括号
        // 补充属性
        return "{" + json + ")"; //返回新的json字符串
    }
}
#endregion