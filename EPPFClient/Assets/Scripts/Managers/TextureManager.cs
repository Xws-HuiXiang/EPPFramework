using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 图片管理器
/// </summary>
public class TextureManager : Singleton<TextureManager>
{
    //写三个字典是因为想用空间换时间，省略由bytes转为图片的时间过程
    private Dictionary<string, byte[]> texture2DBytesDict = new Dictionary<string, byte[]>();
    private Dictionary<string, Texture2D> texture2DDict = new Dictionary<string, Texture2D>();
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    private TextureManager() { }

    /// <summary>
    /// 获取图片的bytes数据。如果已经下载过则返回缓存中的内容，否则开启线程进行下载
    /// </summary>
    /// <param name="url"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="callback"></param>
    public void GetTexture2DBytes(string url, int width, int height, Action<byte[]> callback)
    {
        if (texture2DBytesDict.TryGetValue(url, out byte[] data))
        {
            //已经下载过这个图片，返回缓存中的图片信息
            if(callback != null)
            {
                callback.Invoke(data);
            }
        }
        else
        {
            //没有下载过图片，开启一个线程进行下载
            DownloadManager.DownloadFile(url, (bytes) => 
            {
                //下载完成。首先加入缓存中。这里要加三个缓存
                AddToAllDict(url, bytes, width, height);

                //执行回调
                if (callback != null)
                {
                    callback.Invoke(bytes);
                }
            }, null);
        }
    }

    /// <summary>
    /// 获取Texture2D格式的图片。如果已经下载过则返回缓存中的内容，否则开启线程进行下载
    /// </summary>
    /// <param name="url"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="callback"></param>
    public void GetTexture2D(string url, int width, int height, Action<Texture2D> callback)
    {
        if (texture2DDict.TryGetValue(url, out Texture2D tex))
        {
            //已经下载过这个图片，返回缓存中的图片信息
            if (callback != null)
            {
                callback.Invoke(tex);
            }
        }
        else
        {
            //没有下载过图片，开启一个线程进行下载
            DownloadManager.DownloadFile(url, (bytes) =>
            {
                //下载完成。首先加入缓存中。这里要加三个缓存
                AddToAllDict(url, bytes, width, height);

                Texture2D tex2D = texture2DDict[url];
                //执行回调
                if (callback != null)
                {
                    callback.Invoke(tex2D);
                }
            }, null);
        }
    }

    /// <summary>
    /// 获取Sprite格式的图片。如果已经下载过则返回缓存中的内容，否则开启线程进行下载
    /// </summary>
    /// <param name="url"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="callback"></param>
    public void GetSprite(string url, int width, int height, Action<Sprite> callback)
    {
        if (spriteDict.TryGetValue(url, out Sprite spr))
        {
            //已经下载过这个图片，返回缓存中的图片信息
            if (callback != null)
            {
                callback.Invoke(spr);
            }
        }
        else
        {
            //没有下载过图片，开启一个线程进行下载
            DownloadManager.DownloadFile(url, (bytes) =>
            {
                //下载完成。首先加入缓存中。这里要加三个缓存
                AddToAllDict(url, bytes, width, height);

                Sprite s = spriteDict[url];
                //执行回调
                if (callback != null)
                {
                    callback.Invoke(s);
                }
            }, null);
        }
    }

    /// <summary>
    /// 将下载回来的字节数据添加到三个数组
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    private void AddToAllDict(string url, byte[] data, int width, int height)
    {
        //字节数组
        if (texture2DBytesDict.ContainsKey(url))
        {
            texture2DBytesDict[url] = data;
        }
        else
        {
            texture2DBytesDict.Add(url, data);
        }

        //Texture2D
        Texture2D tex = new Texture2D(width, height);
        tex.LoadImage(data);
        tex.Compress(true);
        if (texture2DDict.ContainsKey(url))
        {
            texture2DDict[url] = tex;
        }
        else
        {
            texture2DDict.Add(url, tex);
        }

        //Sprite
        Sprite s = Sprite.Create(tex, new Rect(0, 0, width, height), Vector2.zero);
        if (spriteDict.ContainsKey(url))
        {
            spriteDict[url] = s;
        }
        else
        {
            spriteDict.Add(url, s);
        }
    }
}
