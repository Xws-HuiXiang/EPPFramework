using DG.Tweening;
using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 工具类。提供各种工具方法
/// </summary>
public static class Utils
{
    /// <summary>
    /// 主要是给Lua用的切割字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string[] StringSplit(string str, char separator)
    {
        return str.Split(separator);
    }

    /// <summary>
    /// 主要是给Lua用的切割字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string[] StringSplit(string str, string separator)
    {
        try
        {
            char c = char.Parse(separator);

            return str.Split(c);
        }
        catch(Exception e)
        {
            FDebugger.LogErrorFormat("切割字符串时，给的切割字符串无法解析为单个字符。给定的字符串为：{0}。错误信息：{1}", separator, e.Message);
        }

        return new string[1] { str };
    }

    /// <summary>
    /// 字节数组转AudioClip
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="rawData"></param>
    /// <returns></returns>
    public static AudioClip ConvertBytesToClip(string clipName, byte[] rawData)
    {
        float[] samples = new float[rawData.Length / 2];
        float rescaleFactor = 32767;
        short st = 0;
        float ft = 0;

        for (int i = 0; i < rawData.Length; i += 2)
        {
            st = System.BitConverter.ToInt16(rawData, i);
            ft = st / rescaleFactor;
            samples[i / 2] = ft;
        }

        AudioClip audioClip = AudioClip.Create(clipName, samples.Length, 1, 44100, false);
        audioClip.SetData(samples, 0);

        return audioClip;
    }

    /// <summary>
    /// AudioClip转字节数组
    /// </summary>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    public static byte[] ConvertClipToBytes(AudioClip audioClip)
    {
        float[] samples = new float[audioClip.samples];

        audioClip.GetData(samples, 0);

        short[] intData = new short[samples.Length];

        byte[] bytesData = new byte[samples.Length * 2];

        int rescaleFactor = 32767;

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            byte[] byteArr = new byte[2];
            byteArr = System.BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        return bytesData;
    }

    /// <summary>
    /// 获取某个路径中所有文件的完整路径
    /// </summary>
    /// <param name="directory">文件夹路径</param>
    /// <param name="searchPattern">文件名匹配串</param>
    /// <returns></returns>
    public static string[] GetFilesByDirectory(string directory, string searchPattern)
    {
        if (!Directory.Exists(directory))
        {
            //路径不存在
            FDebugger.LogWarningFormat("获取文件时指定的路径[{0}]不存在", directory);

            return null;
        }

        return Directory.GetFiles(directory, searchPattern);
    }

    /// <summary>
    /// 获取某个路径中所有文件的完整路径。结果不包含指定后缀名的文件
    /// </summary>
    /// <param name="directory">文件夹路径</param>
    /// <param name="searchPattern">文件名匹配串</param>
    /// <param name="blackList">需要去掉的某些后缀名的文件</param>
    /// <returns></returns>
    public static string[] GetFilesByDirectory(string directory, string searchPattern, string[] blackList)
    {
        string[] fileArray = GetFilesByDirectory(directory, searchPattern);
        List<string> fileList = new List<string>();
        for (int i = 0; i < blackList.Length; i++)
        {
            string blackName = blackList[i];
            for(int j = 0; j < fileArray.Length; j++)
            {
                string fileName = fileArray[j];
                if (!fileName.EndsWith(blackName))
                {
                    fileName = fileName.Replace('/', Path.DirectorySeparatorChar);
                    fileName = fileName.Replace('\\', Path.DirectorySeparatorChar);
                    fileList.Add(fileName);
                }
            }
        }

        return fileList.ToArray();
    }

    /// <summary>
    /// 将字符串类型转换为bool类型
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool ParseBoolean(string value)
    {
        bool res;
        try
        {
            res = bool.Parse(value);
        }
        catch(Exception e)
        {
            FDebugger.LogWarningFormat("字符串转bool时出错，错误信息：{0}", e.Message);

            res = default;
        }

        return res;
    }

    /// <summary>
    /// 获取字符串长度。字符串中的中文按两个字符长度计算
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int GetStringLength(string str)
    {
        if (str.Equals(string.Empty))
        {
            return 0;
        }

        int strlen = 0;
        ASCIIEncoding strData = new ASCIIEncoding();
        //将字符串转换为ASCII编码的字节数字
        byte[] strBytes = strData.GetBytes(str);
        for (int i = 0; i <= strBytes.Length - 1; i++)
        {
            if (strBytes[i] == 63)  //中文都将编码为ASCII编码63,即"?"号
                strlen++;
            strlen++;
        }

        return strlen;
    }

    /// <summary>
    /// 去除字符串两端的空格
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string StringTrim(string str)
    {
        return str.Trim();
    }

    /// <summary>
    /// 去除字符串开头的空格
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string StringTrimStart(string str)
    {
        return str.TrimStart();
    }

    /// <summary>
    /// 去除字符串结尾的空格
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string StringTrimEnd(string str)
    {
        return str.TrimEnd();
    }

    /// <summary>
    /// 取整形的随机数
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int RandomRangeInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    /// <summary>
    /// 取浮点型的随机数
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float RandomRangeFloat(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    /// <summary>
    /// 根据总秒数返回小时、分钟和秒
    /// </summary>
    /// <param name="totalSeconds"></param>
    /// <param name="hours"></param>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    public static void FormatTimeFromTotalSeconds(long totalSeconds, out int hours, out int minutes, out int seconds)
    {
        DateTime t = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(totalSeconds);
        hours = t.Hour;
        minutes = t.Minute;
        seconds = t.Second;
    }

    /// <summary>
    /// 获取当前的时间戳
    /// </summary>
    /// <returns></returns>
    public static long GetTimeStamp()
    {
        //计算机时间均从1970年1月1日0时0分0秒0毫秒开始计算
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

        return Convert.ToInt64(ts.TotalSeconds);
    }

    /// <summary>
    /// 获取当前的时间戳的string版本
    /// </summary>
    /// <returns></returns>
    public static string GetTimeStampString()
    {
        return Utils.GetTimeStamp().ToString();
    }

    /// <summary>
    /// 创建显示在UI上的3d物体的相机
    /// </summary>
    /// <param name="parent">相机父物体。如果没有父物体则为null</param>
    /// <param name="localPosition">相机的局部坐标</param>
    /// <param name="eulerAngles">相机的局部旋转角</param>
    /// <param name="cullingMask">渲染层</param>
    /// <param name="texture">输出的RenderTexture对象。如果没有则为null</param>
    /// <returns>返回创建的相机对象</returns>
    public static Camera CreateTopUICamera(Transform parent, Vector3 localPosition, Vector3 eulerAngles, int[] cullingMask, RenderTexture texture)
    {
        Transform cameraTrans = new GameObject("TopUICamera").transform;
        Camera camera = cameraTrans.gameObject.AddComponent<Camera>();

        cameraTrans.SetParent(parent);
        cameraTrans.localEulerAngles = eulerAngles;
        cameraTrans.localPosition = localPosition;
        cameraTrans.localScale = Vector3.one;

        //相机渲染层
        if(cullingMask != null && cullingMask.Length > 0)
        {
            camera.cullingMask = 0;
            int cullingMaskValue = 0;
            for(int i = 0; i < cullingMask.Length; i++)
            {
                int temp = 1 << cullingMask[i];
                cullingMaskValue += temp;
            }
            if(cullingMaskValue != 0)
            {
                camera.cullingMask = cullingMaskValue;
            }
        }
        //设置渲染输出对象
        camera.targetTexture = texture;
        //设置擦除标记
        camera.clearFlags = CameraClearFlags.Color;

        return camera;
    }

    /// <summary>
    /// 创建UI的游戏物体（预制体中一定要包含RectTransform）
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static RectTransform CreateUIGameObject(Transform prefab, Transform parent)
    {
        RectTransform rectTrans = GameObject.Instantiate(prefab).GetComponent<RectTransform>();

        rectTrans.SetParent(parent);
        rectTrans.localScale = Vector3.one;
        rectTrans.localRotation = Quaternion.identity;

        rectTrans.gameObject.SetActive(true);

        return rectTrans;
    }

    /// <summary>
    /// DOTween的To方法封装
    /// </summary>
    /// <param name="getter"></param>
    /// <param name="setter"></param>
    /// <param name="endValue"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static Tweener DOTweenTo(Func<float> getter, Action<float> setter, float endValue, float duration)
    {
        return DOTween.To(() => { float value = getter.Invoke(); return value; }, (v) => { setter.Invoke(v); }, endValue, duration);
    }

    /// <summary>
    /// 清空持久化目录中的文件
    /// </summary>
    public static void ClearPersistentDataFolder()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i]);
        }
    }
}
