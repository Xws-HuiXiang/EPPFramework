using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对LitJson的JsonData进行扩展。主要给Lua用，因为Lua不能使用'this[]'语法糖
/// </summary>
public static class JsonDataExtension
{
    public static JsonData GetValue(this JsonData data, string key)
    {
        return data[key];
    }

    public static JsonData GetValue(this JsonData data, int key)
    {
        return data[key];
    }
}
