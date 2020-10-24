using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 需要保存在本地的配置信息
/// </summary>
public class ConfigData
{
    public int ResVersion { get; set; }
    public int LuaVersion { get; set; }

    public static void SetResVersion(int version, bool autoSave = true)
    {
        if(GameManager.Instance.Config != null)
        {
            GameManager.Instance.Config.ResVersion = version;

            if (autoSave)
            {
                SaveConfig();
            }
        }
        else
        {
            FDebugger.LogError("没有配置文件信息，无法保存数据");
        }
    }

    public static void SetLuaVersion(int version, bool autoSave = true)
    {
        if (GameManager.Instance.Config != null)
        {
            GameManager.Instance.Config.LuaVersion = version;

            if (autoSave)
            {
                SaveConfig();
            }
        }
        else
        {
            FDebugger.LogError("没有配置文件信息，无法保存数据");
        }
    }

    /// <summary>
    /// 将配置信息保存到文件
    /// </summary>
    public static void SaveConfig()
    {
        if (GameManager.Instance.Config != null)
        {
            string configString = JsonMapper.ToJson(GameManager.Instance.Config);
            string configPath = AppConst.GetConfigFileFullPath();
            if (File.Exists(configPath))
            {
                //Truncate模式用于清空文件内容
                FileStream configFile = new FileStream(configPath, FileMode.Truncate, FileAccess.Write);
                configFile.Seek(0, SeekOrigin.Begin);
                byte[] data = Encoding.UTF8.GetBytes(configString);
                configFile.Write(data, 0, data.Length);

                configFile.Close();
            }
            else
            {
                FDebugger.LogError("持久化目录中没有配置文件，无法保存数据");
            }
        }
        else
        {
            FDebugger.LogError("没有配置文件信息，无法保存数据");
        }
    }
}
