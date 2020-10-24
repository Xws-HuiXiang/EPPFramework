using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EPPTools.PluginSettings
{
    public class DrawAssetHandlerOptions
    {
        private static int extendNameIndex = 0;
        private static int extendNameIndexLast = 0;
        private static string extendName = "";
        private static string processPath = "";
        private static Dictionary<string, string> customProcessDict = new Dictionary<string, string>();

        /// <summary>
        /// 初始化打开资源的设置
        /// </summary>
        public static void InitAssetHandler()
        {
            if (EPPToolsSettingWindow.IncludeAssetHandlePackage)
            {
                customProcessDict.Clear();
                for (int i = 0; i < EPPToolsSettingAsset.Instance.AssetHandle.keys.Count; i++)
                {
                    customProcessDict.Add(EPPToolsSettingAsset.Instance.AssetHandle.keys[i], EPPToolsSettingAsset.Instance.AssetHandle.values[i]);
                }
                if (customProcessDict.Count > 0)
                {
                    extendName = EPPToolsSettingAsset.Instance.AssetHandle.keys[extendNameIndex];
                    processPath = EPPToolsSettingAsset.Instance.AssetHandle.values[extendNameIndex];
                }

                EPPToolsSettingWindow.SetShowMessage("", MessageType.Info);
            }
        }

        /// <summary>
        /// 画GUI
        /// </summary>
        public static void Draw()
        {
            if (EPPToolsSettingWindow.IncludeAssetHandlePackage)
            {
                GUILayout.Label("自定义文件打开方式");
                EditorGUILayout.BeginHorizontal();
                extendNameIndex = EditorGUILayout.Popup("已定义后缀", extendNameIndex, EPPToolsSettingAsset.Instance.AssetHandle.keys.ToArray());
                if (extendNameIndexLast != extendNameIndex)
                {
                    extendNameIndexLast = extendNameIndex;

                    extendName = EPPToolsSettingAsset.Instance.AssetHandle.keys[extendNameIndex];
                    processPath = EPPToolsSettingAsset.Instance.AssetHandle.values[extendNameIndex];
                }
                if (GUILayout.Button("删除", GUILayout.Width(70f)))
                {
                    if (EPPToolsSettingAsset.Instance.AssetHandle.keys.Count > 0)
                    {
                        string deleteKey = EPPToolsSettingAsset.Instance.AssetHandle.keys[extendNameIndex];
                        customProcessDict.Remove(deleteKey);
                        extendNameIndex--;
                        if (extendNameIndex < 0)
                        {
                            extendNameIndex = 0;

                            extendName = "";
                            processPath = "";
                        }

                        AssetHandleConfig config = new AssetHandleConfig();
                        config.keys = new List<string>();
                        config.values = new List<string>();
                        foreach (KeyValuePair<string, string> item in customProcessDict)
                        {
                            config.keys.Add(item.Key.ToLower());
                            config.values.Add(item.Value);
                        }
                        EPPToolsSettingAsset.Instance.SetAssetHandleConfig(config);
                    }
                    else
                    {
                        EPPToolsSettingWindow.SetShowMessage("请选择一个项再删除", MessageType.Info);
                    }
                }
                EditorGUILayout.EndHorizontal();
                extendName = EditorGUILayout.TextField("后缀名", extendName);
                EditorGUILayout.BeginHorizontal();
                processPath = EditorGUILayout.TextField("应用路径", processPath);
                if (GUILayout.Button("默认程序", GUILayout.MaxWidth(70f)))
                {
                    processPath = "default";
                    //取消控件焦点刷新输入框文字
                    GUI.FocusControl(null);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("检查应用路径"))
                {
                    if (processPath == "")
                    {
                        EPPToolsSettingWindow.SetShowMessage("应用程序的路径为空", MessageType.Warning);
                    }
                    else
                    {
                        if (processPath == "default" || File.Exists(processPath))
                        {
                            EPPToolsSettingWindow.SetShowMessage("应用程序路径检查通过", MessageType.Info);
                        }
                        else
                        {
                            EPPToolsSettingWindow.SetShowMessage("应用程序路径有误，未找到文件", MessageType.Error);
                        }
                    }
                }
                if (GUILayout.Button("添加"))
                {
                    if (string.IsNullOrEmpty(extendName) || string.IsNullOrEmpty(processPath))
                    {
                        EPPToolsSettingWindow.SetShowMessage("后缀名或应用程序路径为空", MessageType.Error);

                        return;
                    }
                    if (customProcessDict.ContainsKey(extendName))
                    {
                        customProcessDict[extendName] = processPath;
                        EPPToolsSettingWindow.SetShowMessage(string.Format("已修改后缀为[{0}]的文件打开程序为[{1}]", extendName, processPath), MessageType.Info);
                    }
                    else
                    {
                        customProcessDict.Add(extendName, processPath);
                    }
                    AssetHandleConfig config = new AssetHandleConfig();
                    config.keys = new List<string>();
                    config.values = new List<string>();
                    foreach (KeyValuePair<string, string> item in customProcessDict)
                    {
                        config.keys.Add(item.Key.ToLower());
                        config.values.Add(item.Value);
                    }
                    EPPToolsSettingAsset.Instance.SetAssetHandleConfig(config);

                    EPPToolsSettingWindow.SetShowMessage("设置成功", MessageType.Info);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EPPToolsSettingWindow.SetShowMessage("工程中不包含自定义资源打开方式的代码，有需要请导入相关unitypackage", MessageType.Info);
            }
        }
    }
}
