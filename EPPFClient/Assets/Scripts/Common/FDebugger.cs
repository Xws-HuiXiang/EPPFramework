//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 受AppConst的DevMode属性控制的日志输出封装类
///// </summary>
//public static class FDebugger
//{
//    static FDebugger()
//    {

//    }

//    /// <summary>
//    /// 打印一条日志
//    /// </summary>
//    /// <param name="message"></param>
//    public static void Log(object message)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.Log(message);
//        }
//    }

//    /// <summary>
//    /// 打印一条日志
//    /// </summary>
//    /// <param name="message"></param>
//    /// <param name="context"></param>
//    public static void Log(object message, Object context)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.Log(message, context);
//        }
//    }

//    /// <summary>
//    /// 打印一条日志。以格式化字符串的形式打印
//    /// </summary>
//    /// <param name="format"></param>
//    /// <param name="args"></param>
//    public static void LogFormat(string format, params object[] args)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.LogFormat(format, args);
//        }
//    }

//    /// <summary>
//    /// 打印一条日志。以格式化字符串的形式打印
//    /// </summary>
//    /// <param name="context"></param>
//    /// <param name="format"></param>
//    /// <param name="args"></param>
//    public static void LogFormat(Object context, string format, params object[] args)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.LogFormat(context, format, args);
//        }
//    }

//    /// <summary>
//    /// 打印一条日志。以格式化字符串的形式打印
//    /// </summary>
//    /// <param name="logType"></param>
//    /// <param name="logOptions"></param>
//    /// <param name="context"></param>
//    /// <param name="format"></param>
//    /// <param name="args"></param>
//    public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.LogFormat(logType, logOptions, context, format, args);
//        }
//    }

//    /// <summary>
//    /// 打印一条警告信息
//    /// </summary>
//    /// <param name="message"></param>
//    public static void LogWarning(object message)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.LogWarning(message);
//        }
//    }

//    /// <summary>
//    /// 打印一条警告信息
//    /// </summary>
//    /// <param name="message"></param>
//    /// <param name="context"></param>
//    public static void LogWarning(object message, Object context)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.LogWarning(message, context);
//        }
//    }

//    /// <summary>
//    /// 打印一条警告信息。以格式化字符串的形式打印
//    /// </summary>
//    /// <param name="format"></param>
//    /// <param name="args"></param>
//    public static void LogWarningFormat(string format, params object[] args)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.LogWarningFormat(format, args);
//        }
//    }

//    /// <summary>
//    /// 打印一条警告信息。以格式化字符串的形式打印
//    /// </summary>
//    /// <param name="context"></param>
//    /// <param name="format"></param>
//    /// <param name="args"></param>
//    public static void LogWarningFormat(Object context, string format, params object[] args)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.LogWarningFormat(context, format, args);
//        }
//    }

//    /// <summary>
//    /// 打印一条错误信息
//    /// </summary>
//    /// <param name="message"></param>
//    public static void LogError(object message)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.LogError(message);
//        }
//    }

//    /// <summary>
//    /// 打印一条错误信息
//    /// </summary>
//    /// <param name="message"></param>
//    /// <param name="context"></param>
//    public static void LogError(object message, Object context)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.LogError(message, context);
//        }
//    }

//    /// <summary>
//    /// 打印一条错误信息。以格式化字符串的形式打印
//    /// </summary>
//    /// <param name="format"></param>
//    /// <param name="args"></param>
//    public static void LogErrorFormat(string format, params object[] args)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.LogErrorFormat(format, args);
//        }
//    }

//    /// <summary>
//    /// 打印一条错误信息。以格式化字符串的形式打印
//    /// </summary>
//    /// <param name="context"></param>
//    /// <param name="format"></param>
//    /// <param name="args"></param>
//    public static void LogErrorFormat(Object context, string format, params object[] args)
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.LogErrorFormat(context, format, args);
//        }
//    }

//    /// <summary>
//    /// 暂停编辑器
//    /// </summary>
//    public static void Break()
//    {
//        if (AppConst.DevMode)
//        {
//            Debug.Break();
//        }
//    }
//}
