using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EPPTools.PluginSettings
{
    public class DrawCreateAssetsBundleOptions
    {
        private static BuildBundleFunction buildBundleFunction;
        private static string targetFolderRootFolder;
        private static string bundlesName;
        private static bool autoChangeAssetBundleName;
        private static string outPutPath;
        private static BuildAssetBundleOptions buildAssetBundleOptions;
        private static BuildTarget buildTarget;
        private static string suffixName;
        /// <summary>
        /// TargetFolder模式下的lua代码根文件夹
        /// </summary>
        private static string luaTargetFolderRootFolder;
        /// <summary>
        /// 是否打包资源
        /// </summary>
        private static bool packageRes;
        /// <summary>
        /// 是否打包lua代码
        /// </summary>
        private static bool packageLuaCode;
        /// <summary>
        /// 是否生成zip文件
        /// </summary>
        private static bool generateZipFile;
        /// <summary>
        /// 生成的zip文件保存路径
        /// </summary>
        private static string zipOutPutPath;

        private static string luaOutPutPath;
        private static string zipFilePassword;
        private static string toLuaTargetFolderRootFolder;

        public static void InitCreateAssetsBundle()
        {
            if (EPPToolsSettingWindow.IncludeCreateAssetsBundle)
            {
                buildBundleFunction = (BuildBundleFunction)EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.buildBundleFunction;
                targetFolderRootFolder = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.targetFolderRootFolder;
                bundlesName = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.bundlesName;
                autoChangeAssetBundleName = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.autoChangeAssetBundleName;
                outPutPath = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.outPutPath;
                buildAssetBundleOptions = (BuildAssetBundleOptions)EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.buildAssetBundleOptions;
                buildTarget = (BuildTarget)EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.buildTarget;
                suffixName = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.suffixName;
                luaTargetFolderRootFolder = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.luaTargetFolderRootFolder;
                packageRes = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.packageRes;
                packageLuaCode = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.packageLuaCode;
                generateZipFile = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.generateZipFile;
                zipOutPutPath = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.zipOutPutPath;
                luaOutPutPath = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.luaOutPutPath;
                zipFilePassword = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.zipFilePassword;
                toLuaTargetFolderRootFolder = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.toLuaTargetFolderRootFolder;
            }
        }

        public static void Draw()
        {
            if (EPPToolsSettingWindow.IncludeCreateAssetsBundle)
            {
                buildBundleFunction = (BuildBundleFunction)EditorGUILayout.EnumPopup("资源包范围", buildBundleFunction);
                targetFolderRootFolder = EditorGUILayout.TextField("根目录", targetFolderRootFolder);
                luaTargetFolderRootFolder = EditorGUILayout.TextField("Lua代码根目录", luaTargetFolderRootFolder);
                bundlesName = EditorGUILayout.TextField("统一包名称", bundlesName);
                autoChangeAssetBundleName = EditorGUILayout.ToggleLeft("自动修改包名", autoChangeAssetBundleName);
                packageRes = EditorGUILayout.ToggleLeft("打包资源", packageRes);
                packageLuaCode = EditorGUILayout.ToggleLeft("打包Lua代码", packageLuaCode);
                generateZipFile = EditorGUILayout.ToggleLeft("生成zip文件", generateZipFile);
                zipOutPutPath = EditorGUILayout.TextField("zip文件保存路径", zipOutPutPath);
                outPutPath = EditorGUILayout.TextField("输出路径", outPutPath);
                suffixName = EditorGUILayout.TextField("后缀名", suffixName);
                luaOutPutPath = EditorGUILayout.TextField("lua保存路径", luaOutPutPath);
                zipFilePassword = EditorGUILayout.TextField("zip文件密码", zipFilePassword);
                toLuaTargetFolderRootFolder = EditorGUILayout.TextField("tolua代码根目录", toLuaTargetFolderRootFolder);
                buildAssetBundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup("压缩方式", buildAssetBundleOptions);
                buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("目标平台", buildTarget);

                if (GUILayout.Button("保存"))
                {
                    CreateAssetsBundleConfig config = new CreateAssetsBundleConfig();
                    config.buildBundleFunction = (int)buildBundleFunction;
                    config.targetFolderRootFolder = targetFolderRootFolder;
                    config.bundlesName = bundlesName;
                    config.autoChangeAssetBundleName = autoChangeAssetBundleName;
                    config.outPutPath = outPutPath;
                    config.buildAssetBundleOptions = (int)buildAssetBundleOptions;
                    config.buildTarget = (int)buildTarget;
                    config.suffixName = suffixName;
                    config.luaTargetFolderRootFolder = luaTargetFolderRootFolder;
                    config.packageRes = packageRes;
                    config.packageLuaCode = packageLuaCode;
                    config.generateZipFile = generateZipFile;
                    config.zipOutPutPath = zipOutPutPath;
                    config.luaOutPutPath = luaOutPutPath;
                    config.zipFilePassword = zipFilePassword;
                    config.toLuaTargetFolderRootFolder = toLuaTargetFolderRootFolder;

                    EPPToolsSettingAsset.Instance.SetCreateAssetsBundleConfig(config);

                    EPPToolsSettingWindow.SetShowMessage("设置成功", MessageType.Info);
                }
            }
            else
            {
                EPPToolsSettingWindow.SetShowMessage("工程中不包含创建AssetsBundle的代码，有需要请导入相关unitypackage", MessageType.Info);
            }
        }
    }
}
