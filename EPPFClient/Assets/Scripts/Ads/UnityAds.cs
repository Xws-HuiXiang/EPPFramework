using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// Unity的广告相关接口实现
/// </summary>
public class UnityAds : IUnityAdsListener
{
    private bool canShow = false;
    /// <summary>
    /// 广告当前是否可以展示
    /// </summary>
    public bool CanShow { get { return canShow; } }

    /// <summary>
    /// OnUnityAdsReady函数的事件
    /// </summary>
    public event Action<string> UnityAdsReadyEvent = delegate { };
    /// <summary>
    /// OnUnityAdsDidError函数的事件
    /// </summary>
    public event Action<string> UnityAdsDidErrorEvent = delegate { };
    /// <summary>
    /// OnUnityAdsDidStart函数的事件
    /// </summary>
    public event Action<string> UnityAdsDidStartEvent = delegate { };
    /// <summary>
    /// OnUnityAdsDidFinish函数的事件。广告播放失败
    /// </summary>
    public event Action<string, ShowResult> UnityAdsDidFinishFailedEvent = delegate { };
    /// <summary>
    /// OnUnityAdsDidFinish函数的事件。广告播放跳过
    /// </summary>
    public event Action<string, ShowResult> UnityAdsDidFinishSkippedEvent = delegate { };
    /// <summary>
    /// OnUnityAdsDidFinish函数的事件。广告播放完成
    /// </summary>
    public event Action<string, ShowResult> UnityAdsDidFinishFinishedEvent = delegate { };

    /// <summary>
    /// 调用Show方法时，如果广告准备好播放的事件
    /// </summary>
    public event Action AdsIsReadyEvent = delegate { };
    /// <summary>
    /// 调用Show方法时，如果广告未准备好时的事件
    /// </summary>
    public event Action AdsIsNotReadyEvent = delegate { };

    public UnityAds(string gameID)
    {
        //广告SDK初始化
        Advertisement.Initialize(gameID, AppConst.AdsTestMod);

        Advertisement.AddListener(this);
    }

    /// <summary>
    /// 接口方法。广告准备完成
    /// </summary>
    /// <param name="placementId"></param>
    public void OnUnityAdsReady(string placementId)
    {
        canShow = true;
        if(UnityAdsReadyEvent != null)
        {
            UnityAdsReadyEvent.Invoke(placementId);
        }
    }

    /// <summary>
    /// 接口方法。广告播放异常
    /// </summary>
    /// <param name="message"></param>
    public void OnUnityAdsDidError(string message)
    {
        if (UnityAdsDidErrorEvent != null)
        {
            UnityAdsDidErrorEvent.Invoke(message);
        }
    }

    /// <summary>
    /// 接口方法。广告开始播放
    /// </summary>
    /// <param name="placementId"></param>
    public void OnUnityAdsDidStart(string placementId)
    {
        canShow = false;

        if (UnityAdsDidStartEvent != null)
        {
            UnityAdsDidStartEvent.Invoke(placementId);
        }
    }

    /// <summary>
    /// 接口方法。广告播放完成
    /// </summary>
    /// <param name="placementId"></param>
    /// <param name="showResult"></param>
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:
                if(UnityAdsDidFinishFailedEvent != null)
                {
                    UnityAdsDidFinishFailedEvent.Invoke(placementId, showResult);
                }
                break;
            case ShowResult.Skipped:
                if (UnityAdsDidFinishSkippedEvent != null)
                {
                    UnityAdsDidFinishSkippedEvent.Invoke(placementId, showResult);
                }
                break;
            case ShowResult.Finished:
                if (UnityAdsDidFinishFinishedEvent != null)
                {
                    UnityAdsDidFinishFinishedEvent.Invoke(placementId, showResult);
                }
                break;
        }
    }

    /// <summary>
    /// 展示一个广告
    /// </summary>
    public void Show()
    {
        if (CanShow)
        {
            if(AdsIsReadyEvent != null)
            {
                AdsIsReadyEvent.Invoke();
            }

            Advertisement.Show();
        }
        else
        {
            //当前广告未准备好
            FDebugger.LogWarning("广告当前未准备好，请稍后再试");

            if(AdsIsNotReadyEvent != null)
            {
                AdsIsNotReadyEvent.Invoke();
            }
        }
    }

    /// <summary>
    /// 添加OnUnityAdsReady函数的事件
    /// </summary>
    /// <param name="action"></param>
    public void AddUnityAdsReadyEvent(Action<string> action)
    {
        UnityAdsReadyEvent += action;
    }
    /// <summary>
    /// 移除OnUnityAdsReady函数的事件
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUnityAdsReadyEvent(Action<string> action)
    {
        UnityAdsReadyEvent -= action;
    }

    /// <summary>
    /// 添加OnUnityAdsDidError函数的事件
    /// </summary>
    /// <param name="action"></param>
    public void AddUnityAdsDidErrorEvent(Action<string> action)
    {
        UnityAdsDidErrorEvent += action;
    }
    /// <summary>
    /// 移除OnUnityAdsDidError函数的事件
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUnityAdsDidErrorEvent(Action<string> action)
    {
        UnityAdsDidErrorEvent -= action;
    }

    /// <summary>
    /// 添加OnUnityAdsDidStart函数的事件
    /// </summary>
    /// <param name="action"></param>
    public void AddUnityAdsDidStartEvent(Action<string> action)
    {
        UnityAdsDidStartEvent += action;
    }
    /// <summary>
    /// 移除OnUnityAdsDidStart函数的事件
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUnityAdsDidStartEvent(Action<string> action)
    {
        UnityAdsDidStartEvent -= action;
    }

    /// <summary>
    /// 添加OnUnityAdsDidFinish函数的事件
    /// </summary>
    /// <param name="action"></param>
    public void AddUnityAdsDidFinishFailedEvent(Action<string, ShowResult> action)
    {
        UnityAdsDidFinishFailedEvent += action;
    }
    /// <summary>
    /// 移除OnUnityAdsDidFinish函数的事件
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUnityAdsDidFinishFailedEvent(Action<string, ShowResult> action)
    {
        UnityAdsDidFinishFailedEvent -= action;
    }

    /// <summary>
    /// 添加OnUnityAdsDidFinish函数的事件
    /// </summary>
    /// <param name="action"></param>
    public void AddUnityAdsDidFinishSkippedEvent(Action<string, ShowResult> action)
    {
        UnityAdsDidFinishSkippedEvent += action;
    }
    /// <summary>
    /// 移除OnUnityAdsDidFinish函数的事件
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUnityAdsDidFinishSkippedEvent(Action<string, ShowResult> action)
    {
        UnityAdsDidFinishSkippedEvent -= action;
    }

    /// <summary>
    /// 添加OnUnityAdsDidFinish函数的事件
    /// </summary>
    /// <param name="action"></param>
    public void AddUnityAdsDidFinishFinishedEvent(Action<string, ShowResult> action)
    {
        UnityAdsDidFinishFinishedEvent += action;
    }
    /// <summary>
    /// 移除OnUnityAdsDidFinish函数的事件
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUnityAdsDidFinishFinishedEvent(Action<string, ShowResult> action)
    {
        UnityAdsDidFinishFinishedEvent -= action;
    }

    /// <summary>
    /// 添加调用Show方法时，广告已经准备好播放的事件
    /// </summary>
    /// <param name="action"></param>
    public void AddAdsIsReadyEvent(Action action)
    {
        AdsIsReadyEvent += action;
    }
    /// <summary>
    /// 移除调用Show方法时，广告已经准备好播放的事件
    /// </summary>
    /// <param name="action"></param>
    public void RemoveIsReadyEvent(Action action)
    {
        AdsIsReadyEvent -= action;
    }

    /// <summary>
    /// 添加调用Show方法时，广告没有准备好播放的事件
    /// </summary>
    /// <param name="action"></param>
    public void AddAdsIsNotReadyEvent(Action action)
    {
        AdsIsNotReadyEvent += action;
    }
    /// <summary>
    /// 移除调用Show方法时，广告没有准备好播放的事件
    /// </summary>
    /// <param name="action"></param>
    public void RemoveAdsIsNotReadyEvent(Action action)
    {
        AdsIsNotReadyEvent -= action;
    }
}
