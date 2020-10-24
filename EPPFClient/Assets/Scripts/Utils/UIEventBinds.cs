using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// UI添加事件绑定
/// </summary>
public class UIEventBinds
{
    /// <summary>
    /// 按钮添加点击事件
    /// </summary>
    /// <param name="buttonTransform">Button组件所在物体的Transform</param>
    /// <param name="action">要绑定的事件</param>
    /// <returns></returns>
    public static bool ButtonAddOnClick(Transform buttonTransform, UnityAction action)
    {
        if(buttonTransform.TryGetComponent<Button>(out Button button))
        {
            button.onClick.AddListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Button组件", buttonTransform.name);

        return false;
    }

    /// <summary>
    /// 按钮移除点击事件
    /// </summary>
    /// <param name="buttonTransform">Button组件所在物体的Transform</param>
    /// <param name="action">要解绑的事件</param>
    /// <returns></returns>
    public static bool ButtonRemoveOnClick(Transform buttonTransform, UnityAction action)
    {
        if (buttonTransform.TryGetComponent<Button>(out Button button))
        {
            button.onClick.RemoveListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Button组件", buttonTransform.name);

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="toggleTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool ToggleAddValueChange(Transform toggleTransform, UnityAction<bool> action)
    {
        if (toggleTransform.TryGetComponent<Toggle>(out Toggle toggle))
        {
            toggle.onValueChanged.AddListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Toggle组件", toggleTransform.name);

        return false;
    }
}
