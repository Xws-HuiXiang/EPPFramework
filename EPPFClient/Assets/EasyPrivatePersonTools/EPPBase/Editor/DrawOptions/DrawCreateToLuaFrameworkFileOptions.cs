using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EPPTools.PluginSettings
{
    public class DrawCreateToLuaFrameworkFileOptions
    {
        public static string ctrlFilePath;
        public static string panelFilePath;
        public static bool addToCtrlLuaManager;
        public static string luaManagerFilePath;
        public static string requireCtrlFilePath;
        public static bool generateButtonListener;
        public static bool generateToggleListener;
        public static bool createCtrlFile;
        public static bool createPanelFile;
        public static bool writeInClipboard;
        public static string assetBundleName;
        public static bool attachObjectListFoldout;

        public static void InitCreateToLuaFrameworkFileOptions()
        {
            ctrlFilePath = EPPToolsSettingAsset.Instance.CreateToLuaTemplateFileConfig.ctrlFilePath;
            panelFilePath = EPPToolsSettingAsset.Instance.CreateToLuaTemplateFileConfig.panelFilePath;
            addToCtrlLuaManager = EPPToolsSettingAsset.Instance.CreateToLuaTemplateFileConfig.addToCtrlLuaManager;
            luaManagerFilePath = EPPToolsSettingAsset.Instance.CreateToLuaTemplateFileConfig.luaManagerFilePath;
            requireCtrlFilePath = EPPToolsSettingAsset.Instance.CreateToLuaTemplateFileConfig.requireCtrlFilePath;
            generateButtonListener = EPPToolsSettingAsset.Instance.CreateToLuaTemplateFileConfig.generateButtonListener;
            generateToggleListener = EPPToolsSettingAsset.Instance.CreateToLuaTemplateFileConfig.generateToggleListener;
            createCtrlFile = EPPToolsSettingAsset.Instance.CreateToLuaTemplateFileConfig.createCtrlFile;
            createPanelFile = EPPToolsSettingAsset.Instance.CreateToLuaTemplateFileConfig.createPanelFile;
            writeInClipboard = EPPToolsSettingAsset.Instance.CreateToLuaTemplateFileConfig.writeInClipboard;
            assetBundleName = EPPToolsSettingAsset.Instance.CreateToLuaTemplateFileConfig.assetBundleName;
            attachObjectListFoldout = EPPToolsSettingAsset.Instance.CreateToLuaTemplateFileConfig.attachObjectListFoldout;
        }

        public static void Draw()
        {
            ctrlFilePath = EditorGUILayout.TextField("Ctrl文件保存路径", ctrlFilePath);
            panelFilePath = EditorGUILayout.TextField("Panel文件保存路径", panelFilePath);
            assetBundleName = EditorGUILayout.TextField("AssetBundle名称", assetBundleName);

            generateButtonListener = EditorGUILayout.ToggleLeft("添加Button事件监听", generateButtonListener);
            generateToggleListener = EditorGUILayout.ToggleLeft("添加Toggle事件监听", generateToggleListener);
            createCtrlFile = EditorGUILayout.ToggleLeft("创建ctrl文件", createCtrlFile);
            createPanelFile = EditorGUILayout.ToggleLeft("创建panel文件", createPanelFile);
            addToCtrlLuaManager = EditorGUILayout.ToggleLeft("在LuaManager中添加引用", addToCtrlLuaManager);
            if (createCtrlFile || createPanelFile)
            {
                writeInClipboard = EditorGUILayout.ToggleLeft("不创建文件，将内容写入剪切板", writeInClipboard);
            }
            if (addToCtrlLuaManager)
            {
                luaManagerFilePath = EditorGUILayout.TextField("LuaManager文件相对路径", luaManagerFilePath);
                requireCtrlFilePath = EditorGUILayout.TextField("require引用ctrl时的前缀路径", requireCtrlFilePath);
            }

            if (GUILayout.Button("保存"))
            {
                CreateToLuaTemplateFileConfig config = new CreateToLuaTemplateFileConfig();
                config.ctrlFilePath = ctrlFilePath;
                config.panelFilePath = panelFilePath;
                config.addToCtrlLuaManager = addToCtrlLuaManager;
                config.luaManagerFilePath = luaManagerFilePath;
                config.requireCtrlFilePath = requireCtrlFilePath;
                config.generateButtonListener = generateButtonListener;
                config.generateToggleListener = generateToggleListener;
                config.createCtrlFile = createCtrlFile;
                config.createPanelFile = createPanelFile;
                config.writeInClipboard = writeInClipboard;
                config.assetBundleName = assetBundleName;
                config.attachObjectListFoldout = attachObjectListFoldout;

                EPPToolsSettingAsset.Instance.SetCreateToLuaFrameworkFileConfig(config);

                EPPToolsSettingWindow.SetShowMessage("设置成功", MessageType.Info);
            }
        }
    }
}
