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
    /// <summary>
    /// Development字符串
    /// </summary>
    public const string DEVELOPMENT_STRING = "Development";
    /// <summary>
    /// Release字符串
    /// </summary>
    public const string RELEASE_STRING = "Release";
    /// <summary>
    /// 表示资源的字符串
    /// </summary>
    public static string ResString { get { return "Res"; } }
    /// <summary>
    /// 表示lua代码的字符串
    /// </summary>
    public static string LuaString { get { return "Lua"; } }

    private static bool devMode = true;
    /// <summary>
    /// 是否为开发模式
    /// </summary>
    public static bool DevMode { get { return devMode; } }

    private static bool loadLoaclAssetBundle = true;
    /// <summary>
    /// 是否加载工程目录下的AssetBundle
    /// </summary>
    public static bool LoadLoaclAssetBundle { get { return loadLoaclAssetBundle; } }
    private static bool useLocalIP = true;
    /// <summary>
    /// 是否使用本地IP
    /// </summary>
    public static bool UseLocalIP { get { return useLocalIP; } }
    private static bool useDevPort = true;
    /// <summary>
    /// 是否使用开发模式的端口
    /// </summary>
    public static bool UseDevPort { get { return useDevPort; } }
    private static bool adsTestMod = false;
    /// <summary>
    /// UnityAds的广告投放是否启用开发模式
    /// </summary>
    public static bool AdsTestMod { get { return adsTestMod; } }
    /// <summary>
    /// 连接服务器的ip地址
    /// </summary>
    public static string IP 
    {
        get 
        {
            if (UseLocalIP)
            {
                return "127.0.0.1";
            }
            else
            {
                return "";
            }
        }
    }

    /// <summary>
    /// 给http请求用的监听
    /// </summary>
    public static string HttpIP
    {
        get
        {
            if (UseLocalIP)
            {
                return "127.0.0.1";
            }
            else
            {
                return "api.qinghuixiang.com";
            }
        }
    }

    /// <summary>
    /// 连接服务器的端口号
    /// </summary>
    public static int Port
    {
        get
        {
            if (UseDevPort)
            {
                return 1129;
            }
            else
            {
                return 1130;
            }
        }
    }
    /// <summary>
    /// http请求使用的端口号
    /// </summary>
    public static int HttpRequestPort
    {
        get
        {
            if (UseDevPort)
            {
                return 1128;
            }
            else
            {
                return 1131;
            }
        }
    }

    /// <summary>
    /// 服务器存放资源Res的文件夹
    /// </summary>
    public static string ServerResURL
    {
        get
        {
            string devModeString = DevMode ? DEVELOPMENT_STRING : RELEASE_STRING;

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
            string devModeString = DevMode ? DEVELOPMENT_STRING : RELEASE_STRING;

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
            string devModeString = DevMode ? DEVELOPMENT_STRING : RELEASE_STRING;

            return string.Format("https://www.qinghuixiang.com:443/File/UNOHotfixFile/{0}/{1}/Config.cfg", devModeString, GetPlatformName());
        }
    }

    /// <summary>
    /// 服务器版本信息文件路径
    /// </summary>
    /// <returns></returns>
    public static string ServerVersionFileURL
    {
        get
        {
            string devModeString = DevMode ? DEVELOPMENT_STRING : RELEASE_STRING;

            return string.Format("https://www.qinghuixiang.com:443/File/UNOHotfixFile/{0}/{1}/Version.cfg", devModeString, GetPlatformName());
        }
    }

    /// <summary>
    /// 场景名称列表
    /// </summary>
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
            if (sceneNameList == null)
            {
                sceneNameList = new List<string>();
            }

            return sceneNameList;
        }
    }

    /// <summary>
    /// 获得本地资源根路径
    /// </summary>
    /// <returns></returns>
    public static string LocalResRootFolderPath
    { 
        get 
        { 
            return Path.Combine(Application.persistentDataPath, AppConst.GetPlatformName(), AppConst.ResString); 
        } 
    }

    /// <summary>
    /// 存放热更资源压缩包的路径
    /// </summary>
    public static string LocalResourceFolderPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, AppConst.GetPlatformName());
        }
    }

    /// <summary>
    /// 根据开发模式获得对应的文件夹名字（Development或者Release）
    /// </summary>
    /// <returns></returns>
    public static string GetDevModeServerFolderName()
    {
        if (AppConst.DevMode)
        {
            return DEVELOPMENT_STRING;
        }
        else
        {
            return RELEASE_STRING;
        }
    }

    /// <summary>
    /// 获取当前平台持久化目录下Lua脚本根目录
    /// </summary>
    /// <returns></returns>
    public static string LocalLuaRootFolderPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, AppConst.GetPlatformName(), AppConst.LuaString);
        }
    }

    /// <summary>
    /// 获得当前运行的平台名称
    /// </summary>
    /// <returns></returns>
    public static string GetPlatformName()
    {
        string platform = "";
#if UNITY_STANDALONE
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
    /// 获取本地配置文件的路径
    /// </summary>
    /// <returns></returns>
    public static string GetConfigFileFullPath()
    {
        return Path.Combine(Application.persistentDataPath, "Config.cfg");
    }

    private static string zipPassword = "UNO";
    /// <summary>
    /// Res和Lua的zip压缩文件的密码
    /// </summary>
    public static string ZipPassword { get { return zipPassword; } }

    private static string abPackageKey = "abcdefgh";
    /// <summary>
    /// 资源的ab包加解密用的密钥
    /// </summary>
    public static string AbPackageKey { get { return abPackageKey; } }

    /// <summary>
    /// AssetBundle包后缀名。包含'.'
    /// </summary>
    public static string AssetBundleSuffix { get { return ".ab"; } }
    private static string encryptionFileSuffix = ".uu3d";
    /// <summary>
    /// 加密后资源文件的后缀名。包含‘.’
    /// </summary>
    public static string EncryptionFillSuffix { get { return encryptionFileSuffix; } }

    /// <summary>
    /// 存放.pb文件的路径。如果加载本地目录下的ab包，则pb文件也会加载工程目录下的
    /// </summary>
    public static string ProtoFilePath
    {
        get
        {
            if (AppConst.LoadLoaclAssetBundle)
            {
                return Path.Combine(Application.dataPath, "LuaFramework", "Protos");
            }
            else
            {
                return Path.Combine(Application.persistentDataPath, "Protos");
            }
        }
    }

    /// <summary>
    /// 服务器上pb文件列表文件的url地址
    /// </summary>
    public static string ServerPbFileListURL
    {
        get
        {
            string devModeString = DevMode ? DEVELOPMENT_STRING : RELEASE_STRING;

            return string.Format("https://www.qinghuixiang.com:443/File/UNOHotfixFile/{0}/Protos/PbFileList.cfg", devModeString);
        }
    }

    /// <summary>
    /// 根据pb文件的url返回客户端的pb文件完整路径
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetPbFileFullPathByURL(string url)
    {
        int index = url.LastIndexOf("/");
        string fileName = url.Substring(index + 1);

        string path = Path.Combine(AppConst.ClientMsgPbURL, fileName);

        return path;
    }

    /// <summary>
    /// 客户端上用于保存pb文件的路径
    /// </summary>
    public static string ClientMsgPbURL
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "Protos");
        }
    }

    /// <summary>
    /// 工程中存放预制体的文件夹
    /// </summary>
    public static string LocalPrefabPath { get { return Path.Combine("LuaFramework", "Builder"); } }

    /// <summary>
    /// 获取加载中提示文字的API地址
    /// </summary>
    public static string LoadingTipsRequestURL 
    {
        get
        {
            if (UseLocalIP)
            {
                //在本机使用http
                return string.Format("http://{0}:{1}/LoadingTips/GetTip", HttpIP, HttpRequestPort);
            }
            else
            {
                //在上线版使用https
                return string.Format("https://{0}:{1}/LoadingTips/GetTip", HttpIP, HttpRequestPort);
            }
        }
    }
}
