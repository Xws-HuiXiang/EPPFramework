using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CommonProtocol;
using LitJson;

/// <summary>
/// 热更新文件的类型。资源（Res）或者代码（Lua）
/// </summary>
public enum HotfixFileType
{
    Res,
    Lua
}

//TODO:loading界面也需要热更。这个面板要从ab包中加载。包括背景图片应该也是从固定位置加载的。有更新则下载到对应位置
public class LoadingManager : MonoSingleton<LoadingManager>
{
    private Slider progressSlider;
    private Text progressText;

    /// <summary>
    /// 服务器res版本号
    /// </summary>
    private int serverResVersionNum;
    /// <summary>
    /// 服务器lua版本号
    /// </summary>
    private int serverLuaVersionNum;

    private void Awake()
    {
        //添加获取版本信息的协议处理回调
        NetworkManager.AddProtocolHandle((int)MsgHandle.Version, (int)MsgGameVersionHandleMethod.GetGameHotFixVersion, GetHotfixVersionMsgHandle);

        Transform progressTrans = transform.Find("Progress");
        progressSlider = progressTrans.GetComponent<Slider>();
        progressText = transform.Find("Progress/ProgressText").GetComponent<Text>();
    }

    private void Start()
    {
        SetProgressText("对比服务器版本");

        GameHotFixVersion msg = new GameHotFixVersion();
        msg.ProtocolHandleID = (int)MsgHandle.Version;
        msg.ProtocolHandleMethodID = (int)MsgGameVersionHandleMethod.GetGameHotFixVersion;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        msg.platform = RuntimePlatform.WindowsPlayer;
#elif UNITY_ANDROID
        msg.platform = RuntimePlatform.Android;
#elif UNITY_IOS
        msg.platform = RuntimePlatform.IPhonePlayer;
#elif UNITY_PS4
        msg.platform = RuntimePlatform.PS4;
#else
        //没有判断到的平台，默认为PC
        msg.platform = RuntimePlatform.WindowsPlayer;
#endif
        msg.resVersion = 0;
        msg.luaVersion = 0;
        NetworkManager.Instance.SendMsg(msg);
    }

    /// <summary>
    /// 设置加载进度条的提示文字
    /// </summary>
    /// <param name="text"></param>
    public void SetProgressText(string text)
    {
        progressText.text = text;
    }

    /// <summary>
    /// 设置进度条进度值
    /// </summary>
    /// <param name="value"></param>
    public void SetProgressSliderValue(float value)
    {
        this.progressSlider.value = value;
    }

    /// <summary>
    /// 获取服务器版本信息的回调函数
    /// </summary>
    /// <param name="msgBytes"></param>
    private void GetHotfixVersionMsgHandle(byte[] msgBytes)
    {
        GameHotFixVersion msg = NetworkManager.GetObjectOfBytes<GameHotFixVersion>(msgBytes);
        if (msg != null)
        {
            FDebugger.Log("##服务器资源版本号：" + msg.resVersion);
            FDebugger.Log("##服务器代码版本号：" + msg.luaVersion);

            SetProgressText("确认热更新资源");
            SetProgressSliderValue(1);
            string localVersionFilePath = AppConst.GetConfigFileFullPath();
            if (File.Exists(localVersionFilePath))
            {
                //解析配置文件
                ConfigData config = JsonMapper.ToObject<ConfigData>(File.ReadAllText(localVersionFilePath));
                GameManager.Instance.Config = config;
                if (config != null)
                {
                    try
                    {
                        int localResVersionNum = config.ResVersion;
                        int localLuaVersionNum = config.LuaVersion;
                        serverResVersionNum = msg.resVersion;
                        serverLuaVersionNum = msg.luaVersion;

                        if (msg.resVersion > localResVersionNum)
                        {
                            //资源有更新，执行更新
                            SetProgressText("正在下载更新资源");
                            DownloadManager.DownloadServerHotfixAsync(localResVersionNum, msg.resVersion, localLuaVersionNum, msg.luaVersion, DownloadDoneCallback, DownloadUnitDoneCallback, DownloadingCallback);
                        }
                        else
                        {
                            //资源没有更新，加载本地ab包
                            SetProgressText("加载资源中");
                            StartCoroutine(LoadLocalAssetBundle());
                        }
                    }
                    catch (Exception e)
                    {
                        FDebugger.LogWarningFormat("版本信息解析失败，将重新下载所有资源。错误信息：{0}\n服务器资源版本号为：{1}，代码版本号为：{2}", e.Message, msg.resVersion, msg.luaVersion);
                        //下载文件
                        DownloadAll(msg);
                    }
                }
                else
                {
                    //版本文件信息为空，这是一个异常。将重新下载所有资源
                    FDebugger.LogWarning("版本文件内容为空");
                    DownloadAll(msg);
                }
            }
            else
            {
                //没有配置文件，是第一次启动游戏
                DownloadAll(msg);
            }
        }
        else
        {
            FDebugger.LogError("服务器返回的对象数组无法反序列化为GameHotFixVersion对象");
        }
    }

    /// <summary>
    /// 下载中回调
    /// </summary>
    /// <param name="progress"></param>
    private void DownloadingCallback(float progress)
    {
        progressSlider.value = progress;
    }

    /// <summary>
    /// 单个文件下载完成回调
    /// </summary>
    /// <param name="fileType"></param>
    /// <param name="fileName"></param>
    /// <param name="data"></param>
    private void DownloadUnitDoneCallback(HotfixFileType fileType, string fileName, byte[] data)
    {
        SetProgressText("正在将资源保存到本地");

        string fileFullName;
        switch (fileType)
        {
            case HotfixFileType.Res:
                fileFullName = Path.Combine(AppConst.GetLoaclResRootFolderPath(), fileName);
                break;
            case HotfixFileType.Lua:
                fileFullName = Path.Combine(AppConst.GetLocalLuaRootFolderPath(), fileName);
                break;
            default:
                FDebugger.LogWarningFormat("创建文件未能保存到指定位置。未知的文件类型：{0}。文件名：{1}", fileType.ToString(), fileName);
                fileFullName = Path.Combine(Application.dataPath, fileName);
                break;
        }
        
        FileStream fileStream = File.Create(fileFullName);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();

        SetProgressText("正在下载更新资源");
    }

    /// <summary>
    /// 下载res和lua全部完成时的回调
    /// </summary>
    private void DownloadDoneCallback()
    {
        //更新本地配置文件版本号
        ConfigData.SetResVersion(serverResVersionNum, false);
        ConfigData.SetLuaVersion(serverLuaVersionNum, false);
        ConfigData.SaveConfig();

        //TODO：检查资源完整性

        StartCoroutine(LoadLocalAssetBundle());
    }

    /// <summary>
    /// 版本检查完后，加载本地ab包并跳转到AppConst.SceneNameList[2]
    /// </summary>
    private IEnumerator LoadLocalAssetBundle()
    {
        SetProgressText("解压缩资源");
        SetProgressSliderValue(0);

        string abRootPath;
        if (AppConst.LoadLoaclAssetBundle)
        {
            abRootPath = Application.dataPath + "/" + AppConst.GetLocalAssetBundlePath();
        }
        else
        {
            abRootPath = AppConst.GetLoaclResRootFolderPath();
        }
        //加载zip
        List<AssetBundle> abList = new List<AssetBundle>();
        ZIPFileUtil.UnzipABFile(Path.Combine(abRootPath, "Zip", "Res.zip"), out List<byte[]> contentList, "test");
        foreach (byte[] content in contentList)
        {
            AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromMemoryAsync(content);
            SetProgressSliderValue(bundleLoadRequest.progress);
            yield return bundleLoadRequest;
            if(bundleLoadRequest.assetBundle != null)
            {
                abList.Add(bundleLoadRequest.assetBundle);
            }
        }

        ResourcesManager.Instance.AddAssetBundle(abList);

        SetProgressSliderValue(1);
        //跳转到游戏主场景
        SceneManager.LoadScene(AppConst.SceneNameList[2]);
        //加载主场景进入逻辑
        SceneManager.sceneLoaded += EnterGameSceneCallback;
    }

    private void EnterGameSceneCallback(Scene gameScene, LoadSceneMode loadSceneMode)
    {
        GameManager.Instance.EnterMainScene();
    }

    /// <summary>
    /// 下载全部资源
    /// </summary>
    private void DownloadAll(GameHotFixVersion msg)
    {
        //下载一个默认配置文件
        SetProgressText("正在下载更新资源");
        DownloadManager.DownloadFile(
            AppConst.ServerConfigURL,
            (data) =>
            {
                //保存配置文件
                FileStream configFile = File.Create(AppConst.GetConfigFileFullPath());
                configFile.Write(data, 0, data.Length);
                configFile.Close();
                string configJsonString = Encoding.UTF8.GetString(data);
                ConfigData config = JsonMapper.ToObject<ConfigData>(configJsonString);
                GameManager.Instance.Config = config;

                //开始下载更新资源
                //TODO:加载界面显示下载资源的大小 总大小 和 当前网速
                DownloadManager.DownloadServerHotfixAsync(0, msg.resVersion, 0, msg.luaVersion, DownloadDoneCallback, DownloadUnitDoneCallback, DownloadingCallback);
            },
            (progress) =>
            {
                SetProgressSliderValue(progress);
            });
    }
}
