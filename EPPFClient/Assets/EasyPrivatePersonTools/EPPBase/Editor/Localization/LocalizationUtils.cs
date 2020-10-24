using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPPTools.Localization;

namespace EPPTools.PluginSettings
{
    public class LocalizationUtils
    {
        /// <summary>
        /// 根据资源加载方式获取资源加载路径
        /// </summary>
        /// <param name="loadMethod"></param>
        /// <returns></returns>
        public static string GetAssetsPath(LoadAssetsMethod loadMethod, string supportLanguagePath)
        {
            string folderName = Application.dataPath + "\\";
            switch (loadMethod)
            {
                case LoadAssetsMethod.Resources:
                    if (supportLanguagePath.StartsWith("Resources"))
                    {
                        folderName += supportLanguagePath.Substring(10);
                    }
                    else
                    {
                        folderName += supportLanguagePath;
                    }
                    break;
                case LoadAssetsMethod.UnityWebRequest:
                    folderName += supportLanguagePath.Substring(16);
                    break;
                case LoadAssetsMethod.StreamingAssets:
                    folderName += "StreamingAssets/" + supportLanguagePath.Substring(16);
                    break;
                default:
                    break;
            }

            folderName = folderName.Replace('/', '\\');
            char lastChar = folderName[folderName.Length - 1];
            if (lastChar != '\\')
            {
                folderName += '\\';
            }

            return folderName;
        }

        /// <summary>
        /// 对路径进行变换，以适应加载方式的路由规则
        /// </summary>
        /// <param name="loadMethodOld"></param>
        /// <param name="loadMethodNew"></param>
        /// <param name="supportLanguagePath"></param>
        /// <returns></returns>
        public static string TransformAssetsPath(LoadAssetsMethod loadMethodOld, LoadAssetsMethod loadMethodNew, string supportLanguagePath)
        {
            string newPath = string.Copy(supportLanguagePath);
            switch (loadMethodOld)
            {
                case LoadAssetsMethod.Resources:
                    if (supportLanguagePath.StartsWith("Resources"))
                    {
                        newPath = supportLanguagePath.Substring(10);
                    }
                    if (newPath.StartsWith("Assets"))
                    {
                        newPath = newPath.Substring(7);
                    }
                    //从Resources方式切换到StreamingAssets方式的变换
                    if(loadMethodNew == LoadAssetsMethod.StreamingAssets)
                    {
                        int resourcesIndex = supportLanguagePath.IndexOf("Resources:");
                        if(resourcesIndex != -1)
                        {
                            int temp = 0;
                            if(resourcesIndex != 0)
                            {
                                temp = 1;
                            }
                            newPath = newPath.Substring(resourcesIndex + "Resources:".Length + temp);//+1去掉文件夹前面的斜杠
                        }
                        else
                        {
                            newPath = "";
                        }
                    }
                    break;
                case LoadAssetsMethod.UnityWebRequest:
                    //切换到Resources方式则清空输入框
                    if(loadMethodNew == LoadAssetsMethod.Resources)
                    {
                        newPath = "";

                        break;
                    }
                    if (supportLanguagePath.StartsWith("UnityWebRequest"))
                    {
                        newPath = supportLanguagePath.Substring(16);
                    }
                    break;
                case LoadAssetsMethod.StreamingAssets:
                    //切换到Resources方式则清空输入框
                    if (loadMethodNew == LoadAssetsMethod.Resources)
                    {
                        newPath = "";

                        break;
                    }
                    if (supportLanguagePath.StartsWith("StreamingAssets"))
                    {
                        newPath = supportLanguagePath.Substring(16);
                    }
                    if (newPath.StartsWith("Assets"))
                    {
                        newPath = newPath.Substring(7);
                    }
                    break;
                default:
                    newPath = string.Copy(supportLanguagePath);
                    break;
            }

            switch (loadMethodNew)
            {
                case LoadAssetsMethod.Resources:
                    newPath = "Resources:" + newPath;
                    break;
                case LoadAssetsMethod.UnityWebRequest:
                    newPath = "UnityWebRequest:" + newPath;
                    break;
                case LoadAssetsMethod.StreamingAssets:
                    newPath = "StreamingAssets:" + newPath;
                    break;
            }

            return newPath;
        }
    }
}
