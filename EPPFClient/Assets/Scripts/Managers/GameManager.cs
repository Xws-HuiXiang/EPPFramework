using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LuaInterface;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    private LuaState luaState = null;

    /// <summary>
    /// 当前用户的设置信息
    /// </summary>
    public ConfigData Config { get; set; }

    private void Awake()
    {
        //框架入口
        DontDestroyOnLoad(this.gameObject);

        //初始化管理器
        //InitManagers();
        NetworkManager.Init();

        luaState = new LuaState();

        //添加lua文件的搜索目录
        //添加从持久化目录加载lua文件。添加根文件夹下的所有文件夹
        string luaPath = AppConst.GetLocalLuaRootFolderPath();
        if (!Directory.Exists(luaPath))
        {
            Directory.CreateDirectory(luaPath);
        }
        DirectoryInfo rootDirectoryInfo = new DirectoryInfo(luaPath);
        DirectoryInfo[] directoryArray = rootDirectoryInfo.GetDirectories("*", SearchOption.AllDirectories);
        luaState.AddSearchPath(rootDirectoryInfo.FullName);
        for (int i = 0; i < directoryArray.Length; i++)
        {
            //去掉以.开头的文件
            if (!directoryArray[i].FullName.StartsWith("."))
            {
                luaState.AddSearchPath(directoryArray[i].FullName);
            }
        }

#if UNITY_EDITOR
        //添加工程目录中，Lua逻辑代码的所有文件夹
        rootDirectoryInfo = new DirectoryInfo(LuaConst.luaDir);
        directoryArray = rootDirectoryInfo.GetDirectories("*", SearchOption.AllDirectories);
        for (int i = 0; i < directoryArray.Length; i++)
        {
            //去掉以.开头的文件
            if (!directoryArray[i].FullName.StartsWith("."))
            {
                luaState.AddSearchPath(directoryArray[i].FullName);
            }
        }
#endif

        //绑定wrap文件
        LuaBinder.Bind(luaState);
        //注册委托
        DelegateFactory.Init();

        luaState.Start();
    }

    private void Start()
    {
        //连接服务器并获取密钥。等待服务器返回密钥以后再执行其他逻辑
        NetworkManager.Instance.Connection();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        
    }

    /// <summary>
    /// 所有需要初始化的Manager进行初始化
    /// </summary>
    private void InitManagers()
    {
        PanelManager.InitPanelManager();
    }

    /// <summary>
    /// Game场景进入时，lua的入口函数
    /// </summary>
    public void GameSceneLuaMain()
    {
        luaState.DoFile("Main.lua");
        LuaFunction mainFunction = luaState.GetFunction("Main.Main");
        mainFunction.Call();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if(luaState != null)
        {
            luaState.Dispose();
        }
    }

    /// <summary>
    /// 进入游戏主场景时调用这个方法
    /// </summary>
    public void EnterMainScene()
    {
        PanelManager.InitPanelManager();
        GameManager.Instance.GameSceneLuaMain();
    }
}
