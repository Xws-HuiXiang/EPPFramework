using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#pragma warning disable 0162

public enum FontResourcesEnum
{
    /// <summary>
    /// 思源字体
    /// </summary>
    SourceHanSansSCRegular
}

[Serializable]
public struct FontResourcesStruct
{
    public FontResourcesEnum fontType;
    public Font fontResources;
}

public class ResourcesManager : MonoSingleton<ResourcesManager>
{
    [SerializeField]
    private List<FontResourcesStruct> fontResourcesDict = new List<FontResourcesStruct>();

    private AssetBundle[] allAssetBundlesArray;

    /// <summary>
    /// 根据指定的字体类型获取字体
    /// </summary>
    /// <param name="fontType"></param>
    /// <returns></returns>
    public FontResourcesStruct GetFontResourcesStructFromType(FontResourcesEnum fontType)
    {
        foreach(FontResourcesStruct item in fontResourcesDict)
        {
            if(item.fontType == fontType)
            {
                return item;
            }
        }
#if UNITY_EDITOR
        throw new KeyNotFoundException("提供的键：" + fontType.ToString() + "没有在字典[fontResourcesDict]中找到");
#endif
        return new FontResourcesStruct();
    }

    /// <summary>
    /// 保存所有ab包的引用
    /// </summary>
    /// <param name="abList"></param>
    public void AddAssetBundle(List<AssetBundle> abList)
    {
        allAssetBundlesArray = abList.ToArray();
    }

    /// <summary>
    /// 从ab包中加载预制体
    /// </summary>
    /// <param name="assetBundleName">assetBundle名字</param>
    /// <param name="prefabName">ab包中预制体名字</param>
    /// <returns></returns>
    public GameObject LoadPrefabFromAssetBundle(string assetBundleName, string prefabName)
    {
        for(int i = 0; i < allAssetBundlesArray.Length; i++)
        {
            AssetBundle assetBundle = allAssetBundlesArray[i];
            //忽略大小写，因为ab包名均为小写
            if (string.Equals(assetBundle.name, assetBundleName, StringComparison.OrdinalIgnoreCase))
            {
                GameObject bundlePrefab = assetBundle.LoadAsset<GameObject>(prefabName);

                return bundlePrefab;
            }
        }

        FDebugger.LogWarningFormat("没有找到名为{0}的AssetBundle，或不存在名为{1}的预制体资源", assetBundleName, prefabName);

        return null;
    }
}
