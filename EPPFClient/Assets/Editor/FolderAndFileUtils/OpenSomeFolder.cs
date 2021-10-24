using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class OpenSomeFolder
{
    [MenuItem("Folder Util/Open PersistentDataPath")]
    public static void OpenPersistentDataPath()
    {
        string path = Application.persistentDataPath;
        if (Directory.Exists(path))
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
        else
        {
            Debug.LogFormat("路径：{0}。不存在，请自行检查对应位置目录", path);
        }
    }

    [MenuItem("Folder Util/Open Lua Path")]
    public static void OpenLuaPath()
    {
        string path = Path.Combine(Application.dataPath, "LuaFramework", "Lua");
        if (Directory.Exists(path))
        {
            EditorUtility.RevealInFinder(path);
        }
        else
        {
            //没有找到目录。找Main.lua所在的文件夹
            string[] mainLuaFilePath = AssetDatabase.FindAssets("Main.lua");
            if(mainLuaFilePath.Length > 0)
            {
                if(mainLuaFilePath.Length > 1)
                {
                    Debug.Log("该工程具有多个Main.lua文件");
                }
                for (int i = 0; i < mainLuaFilePath.Length; i++)
                {
                    string fullPath = Path.Combine(Application.dataPath, GetLocalPathRemoveAsset(mainLuaFilePath[i]));
                    EditorUtility.RevealInFinder(fullPath);
                }
            }
            else
            {
                Debug.LogFormat("路径：{0}。不存在并且没有找到‘Main.lua’文件。请自行修改path变量指定的相对路径。", path);
            }
        }
    }

    /// <summary>
    /// 打开构建目录
    /// </summary>
    [MenuItem("Folder Util/Open Build Path")]
    public static void OpenBuildPath()
    {
        string path = Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 6), "Builds");
        if (Directory.Exists(path))
        {
            EditorUtility.RevealInFinder(path);
        }
        else
        {
            Debug.LogFormat("路径：{0}。不存在，请自行检查对应位置目录", path);
        }
    }

    /// <summary>
    /// 打开协议类生成工具的目录
    /// </summary>
    [MenuItem("Folder Util/Open Protocol Tools Path")]
    public static void OpenProtocolToolsPath()
    {
        string path = Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 6), "协议相关类生成工具", "EPPFGenerateProtocolCode.exe");
        if (Directory.Exists(path) || File.Exists(path))
        {
            EditorUtility.RevealInFinder(path);
        }
        else
        {
            Debug.LogFormat("路径：{0}。不存在，请自行检查对应位置目录", path);
        }
    }

    /// <summary>
    /// 打开协议类生成工具的目录
    /// </summary>
    [MenuItem("Folder Util/Open Resource Path")]
    public static void OpenResourcePath()
    {
        string path = Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 6), "Resource");
        if (Directory.Exists(path))
        {
            EditorUtility.RevealInFinder(path);
        }
        else
        {
            Debug.LogFormat("路径：{0}。不存在，请自行检查对应位置目录", path);
        }
    }

    /// <summary>
    /// 打开版本控制的目录
    /// </summary>
    [MenuItem("Folder Util/Open VersionControl Path")]
    public static void OpenVersionControlPath()
    {
        string path = Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 6), "热更新资源版本控制工具", "HotPackageVersionControl.exe");
        if (Directory.Exists(path) || File.Exists(path))
        {
            EditorUtility.RevealInFinder(path);
        }
        else
        {
            Debug.LogFormat("路径：{0}。不存在，请自行检查对应位置目录", path);
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
