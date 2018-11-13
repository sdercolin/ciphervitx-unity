using System;
using UnityEngine;

public static class LogUtils
{
    static Logger logger = new UnityLogger();
    public static void Log(string content)
    {
        logger.Log(content);
    }
    public static void SetLogger(Logger newLogger)
    {
        logger = newLogger;
    }
}

public abstract class Logger
{
    public abstract void Log(string content);
}

public class UnityLogger : Logger
{
    public override void Log(string content)
    {
        Debug.Log(content);
    }
}