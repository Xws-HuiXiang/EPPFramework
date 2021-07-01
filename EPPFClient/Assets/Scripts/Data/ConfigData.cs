using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 配置字典的项
/// </summary>
public class ConfigListItem
{
    /// <summary>
    /// 键
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }
}

/// <summary>
/// 需要保存在本地的配置信息
/// </summary>
public class ConfigData
{
    public List<ConfigListItem> ConfigList { get; set; }
    public int ResVersion { get; set; }
    public int LuaVersion { get; set; }

    /// <summary>
    /// 设置热更新的Res版本信息
    /// </summary>
    /// <param name="version"></param>
    /// <param name="autoSave"></param>
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

    /// <summary>
    /// 设置热更新的Lua版本信息
    /// </summary>
    /// <param name="version"></param>
    /// <param name="autoSave"></param>
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
    /// 设置ConfigList的key和value
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="autoSave"></param>
    /// <returns></returns>
    public static bool SetConfigValue(string key, string value, bool autoSave = true)
    {
        if (GameManager.Instance.Config != null)
        {
            ConfigListItem configListItem = null;
            foreach (ConfigListItem cli in GameManager.Instance.Config.ConfigList)
            {
                if(cli.Key == key)
                {
                    configListItem = cli;

                    break;
                }
            }
            if(configListItem == null)
            {
                //没有找到key，新增一个
                configListItem = new ConfigListItem();
                configListItem.Key = key;
                configListItem.Value = value;

                GameManager.Instance.Config.ConfigList.Add(configListItem);
            }
            else
            {
                //找到了key，重新设置值
                configListItem.Value = value;
            }

            if (autoSave)
            {
                SaveConfig();
            }

            return true;
        }
        else
        {
            FDebugger.LogError("没有配置文件信息，无法设置数据");

            return false;
        }
    }

    /// <summary>
    /// 根据key获取ConfigList列表中的value
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="defaultValue">如果没有对应字段则返回该默认值</param>
    /// <returns></returns>
    public static string GetConfigValue(string key, string defaultValue = "")
    {
        if (GameManager.Instance.Config != null)
        {
            foreach (ConfigListItem cli in GameManager.Instance.Config.ConfigList)
            {
                if (cli.Key == key)
                {
                    return cli.Value;
                }
            }

            //没有找到key，返回提供的默认值
            return defaultValue;
        }
        else
        {
            FDebugger.LogError("没有配置文件信息，无法设置数据");

            return defaultValue;
        }
    }

    /// <summary>
    /// 尝试保存配置文件的次数
    /// </summary>
    private static int trySaveConfigCount = 0;

    /// <summary>
    /// 将配置信息保存到文件
    /// </summary>
    /// <param name="defaultData">如果文件不存在，则默认写入的数据</param>
    public static void SaveConfig(byte[] defaultData = null)
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
                //没有配置文件，创建一个空的
                FileStream configFile = File.Create(AppConst.GetConfigFileFullPath());
                if(defaultData != null)
                {
                    configFile.Write(defaultData, 0, defaultData.Length);
                }
                configFile.Close();

                trySaveConfigCount++;
                if (trySaveConfigCount < 5)
                {
                    //再次尝试保存
                    ConfigData.SaveConfig();
                }
                else
                {
                    FDebugger.LogError("无法保存配置文件，因为文件不存在（创建配置文件失败）");
                }
            }
        }
        else
        {
            FDebugger.LogError("没有配置文件信息，无法保存数据");
        }
    }
}
