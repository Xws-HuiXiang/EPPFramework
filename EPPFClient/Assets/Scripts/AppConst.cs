using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 全局变量和常量
/// </summary>
public class AppConst
{
    private static bool devMode = true;
    /// <summary>
    /// 是否为开发模式
    /// </summary>
    public static bool DevMode
    {
        get
        {
            return devMode;
        }
    }

    private static bool loadLoaclAssetBundle = true;
    /// <summary>
    /// 是否加载工程目录下的AssetBundle
    /// </summary>
    public static bool LoadLoaclAssetBundle
    {
        get
        {
            return loadLoaclAssetBundle;
        }
    }

    private static string ip = "127.0.0.1";
    /// <summary>
    /// 连接服务器的ip地址
    /// </summary>
    public static string IP
    {
        get
        {
            return ip;
        }
    }

    /// <summary>
    /// 服务器存放资源Res的文件夹
    /// </summary>
    public static string ServerResURL
    {
        get
        {
            string devModeString = DevMode ? "Development" : "Release";

            return string.Format("https://www.qinghuixiang.com:443/File/UNOHotfixFile/{0}/{1}/Res/", devModeString, GetPlatformName());
        }
    }

    /// <summary>
    /// 服务器存放lua脚本的文件夹
    /// </summary>
    public static string ServerLuaURL
    {
        get
        {
            string devModeString = DevMode ? "Development" : "Release";

            return string.Format("https://www.qinghuixiang.com:443/File/UNOHotfixFile/{0}/{1}/Lua/", devModeString, GetPlatformName());
        }
    }

    /// <summary>
    /// 服务器默认配置文件的路径
    /// </summary>
    public static string ServerConfigURL
    {
        get
        {
            string devModeString = DevMode ? "Development" : "Release";

            return string.Format("https://www.qinghuixiang.com:443/File/UNOHotfixFile/{0}/{1}/Config.cfg", devModeString, GetPlatformName());
        }
    }

    private static int port = 1130;
    /// <summary>
    /// 连接服务器的端口号
    /// </summary>
    public static int Port
    {
        get
        {
            return port;
        }
    }

    private static List<string> sceneNameList = new List<string>()
    {
        "FirstScene",
        "LoadingScene",
        "GameScene",
    };
    /// <summary>
    /// 游戏场景名称列表
    /// </summary>
    public static List<string> SceneNameList
    {
        get
        {
            if(sceneNameList == null)
            {
                sceneNameList = new List<string>();
            }

            return sceneNameList;
        }
    }

    /// <summary>
    /// 获得本地资源根路径
    /// </summary>
    /// <param name="includeLastSlice">是否包含最后的分隔符</param>
    /// <returns></returns>
    public static string GetLoaclResRootFolderPath()
    {
        return Path.Combine(Application.persistentDataPath, AppConst.GetPlatformName(), "Res");
    }

    /// <summary>
    /// 根据开发模式获得对应的文件夹名字
    /// </summary>
    /// <returns></returns>
    private static string GetDevModeServerFolderName()
    {
        if (AppConst.DevMode)
        {
            return "Development";
        }
        else
        {
            return "Release";
        }
    }

    /// <summary>
    /// 获取当前平台持久化目录下Lua脚本根目录
    /// </summary>
    /// <returns></returns>
    public static string GetLocalLuaRootFolderPath()
    {
        return Path.Combine(Application.persistentDataPath, AppConst.GetPlatformName(), "Lua");
    }

    /// <summary>
    /// 获得当前运行的平台名称
    /// </summary>
    /// <returns></returns>
    public static string GetPlatformName()
    {
        string platform = "";
#if UNITY_EDITOR_WIN || UNITY_EDITOR || UNITY_STANDALONE
        platform = "Windows";
#elif UNITY_ANDROID
        platform = "Android";
#elif UNITY_IOS
        platform = "iOS";
#else
        platform = "未知平台";
#endif

        return platform;
    }

    /// <summary>
    /// 获得工程目录下的AssetBundle目录（不包含Asset文件）
    /// </summary>
    /// <returns></returns>
    public static string GetLocalAssetBundlePath()
    {
        return "AssetBundle";
    }

    /// <summary>
    /// 获取本地配置文件的路径
    /// </summary>
    /// <returns></returns>
    public static string GetConfigFileFullPath()
    {
        return Application.persistentDataPath + "/Config.cfg";
    }
}
