using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EPPTools.PluginSettings
{
    public class DrawDebugControllableOptions : MonoBehaviour
    {
        private static bool developModel;
        private static bool lastDevelopModel;

        public static void InitLocalization()
        {
            if (EPPToolsSettingWindow.IncludeDebugControllablePackage)
            {
                developModel = EPPToolsSettingAsset.Instance.DebugControllable.developModel;
                lastDevelopModel = developModel;
            }
        }

        public static void Draw()
        {
            if (EPPToolsSettingWindow.IncludeDebugControllablePackage)
            {
                developModel = GUILayout.Toggle(developModel, "是否处于开发模式");
                if(developModel != lastDevelopModel)
                {
                    lastDevelopModel = developModel;

                    DebugControllableConfig config = new DebugControllableConfig()
                    {
                        developModel = developModel
                    };

                    EPPToolsSettingAsset.Instance.SetDebugControllableConfig(config);
                }
            }
            else
            {
                EPPToolsSettingWindow.SetShowMessage("工程中不包含可控制的调试的代码，有需要请导入相关unitypackage", MessageType.Info);
            }
        }
    }
}
