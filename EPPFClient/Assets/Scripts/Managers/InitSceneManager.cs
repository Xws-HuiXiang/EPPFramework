using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;

/// <summary>
/// 初始化场景的管理器。主要为检测场景ab包的热更新。这个场景不能热更，而且要求FirstScene场景ab包较小否则这个场景会卡的时间久
/// </summary>
public class InitSceneManager : MonoBehaviour
{
    /// <summary>
    /// 场景版本信息数据对象
    /// </summary>
    private class SceneVersionData
    {
        /// <summary>
        /// FirstScene版本信息
        /// </summary>
        public int FirstScene { get; set; }
        /// <summary>
        /// LoadingScene版本信息
        /// </summary>
        public int LoadingScene { get; set; }
        /// <summary>
        /// GameScene版本信息
        /// </summary>
        public int GameScene { get; set; }
    }

    /// <summary>
    /// 场景版本数据对象
    /// </summary>
    private SceneVersionData sceneVersionData;

    private Text progressText;

    private bool firstSceneIsReady = false;
    private bool loadingSceneIsReady = false;
    private bool gameSceneIsReady = false;

    private void Awake()
    {
        if (AppConst.LoadProjectScene)
        {
            //加载工程目录中的场景
            SceneManager.LoadScene(AppConst.SceneNameList[0]);
        }
        else
        {
            //加载持久化目录中的场景文件
            progressText = GameObject.Find("Canvas/ProgressText").GetComponent<Text>();

            TryCreateDirectory();

            //先检查是否存在场景版本信息
            if (File.Exists(AppConst.LocalSceneVersionPath))
            {
                //文件存在，加载信息
                sceneVersionData = JsonMapper.ToObject<SceneVersionData>(File.ReadAllText(AppConst.LocalSceneVersionPath));

                //对比服务器的版本
                ComparedSceneVersion();
            }
            else
            {
                //文件不存在，下载文件
                DownloadManager.DownloadFile(AppConst.ServerSceneVersionURL, DownloadSceneVersionFileDone, DownloadingSceneVersion);
            }
        }
    }

    /// <summary>
    /// 尝试创建Scene相关的ab包文件夹
    /// </summary>
    private void TryCreateDirectory()
    {
        if (!Directory.Exists(AppConst.LocalSceneVersionFolderPath))
        {
            Directory.CreateDirectory(AppConst.LocalSceneVersionFolderPath);
        }
    }

    /// <summary>
    /// 场景版本信息文件下载完成回调
    /// </summary>
    /// <param name="data"></param>
    private void DownloadSceneVersionFileDone(byte[] data)
    {
        string jsonString = Encoding.UTF8.GetString(data);

        sceneVersionData = JsonMapper.ToObject<SceneVersionData>(jsonString);
        
        //对比服务器的版本
        ComparedSceneVersion();
    }

    /// <summary>
    /// 场景版本信息文件下载中回调
    /// </summary>
    /// <param name="progress"></param>
    /// <param name="length"></param>
    private void DownloadingSceneVersion(float progress, ulong length)
    {
        SetProgressText(progress);
    }

    /// <summary>
    /// 对比服务器版本
    /// </summary>
    private void ComparedSceneVersion()
    {
        DownloadManager.DownloadFile(AppConst.ServerSceneVersionURL, (data) =>
        {
            string jsonString = Encoding.UTF8.GetString(data);

            SceneVersionData serverSceneVersionData = JsonMapper.ToObject<SceneVersionData>(jsonString);

            //按顺序检查每一个文件
            //FirstScene
            if (!File.Exists(AppConst.LocalFirstScenePath))
            {
                //文件不存在，下载
                DownloadSceneABFile(AppConst.SceneNameList[0], serverSceneVersionData.FirstScene, AppConst.LocalFirstScenePath);
            }
            else
            {
                //文件存在，对比版本，服务器版本较新则下载
                if(serverSceneVersionData.FirstScene > sceneVersionData.FirstScene)
                {
                    DownloadSceneABFile(AppConst.SceneNameList[0], serverSceneVersionData.FirstScene, AppConst.LocalFirstScenePath);
                }
                else
                {
                    //服务器的场景没有更新
                    firstSceneIsReady = true;
                }
            }
            //LoadingScene
            if (!File.Exists(AppConst.LocalLoadingScenePath))
            {
                //文件不存在，下载
                DownloadSceneABFile(AppConst.SceneNameList[1], serverSceneVersionData.LoadingScene, AppConst.LocalLoadingScenePath);
            }
            else
            {
                //文件存在，对比版本，服务器版本较新则下载
                if (serverSceneVersionData.LoadingScene > sceneVersionData.LoadingScene)
                {
                    DownloadSceneABFile(AppConst.SceneNameList[1], serverSceneVersionData.LoadingScene, AppConst.LocalLoadingScenePath);
                }
                else
                {
                    //服务器的场景没有更新
                    loadingSceneIsReady = true;
                }
            }
            //GameScene
            if (!File.Exists(AppConst.LocalGameScenePath))
            {
                //文件不存在，下载
                DownloadSceneABFile(AppConst.SceneNameList[2], serverSceneVersionData.GameScene, AppConst.LocalGameScenePath);
            }
            else
            {
                //文件存在，对比版本，服务器版本较新则下载
                if (serverSceneVersionData.GameScene > sceneVersionData.GameScene)
                {
                    DownloadSceneABFile(AppConst.SceneNameList[2], serverSceneVersionData.GameScene, AppConst.LocalGameScenePath);
                }
                else
                {
                    //服务器的场景没有更新
                    gameSceneIsReady = true;
                }
            }
            //检查版本完成，将最新的服务器版本信息写入本地文件中
            if (File.Exists(AppConst.LocalSceneVersionPath))
            {
                File.Delete(AppConst.LocalSceneVersionPath);
            }
            FileStream fs = File.Create(AppConst.LocalSceneVersionPath);
            string newSceneVersionJsonString = JsonMapper.ToJson(serverSceneVersionData);
            byte[] newSceneVersionData = Encoding.UTF8.GetBytes(newSceneVersionJsonString);
            fs.Write(newSceneVersionData, 0, newSceneVersionData.Length);

            //如果没有更新，尝试进入第一个场景
            TryEnterFirstScene();
        },
        DownloadingSceneVersion);
    }

    /// <summary>
    /// 从服务器上下载场景的ab包
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="serverSceneVersion"></param>
    /// <param name="localScenePath"></param>
    private void DownloadSceneABFile(string sceneName, int serverSceneVersion, string localScenePath)
    {
        DownloadManager.DownloadFile(AppConst.ServerSceneFileURL(serverSceneVersion, sceneName), (data) =>
        {
            FileStream fs = File.Create(localScenePath);
            fs.Write(data, 0, data.Length);
            fs.Close();

            //更新下载标志位
            if (sceneName.Equals(AppConst.SceneNameList[0]))
            {
                firstSceneIsReady = true;
            }
            else if (sceneName.Equals(AppConst.SceneNameList[1]))
            {
                loadingSceneIsReady = true;
            }
            else if (sceneName.Equals(AppConst.SceneNameList[2]))
            {
                gameSceneIsReady = true;
            }

            //等待所有场景下载完成后加载场景到内存中并进入第一个场景
            TryEnterFirstScene();
        }, null);
    }

    /// <summary>
    /// 如果三个场景都检查并下载完成则进入第一个场景
    /// </summary>
    private void TryEnterFirstScene()
    {
        //三个场景是否已经准备完成
        if (firstSceneIsReady && loadingSceneIsReady && gameSceneIsReady)
        {
            //将三个场景加载到内存
            AssetBundle.LoadFromFile(AppConst.LocalFirstScenePath);
            AssetBundle.LoadFromFile(AppConst.LocalLoadingScenePath);
            AssetBundle.LoadFromFile(AppConst.LocalGameScenePath);
            //跳转到第一个场景开始游戏逻辑
            SceneManager.LoadScene(AppConst.SceneNameList[0]);

            //TODO:正常情况下不会返回FirstScene和LoadingScene，可以将这两个ab包内存卸载（内存占用不高，问题不大~）
        }
    }

    /// <summary>
    /// 设置进度文字
    /// </summary>
    /// <param name="progress"></param>
    private void SetProgressText(float progress)
    {
        progressText.text = Mathf.Ceil(progress * 100).ToString();
    }
}
