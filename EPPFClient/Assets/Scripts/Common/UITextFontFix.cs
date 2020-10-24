using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 修改字体为ResourcesManager面板指定的字体
/// </summary>
public class UITextFontFix : MonoBehaviour
{
    public FontResourcesEnum fontType;
    public Text text;

    private void Awake()
    {
        if(text == null)
        {
            if(!TryGetComponent<Text>(out text))
            {
                FDebugger.LogWarningFormat("物体{0}上不存在Text组件", transform.name);

                return;
            }
        }

        FontResourcesStruct fontResourcesStruct = ResourcesManager.Instance.GetFontResourcesStructFromType(FontResourcesEnum.SourceHanSansSCRegular);
        if(fontResourcesStruct.fontResources != null)
        {
            text.font = fontResourcesStruct.fontResources;
        }
    }
}
