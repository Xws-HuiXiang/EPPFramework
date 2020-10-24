using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using EPPTools.PluginSettings;
using System;
using EPPTools.Utils;

namespace EPPTools.CreateAssetsBundleTools
{
    [Serializable]
    public class CreateAssetsBundleWindow : EditorWindow
    {
        /// <summary>
        /// 资源包打包的资源范围
        /// </summary>
        [SerializeField]
        private static BuildBundleFunction buildBundleFunction = BuildBundleFunction.TargetFolder;

        /// <summary>
        /// TargetFolder模式下的资源根文件夹
        /// </summary>
        [SerializeField]
        private static string resTargetFolderRootFolder;
        /// <summary>
        /// TargetFolder模式下的lua代码根文件夹
        /// </summary>
        [SerializeField]
        private static string luaTargetFolderRootFolder;
        /// <summary>
        /// TargetFolder模式下的Tolua代码根文件夹。ToLua框架提供的基础lua脚本目录
        /// </summary>
        [SerializeField]
        private static string toLuaTargetFolderRootFolder;
        /// <summary>
        /// 是否打包资源
        /// </summary>
        [SerializeField]
        private static bool packageRes;
        /// <summary>
        /// 是否打包lua代码
        /// </summary>
        [SerializeField]
        private static bool packageLuaCode;
        /// <summary>
        /// lua文件输出路径
        /// </summary>
        [SerializeField]
        private static string luaOutPutPath;
        /// <summary>
        /// 是否生成zip文件
        /// </summary>
        [SerializeField]
        private static bool generateZipFile;
        /// <summary>
        /// zip压缩文件密码
        /// </summary>
        [SerializeField]
        private static string zipFilePassword;
        /// <summary>
        /// zip文件保存路径
        /// </summary>
        [SerializeField]
        private static string zipOutPutPath;

        /// <summary>
        /// 统一bundle的名称
        /// </summary>
        [SerializeField]
        private static string bundlesName;
        /// <summary>
        /// 打包前自动修改包名
        /// </summary>
        [SerializeField]
        private static bool autoChangeAssetBundleName;

        /// <summary>
        /// AB包输出路径
        /// </summary>
        [SerializeField]
        private static string outPutPath;
        /// <summary>
        /// ab包压缩设置
        /// </summary>
        [SerializeField]
        private static BuildAssetBundleOptions buildAssetBundleOptions;
        /// <summary>
        /// ab包目标平台
        /// </summary>
        [SerializeField]
        private static BuildTarget buildTarget;
        /// <summary>
        /// 后缀名
        /// </summary>
        [SerializeField]
        private static string suffixName;

        [MenuItem("EPP Tools/Create Assets Bundle")]
        public static void OpenWindow()
        {
            CreateAssetsBundleWindow window = EditorWindow.GetWindow<CreateAssetsBundleWindow>("Assets Bundle工具");

            buildBundleFunction = (BuildBundleFunction)EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.buildBundleFunction;
            resTargetFolderRootFolder = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.targetFolderRootFolder;
            bundlesName = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.bundlesName;
            autoChangeAssetBundleName = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.autoChangeAssetBundleName;
            outPutPath = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.outPutPath;
            buildAssetBundleOptions = (BuildAssetBundleOptions)EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.buildAssetBundleOptions;
            buildTarget = (BuildTarget)EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.buildTarget;
            suffixName = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.suffixName;
            luaTargetFolderRootFolder = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.luaTargetFolderRootFolder;
            //toLuaTargetFolderRootFolder = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.toLuaTargetFolderRootFolder;
            packageRes = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.packageRes;
            packageLuaCode = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.packageLuaCode;
            generateZipFile = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.generateZipFile;
            //zipFileSavePath = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.zipOutPutPath;
            toLuaTargetFolderRootFolder = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.toLuaTargetFolderRootFolder;
            zipFilePassword = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.zipFilePassword;
            zipOutPutPath = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.zipOutPutPath;
            luaOutPutPath = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.luaOutPutPath;

            window.Show();
        }

        private void OnGUI()
        {
            buildBundleFunction = (BuildBundleFunction)EditorGUILayout.EnumPopup("资源打包范围", buildBundleFunction);
            switch (buildBundleFunction)
            {
                case BuildBundleFunction.AllAssets:

                    break;
                case BuildBundleFunction.TargetFolder:
                    PickUpPathButtonGUI("根目录", ref resTargetFolderRootFolder);
                    EditorGUILayout.BeginHorizontal();
                    bundlesName = EditorGUILayout.TextField("统一包名称为", bundlesName);
                    if (GUILayout.Button("修改", GUILayout.MaxWidth(60f)))
                    {
                        if (!string.IsNullOrEmpty(bundlesName))
                        {
                            ClearAllAssetsBundleName();
                            SetAssetBundleName(resTargetFolderRootFolder, bundlesName);
                        }
                    }
                    autoChangeAssetBundleName = EditorGUILayout.ToggleLeft("自动修改包名", autoChangeAssetBundleName, GUILayout.MaxWidth(100));
                    EditorGUILayout.EndHorizontal();
                    break;
                case BuildBundleFunction.DistributedFolder:
                    PickUpPathButtonGUI("资源根目录", ref resTargetFolderRootFolder);
                    PickUpPathButtonGUI("Lua代码根目录", ref luaTargetFolderRootFolder);
                    PickUpPathButtonGUI("ToLua代码根目录", ref toLuaTargetFolderRootFolder);
                    packageRes = EditorGUILayout.ToggleLeft("打包资源", packageRes);
                    packageLuaCode = EditorGUILayout.ToggleLeft("打包Lua代码", packageLuaCode);
                    if (packageLuaCode)
                    {
                        PickUpPathButtonGUI("lua文件保存路径", ref luaOutPutPath);
                    }
                    generateZipFile = EditorGUILayout.ToggleLeft("生成zip文件", generateZipFile);
                    if (generateZipFile)
                    {
                        zipFilePassword = EditorGUILayout.TextField("zip文件密码", zipFilePassword);
                        PickUpPathButtonGUI("zip文件保存路径", ref zipOutPutPath);
                    }
                    break;
            }
            PickUpPathButtonGUI("ab包保存路径", ref outPutPath);

            suffixName = EditorGUILayout.TextField("后缀名", suffixName);
            buildAssetBundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup("压缩方式", buildAssetBundleOptions);
            buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("目标平台", buildTarget);

            if (GUILayout.Button("打包"))
            {
                if (packageRes)
                {
                    //打包资源
                    //清理文件夹
                    ClearFolder(Application.dataPath + outPutPath.Substring(6));

                    //单独处理分文件夹打包的情况
                    if (buildBundleFunction == BuildBundleFunction.DistributedFolder)
                    {
                        //重新设置目标文件夹下的所有资源的ab包名称
                        ResetAssetsBundleName(resTargetFolderRootFolder, null);

                        BuildPipeline.BuildAssetBundles(outPutPath, buildAssetBundleOptions, buildTarget);
                    }
                    else
                    {
                        //当选择为目标文件夹模式时统一修改包名称
                        if (buildBundleFunction == BuildBundleFunction.TargetFolder && autoChangeAssetBundleName)
                        {
                            SetAssetBundleName(resTargetFolderRootFolder, bundlesName);
                        }

                        BuildPipeline.BuildAssetBundles(outPutPath, buildAssetBundleOptions, buildTarget);
                    }
                }
                if (packageLuaCode)
                {
                    //lua代码。不打成ab包，直接将文件复制到目标目录
                    if (!Directory.Exists(luaOutPutPath))
                    {
                        Directory.CreateDirectory(luaOutPutPath);
                    }
                    List<string> ignore = new List<string>();
                    ignore.Add(".meta");
                    //复制项目的业务代码
                    DirectoryUtils.CopyDirectory(luaTargetFolderRootFolder, luaOutPutPath, ignore, true);
                    //复制ToLua框架代码
                    DirectoryUtils.CopyDirectory(toLuaTargetFolderRootFolder, luaOutPutPath, ignore, true);
                }
                if (generateZipFile)
                {
                    if (!Directory.Exists(zipOutPutPath))
                    {
                        Directory.CreateDirectory(zipOutPutPath);
                    }

                    //资源的zip包
                    string assetZIPName = Application.dataPath + zipOutPutPath.Substring(6) + "/Res_" + System.DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".zip";
                    ZIPFileUtil.CreateZIPFile(assetZIPName, Application.dataPath + outPutPath.Substring(6), zipFilePassword, null);
                    //lua代码的zip包
                    string luaZIPName = Application.dataPath + zipOutPutPath.Substring(6) + "/Lua_" + System.DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".zip";
                    ZIPFileUtil.CreateZIPFile(luaZIPName, Application.dataPath + luaOutPutPath.Substring(6), zipFilePassword, null);
                }

                SaveConfig();

                AssetDatabase.Refresh();

                Debug.Log("资源打包完成");
            }
        }

        /// <summary>
        /// 重新设置所有资源的ab名称
        /// </summary>
        /// <param name="rootFolder">打包时的ab包根路径</param>
        /// <param name="abName">ab包的名称。如果为null则使用资源所在的文件夹名称</param>
        private void ResetAssetsBundleName(string rootFolder, string abName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + rootFolder.Substring(6));
            DirectoryInfo[] childDirectoryInfos = directoryInfo.GetDirectories();

            //清理包名称
            ClearAllAssetsBundleName();

            //设置所有的资源名称
            for (int i = 0; i < childDirectoryInfos.Length; i++)
            {
                //如果不指定ab名称则使用文件夹名称作为ab包的名称
                string bundleName = (abName != null) ? abName : childDirectoryInfos[i].Name;

                if (!string.IsNullOrEmpty(suffixName))
                {
                    char firstChar = suffixName[0];
                    if (firstChar != '.')
                    {
                        bundleName += ".";
                    }
                    bundleName += suffixName;
                }
                SetAssetBundleName(rootFolder + "\\" + childDirectoryInfos[i].Name, bundleName);
            }
        }

        /// <summary>
        /// 设置目标文件夹下的所有文件的AssetBundle名称
        /// </summary>
        /// <param name="rootFolderPath"></param>
        /// <param name="bundlesName"></param>
        private void SetAssetBundleName(string rootFolderPath, string bundlesName)
        {
            //名称分隔
            string[] arr = bundlesName.Split('.');

            //遍历文件夹下的所有文件并设置包名
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + rootFolderPath.Substring(6));
            //获取所有文件
            FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            List<string> filePathList = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                //处理文件路径为unity工程内的局部路径
                string fileFullName = files[i].FullName;
                int startIndex = fileFullName.IndexOf("Assets\\");
                string localPath = "";
                if (startIndex != -1)
                {
                    localPath = fileFullName.Substring(startIndex, fileFullName.Length - startIndex);
                }
                filePathList.Add(localPath);
            }
            foreach (string path in filePathList)
            {
                //剔除meta文件
                if (!path.EndsWith(".meta"))
                {
                    AssetImporter importer = AssetImporter.GetAtPath(path);
                    if(importer != null)
                    {
                        importer.assetBundleName = arr[0];
                        if (arr.Length > 1)
                        {
                            importer.assetBundleVariant = arr[1];
                        }
                    }
                    else
                    {
                        Debug.LogWarningFormat("importer为空，资源路径为：{0}", path);
                    }
                }
            }
        }

        /// <summary>
        /// 清理所有的资源ab包名称
        /// </summary>
        private void ClearAllAssetsBundleName()
        {
            //清理所有的ab包名称
            string[] allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < allAssetBundleNames.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(allAssetBundleNames[i], true);
            }
        }

        /// <summary>
        /// 清空文件夹
        /// </summary>
        /// <param name="dir"></param>
        private void ClearFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件 
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        ClearFolder(d1.FullName);////递归删除子文件夹
                    }
                    Directory.Delete(d);
                }
            }
        }

        /// <summary>
        /// 绘制拾取路径按钮
        /// </summary>
        /// <param name="outPutString"></param>
        private void PickUpPathButtonGUI(string fieldTitle, ref string outPutString)
        {
            EditorGUILayout.BeginHorizontal();
            outPutString = EditorGUILayout.TextField(fieldTitle, outPutString);
            if (GUILayout.Button("拾取", GUILayout.MaxWidth(60f)))
            {
                if (Selection.assetGUIDs.Length > 0)
                {
                    string selectedGUID = Selection.assetGUIDs[0];
                    string assetsPath = AssetDatabase.GUIDToAssetPath(selectedGUID);
                    if (assetsPath.Contains("."))
                    {
                        //是一个文件，选择当前文件所在的文件夹
                        int lastSlice = assetsPath.LastIndexOf('/');
                        outPutString = assetsPath.Substring(0, lastSlice);
                    }
                    else
                    {
                        //选了一个文件夹，直接赋值路径
                        outPutString = AssetDatabase.GUIDToAssetPath(selectedGUID);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void SaveConfig()
        {
            CreateAssetsBundleConfig config = new CreateAssetsBundleConfig();

            config.buildBundleFunction = (int)buildBundleFunction;
            config.targetFolderRootFolder = resTargetFolderRootFolder;
            config.bundlesName = "空";
            config.autoChangeAssetBundleName = autoChangeAssetBundleName;
            config.outPutPath = outPutPath;
            config.buildAssetBundleOptions = (int)buildAssetBundleOptions;
            config.buildTarget = (int)buildTarget;
            config.suffixName = suffixName;
            config.luaTargetFolderRootFolder = luaTargetFolderRootFolder;
            config.toLuaTargetFolderRootFolder = toLuaTargetFolderRootFolder;
            config.packageRes = packageRes;
            config.packageLuaCode = packageLuaCode;
            config.generateZipFile = generateZipFile;
            config.zipOutPutPath = zipOutPutPath;
            config.luaOutPutPath = luaOutPutPath;
            config.zipFilePassword = zipFilePassword;

            EPPToolsSettingAsset.Instance.SetCreateAssetsBundleConfig(config);
        }
    }
}
