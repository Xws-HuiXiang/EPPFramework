using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;
using EPPTools.Localization;

namespace EPPTools.PluginSettings
{
    //[CreateAssetMenu(menuName = "EPP Tools/Setting Asset")]
    [Serializable]
    public class EPPToolsSettingAsset : ScriptableObject
    {
        private const string assetFilePath = "Assets/EasyPrivatePersonTools/EPPBase/Editor/EPPToolsSettingAsset.asset";

        [SerializeField]
        private AssetHandleConfig assetHandle;
        public AssetHandleConfig AssetHandle { get { return assetHandle; } }
        [SerializeField]

        private AutoSaveSceneConfig autoSaveScene;
        public AutoSaveSceneConfig AutoSaveScene { get { return autoSaveScene; } }

        [SerializeField]
        private ExportEPPToolsPackageConfig exportEPPToolsPackage;
        public ExportEPPToolsPackageConfig ExportEPPToolsPackage { get { return exportEPPToolsPackage; } }

        [SerializeField]
        private LocalizationConfig localization;
        public LocalizationConfig Localization { get { return localization; } }

        [SerializeField]
        private DebugControllableConfig debugControllable;
        public DebugControllableConfig DebugControllable { get { return debugControllable; } }

        [SerializeField]
        private CreateAssetsBundleConfig createAssetsBundleConfig;
        public CreateAssetsBundleConfig CreateAssetsBundleConfig { get { return createAssetsBundleConfig; } }

        [SerializeField]
        private CreateToLuaTemplateFileConfig createToLuaTemplateFileConfig;
        public CreateToLuaTemplateFileConfig CreateToLuaTemplateFileConfig { get { return createToLuaTemplateFileConfig; } }

        private static EPPToolsSettingAsset instance;
        public static EPPToolsSettingAsset Instance
        {
            get
            {
                if (instance == null)
                {
                    if(File.Exists(Application.dataPath + assetFilePath.Substring(6)))
                    {
                        instance = AssetDatabase.LoadAssetAtPath<EPPToolsSettingAsset>(assetFilePath);

                        UnSerializableInit(false);
                    }
                    else
                    {
                        instance = CreateInstance<EPPToolsSettingAsset>();
                        AssetDatabase.CreateAsset(instance, assetFilePath);

                        Init();
                        UnSerializableInit(true);
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// 创建资源时进行部分结构体内的值初始化
        /// </summary>
        private static void Init()
        {
            Instance.assetHandle = new AssetHandleConfig()
            {
                keys = new List<string>(),
                values = new List<string>()
            };
            Instance.autoSaveScene = new AutoSaveSceneConfig()
            {
                openAutoSave = false,
                openTimerSave = false,
                cycleTime = 5 
            };
            Instance.exportEPPToolsPackage = new ExportEPPToolsPackageConfig()
            {
                exportRootFolder = "Assets/EasyPrivatePersonTools",
                exportPath = "",
                exportAllAssetPackage = true,
                exportPartAssetPackage = true,
                mainVersion = "1.0.0",
                exportAllAssetNameFormat = "EPPTools_{PartName}_v{MainVersion}{TestFlag}.unitypackage",
                exportPartAssetNameFormat = "EPPTools_{PartName}_v{MainVersion}{SliceFlag}{PartVersion}{TestFlag}.unitypackage",
                directoryInfosSelectedState = new List<bool>(),
                //mainAssetFlag = PackageTestFlag.NoneFlag,
                //partAssetFlag = new List<PackageTestFlag>(),
                //directoryVersionList = new List<string>(),
            };
            Instance.localization = new LocalizationConfig()
            {
                supportLanguageList = new List<Language>(),
                supportLanguagePath = "",
                onlyShowLocalizationText = false,
                loadAssetsMethod = LoadAssetsMethod.Resources,
                //resourcesFolderPath = ""
                useBaiDuTranslate = false,
                useGoogleTranslate = false,
                baiDuTranslateAPPID = "",
                baiduTranslateKey = "",
                translateCallRate = 1.5f,
                contents = new List<string>(),
                resLanguage = Language.zh_cn,
                resourcesStorageType = StorageType.Path
            };
            Instance.debugControllable = new DebugControllableConfig()
            {
                developModel = true
            };
            Instance.createToLuaTemplateFileConfig = new CreateToLuaTemplateFileConfig()
            {
                ctrlFilePath = "Assets/LuaFramework/Lua/Ctrl",
                panelFilePath = "Assets/LuaFramework/Lua/View",
                addToCtrlLuaManager = true,
                luaManagerFilePath = "Assets/LuaFramework/Lua/Ctrl/CtrlLuaManager.lua",
                requireCtrlFilePath = "Ctrl",
                generateButtonListener = true,
                generateToggleListener = true,
                createCtrlFile = true,
                createPanelFile = true,
                writeInClipboard = false,
                assetBundleName = "",
                attachObjectListFoldout = true
            };
        }

        /// <summary>
        /// 不可序列化的值初始化方法
        /// </summary>
        /// <param name="isInit"></param>
        private static void UnSerializableInit(bool isInit)
        {
            //导出EPP Tools工具包的值
            string exportRootFolder = Instance.ExportEPPToolsPackage.exportRootFolder;
            bool emptyRootFolder = false;
            if (string.IsNullOrEmpty(exportRootFolder))
            {
                exportRootFolder = Application.dataPath;
                emptyRootFolder = true;
            }
            Instance.exportEPPToolsPackage.directoryInfos = new DirectoryInfo(exportRootFolder).GetDirectories();
            if (isInit || emptyRootFolder)
            {
                foreach (DirectoryInfo _ in Instance.ExportEPPToolsPackage.directoryInfos)
                {
                    Instance.ExportEPPToolsPackage.directoryInfosSelectedState.Add(true);
                    //Instance.ExportEPPToolsPackage.partAssetFlag.Add(PackageTestFlag.NoneFlag);
                    //Instance.ExportEPPToolsPackage.directoryVersionList.Add("");
                }
            }
            //more...
        }

        /// <summary>
        /// 修改 自定义资源打开方式 的设置
        /// </summary>
        /// <param name="config"></param>
        public void SetAssetHandleConfig(AssetHandleConfig config)
        {
            this.assetHandle = config;

            EditorUtility.SetDirty(EPPToolsSettingAsset.Instance);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 修改 运行前自动保存场景 的设置
        /// </summary>
        /// <param name="config"></param>
        public void SetAutoSaveSceneConfig(AutoSaveSceneConfig config)
        {
            this.autoSaveScene = config;

            EditorUtility.SetDirty(EPPToolsSettingAsset.Instance);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 修改 导出EPP Tools工具包 的设置
        /// </summary>
        /// <param name="config"></param>
        public void SetExportUnityPackageConfig(ExportEPPToolsPackageConfig config)
        {
            this.exportEPPToolsPackage = config;

            EditorUtility.SetDirty(EPPToolsSettingAsset.Instance);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 修改 导出EPP Tools工具包 的 directoryInfos字段
        /// </summary>
        /// <param name="directoryInfos"></param>
        public void SetExportUnityPackageDirectoryInfos(DirectoryInfo[] directoryInfos)
        {
            EPPToolsSettingAsset.instance.exportEPPToolsPackage.directoryInfos = directoryInfos;

            EditorUtility.SetDirty(EPPToolsSettingAsset.Instance);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 修改 多语言本地化 的配置
        /// </summary>
        /// <param name="config"></param>
        public void SetLocalizationConfig(LocalizationConfig config)
        {
            this.localization = config;

            EditorUtility.SetDirty(EPPToolsSettingAsset.Instance);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 修改 调试控制的 配置
        /// </summary>
        /// <param name="config"></param>
        public void SetDebugControllableConfig(DebugControllableConfig config)
        {
            this.debugControllable = config;

            EditorUtility.SetDirty(EPPToolsSettingAsset.Instance);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 修改 创建AssetBundle 配置
        /// </summary>
        /// <param name="config"></param>
        public void SetCreateAssetsBundleConfig(CreateAssetsBundleConfig config)
        {
            this.createAssetsBundleConfig = config;

            EditorUtility.SetDirty(EPPToolsSettingAsset.Instance);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 修改 创建EPPFramework的ctrl和panel文件 配置
        /// </summary>
        /// <param name="config"></param>
        public void SetCreateToLuaFrameworkFileConfig(CreateToLuaTemplateFileConfig config)
        {
            this.createToLuaTemplateFileConfig = config;

            EditorUtility.SetDirty(EPPToolsSettingAsset.Instance);
            AssetDatabase.SaveAssets();
        }
    }

    /// <summary>
    /// 自定义资源打开方式配置项
    /// </summary>
    [Serializable]
    public struct AssetHandleConfig
    {
        public List<string> keys;
        public List<string> values;
    }

    /// <summary>
    /// 自动保存场景用的配置项
    /// </summary>
    [Serializable]
    public struct AutoSaveSceneConfig
    {
        public bool openAutoSave;
        public bool openTimerSave;
        /// <summary>
        /// 自动保存的间隔时间
        /// </summary>
        public float cycleTime;
    }

    [Serializable]
    public struct ExportEPPToolsPackageConfig
    {
        /// <summary>
        /// 导出工具包的根路径
        /// </summary>
        public string exportRootFolder;
        public string exportPath;
        public bool exportAllAssetPackage;
        public bool exportPartAssetPackage;
        public string mainVersion;
        [Tooltip("识别的标志位：{PartName}、{MainVersion}和{TestFlag}。分别会替换为：当前包名称、插件主版本和测试标志（B、A、R或不显示）")]
        public string exportAllAssetNameFormat;
        [Tooltip("识别的标志位：{PartName}、{MainVersion}、{SliceFlag}、{PartVersion}和{TestFlag}。分别会替换为：当前包名称、插件主版本、-、附属包名称和测试标志（B、A、R或不显示）")]
        public string exportPartAssetNameFormat;

        public DirectoryInfo[] directoryInfos;
        public List<bool> directoryInfosSelectedState;
        //public PackageTestFlag mainAssetFlag;
        //public List<PackageTestFlag> partAssetFlag;
        //public List<string> directoryVersionList;
    }

    [Serializable]
    public struct LocalizationConfig
    {
        public List<Language> supportLanguageList;
        public string supportLanguagePath;
        private Dictionary<string, Dictionary<Language, string>> keyAndValues;
        public List<string> keys;
        public List<string> contents;
        public bool onlyShowLocalizationText;
        public LoadAssetsMethod loadAssetsMethod;
        //public string resourcesFolderPath;
        public bool useBaiDuTranslate;
        public bool useGoogleTranslate;

        public string baiDuTranslateAPPID;
        public string baiduTranslateKey;
        /// <summary>
        /// 翻译速率。调用API的速度（如百度免费版是限制每秒调用一次）
        /// </summary>
        public float translateCallRate;
        /// <summary>
        /// 使用翻译时，原文语言类型
        /// </summary>
        public Language resLanguage;

        public StorageType resourcesStorageType;

        public Dictionary<string, Dictionary<Language, string>> GetKeyAndValues()
        {
            if (this.keyAndValues != null) return this.keyAndValues;

            Dictionary<string, Dictionary<Language, string>> keyAndValues = new Dictionary<string, Dictionary<Language, string>>();
            if (keys == null) keys = new List<string>();

            int i = 0;
            foreach(string key in keys)
            {
                Dictionary<Language, string> data = new Dictionary<Language, string>();
                foreach (Language language in supportLanguageList)
                {
                    data.Add(language, contents[i]);
                    i++;
                }

                keyAndValues.Add(key, data);
            }

            this.keyAndValues = keyAndValues;
            return this.keyAndValues;
        }

        /// <summary>
        /// 设置keys的内容
        /// </summary>
        /// <param name="keys"></param>
        public void SetKeys(List<string> keys)
        {
            this.keys.Clear();
            foreach(string key in keys)
            {
                this.keys.Add(key);
            }
        }

        /// <summary>
        /// 设置contents内容
        /// </summary>
        /// <param name="contents"></param>
        public void SetContents(List<string> contents)
        {
            this.contents.Clear();
            foreach (string item in contents)
            {
                this.contents.Add(item);
            }
        }
    }

    [Serializable]
    public struct DebugControllableConfig
    {
        /// <summary>
        /// 是否处于开发模式
        /// </summary>
        public bool developModel;
    }

    /// <summary>
    /// 创建AssetsBundle配置文件
    /// </summary>
    [Serializable]
    public struct CreateAssetsBundleConfig
    {
        public int buildBundleFunction;
        public string targetFolderRootFolder;
        public string bundlesName;
        public bool autoChangeAssetBundleName;
        public string outPutPath;
        public int buildAssetBundleOptions;
        public int buildTarget;
        public string suffixName;
        /// <summary>
        /// TargetFolder模式下的lua代码根文件夹
        /// </summary>
        public string luaTargetFolderRootFolder;
        /// <summary>
        /// TargetFolder模式下的lua代码根文件夹
        /// </summary>
        public string toLuaTargetFolderRootFolder;
        /// <summary>
        /// 是否打包资源
        /// </summary>
        public bool packageRes;
        /// <summary>
        /// 是否打包lua代码
        /// </summary>
        public bool packageLuaCode;
        /// <summary>
        /// 是否生成zip文件
        /// </summary>
        public bool generateZipFile;
        /// <summary>
        /// 生成的zip文件保存路径 
        /// </summary>
        public string zipOutPutPath;
        /// <summary>
        /// lua文件保存路径
        /// </summary>
        public string luaOutPutPath;
        /// <summary>
        /// zip文件密码
        /// </summary>
        public string zipFilePassword;
        /// <summary>
        /// 热更新资源版本
        /// </summary>
        public int resHotfixVersion;
        /// <summary>
        /// 热更新代码版本
        /// </summary>
        public int luaHotfixVersion;
    }

    /// <summary>
    /// 创建EPPFramework的Ctrl和Panel文件的面板配置文件
    /// </summary>
    [Serializable]
    public struct CreateToLuaTemplateFileConfig
    {
        public string ctrlFilePath;
        public string panelFilePath;
        public bool addToCtrlLuaManager;
        public string luaManagerFilePath;
        public string requireCtrlFilePath;
        public bool generateButtonListener;
        public bool generateToggleListener;
        public bool createCtrlFile;
        public bool createPanelFile;
        public bool writeInClipboard;
        public string assetBundleName;
        public bool attachObjectListFoldout;
    }
}
