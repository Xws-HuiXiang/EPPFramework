using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

public class OpenSomeFolder
{
    [MenuItem("Folder Util/Open PersistentDataPath")]
    public static void OpenPersistentDataPath()
    {
        string path = Application.persistentDataPath.Replace("/", "\\");
        if (Directory.Exists(path))
        {
            Process.Start("explorer.exe", path);
        }
        else
        {
            FDebugger.LogFormat("路径：{0}。不存在，请自行检查对应位置目录", path);
        }
    }

    [MenuItem("Folder Util/Open Lua Path")]
    public static void OpenLuaPath()
    {
        string path = "Assets/LuaFramework/Lua";
        if (Directory.Exists(path))
        {
            string fullPath = (Application.dataPath + GetLocalPathRemoveAsset(path)).Replace("/", "\\");
            Process.Start("explorer.exe", fullPath);
        }
        else
        {
            //没有找到目录。找Main.lua所在的文件夹
            string[] mainLuaFilePath = AssetDatabase.FindAssets("Main.lua");
            if(mainLuaFilePath.Length > 0)
            {
                if(mainLuaFilePath.Length > 1)
                {
                    UnityEngine.Debug.Log("该工程具有多个Main.lua文件");
                }
                for (int i = 0; i < mainLuaFilePath.Length; i++)
                {
                    string fullPath = (Application.dataPath + GetLocalPathRemoveAsset(mainLuaFilePath[i])).Replace("/", "\\");
                    Process.Start("explorer.exe", fullPath);
                }
            }
            else
            {
                UnityEngine.Debug.LogFormat("路径：{0}。不存在并且没有找到‘Main.lua’文件。请自行修改path变量指定的相对路径。", path);
            }
        }
    }

    /// <summary>
    /// 打开构建目录
    /// </summary>
    [MenuItem("Folder Util/Open Build Path")]
    public static void OpenBuildPath()
    {
        string path = Application.dataPath;
        path = path.Substring(0, path.Length - 6) + "Builds";
        path = path.Replace("/", "\\");
        if (Directory.Exists(path))
        {
            Process.Start("explorer.exe", path);
        }
        else
        {
            FDebugger.LogFormat("路径：{0}。不存在，请自行检查对应位置目录", path);
        }
    }

    /// <summary>
    /// 打开协议类生成工具的目录
    /// </summary>
    [MenuItem("Folder Util/Open Protocol Tools Path")]
    public static void OpenProtocolToolsPath()
    {
        string path = Application.dataPath;
        path = path.Substring(0, path.Length - 6) + "协议相关类生成工具";
        path = path.Replace("/", "\\");
        if (Directory.Exists(path))
        {
            Process.Start("explorer.exe", path);
        }
        else
        {
            FDebugger.LogFormat("路径：{0}。不存在，请自行检查对应位置目录", path);
        }
    }

    /// <summary>
    /// 获得不包含Assets/开头的相对资源路径
    /// </summary>
    /// <param name="localPath"></param>
    /// <returns></returns>
    private static string GetLocalPathRemoveAsset(string localPath)
    {
        return localPath.Substring(6);
    }
}
