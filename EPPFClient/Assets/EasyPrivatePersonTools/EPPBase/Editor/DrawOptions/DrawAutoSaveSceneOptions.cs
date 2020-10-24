using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;

namespace EPPTools.PluginSettings
{
    public class DrawAutoSaveSceneOptions
    {
        private static bool autoSaveSceneRunBefore = false;
        private static bool timerSaveScene = false;
        private static float cycleTime = 5f;

        /// <summary>
        /// 初始化自动保存场景的设置
        /// </summary>
        public static void InitAutoSaveScene()
        {
            if (EPPToolsSettingWindow.IncludeAutoSaveScenePackage)
            {
                autoSaveSceneRunBefore = EPPToolsSettingAsset.Instance.AutoSaveScene.openAutoSave;
                timerSaveScene = EPPToolsSettingAsset.Instance.AutoSaveScene.openTimerSave;
                cycleTime = EPPToolsSettingAsset.Instance.AutoSaveScene.cycleTime;

                EPPToolsSettingWindow.SetShowMessage("", MessageType.Info);
            }
        }

        /// <summary>
        /// 画GUI
        /// </summary>
        public static void Draw()
        {
            if (EPPToolsSettingWindow.IncludeAutoSaveScenePackage)
            {
                GUILayout.Label("运行前自动保存场景");
                autoSaveSceneRunBefore = EditorGUILayout.Toggle("是否开启自动保存", autoSaveSceneRunBefore);
                timerSaveScene = EditorGUILayout.Toggle("定时保存", timerSaveScene);
                if (timerSaveScene)
                {
                    cycleTime = EditorGUILayout.FloatField("保存时间间隔(分钟)", cycleTime);
                    if (GUILayout.Button("打开定时保存面板"))
                    {
                        Type type = Type.GetType("EPPTools.AutoSaveScene.TimerSaveSceneWindow");
                        if (type != null)
                        {
                            //当前项目包含了自动保存场景的包
                            System.Object obj = ScriptableObject.CreateInstance(type);
                            MethodInfo openWindow = type.GetMethod("OpenWindow");
                            openWindow.Invoke(obj, null);
                        }
                        else
                        {
                            //类型缺失？
                            EPPToolsSettingWindow.SetShowMessage("类型为[EPPTools.AutoSaveScene.TimerSaveSceneWindow]的相关文件缺失", MessageType.Error);
                        }
                    }
                }
                if (GUILayout.Button("应用"))
                {
                    AutoSaveSceneConfig config = new AutoSaveSceneConfig();
                    config.openAutoSave = autoSaveSceneRunBefore;
                    config.openTimerSave = timerSaveScene;
                    config.cycleTime = cycleTime;

                    EPPToolsSettingAsset.Instance.SetAutoSaveSceneConfig(config);

                    EPPToolsSettingWindow.SetShowMessage("设置成功", MessageType.Info);
                }
            }
            else
            {
                EPPToolsSettingWindow.SetShowMessage("工程中不包含自动保存场景的代码，有需要请导入相关unitypackage", MessageType.Info);
            }
        }
    }
}
