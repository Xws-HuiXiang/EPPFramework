using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于转换ctrl和panel的字符串名称
/// </summary>
public static class PanelNameUtil
{
    /// <summary>
    /// 尝试获取对应的ctrl名称。该方法会替换参数最后的‘panel’字符串为ctrl，如果字符串以panel结尾
    /// </summary>
    /// <param name="panelName"></param>
    /// <returns></returns>
    public static string TryGetCtrlName(string panelName)
    {
        if (string.IsNullOrEmpty(panelName))
        {
            return panelName;
        }

        string res = panelName;
        if (panelName.EndsWith("Panel", StringComparison.OrdinalIgnoreCase))
        {
            res = panelName.Substring(0, panelName.Length - 4);
            res += "Ctrl";
        }

        return res;
    }

    /// <summary>
    /// 尝试获取对应的panel名称。该方法会替换参数最后的‘ctrl’字符串为panel，如果字符串以ctrl结尾
    /// </summary>
    /// <param name="ctrlName"></param>
    /// <returns></returns>
    public static string TryGetPanelName(string ctrlName)
    {
        if (string.IsNullOrEmpty(ctrlName))
        {
            return ctrlName;
        }

        string res = ctrlName;
        if (ctrlName.EndsWith("Ctrl", StringComparison.OrdinalIgnoreCase))
        {
            res = ctrlName.Substring(0, ctrlName.Length - 4);
            res += "Panel";
        }

        return res;
    }

    /// <summary>
    /// 该方法会尝试去掉字符串最后的‘ctrl’或‘panel’
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string TryGetNoSuffixName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        string res = name;
        if (name.EndsWith("Ctrl", StringComparison.OrdinalIgnoreCase))
        {
            res = name.Substring(0, name.Length - 4);
        }
        else if(name.EndsWith("Panel", StringComparison.OrdinalIgnoreCase))
        {
            res = name.Substring(0, name.Length - 5);
        }

        return res;
    }
}
