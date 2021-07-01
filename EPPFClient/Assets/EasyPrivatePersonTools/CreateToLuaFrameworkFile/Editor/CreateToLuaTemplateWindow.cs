using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using EPPTools.Utils;
using System.Text;
using UnityEngine.UI;
using EPPTools.PluginSettings;

namespace EPPTools.CreateToLuaFrameworkFile
{
    public class CreateToLuaTemplateWindow : EditorWindow
    {
        /// <summary>
        /// Lua的Ctrl文件模板内容
        /// </summary>
        private const string LUACTRLFILETEMPLATESTRING =
    @"require 'View/{PanelName}'

{ClassName} = {};
local this = {ClassName};
local panelStruct;--当前面板的结构对象
local panel;--当前物体对应的panel

function this.New()
    return this;
end

function this.OpenPanel()
    panelStruct = PanelManager.OpenPanel('{AssetBundleName}', '{PanelName}', this.Awake, this.Start, this.Enable, this.Close, true);
    panel = {PanelName}.New(panelStruct.panelGameObject.transform);
    panelStruct:Init();--执行周期函数
end

function this.Awake()
{AddButtonEvent}

{AddToggleEvent}
end

function this.Start()

end

function this.Enable()

end

{ButtonEvent}

{ToggleEvent}
function this.Close()

end
";
        /// <summary>
        /// Lua的Panel文件模板内容
        /// </summary>
        private const string LUAPANELFILETEMPLATESTRING =
    @"{ClassName} = {};
local this = {ClassName};

function this.New(panelTransform)
{ObjectReference}

    return this;
end
";

        /// <summary>
        /// 预制体来源
        /// </summary>
        private enum PrefabSource
        {
            Project = 1,
            Hierarchy = 2
        }

        private static CreateToLuaTemplateWindow window;

        //将lua文件创建到哪个目录
        private string ctrlFilePath;
        private string panelFilePath;
        private bool addToCtrlLuaManager = true;//在LuaManager中添加这个面板的引用
        private string luaManagerFilePath;
        private string requireCtrlFilePath;//写require时ctrl文件前缀

        private bool generateButtonListener = true;
        private bool generateToggleListener = true;
        private List<Transform> attachReferenceList = new List<Transform>();//要额外添加的物体引用

        private bool createCtrlFile = true;
        private bool createPanelFile = true;

        private bool writeInClipboard = false;
        private string assetBundleName;//创建面板方法（PanelManager.OpenPanel）的第一个参数

        //展示额外添加的物体引用列表
        private bool attachObjectListFoldout = false;
        //展示当前选择的物体列表
        private bool nowSelectionFoldout = true;
        //当前选择了哪些物体的文字style
        private GUIStyle selectObjectTipsStyle = new GUIStyle();

        private Vector2 scrollViewPos;

        [MenuItem("EPP Tools/Create ToLua File")]
        public static void OpenCreateLuaTemplateWindow()
        {
            window = EditorWindow.GetWindow<CreateToLuaTemplateWindow>();

            window.Show();
        }

        private void OnEnable()
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

            attachReferenceList.Clear();

            selectObjectTipsStyle.normal.textColor = Color.blue;
        }

        private void Update()
        {
            window.Repaint();
        }
        
        private void OnGUI()
        {
            scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);

            nowSelectionFoldout = EditorGUILayout.Foldout(nowSelectionFoldout, "当前选择了" + Selection.objects.Length + "个物体");
            if (nowSelectionFoldout)
            {
                for(int i = 0; i < Selection.objects.Length; i++)
                {
                    EditorGUILayout.LabelField("    " + Selection.objects[i].name, selectObjectTipsStyle);
                }
            }

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

            EditorGUILayout.LabelField("将物体拖拽到面板上即可在生成时添加该物体的引用");
            attachObjectListFoldout = EditorGUILayout.Foldout(attachObjectListFoldout, "附加引用物体");
            if (attachObjectListFoldout)
            {
                for (int i = 0; i < attachReferenceList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(attachReferenceList[i].name);
                    if (GUILayout.Button("删除", GUILayout.MaxWidth(50)))
                    {
                        attachReferenceList.Remove(attachReferenceList[i]);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            if (GUILayout.Button("创建空Lua文件"))
            {
                CreateOtherFileUtils.CreateFile("New Lua.lua");
            }

            if (GUILayout.Button("从Lua模板创建"))
            {
                //如果选择了写入剪切板，先把剪切板清空
                if (writeInClipboard)
                {
                    GUIUtility.systemCopyBuffer = "";
                }

                //选择了Hierarchy面板下的物体
                if (Selection.gameObjects != null && Selection.gameObjects.Length > 0)
                {
                    //创建文件时可能会修改当前项目选择，这里把选择的对象记录一下
                    UnityEngine.Object[] selectionObjects = new UnityEngine.Object[Selection.objects.Length];
                    Selection.objects.CopyTo(selectionObjects, 0);
                    //选择多个就创建多个
                    for (int i = 0; i < selectionObjects.Length; i++)
                    {
                        GameObject go = (selectionObjects[i] as GameObject);

                        //检查一下物体名称是否以panel结尾，如果不是则给出警告
                        if(!go.name.EndsWith("Panel", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!EditorUtility.DisplayDialog("警告", string.Format("选择的场景物体名称[{0}]不是以‘Panel’结尾，是否继续？", go.name), "是", "否"))
                            {
                                //选择了否则跳出循环
                                break;
                            }
                        }

                        Button[] buttonArray = go.GetComponentsInChildren<Button>();
                        Toggle[] toggleArray = go.GetComponentsInChildren<Toggle>();
                        string prefabName = go.name;
                        CreateFileAndWriteTemplate(prefabName, buttonArray, toggleArray, attachReferenceList, PrefabSource.Hierarchy);

                        //是否在LuaManager中添加引用
                        if (addToCtrlLuaManager)
                        {
                            AddLuaManagerReference(selectionObjects, i);
                        }
                    }
                }
                //选择了Project视图的物体
                else if (Selection.objects != null && Selection.objects.Length > 0)
                {
                    //创建文件时可能会修改当前项目选择，这里把选择的对象记录一下
                    UnityEngine.Object[] selectionObjects = new UnityEngine.Object[Selection.objects.Length];
                    Selection.objects.CopyTo(selectionObjects, 0);
                    //选择多个就创建多个
                    for (int i = 0; i < selectionObjects.Length; i++)
                    {
                        //排除Hierarchy面板物体 且 选择的是预制体
                        if (IsPrefabAsset(selectionObjects[i], false))
                        {
                            Button[] buttonArray = (selectionObjects[i] as GameObject).GetComponentsInChildren<Button>();
                            Toggle[] toggleArray = (selectionObjects[i] as GameObject).GetComponentsInChildren<Toggle>();
                            CreateFileAndWriteTemplate(AssetDatabase.GetAssetPath(selectionObjects[i]), buttonArray, toggleArray, attachReferenceList, PrefabSource.Project);

                            //是否在LuaManager中添加引用
                            if (addToCtrlLuaManager)
                            {
                                AddLuaManagerReference(selectionObjects, i);
                            }
                        }
                    }
                }

                attachReferenceList.Clear();

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

                Debug.Log("Lua文件创建完成");
            }

            Event e = Event.current;
            if (e.type == EventType.DragUpdated || e.type == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                //监听面板上拖拽资源并放开的事件
                if (e.type == EventType.DragPerform)
                {
                    //同意一个拖拽的行为
                    DragAndDrop.AcceptDrag();
                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {
                        UnityEngine.Object handleObj = DragAndDrop.objectReferences[i];
                        if (handleObj != null)
                        {
                            //拖拽事件结束，执行自定义逻辑
                            GameObject targetTransform = handleObj as GameObject;
                            if (!attachReferenceList.Contains(targetTransform.transform))
                            {
                                attachReferenceList.Add(targetTransform.transform);
                            }
                            else
                            {
                                Debug.Log("附加引用的列表中已经包含该物体");
                            }
                        }
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// 向指定文件中写入模板内容
        /// </summary>
        /// <param name="prefabNameOrFullPath"></param>
        /// <param name="buttonArray"></param>
        /// <param name="toggleArray"></param>
        /// <param name="attachReferenceList"></param>
        /// <param name="source"></param>
        private void CreateFileAndWriteTemplate(string prefabNameOrFullPath, Button[] buttonArray, Toggle[] toggleArray, List<Transform> attachReferenceList, PrefabSource source)
        {
            //预制体完整路径 包含后缀名
            string prefabFileFullPath = ""; //Application.dataPath + localPrefabFilePath.Substring(6);
            int lastPrefabFileNameSlice = -1;// prefabFileFullPath.LastIndexOf("/");if(source == PrefabSource.Project)
            if(source == PrefabSource.Project)
            {
                prefabFileFullPath = Application.dataPath + prefabNameOrFullPath.Substring(6);
                lastPrefabFileNameSlice = prefabFileFullPath.LastIndexOf("/");
            }
            if (lastPrefabFileNameSlice != -1 || source == PrefabSource.Hierarchy)
            {
                //预制体名称
                string prefabName = "";
                if (source == PrefabSource.Project)
                {
                    prefabName = prefabFileFullPath.Substring(lastPrefabFileNameSlice + 1);
                }
                else if (source == PrefabSource.Hierarchy)
                {
                    prefabName = prefabNameOrFullPath;
                }
                //处理名字中的空格为下划线
                prefabName = prefabName.Replace(' ', '_');
                //去掉.prefab
                int dotIndex = prefabName.IndexOf('.');
                string fileNameNoSuffix = prefabName;
                if (dotIndex != -1)
                {
                    //没有后缀名的文件完整路径
                    fileNameNoSuffix = prefabName.Substring(0, dotIndex);
                }
                //如果以Panel结尾则去掉这个后缀
                string fileNameNoPanelSuffix = string.Copy(fileNameNoSuffix);
                if (fileNameNoPanelSuffix.EndsWith("Panel", StringComparison.OrdinalIgnoreCase))
                {
                    fileNameNoPanelSuffix = fileNameNoPanelSuffix.Substring(0, fileNameNoPanelSuffix.Length - 5);
                }
                //获取Ctrl后缀文件名和Panel后缀的文件名
                string luaCtrlFileName = TranslateToFunctionName(fileNameNoPanelSuffix) + "Ctrl.lua";
                string luaPanelFileName = TranslateToFunctionName(fileNameNoPanelSuffix) + "Panel.lua";
                //ctrl文件完整路径
                string luaCtrlFileFullName = Application.dataPath + ctrlFilePath.Substring(6) + "/" + luaCtrlFileName;
                //panel文件完整路径
                string luaPanelFileFullName = Application.dataPath + panelFilePath.Substring(6) + "/" + luaPanelFileName;

                //创建Lua的Ctrl文件
                if (createCtrlFile)
                {
                    FileStream fs = File.Create(luaCtrlFileFullName);

                    //处理模板标志位中的内容
                    Dictionary<string, string> tagDict = new Dictionary<string, string>();
                    //ClassName
                    string className = luaCtrlFileName.Substring(0, luaCtrlFileName.Length - 4);
                    tagDict.Add("ClassName", className);
                    //PanelName
                    string panelName = luaPanelFileName.Substring(0, luaPanelFileName.Length - 4);
                    tagDict.Add("PanelName", panelName);
                    //AssetBundleName
                    tagDict.Add("AssetBundleName", assetBundleName);
                    //ButtonEvent
                    string buttonEvent = CreateButtonEvent(buttonArray);
                    tagDict.Add("ButtonEvent", buttonEvent);
                    //ToggleEvent
                    string toggleEvent = CreateToggleEvent(toggleArray);
                    tagDict.Add("ToggleEvent", toggleEvent);
                    //AddButtonEvent
                    string addButtonEvent = CreateAddButtonEvent(buttonArray, source);
                    tagDict.Add("AddButtonEvent", addButtonEvent);
                    //AddToggleEvent
                    string addToggleEvent = CreateAddToggleEvent(toggleArray, source);
                    tagDict.Add("AddToggleEvent", addToggleEvent);

                    //将处理好的数据写入文件或剪切板
                    string dataString = ReplaceLuaTemplateTag(tagDict, LUACTRLFILETEMPLATESTRING);
                    if (writeInClipboard)
                    {
                        GUIUtility.systemCopyBuffer += dataString;

                        fs.Close();
                        File.Delete(luaCtrlFileFullName);//没想到更好的方法，直接创建空的然后删了叭
                    }
                    else
                    {
                        byte[] data = Encoding.UTF8.GetBytes(dataString);
                        fs.Write(data, 0, data.Length);

                        fs.Close();
                    }

                    fs.Close();
                }

                //创建Lua的Panel文件
                if (createPanelFile)
                {
                    FileStream fs = File.Create(luaPanelFileFullName);

                    //处理模板标志位中的内容
                    Dictionary<string, string> tagDict = new Dictionary<string, string>();
                    //ClassName
                    tagDict.Add("ClassName", luaPanelFileName.Substring(0, luaPanelFileName.Length - 4));
                    //ObjectReference
                    string panelObjectReference = CreatePanelObjectReference(buttonArray, toggleArray, attachReferenceList, source);
                    tagDict.Add("ObjectReference", panelObjectReference);

                    //将处理好的数据写入文件
                    string dataString = ReplaceLuaTemplateTag(tagDict, LUAPANELFILETEMPLATESTRING);
                    if (writeInClipboard)
                    {
                        GUIUtility.systemCopyBuffer += dataString;

                        fs.Close();
                        File.Delete(luaPanelFileFullName);//没想到更好的方法，直接创建空的然后删了叭
                    }
                    else
                    {
                        byte[] data = Encoding.UTF8.GetBytes(dataString);
                        fs.Write(data, 0, data.Length);

                        fs.Close();
                    }

                    fs.Close();
                }

                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogErrorFormat("指定的文件路径{0}错误。无法确定文件名称。处理时的完整路径为：{1}", prefabNameOrFullPath, prefabFileFullPath);
            }
        }

        /// <summary>
        /// 创建所有物体的引用lua代码
        /// </summary>
        /// <param name="buttonArray"></param>
        /// <param name="toggleArray"></param>
        /// <param name="attachObjectArray"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private string CreatePanelObjectReference(Button[] buttonArray, Toggle[] toggleArray, List<Transform> attachObjectArray, PrefabSource source)
        {
            StringBuilder builder = new StringBuilder();

            if (generateButtonListener)
            {
                for (int i = 0; i < buttonArray.Length; i++)
                {
                    builder.Append("\tthis.");
                    string buttonName = GetFullVariableName(buttonArray[i].transform, source);
                    builder.Append(buttonName);
                    builder.Append(" = panelTransform:Find('");
                    string buttonPath = GetFullPathName(buttonArray[i].transform, source);
                    builder.Append(buttonPath);
                    builder.Append("');\n");
                }

                builder.Append("\n");
            }

            if (generateToggleListener)
            {
                for (int i = 0; i < toggleArray.Length; i++)
                {
                    builder.Append("\tthis.");
                    string toggleName = GetFullVariableName(toggleArray[i].transform, source);
                    builder.Append(toggleName);
                    builder.Append(" = panelTransform:Find('");
                    string togglePath = GetFullPathName(toggleArray[i].transform, source);
                    builder.Append(togglePath);
                    builder.Append("');\n");
                }

                builder.Append("\n");
            }

            for (int i = 0; i < attachObjectArray.Count; i++)
            {
                builder.Append("\tthis.");
                string attachObjectName = GetFullVariableName(attachObjectArray[i].transform, source);
                builder.Append(attachObjectName);
                builder.Append(" = panelTransform:Find('");
                string attachObjectPath = GetFullPathName(attachObjectArray[i].transform, source);
                builder.Append(attachObjectPath);
                builder.Append("');\n");
            }

            return builder.ToString();
        }

        /// <summary>
        /// 判断Object是否是预制体资源。如果是则返回true，如果不是则返回false
        /// </summary>
        /// <param name="obj">UnityEngine.Object</param>
        /// <param name="includePrefabInstance">是否将预制体资源的Scene实例视为预制体资源？</param>
        /// <returns></returns>
        private bool IsPrefabAsset(UnityEngine.Object obj, bool includePrefabInstance)
        {
            if (!obj)
            {
                return false;
            }

            PrefabAssetType type = UnityEditor.PrefabUtility.GetPrefabAssetType(obj);
            if (type == UnityEditor.PrefabAssetType.NotAPrefab)
            {
                return false;
            }

            PrefabInstanceStatus status = UnityEditor.PrefabUtility.GetPrefabInstanceStatus(obj);
            if (status != UnityEditor.PrefabInstanceStatus.NotAPrefab && !includePrefabInstance)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建“添加Toggle事件”的lua代码
        /// </summary>
        /// <param name="toggleArray"></param>
        /// <returns></returns>
        private string CreateAddToggleEvent(Toggle[] toggleArray, PrefabSource source)
        {
            if (generateToggleListener)
            {
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < toggleArray.Length; i++)
                {
                    builder.Append("\tUIEventBinds.ToggleAddValueChange(panel.");
                    builder.Append(GetFullVariableName(toggleArray[i].transform, source));
                    builder.Append(", this.On");
                    builder.Append(TranslateToFunctionName(toggleArray[i].name));
                    builder.Append("ValueChange);\n");
                }

                return builder.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 创建“添加Button事件”的lua代码
        /// </summary>
        /// <param name="buttonArray"></param>
        /// <returns></returns>
        private string CreateAddButtonEvent(Button[] buttonArray, PrefabSource source)
        {
            if (generateButtonListener)
            {
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < buttonArray.Length; i++)
                {
                    builder.Append("\tUIEventBinds.ButtonAddOnClick(panel.");
                    builder.Append(GetFullVariableName(buttonArray[i].transform, source));
                    builder.Append(", this.On");
                    builder.Append(TranslateToFunctionName(buttonArray[i].name));
                    builder.Append("ButtonClick);\n");
                }

                return builder.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 创建“Toggle事件函数体”的lua代码
        /// </summary>
        /// <param name="toggleArray"></param>
        /// <returns></returns>
        private string CreateToggleEvent(Toggle[] toggleArray)
        {
            if (generateToggleListener)
            {
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < toggleArray.Length; i++)
                {
                    string buttonName = TranslateToFunctionName(toggleArray[i].name);

                    builder.Append("function this.On");
                    builder.Append(buttonName);
                    builder.Append("ValueChange(isOn)\n\nend\n\n");
                }

                return builder.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 创建“Button事件函数体”的lua代码
        /// </summary>
        /// <param name="buttonArray"></param>
        /// <returns></returns>
        private string CreateButtonEvent(Button[] buttonArray)
        {
            if (generateButtonListener)
            {
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < buttonArray.Length; i++)
                {
                    string buttonName = TranslateToFunctionName(buttonArray[i].name);

                    builder.Append("function this.On");
                    builder.Append(buttonName);
                    builder.Append("ButtonClick()\n\nend\n\n");
                }

                return builder.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 修改物体名称为函数名称格式（去空格、首字母大写等）
        /// </summary>
        /// <param name="transformName"></param>
        /// <returns></returns>
        private string TranslateToFunctionName(string transformName)
        {
            if (transformName.Length > 0)
            {
                //首字母转大写
                transformName = transformName[0].ToString().ToUpper() + transformName.Substring(1);
            }
            //替换空格和减号为下划线
            transformName = transformName.Replace(' ', '_').Replace('-', '_');

            return transformName;
        }

        /// <summary>
        /// 替换模板内的标志位返回替换结果
        /// </summary>
        /// <returns></returns>
        private string ReplaceLuaTemplateTag(Dictionary<string, string> tagDict, string template)
        {
            string res = string.Copy(template);
            foreach (var tag in tagDict)
            {
                res = res.Replace("{" + tag.Key + "}", tag.Value);
            }

            return res;
        }

        /// <summary>
        /// 获得完整变量名。panel.后面的变量名。这个变量名不包含canvas和canvas的直接子物体
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private string GetFullVariableName(Transform target, PrefabSource source)
        {
            Transform tempParent = target.parent;
            string res = TranslateToFunctionName(target.name);

            //这里不支持预制体。project视图下的预制体不存在canvas父物体
            while (tempParent != null)
            {
                res = '_' + res;
                res = TranslateToFunctionName(tempParent.name) + res;

                tempParent = tempParent.parent;
            }
            //去掉前一/两级的父物体路径
            int targetCount = 0;
            int index = -1;
            for (int i = 0; i < res.Length && targetCount < (int)source; i++)
            {
                if (res[i] == '_')
                {
                    index = i;
                    targetCount++;

                    continue;
                }
            }
            if (index != -1)
            {
                res = res.Substring(index + 1);
            }
            //开头小写
            if (res.Length > 0)
            {
                res = res[0].ToString().ToLower() + res.Substring(1);
            }

            return res;
        }

        /// <summary>
        /// 获得完整路径。这个路径名不包含canvas和canvas的直接子物体
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private string GetFullPathName(Transform target, PrefabSource source)
        {
            Transform tempParent = target.parent;
            //StringBuilder builder = new StringBuilder();
            //builder.Append(target.name);
            string res = target.name;

            while (tempParent != null)
            {
                //builder.Append(tempParent.name + "/" + builder);
                res = tempParent.name + "/" + res;

                tempParent = tempParent.parent;
            }
            ////去掉前两级的父物体路径
            //string[] arr = builder.ToString().Split('/');
            //if (arr.Length > 2)
            //{
            //    builder.Clear();

            //    for (int i = 2; i < arr.Length; i++)
            //    {
            //        builder.Append(arr[i]);
            //        //去掉最后一个分隔符
            //        if (i < arr.Length - 1)
            //        {
            //            builder.Append("/");
            //        }
            //    }
            //}
            //去掉前一/两级的父物体路径
            int targetCount = 0;
            int index = -1;
            for (int i = 0; i < res.Length && targetCount < (int)source; i++)
            {
                if (res[i] == '/')
                {
                    index = i;
                    targetCount++;

                    continue;
                }
            }
            if (index != -1)
            {
                res = res.Substring(index + 1);
            }

            //return builder.ToString();
            return res;
        }

        /// <summary>
        /// 修改LuaManager，在其中添加对ctrl的引用
        /// </summary>
        private void AddLuaManagerReference(UnityEngine.Object[] selectionObjects, int index)
        {
            //是否在LuaManager中添加引用
            if (addToCtrlLuaManager)
            {
                string luaManagerFileFulePath = Application.dataPath + luaManagerFilePath.Substring(6);
                string[] lines = File.ReadAllLines(luaManagerFileFulePath);
                FileStream fs = File.OpenWrite(luaManagerFileFulePath);
                //把文件重新写一遍
                fs.Seek(0, SeekOrigin.Current);
                string requireBlock = "require ";

                string objectName = selectionObjects[index].name;
                //去掉后面的panel
                if (objectName.EndsWith("Panel", StringComparison.OrdinalIgnoreCase))
                {
                    objectName = objectName.Substring(0, objectName.Length - 5);
                }
                if (!string.IsNullOrEmpty(requireCtrlFilePath))
                {
                    requireBlock += "\"" + requireCtrlFilePath + "/" + TranslateToFunctionName(objectName) + "Ctrl\"\n";
                }
                else
                {
                    requireBlock += "\"" + TranslateToFunctionName(objectName) + "\"\n";
                }
                byte[] requireBlockByte = Encoding.UTF8.GetBytes(requireBlock);
                fs.Write(requireBlockByte, 0, requireBlockByte.Length);
                for (int j = 0; j < lines.Length; j++)
                {
                    byte[] lineByte = Encoding.UTF8.GetBytes(lines[j] + "\n");
                    fs.Write(lineByte, 0, lineByte.Length);

                    //添加CtrlNames
                    string ctrlName = selectionObjects[index].name.Replace(" ", "_");
                    //如果以Panel结尾则去掉
                    if (ctrlName.EndsWith("Panel", StringComparison.OrdinalIgnoreCase))
                    {
                        ctrlName = ctrlName.Substring(0, ctrlName.Length - 5);
                    }
                    //添加Ctrl后缀
                    ctrlName += "Ctrl";
                    //开头转小写
                    string ctrlNameLower = ctrlName[0].ToString().ToLower() + ctrlName.Substring(1);
                    string ctrlNameUpper = ctrlName[0].ToString().ToUpper() + ctrlName.Substring(1);
                    if (lines[j].Trim() == "CtrlNames = {")
                    {
                        string newLine = "\t" + ctrlNameLower + " = \"" + ctrlNameUpper + "\",\n";
                        byte[] newLineByte = Encoding.UTF8.GetBytes(newLine);

                        fs.Write(newLineByte, 0, newLineByte.Length);
                    }

                    if (lines[j].Trim() == "function this.New()")
                    {
                        string newLine = "\tthis.CtrlList[CtrlNames." + ctrlNameLower + "] = " + ctrlNameUpper + ".New();\n";
                        byte[] newLineByte = Encoding.UTF8.GetBytes(newLine);

                        fs.Write(newLineByte, 0, newLineByte.Length);
                    }
                }
            }
        }
    }
}
