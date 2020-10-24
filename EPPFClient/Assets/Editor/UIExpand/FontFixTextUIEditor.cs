using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 字体更换组件，带有这个组件的Text，加载时会更改为组件指定的字体（ResourcesManager面板指定的字体）
/// </summary>
public class FontFixTextUIEditor
{
    [MenuItem("GameObject/UI/FontFix Text")]
    public static void CreateFontFixText()
    {
        EditorApplication.ExecuteMenuItem("GameObject/UI/Text");
        Text textComponent = Selection.activeGameObject.GetComponent<Text>();
        UITextFontFix fontFixComponent = Undo.AddComponent<UITextFontFix>(Selection.activeGameObject);

        fontFixComponent.text = textComponent;
        textComponent.font = ResourcesManager.Instance.GetFontResourcesStructFromType(FontResourcesEnum.SourceHanSansSCRegular).fontResources;
    }
}
