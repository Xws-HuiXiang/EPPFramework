using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LuaInterface;
using System.IO;
using System;
using System.Text.RegularExpressions;
using Ciphertext;

public class GameManager : MonoSingleton<GameManager>
{
    private static LuaState luaState = null;
    /// <summary>
    /// 全局的LuaState
    /// </summary>
    public static LuaState LuaState { get { return luaState; } }
    private static LuaLooper luaLooper = null;
    public static LuaLooper LuaLoop { get { return luaLooper; } }

    /// <summary>
    /// 当前用户的设置信息
    /// </summary>
    public ConfigData Config { get; set; }

    private List<Action> updateActionList = new List<Action>();
    private List<Action> fixedUpdateActionList = new List<Action>();
    private List<Action> lateUpdateActionList = new List<Action>();

    /// <summary>
    /// Unity的主线程中执行的函数队列。如果需要在Unity主线程中执行一个函数，则将函数添加到队列中
    /// </summary>
    private Queue<Action> mainThreadUpdateQueue = new Queue<Action>();

    private void Awake()
    {
        //框架入口
        DontDestroyOnLoad(this.gameObject);

        //自定义lua加载器
        new LuaCustomLoader();
        //初始化管理器
        NetworkManager.Init();

        //初始化日志输出类
        FDebugger.Init(AppConst.DevMode); 

        //构建lua环境
        luaState = new LuaState();
        
        //添加lua文件的搜索目录
        DirectoryInfo rootDirectoryInfo = new DirectoryInfo(AppConst.LocalLuaRootFolderPath);
#if UNITY_EDITOR
        LuaState.AddSearchPath(rootDirectoryInfo.FullName);
#elif UNITY_ANDROID
        //string uri = new Uri(rootDirectoryInfo.FullName).AbsoluteUri;
        //LuaState.AddSearchPath(uri);
        LuaState.AddSearchPath(rootDirectoryInfo.FullName);
#elif UNITY_IOS
        //throw new Exception("没有添加苹果平台的Lua搜索路径");
        LuaState.AddSearchPath(rootDirectoryInfo.FullName);
#else
        luaState.AddSearchPath(rootDirectoryInfo.FullName);
#endif
        //跳转到loading场景
        SceneManager.LoadScene(AppConst.SceneNameList[1]);

        //设置手机不息屏
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update()
    {
        for(int i = 0; i < updateActionList.Count; i++)
        {
            Action action = updateActionList[i];
            if(action != null) 
            {
                action.Invoke();
            }
        }

        if(mainThreadUpdateQueue != null && mainThreadUpdateQueue.Count > 0)
        {
            Action action = mainThreadUpdateQueue.Dequeue();
            if(action != null)
            {
                action.Invoke();
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < fixedUpdateActionList.Count; i++)
        {
            Action action = fixedUpdateActionList[i];
            if (action != null)
            {
                action.Invoke();
            }
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < lateUpdateActionList.Count; i++)
        {
            Action action = lateUpdateActionList[i];
            if (action != null)
            {
                action.Invoke();
            }
        }
    }

    /// <summary>
    /// Game场景进入时，lua的入口函数
    /// </summary>
    public void GameSceneLuaMain()
    {
        LuaState.DoFile("Main.lua");
        LuaFunction mainFunction = LuaState.GetFunction("Main.Main");
        if(mainFunction != null)
        {
            mainFunction.Call();
        }
        else
        {
            FDebugger.LogWarning("Main.lua文件中不存在‘Main.Main’入口方法");
        }
    }

    /// <summary>
    /// 进入游戏主场景时调用这个方法
    /// </summary>
    public void EnterMainScene()
    {
        PanelManager.InitPanelManager();
        AndroidManager.Instance.Init();
        GameManager.Instance.GameSceneLuaMain();
        AudioManager.Instance.Init();
    }

    #region Update相关方法定义
    /// <summary>
    /// 添加GameManager的update事件
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateAction(Action action)
    {
        if(action != null)
        {
            updateActionList.Add(action);
        }
    }

    /// <summary>
    /// 移除GameManager的update事件
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateAction(Action action)
    {
        if (updateActionList.Contains(action))
        {
            updateActionList.Remove(action);
        }
        else
        {
            FDebugger.LogWarningFormat("移除监听失败。updateActionList中不存在事件{0}", action.ToString());
        }
    }

    /// <summary>
    /// 清空GameManager的update事件
    /// </summary>
    public void ClearUpdateAction()
    {
        if(updateActionList != null)
        {
            updateActionList.Clear();
        }
    }

    /// <summary>
    /// 判断Update中是否存在action
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool ContainsUpdateAction(Action action)
    {
        return updateActionList.Contains(action);
    }

    /// <summary>
    /// UpdateActionList的监听事件数量
    /// </summary>
    public int UpdateActionListCount { get { return updateActionList.Count; } }

    /// <summary>
    /// 添加GameManager的FixedUpdate事件
    /// </summary>
    /// <param name="action"></param>
    public void AddFixedUpdateAction(Action action)
    {
        if(action != null)
        {
            fixedUpdateActionList.Add(action);
        }
    }

    /// <summary>
    /// 移除GameManager的FixedUpdate事件
    /// </summary>
    /// <param name="action"></param>
    public void RemoveFixedUpdateAction(Action action)
    {
        if (fixedUpdateActionList.Contains(action))
        {
            fixedUpdateActionList.Remove(action);
        }
        else
        {
            FDebugger.LogWarningFormat("移除监听失败。fixedUpdateActionList中不存在事件{0}", action.ToString());
        }
    }

    /// <summary>
    /// 清空GameManager的FixedUpdate事件
    /// </summary>
    public void ClearFixedUpdateAction()
    {
        fixedUpdateActionList.Clear();
    }

    /// <summary>
    /// 添加GameManager的LateUpdate事件
    /// </summary>
    /// <param name="action"></param>
    public void AddLateUpdateAction(Action action)
    {
        if (action != null)
        {
            lateUpdateActionList.Add(action);
        }
    }

    /// <summary>
    /// 移除GameManager的LateUpdate事件
    /// </summary>
    /// <param name="action"></param>
    public void RemoveLateUpdateAction(Action action)
    {
        if (lateUpdateActionList.Contains(action))
        {
            lateUpdateActionList.Remove(action);
        }
        else
        {
            FDebugger.LogWarningFormat("移除监听失败。lateUpdateActionList中不存在事件{0}", action.ToString());
        }
    }

    /// <summary>
    /// 清空GameManager的LateUpdate事件
    /// </summary>
    public void ClearLateUpdateAction()
    {
        if (lateUpdateActionList != null)
        {
            lateUpdateActionList.Clear();
        }
    }
    #endregion

    /// <summary>
    /// 注册LuaProtobuf部分
    /// </summary>
    public void OpenLuaProtobuf()
    {
        LuaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
        LuaState.OpenLibs(LuaDLL.luaopen_pb);
        LuaState.LuaSetField(-2, "pb");

        LuaState.OpenLibs(LuaDLL.luaopen_pb_io);
        LuaState.LuaSetField(-2, "pb.io");

        LuaState.OpenLibs(LuaDLL.luaopen_pb_conv);
        LuaState.LuaSetField(-2, "pb.conv");

        LuaState.OpenLibs(LuaDLL.luaopen_pb_buffer);
        LuaState.LuaSetField(-2, "pb.buffer");

        LuaState.OpenLibs(LuaDLL.luaopen_pb_slice);
        LuaState.LuaSetField(-2, "pb.slice");
    }

    /// <summary>
    /// 设置LuaLooper的值
    /// </summary>
    /// <param name="loop"></param>
    public static void SetLuaLooper(LuaLooper loop)
    {
        GameManager.luaLooper = loop;
    }

    /// <summary>
    /// 添加主线程update函数的执行队列
    /// </summary>
    /// <param name="action"></param>
    public void AddMainThreadUpdateQueueAction(Action action)
    {
        if(action != null)
        {
            mainThreadUpdateQueue.Enqueue(action);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (LuaState != null)
        {
            LuaState.Dispose();
        }
    }
}
