using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadManager : MonoSingleton<DownloadManager>
{
    private struct DownloadStruct
    {
        /// <summary>
        /// 下载的url
        /// </summary>
        public string url;
        /// <summary>
        /// 下载完成回调
        /// </summary>
        public Action<byte[]> doneCallback;
        /// <summary>
        /// 下载中回调
        /// </summary>
        public Action<float, ulong> downloadingCallback;
    }

    /// <summary>
    /// 资源文件目前正在下载的项。有下载项时有值，否则为null
    /// </summary>
    private static UnityWebRequest nowHotfixUnityWebRequest = null;
    /// <summary>
    /// 资源文件正在下载的回调函数
    /// </summary>
    private static Action<float, ulong> nowHotfixDownloadingAction = null;

    /// <summary>
    /// 普通文件目前正在下载的项。有下载项时有值，否则为null
    /// </summary>
    private static UnityWebRequest nowUnityWebRequest = null;
    /// <summary>
    /// 普通文件正在下载时的回调函数
    /// </summary>
    private static Action<float, ulong> nowDownloadingAction = null;
    /// <summary>
    /// 下载队列
    /// </summary>
    private static Queue<DownloadStruct> downloadingQueue = new Queue<DownloadStruct>();
    /// <summary>
    /// 当前是否正在下载东西
    /// </summary>
    private static bool downloadingNow = false;

    private void Update()
    {
        if(nowHotfixDownloadingAction != null && nowHotfixUnityWebRequest != null)
        {
            nowHotfixDownloadingAction.Invoke(nowHotfixUnityWebRequest.downloadProgress, nowHotfixUnityWebRequest.downloadedBytes);
        }

        //下载队列不为空且没有正在下载东西
        if(downloadingQueue.Count > 0 && !downloadingNow)
        {
            DownloadStruct download = downloadingQueue.Dequeue();
            if(nowUnityWebRequest != null && nowDownloadingAction != null)
            {
                nowDownloadingAction.Invoke(nowUnityWebRequest.downloadProgress, nowUnityWebRequest.downloadedBytes);
            }
            
            StartCoroutine(DownloadIE(download));

            downloadingNow = true;
        }
    }

    /// <summary>
    /// 异步下载服务器热更新资源。同时会更新一次pb文件
    /// </summary>
    /// <param name="localResVersion">本地res版本</param>
    /// <param name="serverResVersion">服务器res版本</param>
    /// <param name="localLuaVersion">本地lua版本</param>
    /// <param name="serverLuaVersion">服务器lua版本</param>
    /// <param name="doneCallback">全部下载完成时回调</param>
    /// <param name="unitDoneCallback">单个zip下载完成回调</param>
    /// <param name="downloadingCallback">下载中回调</param>
    public static void DownloadServerHotfixAsync(int localResVersion, int serverResVersion, int localLuaVersion, int serverLuaVersion, Action doneCallback, Action<HotfixFileType, string, byte[]> unitDoneCallback = null, Action<float, ulong> downloadingCallback = null)
    {
        DownloadManager.Instance.StartCoroutine(DownloadServerHotfixIE(localResVersion, serverResVersion, localLuaVersion, serverLuaVersion, doneCallback, unitDoneCallback, downloadingCallback));
    }

    /// <summary>
    /// 异步下载服务器热更新资源 具体执行逻辑的协程
    /// </summary>
    /// <param name="localResVersion">本地res版本</param>
    /// <param name="serverResVersion">服务器res版本</param>
    /// <param name="localLuaVersion">本地lua版本</param>
    /// <param name="serverLuaVersion">服务器lua版本</param>
    /// <param name="doneCallback">全部下载完成时回调</param>
    /// <param name="unitDoneCallback">单个zip下载完成回调</param>
    /// <param name="downloadingCallback">下载中回调</param>
    /// <returns></returns>
    private static IEnumerator DownloadServerHotfixIE(int localResVersion, int serverResVersion, int localLuaVersion, int serverLuaVersion, Action doneCallback, Action<HotfixFileType, string, byte[]> unitDoneCallback = null, Action<float, ulong> downloadingCallback = null)
    {
        //res
        for (int versionNumber = localResVersion + 1; versionNumber <= serverResVersion; versionNumber++)
        {
            string fileName = AppConst.ResString + versionNumber + ".zip";
            string url = AppConst.ServerResURL + fileName;
            FDebugger.Log("下载热更资源res路径：" + url);
            nowHotfixUnityWebRequest = UnityWebRequest.Get(url);
            nowHotfixDownloadingAction = downloadingCallback;

            yield return nowHotfixUnityWebRequest.SendWebRequest();

            if(nowHotfixUnityWebRequest.isNetworkError || nowHotfixUnityWebRequest.isHttpError)
            {
                FDebugger.LogErrorFormat("下载失败。下载链接：[{0}]。错误信息：\n{1}", url, nowHotfixUnityWebRequest.error);
                nowHotfixUnityWebRequest = null;
                nowHotfixDownloadingAction = null;

                yield break;
            }

            if (unitDoneCallback != null)
            {
                unitDoneCallback.Invoke(HotfixFileType.Res, fileName, nowHotfixUnityWebRequest.downloadHandler.data);
            }
            nowHotfixUnityWebRequest = null;
            nowHotfixDownloadingAction = null;
        }

        //lua
        for (int versionNumber = localLuaVersion + 1; versionNumber <= serverLuaVersion; versionNumber++)
        {
            string fileName = AppConst.LuaString + versionNumber + ".zip";
            string url = AppConst.ServerLuaURL + fileName;
            FDebugger.Log("下载热更资源lua路径：" + url);
            nowHotfixUnityWebRequest = UnityWebRequest.Get(url);
            nowHotfixDownloadingAction = downloadingCallback;

            yield return nowHotfixUnityWebRequest.SendWebRequest();

            if (nowHotfixUnityWebRequest.isNetworkError || nowHotfixUnityWebRequest.isHttpError)
            {
                FDebugger.LogErrorFormat("下载失败。下载链接：[{0}]。错误信息：\n{1}", url, nowHotfixUnityWebRequest.error);
                nowHotfixUnityWebRequest = null;
                nowHotfixDownloadingAction = null;

                yield break;
            }

            if(unitDoneCallback != null)
            {
                unitDoneCallback.Invoke(HotfixFileType.Lua, fileName, nowHotfixUnityWebRequest.downloadHandler.data);
            }
            nowHotfixUnityWebRequest = null;
            nowHotfixDownloadingAction = null;
        }

        //pb文件。只要有更新就更新一次
        UnityWebRequest pbFileListRequest = UnityWebRequest.Get(AppConst.ServerPbFileListURL);
        
        yield return pbFileListRequest.SendWebRequest();

        if (pbFileListRequest.isNetworkError || pbFileListRequest.isHttpError)
        {
            FDebugger.LogErrorFormat("下载失败。下载链接：[{0}]。错误信息：\n{1}", AppConst.ServerPbFileListURL, pbFileListRequest.error);

            yield break;
        }

        //根据url下载所有pb文件
        //检查路径是否存在
        if (!Directory.Exists(AppConst.ClientMsgPbURL))
        {
            Directory.CreateDirectory(AppConst.ClientMsgPbURL);
        }
        PbFileListData pbFileListData = JsonMapper.ToObject<PbFileListData>(pbFileListRequest.downloadHandler.text);
        foreach(string url in pbFileListData.URLList)
        {
            UnityWebRequest pbFileRequest = UnityWebRequest.Get(url);

            yield return pbFileRequest.SendWebRequest();

            if (pbFileRequest.isNetworkError || pbFileRequest.isHttpError)
            {
                FDebugger.LogErrorFormat("下载失败。下载链接：[{0}]。错误信息：\n{1}", url, pbFileRequest.error);

                yield break;
            }

            string fileFullPath = AppConst.GetPbFileFullPathByURL(url);
            FileStream fs = File.Create(fileFullPath);
            fs.Write(pbFileRequest.downloadHandler.data, 0, pbFileRequest.downloadHandler.data.Length);
            fs.Close();
        }

        //全部下载完成
        if (doneCallback != null)
        {
            doneCallback.Invoke();
        }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="url">下载的url</param>
    /// <param name="doneCallback">下载完成回调</param>
    /// <param name="downloadingCallback">下载中回调</param>
    public static void DownloadFile(string url, Action<byte[]> doneCallback, Action<float, ulong> downloadingCallback)
    {
        DownloadStruct downloadStruct = new DownloadStruct();
        downloadStruct.url = url;
        //添加一个默认下载完成的事件，用于恢复标志位
        downloadStruct.doneCallback += DefaultDoneCallback;
        downloadStruct.doneCallback += doneCallback;
        downloadStruct.downloadingCallback = downloadingCallback;

        downloadingQueue.Enqueue(downloadStruct);
    }

    /// <summary>
    /// 具体执行下载的协程
    /// </summary>
    /// <param name="download"></param>
    /// <returns></returns>
    private static IEnumerator DownloadIE(DownloadStruct download)
    {
        nowUnityWebRequest = UnityWebRequest.Get(download.url);
        nowDownloadingAction = download.downloadingCallback;

        yield return nowUnityWebRequest.SendWebRequest();

        if(nowUnityWebRequest.isNetworkError || nowUnityWebRequest.isHttpError)
        {
            FDebugger.LogErrorFormat("下载失败。下载链接：[{0}]。错误信息：\n{1}", download.url, nowUnityWebRequest.error);
            nowDownloadingAction = null;

            yield break;
        }

        if(download.doneCallback != null)
        {
            download.doneCallback(nowUnityWebRequest.downloadHandler.data);
        }

        nowDownloadingAction = null;
    }

    /// <summary>
    /// 默认下载完成的事件函数
    /// </summary>
    /// <param name="data"></param>
    private static void DefaultDoneCallback(byte[] data)
    {
        downloadingNow = false;
    }
}
