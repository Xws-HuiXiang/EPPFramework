using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System;
using System.Reflection;

namespace EPPTools.PluginSettings
{
    [InitializeOnLoad]
    public class EPPToolsSettingAssetInstance
    {
        private static EPPToolsSettingAsset instance;

        static EPPToolsSettingAssetInstance()
        {
            //添加编译开始和结束的事件监听
            CompilationPipeline.compilationStarted += OnCompilationStartedEvent;
            CompilationPipeline.compilationFinished += OnCompilationFinishedEvent;
        }

        private static void OnCompilationStartedEvent(object o)
        {
            //Debug.Log("编译开始");
            instance = EPPToolsSettingAsset.Instance;
        }

        private static void OnCompilationFinishedEvent(object o)
        {
            //Debug.Log("编译结束");
            Type eppToolsSettingAssetType = Type.GetType("EPPTools.PluginSettings.EPPToolsSettingAsset");
            //object obj = eppToolsSettingAssetType.Assembly.CreateInstance("EPPTools.PluginSettings.EPPToolsSettingAsset");
            object obj = ScriptableObject.CreateInstance(eppToolsSettingAssetType);
            FieldInfo instanceField = eppToolsSettingAssetType.GetField("instance", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if(instanceField != null)
            {
                instanceField.SetValue(obj, EPPToolsSettingAssetInstance.instance);
            }
        }
    }
}
