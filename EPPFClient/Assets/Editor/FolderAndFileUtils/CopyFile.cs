using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using EPPTools.PluginSettings;

public class CopyFile
{
    /// <summary>
    /// 拷贝工程路径中的ab包文件到持久化目录中
    /// </summary>
    [MenuItem("Folder Util/Copy AssetBundle File To PersistentDataPath")]
    public static void ClickCopyABFileToPersistentDataPathMenu()
    {
        //EPPTools插件设置中的ab包输出路径
        if(EPPToolsSettingAsset.Instance != null)
        {
            string abFileOutPutPath = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.outPutPath;
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + abFileOutPutPath.Substring(6));
                FileInfo[] fileInfos = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
                int fileCount = 0;
                for(int i = 0; i < fileInfos.Length; i++)
                {
                    //去掉meta文件
                    if (!fileInfos[i].Name.EndsWith(".meta"))
                    {
                        string destFileName = Path.Combine(AppConst.GetLoaclResRootFolderPath(), fileInfos[i].Name);
                        File.Copy(fileInfos[i].FullName, destFileName, true);

                        fileCount++;
                    }
                }

                Debug.LogFormat("文件拷贝完成，共拷贝了{0}个文件", fileCount);
            }
            catch(Exception e)
            {
                Debug.LogError("拷贝文件时发生错误。错误信息：" + e.Message);
            }
        }
        else
        {
            Debug.LogError("没有找到EPPToolsSettingAsset.Instance的实例。请检查EPPTools插件的基础包(EasyPrivatePersonTools/EPPBase)是否存在并且含有对应的配置信息(执行EPP Tools/EPP Tools Setting菜单项时会自动创建实例和配置文件)");
        }
    }
}
