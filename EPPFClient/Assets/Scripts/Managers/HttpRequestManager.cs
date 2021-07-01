using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class HttpRequestManager : Singleton<TextureManager>
{
    private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

    /// <summary>
    /// GET的字符串
    /// </summary>
    private const string GET_STRING = "GET";
    /// <summary>
    /// POST的字符串
    /// </summary>
    private const string POST_STRING = "POST";
    /// <summary>
    /// 表示UTF-8编码的字符串
    /// </summary>
    private const string UTF_8_STRING = "UTF-8";

    /// <summary>
    /// 发送一个同步的Get请求，返回请求的结果
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string HttpGetRequest(string url)
    {
        string res = null;
        try
        {
            HttpWebRequest getRequest = CreateGetHttpWebRequest(url);
            HttpWebResponse getResponse = getRequest.GetResponse() as HttpWebResponse;
            res = GetHttpResponse(getResponse, GET_STRING);
        }
        catch(Exception e)
        {
            FDebugger.LogErrorFormat("Http的Get请求出错。错误信息：{0}", e.Message);
        }

        return res;
    }

    /// <summary>
    /// 发送一个异步的Get请求。请求响应以回调函数的方式传递
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callback"></param>
    public static async void HttpGetRequestAsync(string url, Action<string> callback)
    {
        string res = null;
        try
        {
            HttpWebRequest getRequest = CreateGetHttpWebRequest(url);
            HttpWebResponse getResponse = await getRequest.GetResponseAsync() as HttpWebResponse;
            res = GetHttpResponse(getResponse, GET_STRING);
        }
        catch (Exception e)
        {
            FDebugger.LogErrorFormat("Http的异步Get请求出错。错误信息：{0}", e.Message);
        }

        if(callback != null)
        {
            callback.Invoke(res);
        }
        else
        {
            FDebugger.LogWarningFormat("Http的Get请求返回了一个响应，但是用于接收信息的回调函数为空。URL为：{0}", url);
        }
    }

    /// <summary>
    /// 发送一个同步的Post请求，返回请求的结果
    /// </summary>
    /// <param name="url"></param>
    /// <param name="postData"></param>
    /// <returns></returns>
    public static string HttpPostRequest(string url, string postData)
    {
        string res = null;
        try
        {
            HttpWebRequest getRequest = CreatePostHttpWebRequest(url, postData);
            HttpWebResponse getResponse = getRequest.GetResponse() as HttpWebResponse;
            res = GetHttpResponse(getResponse, POST_STRING);
        }
        catch (Exception e)
        {
            FDebugger.LogErrorFormat("Http的Post请求出错。错误信息：{0}", e.Message);
        }

        return res;
    }

    /// <summary>
    /// 发送一个异步的Post请求。请求响应以回调函数的方式传递
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callback"></param>
    public static async void HttpGetRequestAsync(string url, string postData, Action<string> callback)
    {
        string res = null;
        try
        {
            HttpWebRequest getRequest = CreatePostHttpWebRequest(url, postData);
            HttpWebResponse getResponse = await getRequest.GetResponseAsync() as HttpWebResponse;
            res = GetHttpResponse(getResponse, POST_STRING);
        }
        catch (Exception e)
        {
            FDebugger.LogErrorFormat("Http的异步Post请求出错。错误信息：{0}", e.Message);
        }

        if (callback != null)
        {
            callback.Invoke(res);
        }
        else
        {
            FDebugger.LogWarningFormat("Http的Post请求返回了一个响应，但是用于接收信息的回调函数为空。URL为：{0}", url);
        }
    }

    /// <summary>
    /// 创建一个get方法的http请求对象
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private static HttpWebRequest CreateGetHttpWebRequest(string url)
    {
        var getRequest = HttpWebRequest.Create(url) as HttpWebRequest;
        getRequest.Method = GET_STRING;
        getRequest.Timeout = 5000;
        getRequest.UserAgent = DefaultUserAgent;

        return getRequest;
    }

    /// <summary>
    /// 从一个http响应中读取响应内容的字符串
    /// </summary>
    /// <param name="response"></param>
    /// <param name="requestType"></param>
    /// <returns></returns>
    private static string GetHttpResponse(HttpWebResponse response, string requestType)
    {
        var responseResult = "";
        string encoding = UTF_8_STRING;
        if (string.Equals(requestType, POST_STRING, StringComparison.OrdinalIgnoreCase))
        {
            encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = UTF_8_STRING;
            }
        }
        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
        {
            responseResult = reader.ReadToEnd();
        }

        return responseResult;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="postData"></param>
    /// <returns></returns>
    private static HttpWebRequest CreatePostHttpWebRequest(string url, string postData)
    {
        HttpWebRequest postRequest = HttpWebRequest.Create(url) as HttpWebRequest;
        postRequest.KeepAlive = false;
        postRequest.Timeout = 5000;
        postRequest.Method = "POST";
        postRequest.ContentType = "application/x-www-form-urlencoded";
        postRequest.ContentLength = postData.Length;
        postRequest.AllowWriteStreamBuffering = false;
        StreamWriter writer = new StreamWriter(postRequest.GetRequestStream(), Encoding.ASCII);
        writer.Write(postData);
        writer.Flush();

        return postRequest;
    }
}
