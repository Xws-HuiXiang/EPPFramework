using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System;
using EPPTools.ExportHandler;

namespace EPPTools.PluginSettings
{
    public class DrawExprotUnityPackageOptions
    {
        private static string exportRootFolder;
        private static string exportPath;
        private static bool exportAllAssetPackage;
        private static bool exportPartAssetPackage;
        private static string mainVersion;
        private static string exportAllAssetNameFormat;
        private static string exportPartAssetNameFormat;

        private static DirectoryInfo[] directoryInfos;
        private static List<bool> directoryInfosSelectedState;
        private static PackageTestFlag mainAssetFlag;
        private static List<PackageTestFlag> partAssetFlag = new List<PackageTestFlag>();
        private static List<string> directoryVersionList = new List<string>();

        private static bool foldout = true;
        private static Vector2 scrollViewPosition;

        public static void InitExportUnityPackage()
        {
            //EPPToolsSettingAsset.UpdateUnityPackageOptions();

            exportRootFolder = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.exportRootFolder;
            exportPath = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.exportPath;
            exportAllAssetPackage = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.exportAllAssetPackage;
            exportPartAssetPackage = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.exportPartAssetPackage;
            mainVersion = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.mainVersion;
            exportAllAssetNameFormat = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.exportAllAssetNameFormat;
            exportPartAssetNameFormat = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.exportPartAssetNameFormat;

            //string rootFolder = string.IsNullOrEmpty(exportRootFolder) ? Application.dataPath : exportRootFolder;
            //directoryInfos = new DirectoryInfo(rootFolder).GetDirectories();
            //EPPToolsSettingAsset.Instance.SetExportUnityPackageDirectoryInfos(directoryInfos);

            //directoryInfosSelectedState = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.directoryInfosSelectedState;
            //if(directoryInfos.Length != directoryInfosSelectedState.Count)
            //{
            //    directoryInfosSelectedState.Clear();
            //    for(int i = 0; i < directoryInfos.Length; i++)
            //    {
            //        directoryInfosSelectedState.Add(true);
            //    }
            //}

            //mainAssetFlag = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.mainAssetFlag;
            //partAssetFlag = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.partAssetFlag;
            //directoryVersionList = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.directoryVersionList;

            partAssetFlag.Clear();
            directoryVersionList.Clear();

            DirectoryInfo[] oldDirectoryInfos = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.directoryInfos;
            List<bool> oldDirectoryInfosSelectedState = EPPToolsSettingAsset.Instance.ExportEPPToolsPackage.directoryInfosSelectedState;
            directoryInfosSelectedState = new List<bool>();

            string rootFolder = string.IsNullOrEmpty(exportRootFolder) ? Application.dataPath : exportRootFolder;
            directoryInfos = new DirectoryInfo(rootFolder).GetDirectories();
            //if(oldDirectoryInfos != null)
            //{

            //如果创建了新文件则添加一个
            if(directoryInfos.Length > oldDirectoryInfosSelectedState.Count)
            {
                oldDirectoryInfosSelectedState.Add(true);
            }

            //修改选择状态
            List<string> newFolderNameList = new List<string>();
            for(int i = 0; i < directoryInfos.Length; i++)
            {
                newFolderNameList.Add(directoryInfos[i].Name);
                directoryInfosSelectedState.Add(true);

                //更新附属包版本信息
                UpdateVersionInfo(directoryInfos[i].FullName + "\\Readme.md", ref directoryVersionList, ref partAssetFlag);
            }
            for (int i = 0; i < oldDirectoryInfos.Length; i++)
            {
                bool res = newFolderNameList.Contains(oldDirectoryInfos[i].Name) && oldDirectoryInfosSelectedState[i];
                //防止删除文件夹时数组越界
                if(i < directoryInfosSelectedState.Count)
                {
                    directoryInfosSelectedState[i] = res;
                }
            }
            //}
            //EPPToolsSettingAsset.Instance.SetExportUnityPackageDirectoryInfos(directoryInfos);
        }

        /// <summary>
        /// 画GUI
        /// </summary>
        public static void Draw()
        {
            if (EPPToolsSettingWindow.IncludeExportUnityPackage)
            {
                scrollViewPosition = EditorGUILayout.BeginScrollView(scrollViewPosition);

                GUILayout.Label("导出EPP Tools工具包");

                exportRootFolder = EditorGUILayout.TextField("要导出的资源根路径", exportRootFolder);
                exportPath = EditorGUILayout.TextField("导出包存储路径", exportPath);
                exportAllAssetPackage = EditorGUILayout.Toggle("导出拥有全部资源的包", exportAllAssetPackage);
                exportPartAssetPackage = EditorGUILayout.Toggle("各部分分别导出包", exportPartAssetPackage);
                mainVersion = EditorGUILayout.TextField("EPP Tools主版本号", mainVersion);
                exportAllAssetNameFormat = EditorGUILayout.TextField("包含全部资源包的命名格式", exportAllAssetNameFormat);
                EditorGUILayout.HelpBox("识别的标志位：{PartName}、{MainVersion}和{TestFlag}。分别会替换为：当前包名称、插件主版本和测试标志（B、A、R或不显示）)]", MessageType.Info);
                exportPartAssetNameFormat = EditorGUILayout.TextField("各部分资源包的命名格式", exportPartAssetNameFormat);
                EditorGUILayout.HelpBox("识别的标志位：{PartName}、{MainVersion}、{SliceFlag}、{PartVersion}和{TestFlag}。分别会替换为：当前包名称、插件主版本、-、附属包名称和测试标志（B、A、R或不显示）)]", MessageType.Info);
                mainAssetFlag = (PackageTestFlag)EditorGUILayout.EnumPopup("插件基础资源测试类型", mainAssetFlag);

                EditorGUILayout.BeginHorizontal();
                foldout = EditorGUILayout.Foldout(foldout, "导出资源列表");
                if (GUILayout.Button("更新版本信息", GUILayout.Width(200)))
                {
                    directoryVersionList.Clear();
                    partAssetFlag.Clear();

                    for (int i = 0; i < directoryInfos.Length; i++)
                    {
                        /*
                        //string readmeFullName = directoryInfos[i].FullName + "\\Readme.md";
                        Match match = DrawExprotUnityPackageOptions.MatchReadmeFileVersion(directoryInfos[i].FullName + "\\Readme.md");
                        //if (File.Exists(readmeFullName))
                        if (match != null)
                        {
                            //string readmeContent = File.ReadAllText(readmeFullName);
                            //string pattern = @"###\s*v\d+\.\d+\.\d+";
                            //Match match = Regex.Match(readmeContent, pattern);

                            if (match.Success)
                            {
                                int index = match.Value.LastIndexOf('v');
                                string versionNumberString = match.Value.Substring(index + 1);
                                directoryVersionList[i] = versionNumberString;
                            }
                            else
                            {
                                directoryVersionList[i] = "No Version";
                            }
                        }
                        else
                        {
                            directoryVersionList[i] = "No 'Readme.md' File";
                        }
                        */


                        //更新附属包版本信息
                        UpdateVersionInfo(directoryInfos[i].FullName + "\\Readme.md", ref directoryVersionList, ref partAssetFlag);
                    }
                }
                EditorGUILayout.EndHorizontal();
                //if (foldout && !(directoryInfos == null))
                if (foldout)
                {
                    if (directoryInfos.Length > 0)
                    {
                        GUILayout.Label("\t文件夹");
                        for (int i = 0; i < directoryInfos.Length; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            directoryInfosSelectedState[i] = EditorGUILayout.Toggle("\t" + directoryInfos[i].Name, directoryInfosSelectedState[i], GUILayout.MaxWidth(170));
                            directoryVersionList[i] = GUILayout.TextField(directoryVersionList[i], GUILayout.Width(140));
                            if (directoryInfosSelectedState[i])
                            {
                                partAssetFlag[i] = (PackageTestFlag)EditorGUILayout.EnumPopup(partAssetFlag[i]);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("全选", GUILayout.Width(50)))
                    {
                        for (int i = 0; i < directoryInfos.Length; i++)
                        {
                            directoryInfosSelectedState[i] = true;
                        }
                    }
                    if(GUILayout.Button("全不选", GUILayout.Width(50)))
                    {
                        for (int i = 0; i < directoryInfos.Length; i++)
                        {
                            directoryInfosSelectedState[i] = false;
                        }
                    }
                    if (GUILayout.Button("反选", GUILayout.Width(50)))
                    {
                        for (int i = 0; i < directoryInfos.Length; i++)
                        {
                            directoryInfosSelectedState[i] = !directoryInfosSelectedState[i];
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("应用"))
                {
                    if (!string.IsNullOrEmpty(exportRootFolder))
                    {
                        directoryInfos = new DirectoryInfo(exportRootFolder).GetDirectories();
                        //directoryInfosSelectedState.Clear();
                        //for(int i = 0; i < directoryInfos.Length; i++)
                        //{
                        //    directoryInfosSelectedState.Add(true);
                        //}

                        EPPToolsSettingWindow.SetShowMessage("", MessageType.Info);

                        ExportEPPToolsPackageConfig config = new ExportEPPToolsPackageConfig();
                        config.exportRootFolder = exportRootFolder;
                        config.exportPath = exportPath;
                        config.exportAllAssetPackage = exportAllAssetPackage;
                        config.exportPartAssetPackage = exportPartAssetPackage;
                        config.mainVersion = mainVersion;
                        config.exportAllAssetNameFormat = exportAllAssetNameFormat;
                        config.exportPartAssetNameFormat = exportPartAssetNameFormat;

                        config.directoryInfos = directoryInfos;
                        config.directoryInfosSelectedState = directoryInfosSelectedState;
                        //config.mainAssetFlag = mainAssetFlag;
                        //config.partAssetFlag = partAssetFlag;
                        //config.directoryVersionList = directoryVersionList;

                        EPPToolsSettingAsset.Instance.SetExportUnityPackageConfig(config);
                        EPPToolsSettingWindow.SetShowMessage("设置保存成功", MessageType.Info);
                    }
                    else
                    {
                        EPPToolsSettingWindow.SetShowMessage("导出根路径为空", MessageType.Warning);
                    }
                }

                EditorGUILayout.EndScrollView();
            }
            else
            {
                EPPToolsSettingWindow.SetShowMessage("工程中不包含导出EPP Tools工具包的代码，有需要请导入相关unitypackage", MessageType.Info);
            }
        }

        /// <summary>
        /// 匹配Readme文件的版本信息
        /// </summary>
        /// <param name="readmeFullName"></param>
        /// <returns></returns>
        public static Match MatchReadmeFileVersion(string readmeFullName)
        {
            //###\s*v\s*\d+\.\d+\.\d+                 \s*(\[[bBaArR]\])?
            string pattern = @"###\s*v\s*\d+\.\d+\.\d+\s*(\[[bBaArR]\])?";
            if (File.Exists(readmeFullName))
            {
                string readmeContent = File.ReadAllText(readmeFullName);
                return Regex.Match(readmeContent, pattern);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 更新版本信息到指定列表
        /// </summary>
        /// <param name="readmeFullName">Readme.md文件的完整路径</param>
        /// <param name="directoryVersionList">获取文件中最新版本数字串</param>
        /// <param name="partAssetFlag">获取文件中测试标志位</param>
        public static void UpdateVersionInfo(string readmeFullName, ref List<string> directoryVersionList, ref List<PackageTestFlag> partAssetFlag)
        {
            //更新为Readme文件中的版本信息
            Match versionInfoMatch = DrawExprotUnityPackageOptions.MatchReadmeFileVersion(readmeFullName);
            if (versionInfoMatch != null)
            {
                if (versionInfoMatch.Success)
                {
                    //修改版本信息列表
                    int index = versionInfoMatch.Value.LastIndexOf('v');
                    string versionNumberString = versionInfoMatch.Value.Substring(index + 1);
                    directoryVersionList.Add(versionNumberString);
                }
                else
                {
                    directoryVersionList.Add(ExportEPPToolsPackageConst.NO_VERSION);
                }

                //更新测试标志位列表
                //数量肯定会大于1，主要是用第二个值是否为空值来判断
                if (versionInfoMatch.Groups.Count > 1 && !string.IsNullOrEmpty(versionInfoMatch.Groups[1].Value))
                {
                    //匹配结果是带方括号的 如[R]
                    string testFlagString = versionInfoMatch.Groups[1].Value.Replace("[", "").Replace("]", "");
                    try
                    {
                        PackageTestFlag flag = (PackageTestFlag)Enum.Parse(typeof(PackageTestFlag), testFlagString);
                        partAssetFlag.Add(flag);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarningFormat("测试标志位[{0}]未能识别。错误信息：{1}", testFlagString, e.Message);
                        partAssetFlag.Add(PackageTestFlag.NoneFlag);
                    }
                }
                else
                {
                    partAssetFlag.Add(PackageTestFlag.NoneFlag);
                }
            }
            else
            {
                //没有Readme.md文件
                directoryVersionList.Add(ExportEPPToolsPackageConst.NO_README_FILE);
                partAssetFlag.Add(PackageTestFlag.NoneFlag);
            }
        }
    }
}
