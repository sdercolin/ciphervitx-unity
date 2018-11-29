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

#if DEBUG
    public static LogLevel Level = LogUtils.LogLevel.Debug;
#else 
    public static LogLevel Level = LogUtils.LogLevel.Error;
#endif

    public enum LogLevel
    {
        Trace = 0,
        Debug = 1,
        Error = 2,
        Fatal = 3,
        Disabled = 4
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