using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using EPPFramework.Common;
using EPPFramework.Http.Attributes;
using EPPFramework.Http.Handler;
using EPPFramework.Log;

namespace EPPFramework.Http
{
    /// <summary>
    /// http请求方法
    /// </summary>
    public enum HttpMethodEnum
    {
        Get,
        Post,
        Put,
        Delete
    }

    /// <summary>
    /// 用于Http请求的接收
    /// </summary>
    public class HttpServerSocket : Singleton<HttpServerSocket>
    {
        public const string GET_STRING = "GET";
        public const string POST_STRING = "POST";
        public const string PUT_STRING = "PUT";
        public const string DELETE_STRING = "DELETE";
        public HttpServerSocket() { }

        private static HttpListener httpListener;

        /// <summary>
        /// http请求的处理字典
        /// </summary>
        private Dictionary<string, Action<HttpListenerContext, Dictionary<string, string>>> httpRequestHandleDict = new Dictionary<string, Action<HttpListenerContext, Dictionary<string, string>>>();
        
        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Init(string ip, int port)
        {
            //添加所有http的请求处理类
            InitHttpRequestHandle();

            try
            {
                httpListener = new HttpListener();
                httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//验证客户端的方案（没懂，可不写。这个是默认值，意思为匿名的）
                httpListener.Prefixes.Add(string.Format("{0}:{1}/", ip, port));
                httpListener.Start();
                Debug.L.Debug(string.Format("开启http监听成功，地址为：{0}:{1}/", ip, port));

                httpListener.BeginGetContext(OnGetContextCallback, null);
            }
            catch(Exception e)
            {
                Debug.L.Error(string.Format("开启http监听失败：{0}", e.Message));
            }
        }

        /// <summary>
        /// http请求处理方法
        /// </summary>
        /// <param name="ar"></param>
        private void OnGetContextCallback(IAsyncResult ar)
        {
            try
            {
                if (httpListener.IsListening)
                {
                    HttpListenerContext context = httpListener.EndGetContext(ar);
                    HttpListenerRequest request = context.Request;

                    //拆分url
                    int firstQuestionMarkIndex = request.RawUrl.IndexOf('?');
                    string url;
                    Dictionary<string, string> parameterDict = null;

                    //如果url最后有斜杠，则去掉
                    if (request.RawUrl.EndsWith('/'))
                    {
                        //url = request.RawUrl.Substring(0, request.RawUrl.Length - 2);
                        url = request.RawUrl[0..^2];//C#8.0增加的语法糖：范围运算符。[startIndex .. endIndex]；^表示从数组末尾计算索引
                    }
                    else
                    {
                        url = request.RawUrl;
                    }
                    //处理参数
                    if (firstQuestionMarkIndex != -1)
                    {
                        //有'?' 连接后面跟着参数
                        parameterDict = new Dictionary<string, string>();

                        string parametersString = request.RawUrl.Substring(firstQuestionMarkIndex + 1);
                        if (!string.IsNullOrEmpty(parametersString))
                        {
                            string[] parametersArray = parametersString.Split('&');
                            for(int i = 0; i < parametersArray.Length; i++)
                            {
                                string parameterString = parametersArray[i];
                                int equalSignIndex = parameterString.IndexOf('=');
                                if(equalSignIndex != -1)
                                {
                                    string key = parameterString.Substring(0, equalSignIndex);
                                    string value = parameterString.Substring(equalSignIndex + 1, parameterString.Length - 1);

                                    if (parameterDict.ContainsKey(key))
                                    {
                                        //参数名称存在重复
                                        Debug.L.Warn(string.Format("url请求的参数中，参数名称存在重复。url为：[{0}]。名称为：[{1}]", request.RawUrl, key));
                                    }
                                    else
                                    {
                                        parameterDict.Add(key, value);
                                    }
                                }
                                else
                                {
                                    Debug.L.Warn(string.Format("url请求的参数中，参数没有等于号，无法区分key和value。url为：[{0}]。参数为：[{1}]", request.RawUrl, parameterString));
                                }
                            }
                        }
                        else
                        {
                            Debug.L.Warn(string.Format("连接请求有‘?’但是没有参数列表。url为：{0}", request.RawUrl));
                        }
                    }

                    if(httpRequestHandleDict.TryGetValue(url, out Action<HttpListenerContext, Dictionary<string, string>> action))
                    {
                        action.Invoke(context, parameterDict);
                    }
                }
                else
                {
                    //当前http请求的服务端没有开启监听
                    Debug.L.Warn("当前http请求的服务端没有开启监听");
                }
            }
            catch(Exception e)
            {
                Debug.L.Error(string.Format("接收数据时出现异常，异常信息：{0}", e.Message));
            }
            finally
            {
                //再次开启监听
                httpListener.BeginGetContext(OnGetContextCallback, null);
            }
        }

        /// <summary>
        /// 将http方法的枚举项转换为对应的字符串名称
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        public static string GetHttpMethodStringByEnum(HttpMethodEnum httpMethod)
        {
            string res;
            switch (httpMethod)
            {
                case HttpMethodEnum.Get:
                    res = GET_STRING;
                    break;
                case HttpMethodEnum.Post:
                    res = POST_STRING;
                    break;
                case HttpMethodEnum.Put:
                    res = PUT_STRING;
                    break;
                case HttpMethodEnum.Delete:
                    res = DELETE_STRING;
                    break;
                default:
                    Debug.L.Error(string.Format("http方法名称字符串获取时，遇到未知的类型：{0}", nameof(httpMethod)));
                    res = GET_STRING;
                    break;
            }

            return res;
        }

        /// <summary>
        /// 初始化所有标记了HttpHandleAttribute特性的类
        /// </summary>
        private void InitHttpRequestHandle()
        {
            httpRequestHandleDict.Clear();

            //获取带有“http消息处理类特性”的所有class
            //首先获取当前程序集
            AssemblyName currentAssemblyName = Assembly.GetCallingAssembly().GetName();
            Assembly asm = Assembly.Load(currentAssemblyName);
            //获取所有自定义类型
            Type[] customTypes = asm.GetExportedTypes();
            //验证当前类型是否含有“http消息处理类特性”
            for (int i = 0; i < customTypes.Length; i++)
            {
                //获取当前类的所有特性
                Type t = customTypes[i];
                IEnumerable<Attribute> attributeList = t.GetCustomAttributes();
                foreach (Attribute attribute in attributeList)
                {
                    //检查是否有标记为“http消息处理类”
                    if (attribute is HttpHandleAttribute)
                    {
                        //当前类标记为了“http消息处理类”
                        HttpHandleAttribute httpRequestHandle = attribute as HttpHandleAttribute;

                        //给当前类赋值单例
                        FieldInfo instanceFiledInfo = t.BaseType.GetField("instance", BindingFlags.NonPublic | BindingFlags.Instance);
                        //创建一个该类的对象
                        dynamic obj = asm.CreateInstance(customTypes[i].FullName, false, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null, null);
                        instanceFiledInfo.SetValue(obj, obj);

                        //查找类中标记了http请求处理方法的函数
                        MethodInfo[] methods = t.GetMethods();
                        for (int j = 0; j < methods.Length; j++)
                        {
                            IEnumerable<Attribute> methodAttributeList = methods[j].GetCustomAttributes();
                            foreach (Attribute methodAttribute in methodAttributeList)
                            {
                                if (methodAttribute is HttpHandleMethodAttribute)
                                {
                                    //当前方法标记为了“http请求处理方法”
                                    HttpHandleMethodAttribute httpRequestMethodHandle = methodAttribute as HttpHandleMethodAttribute;

                                    MethodInfo method = methods[j];

                                    //组拼正确的url（容错，处理斜杠的问题）
                                    string httpRequestURL = httpRequestHandle.URL.StartsWith('/') ? httpRequestHandle.URL : "/" + httpRequestHandle.URL;
                                    httpRequestURL = httpRequestURL.EndsWith('/') ? httpRequestURL : httpRequestURL + "/";
                                    string httpRequestMethodURL = httpRequestMethodHandle.URL.StartsWith('/') ? httpRequestMethodHandle.URL.Substring(1) : httpRequestMethodHandle.URL;
                                    httpRequestMethodURL = httpRequestMethodURL.EndsWith('/') ? httpRequestMethodURL[0..^2] : httpRequestMethodURL;
                                    string urlKey = httpRequestURL + httpRequestMethodURL;

                                    if (httpRequestHandleDict.ContainsKey(urlKey))
                                    {
                                        Debug.L.Warn(string.Format("Http请求处理方法字典，尝试添加重复的key：{0}", urlKey));
                                    }
                                    else
                                    {
                                        httpRequestHandleDict.Add(urlKey, (context, parameterDict) => 
                                        {
                                            method.Invoke(obj, new object[] { context, parameterDict });
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
