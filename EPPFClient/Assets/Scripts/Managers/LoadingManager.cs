using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;
using UnityEngine.Networking;
using LuaInterface;
using Ciphertext;

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

    /// <summary>
    /// 是否执行加载Game场景的这个逻辑
    /// </summary>
    private bool runLoadGameScene = true;

    /// <summary>
    /// 最下面的提示文字框
    /// </summary>
    private Text tip;

    /// <summary>
    /// 下载更新资源时，已经下载的数据大小
    /// </summary>
    private ulong downloadedBytes = 0;

    private void Awake()
    {
        mInstance = this;

        Transform progressTrans = transform.Find("Progress");
        progressSlider = progressTrans.GetComponent<Slider>();
        progressText = transform.Find("Progress/ProgressText").GetComponent<Text>();

        tip = transform.Find("Tips").GetComponent<Text>();
    }

    private void Start()
    {
        SetProgressText("对比服务器版本");

        //加载工程目录下的资源时，不检查版本
        if (!AppConst.LoadLoaclAssetBundle)
        {
            DownloadManager.DownloadFile(AppConst.ServerVersionFileURL,
                (data) => 
                {
                    string text = Encoding.UTF8.GetString(data);

                    HotfixVersionData versionData = JsonMapper.ToObject<HotfixVersionData>(text);

                    GetHotfixVersionHandle(versionData.ResVersion, versionData.LuaVersion);
                },
                (progress, downloadedBytes) => 
                {
                    SetProgressSliderValue(progress);
                });
        }
        else
        {
            FDebugger.Log("处于开发模式，不下载版本信息");

            GetHotfixVersionHandle(1, 1);
        }

        //从服务器拉取最下面显示的提示文字
        if (tip != null)
        {
            string tipURL = AppConst.LoadingTipsRequestURL;
            HttpRequestManager.HttpGetRequestAsync(tipURL, (content) =>
            {
                if(content != null && tip != null)
                {
                    GetALoadingTipData data = JsonMapper.ToObject<GetALoadingTipData>(content);
                    tip.text = data.Content;
                }
            });
        }
    }

    private void Update()
    {
        //因为连接服务器使用的是异步方法，而Unity规定加载场景只能放在Unity的主线程中。所以在这里写：当连接成功后，加载Game场景
        //加载并进入场景后，Loading场景销毁，此update不会被额外调用
        if (NetworkManager.Instance.IsConnection && runLoadGameScene)
        {
            runLoadGameScene = false;

            SceneManager.LoadScene(AppConst.SceneNameList[2]);
        }

        if (progressTextAsyncTag)
        {
            progressTextAsyncTag = false;
            SetProgressText(progressTextAsyncText);
        }
    }

    /// <summary>
    /// 设置加载进度条的提示文字
    /// </summary>
    /// <param name="text">文字内容</param>
    public void SetProgressText(string text)
    {
        progressText.text = text;
    }

    private string progressTextAsyncText;
    private bool progressTextAsyncTag = false;
    /// <summary>
    /// 设置加载进度条的提示文字。异步方法
    /// </summary>
    /// <param name="text"></param>
    public void SetProgressTextAsync(string text)
    {
        progressTextAsyncText = text;
        progressTextAsyncTag = true;
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
    /// <param name="resVersion">res的版本号</param>
    /// <param name="luaVersion">lua的版本号</param>
    private void GetHotfixVersionHandle(int resVersion, int luaVersion)
    {
        FDebugger.Log("服务器资源版本号：" + resVersion);
        FDebugger.Log("服务器代码版本号：" + luaVersion);

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
                    //加载工程目录资源的模式下不检查版本更新
                    if (!AppConst.LoadLoaclAssetBundle)
                    {
                        int localResVersionNum = config.ResVersion;
                        int localLuaVersionNum = config.LuaVersion;
                        serverResVersionNum = resVersion;
                        serverLuaVersionNum = luaVersion;

                        if (resVersion > localResVersionNum || luaVersion > localLuaVersionNum)
                        {
                            //资源有更新，执行更新
                            FDebugger.LogFormat("资源有更新，执行更新。服务器版本号：res[{0}] lua[{1}]，客户端版本号：res[{2}] lua[{3}]", resVersion, luaVersion, localResVersionNum, localLuaVersionNum);
                            SetProgressText("正在下载更新资源");
                            DownloadManager.DownloadServerHotfixAsync(localResVersionNum, resVersion, localLuaVersionNum, luaVersion, DownloadDoneCallback, DownloadUnitDoneCallback, DownloadingCallback);
                        
                            //下载中的刷新提示文字的协程
                            StartCoroutine(DownloadingCallbackIE());
                        }
                        else
                        {
                            //资源没有更新，加载本地ab包
                            FDebugger.Log("资源没有更新，加载本地ab包");
                            SetProgressText("加载资源中");

                            LoadLocalAssetBundle();
                        }
                    }
                    else
                    {
                        serverResVersionNum = resVersion;
                        serverLuaVersionNum = luaVersion;

                        LoadLocalAssetBundle();
                    }
                }
                catch (Exception e)
                {
                    FDebugger.LogWarningFormat("版本信息解析失败，将重新下载所有资源。错误信息：{0}\n服务器资源版本号为：{1}，代码版本号为：{2}", e.Message, resVersion, luaVersion);
                    if (!AppConst.DevMode)
                    {
                        //非开发模式才下载
                        DownloadAll(resVersion, luaVersion);
                    }
                    else
                    {
                        //开发模式不下载资源
                        FDebugger.LogWarning("开发模式下不下载更新资源");
                    }
                }
            }
            else
            {
                //版本文件信息为空，这是一个异常。将重新下载所有资源
                FDebugger.LogWarning("版本文件内容为空");
                DownloadAll(resVersion, luaVersion);
            }
        }
        else
        {
            //没有配置文件，是第一次启动游戏
            FDebugger.Log("第一次启动游戏，下载配置文件和资源文件");
            DownloadAll(resVersion, luaVersion);
        }
    }

    /// <summary>
    /// 下载中回调
    /// </summary>
    /// <param name="progress">下载进度</param>
    /// <param name="downloadedBytes">已经下载的数据大小</param>
    private void DownloadingCallback(float progress, ulong downloadedBytes)
    {
        progressSlider.value = progress;

        this.downloadedBytes = downloadedBytes;
    }

    /// <summary>
    /// 下载时，每隔一秒调用一次，用于刷新下载进度以便更新提示文字
    /// </summary>
    /// <returns></returns>
    private IEnumerator DownloadingCallbackIE()
    {
        yield return new WaitForSeconds(1);

        float mb = this.downloadedBytes;
        for (int i = 0; i < 2; i++)
        {
            mb /= 1024;
        }
        SetProgressText(string.Format("正在下载更新资源。已下载[{0}MB]", mb.ToString("f2")));

        StartCoroutine(DownloadingCallbackIE());
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

        string resourcesFolderPath = AppConst.LocalResourceFolderPath;
        if (!Directory.Exists(resourcesFolderPath))
        {
            Directory.CreateDirectory(resourcesFolderPath);
        }

        string fileFullName;
        switch (fileType)
        {
            case HotfixFileType.Res:
                fileFullName = Path.Combine(resourcesFolderPath, fileName);
                break;
            case HotfixFileType.Lua:
                fileFullName = Path.Combine(resourcesFolderPath, fileName);
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
        StopCoroutine(DownloadingCallbackIE());

        //更新本地配置文件版本号
        ConfigData.SetResVersion(serverResVersionNum, false);
        ConfigData.SetLuaVersion(serverLuaVersionNum, false);
        ConfigData.SaveConfig();
        //全部下载完成，解压缩
        UnzipResAndLuaFile();

        //TODO：检查资源完整性

        //加载ab包
        LoadLocalAssetBundle();
    }

    /// <summary>
    /// 版本检查完后，加载本地ab包并跳转到AppConst.SceneNameList[2]
    /// </summary>
    private void LoadLocalAssetBundle()
    {
        SetProgressText("加载游戏资源");
        SetProgressSliderValue(0);

        //添加进入主场景后的事件
        SceneManager.sceneLoaded += EnterGameSceneCallback;

        //启动lua环境
        SetProgressText("启动游戏环境");
        GameManager.LuaState.Start();
        //添加LuaLooper
        LuaLooper loop = GameManager.Instance.gameObject.AddComponent<LuaLooper>();
        loop.luaState = GameManager.LuaState;
        GameManager.SetLuaLooper(loop);
        //绑定wrap文件
        LuaBinder.Bind(GameManager.LuaState);
        //注册委托
        DelegateFactory.Init();
        //注册协程相关东西
        LuaCoroutine.Register(GameManager.LuaState, GameManager.Instance);

        SetProgressText("网络资源准备");
        GameManager.Instance.OpenLuaProtobuf();

        //执行Main.lua中的Awake方法
        GameManager.LuaState.DoFile("AwakeMain.lua");
        LuaFunction mainAwakeFun = GameManager.LuaState.GetFunction("AwakeMain.Awake");
        if(mainAwakeFun != null)
        {
            mainAwakeFun.Call();
        }
        else
        {
            FDebugger.LogWarning("不存在Main.Awake()方法。该方法在Main方法之前调用，用于处理业务逻辑之前的部分事件");
        }

        SetProgressText("正在连接服务器");
        FDebugger.Log("正在连接服务器");
        NetworkManager.Instance.Connection();
    }

    /// <summary>
    /// 进入主场景后的事件
    /// </summary>
    /// <param name="gameScene"></param>
    /// <param name="loadSceneMode"></param>
    private void EnterGameSceneCallback(Scene gameScene, LoadSceneMode loadSceneMode)
    {
        GameManager.Instance.EnterMainScene();
    }

    /// <summary>
    /// 下载全部资源
    /// </summary>
    /// <param name="resVersion"></param>
    /// <param name="luaVersion"></param>
    private void DownloadAll(int resVersion, int luaVersion)
    {
        //下载一个默认配置文件
        SetProgressText("正在下载更新资源");
        DownloadManager.DownloadFile(
            AppConst.ServerConfigURL,
            (data) =>
            {
                //配置的数据
                string configJsonString = Encoding.UTF8.GetString(data);
                ConfigData config = JsonMapper.ToObject<ConfigData>(configJsonString);
                GameManager.Instance.Config = config;

                serverResVersionNum = config.ResVersion;
                serverLuaVersionNum = config.LuaVersion;

                //开始下载更新资源
                //TODO:加载界面显示下载资源的 当前网速
                DownloadManager.DownloadServerLatestHotfixAsync(resVersion, luaVersion, DownloadDoneCallback, DownloadUnitDoneCallback, DownloadingCallback);

                //下载中的刷新提示文字的协程
                StartCoroutine(DownloadingCallbackIE());
            },
            (progress, downloadedBytes) =>
            {
                SetProgressSliderValue(progress);
            });
    }

    /// <summary>
    /// 解压缩res和lua的压缩文件
    /// </summary>
    private void UnzipResAndLuaFile()
    {
        //解压res部分
        if (!AppConst.LoadLoaclAssetBundle)
        {
            for (int i = GameManager.Instance.Config.ResVersion; i <= serverResVersionNum; i++)
            {
                string zipPath;
#if UNITY_EDITOR || UNITY_STANDALONE
                zipPath = Path.Combine(AppConst.LocalResourceFolderPath, AppConst.ResString + i.ToString() + ".zip");
#elif UNITY_ANDROID
                //zipPath = new Uri(Path.Combine(AppConst.LocalResourceFolderPath, AppConst.ResString + i.ToString() + ".zip")).AbsoluteUri;
                zipPath = Path.Combine(AppConst.LocalResourceFolderPath, AppConst.ResString + i.ToString() + ".zip");
#elif UNITY_IOS
                zipPath = Path.Combine(AppConst.LocalResourceFolderPath, AppConst.ResString + i.ToString() + ".zip");
#else
                zipPath = Path.Combine(AppConst.LocalResourceFolderPath, AppConst.ResString + i.ToString() + ".zip");
#endif

                FDebugger.Log("zipPath:" + zipPath);
                if (!Directory.Exists(AppConst.LocalResRootFolderPath))
                {
                    Directory.CreateDirectory(AppConst.LocalResRootFolderPath);
                }
                ZIPFileUtil.UnzipFile(zipPath, AppConst.LocalResRootFolderPath, AppConst.ZipPassword);
            }
        }
        //解压lua代码部分
        if (!AppConst.LoadLoaclAssetBundle)
        {
            //将lua代码的zip包解压
            for (int i = GameManager.Instance.Config.LuaVersion; i <= serverLuaVersionNum; i++)
            {
                string luaPath = Path.Combine(AppConst.LocalResourceFolderPath, AppConst.LuaString + i.ToString() + ".zip");
                FDebugger.Log("luaPath:" + luaPath);

                if (!Directory.Exists(AppConst.LocalLuaRootFolderPath))
                {
                    Directory.CreateDirectory(AppConst.LocalLuaRootFolderPath);
                }
                ZIPFileUtil.UnzipFile(luaPath, AppConst.LocalLuaRootFolderPath, AppConst.ZipPassword);
            }
        }
    }

    /// <summary>
    /// 获取加载中文字提示的数据对象
    /// </summary>
    public class GetALoadingTipData
    {
        /// <summary>
        /// 提示文字具体内容
        /// </summary>
        public string Content { get; set; }
    }
}
