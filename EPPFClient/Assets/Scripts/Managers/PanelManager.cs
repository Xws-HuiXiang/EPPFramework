using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager
{
    /// <summary>
    /// 打开面板的结构。包含了面板本身的周期函数和关闭时是否删除
    /// </summary>
    public class OpenPanelStruct
    {
        /// <summary>
        /// 预制体名称，用于区别场景中有没有这个面板（游戏物体名称可能会变）
        /// </summary>
        public string prefabName;
        /// <summary>
        /// 关闭面板时自动销毁
        /// </summary>
        public bool autoDestroy;
        /// <summary>
        /// 当前panel的游戏物体
        /// </summary>
        public GameObject panelGameObject;
        /// <summary>
        /// 相当于物体的Awake方法
        /// </summary>
        public Action awakeAction;
        /// <summary>
        /// 相当于物体的Start方法
        /// </summary>
        public Action startAction;
        /// <summary>
        /// 标识了当前面板是否为第一次打开，是则执行Awake和Start方法
        /// </summary>
        public bool runAwake;
        /// <summary>
        /// 当不销毁面板时，再次显示面板后调用的方法
        /// </summary>
        public Action enableAction;
        /// <summary>
        /// 关闭面板时调用的函数
        /// </summary>
        public Action closeAction;

        /// <summary>
        /// 创建窗口时调用，将执行Awake和Start委托
        /// </summary>
        public void Init()
        {
            //awake和start方法只会在面板创建时调用
            if (runAwake)
            {
                if(awakeAction != null)
                {
                    awakeAction.Invoke();
                }

                //Awake方法先于Start方法执行
                if (startAction != null)
                {
                    startAction.Invoke();
                }

                this.runAwake = false;
            }

            if(enableAction != null)
            {
                enableAction.Invoke();
            }
        }

        /// <summary>
        /// 关闭面板时调用函数
        /// </summary>
        public void Close()
        {
            if(closeAction != null)
            {
                closeAction.Invoke();
            }
        }
    }

    private static Transform mainCanvasTrans;
    private static Transform upperLayerCanvasTrans;

    private static List<OpenPanelStruct> panelList = new List<OpenPanelStruct>();

    public static Transform MainCanvasTrans
    {
        get
        {
            return mainCanvasTrans;
        }
    }

    public static Transform UpperLayerCanvasTrans
    {
        get
        {
            return upperLayerCanvasTrans;
        }
    }

    public static RectTransform MainCanvasRectTrans
    {
        get
        {
            return mainCanvasTrans.GetComponent<RectTransform>();
        }
    }

    public static RectTransform UpperCanvasRectTrans
    {
        get
        {
            return upperLayerCanvasTrans.GetComponent<RectTransform>();
        }
    }

    /// <summary>
    /// 初始化这个管理器
    /// </summary>
    public static void InitPanelManager()
    {
        mainCanvasTrans = GameObject.Find("MainCanvas").transform;
        if(mainCanvasTrans == null)
        {
            Canvas mainCanvas = GameObject.FindObjectOfType<Canvas>();
            mainCanvasTrans = mainCanvas.transform;
        }
        if(mainCanvasTrans == null)
        {
            FDebugger.LogError("场景中没有Canvas，请创建一个Canvas");
        }

        upperLayerCanvasTrans = GameObject.Find("UpperLayerCanvas").transform;
        if(upperLayerCanvasTrans == null)
        {
            Canvas mainCanvas = GameObject.FindObjectOfType<Canvas>();
            upperLayerCanvasTrans = mainCanvas.transform;
        }
        if(upperLayerCanvasTrans == null)
        {
            FDebugger.LogWarning("场景中没有UpperLayerCanvas");
        }
    }

    /// <summary>
    /// 创建并显示一个panel
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="prefabName"></param>
    /// <param name="awake"></param>
    /// <param name="start"></param>
    /// <param name="enable"></param>
    /// <param name="close"></param>
    /// <param name="autoDestroy"></param>
    /// <returns></returns>
    public static OpenPanelStruct OpenPanel(string assetBundleName, string prefabName, Action awake, Action start, Action enable, Action close, bool autoDestroy)
    {
        GameObject panelGameObject = null;
        OpenPanelStruct openPanelStruct = new OpenPanelStruct();
        //先查找面板是否已经打开
        foreach (OpenPanelStruct item in panelList)
        {
            if (item.prefabName == prefabName)
            {
                panelGameObject = item.panelGameObject;
                openPanelStruct = item;
                break;
            }
        }
        //构建面板数据对象
        //为空则没有打开过，否则已经存在这个面板
        if (panelGameObject == null)
        {
            //构建新的面板对象
            openPanelStruct = new OpenPanelStruct()
            {
                prefabName = prefabName,
                autoDestroy = autoDestroy,
                awakeAction = awake,
                startAction = start,
                runAwake = true,
                enableAction = enable,
                closeAction = close
            };

            GameObject panelPrefab = ResourcesManager.Instance.LoadPrefabFromAssetBundle(assetBundleName, prefabName);
            if(panelPrefab != null)
            {
                panelGameObject = GameObject.Instantiate(panelPrefab, mainCanvasTrans);
                openPanelStruct.panelGameObject = panelGameObject;

                panelList.Add(openPanelStruct);
            }
            else
            {
                FDebugger.LogErrorFormat("在包{0}中没有名为{1}的游戏物体", assetBundleName, prefabName);

                return new OpenPanelStruct();
            }
        }

        panelGameObject.transform.SetSiblingIndex(mainCanvasTrans.childCount - 1);
        panelGameObject.name = prefabName;
        panelGameObject.SetActive(true);

        return openPanelStruct;
    }

    /// <summary>
    /// 关闭指定的面板
    /// </summary>
    /// <param name="ctrlName"></param>
    public static void ClosePanel(string ctrlName)
    {
        string panelName = PanelNameUtil.TryGetPanelName(ctrlName);

        OpenPanelStruct temp = new OpenPanelStruct();
        foreach (OpenPanelStruct item in panelList)
        {
            if (string.Equals(item.prefabName, panelName, StringComparison.OrdinalIgnoreCase))
            {
                //当关闭时调用Close方法
                item.Close();
                if (item.autoDestroy)
                {
                    temp = item;
                    GameObject.Destroy(item.panelGameObject);
                }
                else
                {
                    item.panelGameObject.SetActive(false);
                }

                break;
            }
        }
        if(temp.panelGameObject == null && string.IsNullOrEmpty(temp.prefabName))
        {
            FDebugger.LogWarningFormat("关闭面板失败，没有找到名为{0}的面板或预制体开启面板时预制体名称指定错误，指定的ctrlName为：{1}", panelName, ctrlName);
        }
        else
        {
            //如果面板是自动销毁模式则移除
            if (temp.autoDestroy)
            {
                panelList.Remove(temp);
            }
        }
    }

    /// <summary>
    /// 关闭最后创建的面板
    /// </summary>
    public static void CloseLastPanel()
    {
        OpenPanelStruct lastPanel = panelList[panelList.Count - 1];
        if (lastPanel.autoDestroy)
        {
            GameObject.Destroy(lastPanel.panelGameObject);
        }
        else
        {
            lastPanel.panelGameObject.SetActive(false);
        }
        lastPanel.Close();

        panelList.Remove(lastPanel);
    }

    /// <summary>
    /// 获得最后一个打开的面板的结构类
    /// </summary>
    /// <returns></returns>
    public static OpenPanelStruct GetLastPanelStruct()
    {
        if(panelList.Count > 0)
        {
            return panelList[panelList.Count - 1];
        }

        FDebugger.LogWarning("当前没有打开任何面板，无法获取最后一个面板信息");
        return null;
    }

    /// <summary>
    /// 在主画布中检测有没有打开某个面板
    /// </summary>
    /// <param name="ctrlName">面板对应的ctrl名称</param>
    /// <param name="includeDisableState">包括隐藏状态的物体</param>
    /// <returns></returns>
    public static bool CheckPanelIsOpenInMainCanvas(string ctrlName, bool includeDisableState)
    {
        if(MainCanvasTrans == null)
        {
            if (MainCanvasTrans == null)
            {
                FDebugger.LogError("场景中没有Canvas，请创建一个Canvas");
            }

            return false;
        }

        string panelName = PanelNameUtil.TryGetPanelName(ctrlName);
        Transform panel = MainCanvasTrans.Find(panelName);

        if(panel == null)
        {
            return false;
        }
        else
        {
            if (panel.gameObject.activeSelf)
            {
                return true;
            }

            //面板处于隐藏状态
            if (includeDisableState)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 在上层画布中检测有没有打开某个面板
    /// </summary>
    /// <param name="ctrlName">面板对应的ctrl名称</param>
    /// <param name="includeDisableState">包括隐藏状态的物体</param>
    /// <returns></returns>
    public static bool CheckPanelIsOpenInUpperLayerCanvas(string ctrlName, bool includeDisableState)
    {
        if (UpperLayerCanvasTrans == null)
        {
            if (UpperLayerCanvasTrans == null)
            {
                FDebugger.LogWarning("场景中没有UpperLayerCanvas");
            }

            return false;
        }

        string panelName = PanelNameUtil.TryGetPanelName(ctrlName);
        Transform panel = UpperLayerCanvasTrans.Find(panelName);
        if (panel == null)
        {
            return false;
        }
        else
        {
            if (panel.gameObject.activeSelf)
            {
                return true;
            }

            //面板处于隐藏状态
            if (includeDisableState)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 获取MainCanvas下的所有面板
    /// </summary>
    /// <returns></returns>
    public static List<Transform> GetPanelListFromMainCanvas()
    {
        List<Transform> tempList = new List<Transform>();
        if(MainCanvasTrans != null)
        {
            for(int i = 0; i < MainCanvasTrans.childCount; i++)
            {
                tempList.Add(MainCanvasTrans.GetChild(i));
            }
        }

        return tempList;
    }

    /// <summary>
    /// 获取UpperCanvas下的所有面板
    /// </summary>
    /// <returns></returns>
    public static List<Transform> GetPanelListFromUpperCanvas()
    {
        List<Transform> tempList = new List<Transform>();
        if (UpperLayerCanvasTrans != null)
        {
            for (int i = 0; i < UpperLayerCanvasTrans.childCount; i++)
            {
                tempList.Add(UpperLayerCanvasTrans.GetChild(i));
            }
        }

        return tempList;
    }

    /// <summary>
    /// 关闭所有面板
    /// </summary>
    public static void CloseAllPanel()
    {
        CloseAllMainCanvasPanel();
        CloseAllUpperCanvasPanel();
    }

    /// <summary>
    /// 关闭所有MainCanvas下的面板
    /// </summary>
    public static void CloseAllMainCanvasPanel()
    {
        List<Transform> panelList = GetPanelListFromMainCanvas();
        foreach(Transform panelTrans in panelList)
        {
            string ctrlName = PanelNameUtil.TryGetCtrlName(panelTrans.name);
            ClosePanel(ctrlName);
        }
    }

    /// <summary>
    /// 关闭所有UpperCanvas下的面板
    /// </summary>
    public static void CloseAllUpperCanvasPanel()
    {
        List<Transform> panelList = GetPanelListFromUpperCanvas();
        foreach (Transform panelTrans in panelList)
        {
            string ctrlName = PanelNameUtil.TryGetCtrlName(panelTrans.name);
            ClosePanel(ctrlName);
        }
    }
}
