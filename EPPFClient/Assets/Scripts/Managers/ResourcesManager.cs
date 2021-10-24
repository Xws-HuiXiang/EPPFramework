using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using Ciphertext;

public class ResourcesManager : MonoSingleton<ResourcesManager>
{
    private AssetBundle[] allAssetBundlesArray;

    private AssetBundleManifest allBundleManifest;

    private void Awake()
    {
        //加载全部的依赖关系文件
        LoadManifest();
    }

    /// <summary>
    /// 添加ab包的引用
    /// </summary>
    /// <param name="abList"></param>
    public void AddAssetBundle(List<AssetBundle> abList)
    {
        if (abList == null) return;

        if (allAssetBundlesArray != null)
        {
            List<AssetBundle> temp = allAssetBundlesArray.ToList();
            foreach (AssetBundle ab in abList)
            {
                if (!temp.Contains(ab))
                {
                    temp.Add(ab);
                }
                else
                {
                    int index = temp.IndexOf(ab);
                    temp[index] = ab;

                    FDebugger.LogWarningFormat("添加了相同的AssetBundle包[{0}]，前者将被覆盖", ab.name);
                }
            }

            allAssetBundlesArray = temp.ToArray();
        }
        else
        {
            allAssetBundlesArray = abList.ToArray();
        }
    }

    /// <summary>
    /// 添加ab包的引用
    /// </summary>
    /// <param name="ab"></param>
    public void AddAssetBundle(AssetBundle ab)
    {
        if (ab == null) return;

        if (allAssetBundlesArray != null)
        {
            List<AssetBundle> temp = allAssetBundlesArray.ToList();
            
            if (!temp.Contains(ab))
            {
                temp.Add(ab);
            }
            else
            {
                int index = temp.IndexOf(ab);
                temp[index] = ab;

                FDebugger.LogWarningFormat("添加了相同的AssetBundle包[{0}]，前者将被覆盖", ab.name);
            }
            

            allAssetBundlesArray = temp.ToArray();
        }
        else
        {
            allAssetBundlesArray = new AssetBundle[1];
            allAssetBundlesArray[0] = ab;
        }
    }

    /// <summary>
    /// 从ab包中加载预制体
    /// </summary>
    /// <param name="assetBundleName">assetBundle名字</param>
    /// <param name="prefabName">ab包中预制体名字</param>
    /// <returns></returns>
    public GameObject LoadPrefabFromAssetBundle(string assetBundleName, string prefabName)
    {
        return GetAssetsFromAssetBundle<GameObject>(assetBundleName, prefabName);
    }

    /// <summary>
    /// 从ab包中加载sprite资源
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="spriteName"></param>
    /// <returns></returns>
    public Sprite GetSpriteFromAssetBundle(string assetBundleName, string spriteName)
    {
        return GetAssetsFromAssetBundle<Sprite>(assetBundleName, spriteName);
    }

    /// <summary>
    /// 从ab包中加载AudioClip资源
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="audioClipName"></param>
    /// <returns></returns>
    public AudioClip GetAudioClipFromAssetBundle(string assetBundleName, string audioClipName)
    {
        return GetAssetsFromAssetBundle<AudioClip>(assetBundleName, audioClipName);
    }

    /// <summary>
    /// 从ab包中加载Texture资源
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="textureName"></param>
    /// <returns></returns>
    public Texture GetTextureFromAssetBundle(string assetBundleName, string textureName)
    {
        return GetAssetsFromAssetBundle<Texture>(assetBundleName, textureName);
    }

    /// <summary>
    /// 从ab包中加载Texture2D资源
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="textureName"></param>
    /// <returns></returns>
    public Texture2D GetTexture2DFromAssetBundle(string assetBundleName, string textureName)
    {
        return GetAssetsFromAssetBundle<Texture2D>(assetBundleName, textureName);
    }

    /// <summary>
    /// 从ab包中加载TextAsset资源
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="textAssetName"></param>
    /// <returns></returns>
    public TextAsset GetTextAssetsFromAssetBundle(string assetBundleName, string textAssetName)
    {
        return GetAssetsFromAssetBundle<TextAsset>(assetBundleName, textAssetName);
    }

    /// <summary>
    /// 从ab包中加载RenderTexture资源
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="textAssetName"></param>
    /// <returns></returns>
    public RenderTexture GetRenderTextureFromAssetBundle(string assetBundleName, string textAssetName)
    {
        return GetAssetsFromAssetBundle<RenderTexture>(assetBundleName, textAssetName);
    }

    /// <summary>
    /// 从ab包中加载指定类型的资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="assetBundleName">ab包名称</param>
    /// <param name="assetsName">资源名称</param>
    /// <returns></returns>
    private T GetAssetsFromAssetBundle<T>(string assetBundleName, string assetsName) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        //如果在编辑器环境下，开启了工程中prefab的选项，则不走ab直接加载对应的预制体文件
        if (AppConst.LoadLoaclAssetBundle)
        {
            string abName = assetBundleName.Replace(AppConst.AssetBundleSuffix, "");//先去掉ab包后缀名
            assetBundleName = assetBundleName[0].ToString().ToUpper() + assetBundleName.Substring(1);//首字母转大写
            string assetPath = Path.Combine("Assets", AppConst.LocalPrefabPath, abName, assetsName);
            //根据类型添加后缀
            Type tType = typeof(T);
            if (string.Equals(tType.Name, "GameObject", StringComparison.OrdinalIgnoreCase))
            {
                assetPath += ".prefab";
            }
            else if (string.Equals(tType.Name, "Sprite", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(tType.Name, "Texture", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(tType.Name, "Texture2D", StringComparison.OrdinalIgnoreCase))
            {
                assetPath += ".png";
            }
            else if (string.Equals(tType.Name, "AudioClip", StringComparison.OrdinalIgnoreCase))
            {
                assetPath += ".ogg";
            }
            else if(string.Equals(tType.Name, "RenderTexture", StringComparison.OrdinalIgnoreCase))
            {
                assetPath += ".renderTexture";
            }
            T obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (obj == null)
            {
                FDebugger.LogWarningFormat("加载工程目录下的资源。没有找到名为[{0}]的文件夹，资源完整路径：{1}", abName, assetPath);
            }

            return obj;
        }
#endif
        T assets = null;
        if (CheckAbLoaded(assetBundleName, out AssetBundle ab))
        {
            assets = ab.LoadAsset<T>(assetsName);
            if (assets == null)
            {
                FDebugger.LogWarningFormat("未在包{0}中找到名为{1}的资源", assetBundleName, assetsName);
            }
        }
        if (assets == null)
        {
            //ab包未加载，先加载对应的ab包
            string abFullName = GetABFullPath(assetBundleName);
            AssetBundle assetBundle = LoadABRes(abFullName);
            assets = assetBundle.LoadAsset<T>(assetsName);
            if (assets == null)
            {
                FDebugger.LogWarningFormat("加载ab包后，未在包{0}中找到名为{1}的资源", abFullName, assetsName);
            }
        }

        if (assets == null)
        {
            FDebugger.LogWarningFormat("没有找到名为[{0}]的AssetBundle", assetBundleName);
        }

        return assets;
    }

    /// <summary>
    /// 检查某个ab包是否已经加载
    /// </summary>
    /// <param name="assetBundleName">ab包的名称（不是完整路径，单纯的文件名）</param>
    /// <param name="ab"></param>
    /// <returns></returns>
    private bool CheckAbLoaded(string assetBundleName, out AssetBundle ab)
    {
        ab = null;

        if (allAssetBundlesArray != null)
        {
            string keyName = assetBundleName;
            if (!keyName.EndsWith(AppConst.AssetBundleSuffix))
            {
                keyName += AppConst.AssetBundleSuffix;
            }
            for (int i = 0; i < allAssetBundlesArray.Length; i++)
            {
                AssetBundle assetBundle = allAssetBundlesArray[i];
                //忽略大小写，因为ab包名均为小写。这里的后缀名是.ab 不是自己加密的后缀名
                if (string.Equals(assetBundle.name, keyName, StringComparison.OrdinalIgnoreCase))
                {
                    ab = assetBundle;

                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 获取ab包的完整路径
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    private string GetABFullPath(string assetBundleName)
    {
        string abFullName = CheckAssetBundleName(assetBundleName);
        abFullName = Path.Combine(AppConst.LocalResRootFolderPath, abFullName);
//#if UNITY_EDITOR_OSX
//        abFullName = abFullName.Replace("\\", "/");
//#elif UNITY_STANDALONE
//        abFullName = abFullName.Replace("/", "\\");
//#elif UNITY_ANDROID
//        abFullName = abFullName.Replace("\\", "/");
//#elif UNITY_IOS
//        abFullName = abFullName.Replace("/", "\\");
//#else
//        abFullName = abFullName.Replace("/", "\\");
//#endif

        return abFullName;
    }

    /// <summary>
    /// 检查ab包名称，如果不包含对应的后缀则添加
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    private string CheckAssetBundleName(string assetBundleName)
    {
        string abFullName;
        //先尝试去掉.ab
        if (assetBundleName.EndsWith(".ab"))
        {
            assetBundleName = assetBundleName.Substring(0, assetBundleName.Length - 3);
        }
        //添加加密后的文件类型
        if (assetBundleName.EndsWith(AppConst.EncryptionFillSuffix))
        {
            abFullName = assetBundleName;
        }
        else
        {
            abFullName = assetBundleName + AppConst.EncryptionFillSuffix;
        }

        return abFullName;
    }

    /// <summary>
    /// 加载ab包
    /// </summary>
    /// <param name="assetBundleName">资源包的名称（不是完整路径）</param>
    public AssetBundle LoadABRes(string assetBundleName)
    {
        AssetBundle ab = null;
        if (assetBundleName.EndsWith(AppConst.EncryptionFillSuffix))
        {
            //如果是manifest文件，则跳过
            if (assetBundleName.EndsWith(".manifest" + AppConst.EncryptionFillSuffix))
            {
                return null;
            }

            //文件是以对应的后缀名结尾，先解密
            byte[] encryptionContent = File.ReadAllBytes(assetBundleName);
            byte[] abFileBytes = AES.AESDecrypt(encryptionContent, AppConst.AbPackageKey);
            //根据byte加载ab包
            try
            {
                ab = AssetBundle.LoadFromMemory(abFileBytes);
            }
            catch(Exception e)
            {
                FDebugger.LogErrorFormat("加载ab包出错，信息：{0}", e.Message);
            }
        }
        else
        {
            //不是对应后缀名的文件，尝试直接加载ab
            try
            {
                byte[] abFileBytes = File.ReadAllBytes(assetBundleName);
                ab = AssetBundle.LoadFromMemory(abFileBytes);
            }
            catch (Exception e)
            {
                FDebugger.LogErrorFormat("加载ab时遇到一个错误。尝试加载的文件既不是加密文件也不是unity未加密的标准ab文件。文件名：{0}。错误信息：{1}", assetBundleName, e.Message);
            }
        }

        if(ab != null)
        {
            ResourcesManager.Instance.AddAssetBundle(ab);

            //处理依赖
            if(allBundleManifest != null)
            {
                string[] dependenciesArray = allBundleManifest.GetAllDependencies(ab.name);
                for(int i = 0; i < dependenciesArray.Length; i++)
                {
                    string dependenciesName = dependenciesArray[i];
                    //string dependenciesName = dependenciesArray[i].Substring(0, dependenciesArray[i].Length - 3);
                    //string abFullName = CheckAssetBundleName(dependenciesName);
                    //abFullName = Path.Combine(AppConst.LocalResRootFolderPath, abFullName);
                    //abFullName = abFullName.Replace("/", "\\");
                    //判断这个包有没有被加载
                    if (!CheckAbLoaded(dependenciesName, out _))
                    {
                        LoadABRes(dependenciesName);
                    }
                }
            }
            else
            {
                FDebugger.LogWarning("ab包的总依赖信息为空");
            }
        }
        else
        {
            FDebugger.LogWarningFormat("加载后的ab包为空，res的路径为：{0}", assetBundleName);
        }

        return ab;
    }

    /// <summary>
    /// 加载全部的依赖关系文件
    /// </summary>
    public void LoadManifest()
    {
        string abFullName;
        if (AppConst.LoadLoaclAssetBundle)
        {
            abFullName = Path.Combine(Application.dataPath, "AssetBundle", "AssetBundle", "AssetBundle");
        }
        else
        {
            abFullName = Path.Combine(AppConst.LocalResRootFolderPath, "AssetBundle");
        }
        //abFullName = abFullName.Replace("/", "\\");

        if (File.Exists(abFullName))
        {
            AssetBundle manifestAssetBundle = AssetBundle.LoadFromFile(abFullName);
            allBundleManifest = (AssetBundleManifest)manifestAssetBundle.LoadAsset("AssetBundleManifest");
        }
    }
}
