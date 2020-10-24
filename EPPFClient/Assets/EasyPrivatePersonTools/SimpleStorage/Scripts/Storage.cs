using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace EPPTools.SimpleStorage
{
    /// <summary>
    /// 简单存储。用于存储键值对形式的值
    /// </summary>
    public class Storage
    {
        private readonly string filePath;
        private bool autoSave;
        private Encoding encoding;
        private MonoBehaviour mono;
        private Action loadFinishedCallBack;

        private Dictionary<string, object> data = new Dictionary<string, object>();
        /// <summary>
        /// 初始化是否完成
        /// </summary>
        public bool InitFinished { get; private set; } = false;
        /// <summary>
        /// 当前用来存储的对象是否进行自动保存
        /// </summary>
        public bool AutoSave { get { return autoSave; } }

        /// <summary>
        /// 用简单的格式存储简单的数据
        /// </summary>
        /// <param name="filePath">存储文件的完整路径，需要后缀名</param>
        /// <param name="mono">加载文件内容时使用协程异步加载</param>
        public Storage(string filePath, MonoBehaviour mono) : this(filePath, mono, null, null, true)
        {
            this.encoding = new UTF8Encoding(false);
            this.autoSave = true;
            this.mono = mono;
            this.loadFinishedCallBack = null;
        }

        public Storage(string filePath, MonoBehaviour mono, Action loadFinishedCallBack) : this(filePath, mono, loadFinishedCallBack, null, true)
        {
            this.encoding = new UTF8Encoding(false);
            this.autoSave = true;
            this.mono = mono;
            this.loadFinishedCallBack = loadFinishedCallBack;
        }

        /// <summary>
        /// 用简单的格式存储简单的数据
        /// </summary>
        /// <param name="filePath">存储文件的完整路径，需要后缀名</param>
        /// <param name="mono">加载文件内容时使用协程异步加载</param>
        /// <param name="loadFinishedCallBack">加载文件内容完成时的回调函数</param>
        /// <param name="encoding">写入文件时使用的编码格式</param>
        public Storage(string filePath, MonoBehaviour mono, Action loadFinishedCallBack, Encoding encoding) : this(filePath, mono, loadFinishedCallBack, encoding, true)
        {

        }

        /// <summary>
        /// 用简单的格式存储简单的数据
        /// </summary>
        /// <param name="filePath">存储文件的完整路径，需要后缀名</param>
        /// <param name="mono">加载文件内容时使用协程异步加载</param>
        /// <param name="loadFinishedCallBack">加载文件内容完成时的回调函数</param>
        /// <param name="encoding">写入文件时使用的编码格式</param>
        /// <param name="autoSave">自动写入文件</param>
        public Storage(string filePath, MonoBehaviour mono, Action loadFinishedCallBack, Encoding encoding, bool autoSave)
        {
            this.filePath = filePath;
            this.autoSave = autoSave;
            this.encoding = encoding;
            this.mono = mono;
            this.loadFinishedCallBack = loadFinishedCallBack;

            if (File.Exists(this.filePath))
            {
                //已经存在配置文件，读取内容
                mono.StartCoroutine(InitDataDict(loadFinishedCallBack));
            }
            else
            {
                FileStream fs = File.Create(filePath);
                fs.Close();

                this.InitFinished = true;
            }
        }

        /// <summary>
        /// 下载文件。为了兼容安卓和IOS系统等不能直接用File读取的平台
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitDataDict(Action callBack)
        {
            this.InitFinished = false;
            data.Clear();

            UnityWebRequest request = UnityWebRequest.Get(this.filePath);
            yield return request.SendWebRequest();

            if (request.isHttpError)
            {
                Debug.LogError(request.error);

                yield break;
            }
            if (request.isDone)
            {
                string content = File.ReadAllText(this.filePath);
                content = content.Replace("\r", "");
                string[] split = content.Split('\n');
                foreach (string text in split)
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        string[] array = text.Split('=');
                        data.Add(array[0], array[1]);
                    }
                }
            }

            this.InitFinished = true;
            if(callBack != null)
            {
                callBack.Invoke();
            }
        }

        /// <summary>
        /// 重新加载文件内容。加载完成后也会调用加载完成后的回调
        /// </summary>
        public void Reload()
        {
            mono.StartCoroutine(InitDataDict(loadFinishedCallBack));
        }

        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                return GetValue(key);
            }
        }

        /// <summary>
        /// 设置写入文件时的编码格式
        /// </summary>
        /// <param name="encoding">新的文件编码格式</param>
        public void SetEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }

        /// <summary>
        /// 设置是否自动写入文件
        /// </summary>
        /// <param name="value"></param>
        public void SetAutoSave(bool value)
        {
            this.autoSave = value;
        }

        /// <summary>
        /// 设置存储的键与值。若autoSave为false则需要手动调用Save方法写入文件
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void SetValue(string key, object value)
        {
            if (data.ContainsKey(key))
            {
                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }

            if (this.autoSave)
            {
                this.Save();
            }
        }

        /// <summary>
        /// 获取存储的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>当前值</returns>
        public object GetValue(string key, object defaultValue = null)
        {
            if (data.ContainsKey(key))
            {
                return data[key];
            }
            else
            {
                if(defaultValue != null)
                {
                    return defaultValue;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取存储的值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>当前值</returns>
        public T GetValue<T>(string key, T defaultValue = default)
        {
            if (data.ContainsKey(key))
            {
                return (T)data[key];
            }
            else
            {
                if (defaultValue != null)
                {
                    return defaultValue;
                }
                else
                {
                    return default;
                }
            }
        }

        /// <summary>
        /// 获取存储的值。在进行数据类型转换时进行异常捕获
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>当前值</returns>
        public T GetValueCheck<T>(string key, T defaultValue = default)
        {
            if (data.ContainsKey(key))
            {
                object v = data[key];
                T value = default;
                try
                {
                    value = (T)v;
                }
                catch(Exception e)
                {
#if UNITY_EDITOR
                    Debug.LogErrorFormat("类型转换错误。字典[{0}]类型的值类型不能转换为[{1}]类型。错误信息：\n{2}", data.GetType().ToString(), typeof(T).ToString(), e.ToString());
#else
                    Console.WriteLine("类型转换错误。字典[{0}]类型的值类型不能转换为[{1}]类型。错误信息：\n{2}", data.GetType().ToString(), typeof(T).ToString(), e.ToString());
#endif
                }

                return value;
            }
            else
            {
                if (defaultValue != null)
                {
                    return defaultValue;
                }
                else
                {
                    return default;
                }
            }
        }

        /// <summary>
        /// 获取所有的键
        /// </summary>
        /// <returns>所有键的数组</returns>
        public string[] GetAllKeys()
        {
            string[] array = new string[data.Count];
            int i = 0;
            foreach(KeyValuePair<string, object> item in data)
            {
                array[i] = item.Key;
                i++;
            }

            return array;
        }

        /// <summary>
        /// 删除指定的键值对。返回值表示是否删除成功
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteKey(string key)
        {
            if (data.ContainsKey(key))
            {
                data.Remove(key);

                if (this.autoSave)
                {
                    this.Save();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除全部的键值对
        /// </summary>
        public void DeleteAllKey()
        {
            data.Clear();

            if (this.autoSave)
            {
                this.Save();
            }
        }

        /// <summary>
        /// 写入文件。写入格式为key=value\r\n
        /// </summary>
        public void Save()
        {
            StringBuilder builder = new StringBuilder();
            foreach(KeyValuePair<string, object> item in data)
            {
                builder.Append(item.Key);
                builder.Append("=");
                builder.Append(item.Value);
                builder.Append("\r\n");
            }
            char[] valueData = encoding.GetChars(encoding.GetBytes(builder.ToString()));
            FileStream fs = new FileStream(this.filePath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, encoding);
            sw.Write(valueData);
            sw.Close();
            fs.Close();
        }
    }
}
