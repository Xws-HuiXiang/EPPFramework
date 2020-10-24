using EPPTools.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace EPPTools.PluginSettings
{
    public class DrawLocalizationOptions
    {
        private static List<Language> supportLanguageList;
        private static string supportLanguagePath;
        private static List<string> contents;
        private static bool onlyShowLocalizationText;
        private static Language selectedLanguage;
        private static LoadAssetsMethod loadAssetsMethod;
        //private static string resourcesFolderPath;
        private static LoadAssetsMethod lastLoadAssetsMethod;
        private static bool useBaiDuTranslate;
        private static bool useGoogleTranslate;
        private static string baiDuTranslateAPPID;
        private static string baiduTranslateKey;
        private static float translateCallRate;
        private static Language resLanguage;
        private static StorageType resourcesStorageType;

        public static void InitLocalization()
        {
            if (EPPToolsSettingWindow.IncludeLocalizationPackage)
            {
                supportLanguageList = EPPToolsSettingAsset.Instance.Localization.supportLanguageList;
                supportLanguagePath = EPPToolsSettingAsset.Instance.Localization.supportLanguagePath;
                contents = EPPToolsSettingAsset.Instance.Localization.contents;
                onlyShowLocalizationText = EPPToolsSettingAsset.Instance.Localization.onlyShowLocalizationText;
                loadAssetsMethod = EPPToolsSettingAsset.Instance.Localization.loadAssetsMethod;
                //resourcesFolderPath = EPPToolsSettingAsset.Instance.Localization.resourcesFolderPath;
                lastLoadAssetsMethod = loadAssetsMethod;
                useBaiDuTranslate = EPPToolsSettingAsset.Instance.Localization.useBaiDuTranslate;
                useGoogleTranslate = EPPToolsSettingAsset.Instance.Localization.useGoogleTranslate;
                baiDuTranslateAPPID = EPPToolsSettingAsset.Instance.Localization.baiDuTranslateAPPID;
                baiduTranslateKey = EPPToolsSettingAsset.Instance.Localization.baiduTranslateKey;
                translateCallRate = EPPToolsSettingAsset.Instance.Localization.translateCallRate;
                resLanguage = EPPToolsSettingAsset.Instance.Localization.resLanguage;
                resourcesStorageType = EPPToolsSettingAsset.Instance.Localization.resourcesStorageType;
            }
        }

        public static void Draw()
        {
            if (EPPToolsSettingWindow.IncludeLocalizationPackage)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("支持的语言：");
                if (GUILayout.Button("添加", GUILayout.MaxWidth(40)))
                {
                    if (!supportLanguageList.Contains(selectedLanguage))
                    {
                        supportLanguageList.Add(selectedLanguage);
                        contents.Add("");
                    }
                }
                selectedLanguage = (Language)EditorGUILayout.EnumPopup(selectedLanguage, GUILayout.MaxWidth(100));
                if (GUILayout.Button("移除", GUILayout.MaxWidth(40)))
                {
                    if (supportLanguageList.Contains(selectedLanguage))
                    {
                        int languageIndex = supportLanguageList.IndexOf(selectedLanguage);
                        List<int> removeLanguageIndex = new List<int>();
                        for (int i = languageIndex; i < contents.Count; i += supportLanguageList.Count)
                        {
                            removeLanguageIndex.Add(i);
                        }
                        supportLanguageList.Remove(selectedLanguage);
                        for (int i = removeLanguageIndex.Count - 1; i >= 0; i--)
                        {
                            contents.RemoveAt(removeLanguageIndex[i]);
                        }
                    }
                }
                if (GUILayout.Button("清空", GUILayout.MaxWidth(40)))
                {
                    supportLanguageList.Clear();
                    contents.Clear();
                }
                EditorGUILayout.EndHorizontal();

                if (supportLanguageList.Count == 0)
                {
                    GUILayout.Label("无");
                }

                int index = 1;
                foreach (Language item in supportLanguageList)
                {
                    GUILayout.Label(index.ToString() + ":" + item.ToString());
                    index++;
                }

                loadAssetsMethod = (LoadAssetsMethod)EditorGUILayout.EnumPopup("资源加载方式", loadAssetsMethod);
                //if(loadAssetsMethod == LoadAssetsMethod.Resources)
                //{
                //    resourcesFolderPath = EditorGUILayout.TextField("Resources文件夹路径", resourcesFolderPath);
                //}
                if(loadAssetsMethod == LoadAssetsMethod.Resources)
                {
                    resourcesStorageType = (StorageType)EditorGUILayout.EnumPopup("资源存储类型", resourcesStorageType);
                }
                if (lastLoadAssetsMethod != loadAssetsMethod)
                {
                    supportLanguagePath = LocalizationUtils.TransformAssetsPath(lastLoadAssetsMethod, loadAssetsMethod, supportLanguagePath);

                    lastLoadAssetsMethod = loadAssetsMethod;
                }
                EditorGUILayout.BeginHorizontal();
                supportLanguagePath = EditorGUILayout.TextField(".lang文件路径", supportLanguagePath);
                if(GUILayout.Button("加载", GUILayout.MaxWidth(40)))
                {
                    //将目录中的lang文件加载到asset缓存
                    if (!string.IsNullOrEmpty(supportLanguagePath))
                    {
                        string folderName = LocalizationUtils.GetAssetsPath(lastLoadAssetsMethod, supportLanguagePath);
                        List<string> keysList = new List<string>();
                        List<string> valueList = new List<string>();
                        for(int i = 0; i < supportLanguageList.Count; i++)
                        {
                            string fullName = folderName + supportLanguageList[i].ToString().Replace('_', '-') + ".lang";
                            if (lastLoadAssetsMethod == LoadAssetsMethod.Resources)
                            {
                                fullName += ".txt";
                            }

                            if (!File.Exists(fullName)) continue;

                            string content;
                            try
                            {
                                content = File.ReadAllText(fullName);
                            }
                            catch (Exception e)
                            {
                                Debug.LogWarning("文件[" + fullName + "]内容读取出错。错误信息：" + e.Message);

                                continue;
                            }
                            content = content.Replace("\r", "");
                            string[] keyValuePair = content.Split('\n');
                            for (int j = 0; j < keyValuePair.Length; j++)
                            {
                                int equalIndex = keyValuePair[j].IndexOf('=');
                                if (equalIndex != -1)
                                {
                                    string key = keyValuePair[j].Substring(0, equalIndex);
                                    if (!keysList.Contains(key))
                                    {
                                        keysList.Add(key);
                                    }
                                    string resValue = keyValuePair[j].Substring(equalIndex + 1);
                                    resValue = Regex.Unescape(resValue);
                                    valueList.Add(resValue);
                                }
                            }
                        }
                        EPPToolsSettingAsset.Instance.Localization.SetKeys(keysList);
                        EPPToolsSettingAsset.Instance.Localization.SetContents(valueList);
                    }
                }
                if(GUILayout.Button("清空", GUILayout.MaxWidth(40)))
                {
                    EPPToolsSettingAsset.Instance.Localization.keys.Clear();
                    EPPToolsSettingAsset.Instance.Localization.contents.Clear();
                }
                EditorGUILayout.EndHorizontal();
                onlyShowLocalizationText = GUILayout.Toggle(onlyShowLocalizationText, "只显示带有Localization Text组件的物体");
                useBaiDuTranslate = GUILayout.Toggle(useBaiDuTranslate, "使用百度翻译功能");
                if (useBaiDuTranslate)
                {
                    baiDuTranslateAPPID = EditorGUILayout.TextField("百度翻译APP ID", baiDuTranslateAPPID);
                    baiduTranslateKey = EditorGUILayout.TextField("百度翻译Key", baiduTranslateKey);
                }
                useGoogleTranslate = GUILayout.Toggle(useGoogleTranslate, "使用谷歌翻译功能（目前不可用，以后也可能不可用）");
                if (useBaiDuTranslate || useGoogleTranslate)
                {
                    //调用api的速率
                    translateCallRate = EditorGUILayout.FloatField("调翻译API速率(多少秒一次)", translateCallRate);
                    resLanguage = (Language)EditorGUILayout.EnumPopup("翻译时原文语言类型", resLanguage);
                }

                if (GUILayout.Button("应用"))
                {
                    //刷新输入框的值
                    GUI.FocusControl(null);

                    if (supportLanguagePath.StartsWith("Assets/"))
                    {
                        supportLanguagePath = supportLanguagePath.Substring(7);
                    }
                    switch (loadAssetsMethod)
                    {
                        case LoadAssetsMethod.UnityWebRequest:
                            if (!supportLanguagePath.StartsWith("UnityWebRequest"))
                            {
                                supportLanguagePath = "UnityWebRequest:" + supportLanguagePath;
                            }
                            break;
                        case LoadAssetsMethod.StreamingAssets:
                            if (!supportLanguagePath.StartsWith("StreamingAssets"))
                            {
                                supportLanguagePath = "StreamingAssets:" + supportLanguagePath;
                            }
                            break;
                    }

                    //if (resourcesFolderPath.StartsWith("Assets"))
                    //{
                    //    resourcesFolderPath = resourcesFolderPath.Substring(7);
                    //}
                    if (translateCallRate < 0) translateCallRate = 0;

                    LocalizationConfig config = new LocalizationConfig()
                    {
                        supportLanguageList = DrawLocalizationOptions.supportLanguageList,
                        supportLanguagePath = DrawLocalizationOptions.supportLanguagePath,
                        keys = EPPToolsSettingAsset.Instance.Localization.keys,
                        contents = DrawLocalizationOptions.contents,
                        onlyShowLocalizationText = DrawLocalizationOptions.onlyShowLocalizationText,
                        loadAssetsMethod = DrawLocalizationOptions.loadAssetsMethod,
                        //resourcesFolderPath = DrawLocalizationOptions.resourcesFolderPath
                        useBaiDuTranslate = DrawLocalizationOptions.useBaiDuTranslate,
                        useGoogleTranslate = DrawLocalizationOptions.useGoogleTranslate,
                        baiDuTranslateAPPID = DrawLocalizationOptions.baiDuTranslateAPPID,
                        baiduTranslateKey = DrawLocalizationOptions.baiduTranslateKey,
                        translateCallRate = DrawLocalizationOptions.translateCallRate,
                        resLanguage = DrawLocalizationOptions.resLanguage,
                        resourcesStorageType = DrawLocalizationOptions.resourcesStorageType
                    };

                    EPPToolsSettingAsset.Instance.SetLocalizationConfig(config);
                }
            }
            else
            {
                EPPToolsSettingWindow.SetShowMessage("工程中不包含本地化插件的代码，有需要请导入相关unitypackage", MessageType.Info);
            }
        }
    }
}
