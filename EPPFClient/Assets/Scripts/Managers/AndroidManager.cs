using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 和安卓通信的管理器
/// </summary>
public class AndroidManager : Singleton<AndroidManager>
{
    //https://blog.csdn.net/wzjssssssssss/article/details/82993057     Unity Android 下载安装打开apk

    private AndroidJavaObject clipboardUtil;
    private AndroidJavaObject toastUtil;
    private AndroidJavaObject clipboardUtilInstance;

    private AndroidManager()
    {

    }

    /// <summary>
    /// 初始化管理器
    /// </summary>
    public void Init()
    {
        clipboardUtil = new AndroidJavaObject("com.QingHuiXiang.UNOProject.Utils.ClipboardUtil");
        toastUtil = new AndroidJavaObject("com.QingHuiXiang.UNOProject.Utils.ToastUtil");
        clipboardUtilInstance = clipboardUtil.CallStatic<AndroidJavaObject>("getInstance");
    }

    /// <summary>
    /// 调用安卓方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="methodName"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public T Call<T>(string methodName, params object[] args)
    {
        return clipboardUtilInstance.Call<T>(methodName, args);
    }

    /// <summary>
    /// 调用‘UNOGame.jar’里面的‘ClipboardUtil.copyToClipboard(string str)’方法。方法内会弹一个toast出来
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string CopyToClipboard(string str)
    {
        return clipboardUtilInstance.Call<string>("copyToClipboard", str);
    }

    /// <summary>
    /// 调用‘UNOGame.jar’里面的‘ToastUtil.makeToast(String text)’方法。弹一个toast出来
    /// </summary>
    /// <param name="text"></param>
    public void MakeToast(string text)
    {
        toastUtil.CallStatic<string>("makeToast", text);
    }
}
