using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace EPPTools.PluginSettings
{
    public class EPPToolsSettingWindow : EditorWindow
    {
        /// <summary>
        /// 当前项目中有哪些包有相关信息保存
        /// </summary>
        private enum SelectedSettingOptions
        {
            None,
            AssetHandler,
            AutoSaveScene,
            ExportUnityPackage,
            Localization,
            DebugControllable,
            CreateAssetsBundle,
            CreateToLuaFrameworkFile
        }

        private static bool includeAssetHandlePackage = false;
        /// <summary>
        /// 是否包含自定义资源打开方式的包
        /// </summary>
        public static bool IncludeAssetHandlePackage { get { return includeAssetHandlePackage; } }
        private static bool includeAutoSaveScenePackage = false;
        /// <summary>
        /// 是否包含自动保存场景的包
        /// </summary>
        public static bool IncludeAutoSaveScenePackage { get { return includeAutoSaveScenePackage; } }
        private static bool includeExportUnityPackage = false;
        /// <summary>
        /// 是否包含导出'EPP Tools工具包'的包
        /// </summary>
        public static bool IncludeExportUnityPackage { get { return includeExportUnityPackage; } }
        private static bool includeLocalizationPackage = false;
        /// <summary>
        /// 是否包含多语言插件的包（本地化）
        /// </summary>
        public static bool IncludeLocalizationPackage { get { return includeLocalizationPackage; } }
        private static bool includeDebugControllablePackage = false;
        /// <summary>
        /// 是否包含调试升级的包
        /// </summary>
        public static bool IncludeDebugControllablePackage { get { return includeDebugControllablePackage; } }
        private static bool includeCreateAssetsBundle = false;
        /// <summary>
        /// 是否包含创建AssetsBundle的包
        /// </summary>
        public static bool IncludeCreateAssetsBundle { get { return includeCreateAssetsBundle; } }
        private static bool includeCreateToLuaFrameworkFile = false;
        /// <summary>
        /// 是否包含创建EPPFramework的Ctrl和Panel文件的包
        /// </summary>
        public static bool IncludeCreateToLuaFrameworkFile { get { return includeCreateToLuaFrameworkFile; } }

        private GUIStyle settingWindowStyle = new GUIStyle();

        private SelectedSettingOptions nowSelectedOptions;
        private SelectedSettingOptions NowSelectedOptions 
        {
            get 
            {
                return nowSelectedOptions;
            } 
            set
            {
                nowSelectedOptions = value;
                showMessage = "";
            }
        }

        private static string showMessage = "";
        private static MessageType showMessageType = MessageType.Info;

        private void Awake()
        {
            settingWindowStyle.border = new RectOffset(0, 0, 0, 0);
            settingWindowStyle.margin = new RectOffset(0, 0, 0, 0);
            settingWindowStyle.padding = new RectOffset(0, 0, 5, 0);
        }

        [MenuItem("EPP Tools/EPP Tools Setting")]
        public static void ShowWindow()
        {
            EPPToolsSettingWindow window = EditorWindow.GetWindow<EPPToolsSettingWindow>("EPP Tools Setting Window");

            //在这里添加判断，决定当前工程中是否包含某个包
            CheckIncludePackage("EPPTools.AutoSaveScene.SaveSceneRunBefore", out includeAutoSaveScenePackage);
            CheckIncludePackage("EPPTools.AssetHandler.AssetHandler", out includeAssetHandlePackage);
            CheckIncludePackage("EPPTools.ExportHandler.ExportUnityPackageHandler", out includeExportUnityPackage);
            CheckIncludePackage("EPPTools.Localization.LocalizationInfoWindow", out includeLocalizationPackage);
            CheckIncludePackage("EPPTools.DebugCongrollable.EPPToolsDebug", out includeDebugControllablePackage);
            CheckIncludePackage("EPPTools.CreateAssetsBundleTools.CreateAssetsBundleWindow", out includeCreateAssetsBundle);
            CheckIncludePackage("EPPTools.CreateToLuaFrameworkFile.CreateToLuaTemplateWindow", out includeCreateToLuaFrameworkFile);

            //在这里添加对应包的初始化函数
            DrawAssetHandlerOptions.InitAssetHandler();
            DrawAutoSaveSceneOptions.InitAutoSaveScene();
            DrawExprotUnityPackageOptions.InitExportUnityPackage();
            DrawLocalizationOptions.InitLocalization();
            DrawDebugControllableOptions.InitLocalization();
            DrawCreateAssetsBundleOptions.InitCreateAssetsBundle();
            DrawCreateToLuaFrameworkFileOptions.InitCreateToLuaFrameworkFileOptions();

            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(settingWindowStyle, GUILayout.MaxWidth(180f), GUILayout.ExpandHeight(true));
            //在这里添加对应包的GUI界面绘制函数
            if (GUILayout.Button("自定义文件打开方式"))
            {
                NowSelectedOptions = SelectedSettingOptions.AssetHandler;
            }
            if (GUILayout.Button("运行前自动保存场景"))
            {
                NowSelectedOptions = SelectedSettingOptions.AutoSaveScene;
            }
            if (GUILayout.Button("导出EPP Tools工具包"))
            {
                NowSelectedOptions = SelectedSettingOptions.ExportUnityPackage;
            }
            if (GUILayout.Button("多语言支持设置"))
            {
                NowSelectedOptions = SelectedSettingOptions.Localization;
            }
            if (GUILayout.Button("可控制的日志输出"))
            {
                NowSelectedOptions = SelectedSettingOptions.DebugControllable;
            }
            if (GUILayout.Button("创建AssetsBundle"))
            {
                NowSelectedOptions = SelectedSettingOptions.CreateAssetsBundle;
            }
            if (GUILayout.Button("创建EPPFramework的Ctrl和Panel文件"))
            {
                NowSelectedOptions = SelectedSettingOptions.CreateToLuaFrameworkFile;
            }
            EditorGUILayout.EndVertical();
            //-------------
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            DrawSettingOptions();
            if (NowSelectedOptions != SelectedSettingOptions.None && !string.IsNullOrEmpty(showMessage))
            {
                EditorGUILayout.HelpBox(showMessage, showMessageType);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawSettingOptions()
        {
            //在这里添加界面绘制的函数
            switch (NowSelectedOptions)
            {
                case SelectedSettingOptions.None:
                    GUILayout.Label("选择一个选项");
                    break;
                case SelectedSettingOptions.AssetHandler:
                    DrawAssetHandlerOptions.Draw();
                    break;
                case SelectedSettingOptions.AutoSaveScene:
                    DrawAutoSaveSceneOptions.Draw();
                    break;
                case SelectedSettingOptions.ExportUnityPackage:
                    DrawExprotUnityPackageOptions.Draw();
                    break;
                case SelectedSettingOptions.Localization:
                    DrawLocalizationOptions.Draw();
                    break;
                case SelectedSettingOptions.DebugControllable:
                    DrawDebugControllableOptions.Draw();
                    break;
                case SelectedSettingOptions.CreateAssetsBundle:
                    DrawCreateAssetsBundleOptions.Draw();
                    break;
                case SelectedSettingOptions.CreateToLuaFrameworkFile:
                    DrawCreateToLuaFrameworkFileOptions.Draw();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 设置要在下方显示的提示
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public static void SetShowMessage(string message, MessageType type)
        {
            showMessage = message;
            showMessageType = type;
        }

        /// <summary>
        /// 检查项目中是否包含某个类。使用了反射机制
        /// </summary>
        private static void CheckIncludePackage(string packageFullClassName, out bool result)
        {
            Type type = Type.GetType(packageFullClassName);
            if (type != null)
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }
    }
}
