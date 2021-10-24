using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFramework.Common
{
    /// <summary>
    /// 各种配置路径的路由标头
    /// </summary>
    public enum FilePathRouting
    {
        Default,
        Local,
        Http,
        Https,
        File
    }

    public static class ServerUtils
    {
        /// <summary>
        /// Development字符串
        /// </summary>
        public const string DEVELOPMENT_STRING = "Development";
        /// <summary>
        /// Release字符串
        /// </summary>
        public const string RELEASE_STRING = "Release";

        public const string ROUTING_LOCAL_STRING = "local://";
        public const string ROUTING_HTTP_STRING = "http://";
        public const string ROUTING_HTTPS_STRING = "https://";
        public const string ROUTING_FILE_STRING = "file://";

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            //计算机时间均从1970年1月1日0时0分0秒0毫秒开始计算
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 服务器图片等资源根路径
        /// </summary>
        public static string ServerFileRootPath
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// 返回当前服务器exe文件的路径
        /// </summary>
        public static string ServerExeFolderPath
        {
            get
            {
                return AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
        }

        /// <summary>
        /// 根据字符串返回加载中提示文件路劲的路由标头
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [Obsolete("应统一使用GetRouting(string path, out string content)重载方法")]
        public static FilePathRouting GetLoadingTipsRouting(string path)
        {
            FilePathRouting routing;
            if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                routing = FilePathRouting.Http;
            }
            else if (path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                routing = FilePathRouting.Https;
            }
            else if (path.StartsWith("Local://", StringComparison.OrdinalIgnoreCase))
            {
                routing = FilePathRouting.Local;
            }
            else if (path.StartsWith("File://", StringComparison.OrdinalIgnoreCase))
            {
                routing = FilePathRouting.File;
            }
            else
            {
                routing = FilePathRouting.Default;
            }

            return routing;
        }

        /// <summary>
        /// 获取一个路径的路由类型并返回路由标头之后的实际路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="routing"></param>
        /// <returns></returns>
        public static string ParseRouting(string path, out FilePathRouting routing)
        {
            string content;
            if (path.StartsWith(ROUTING_HTTP_STRING, StringComparison.OrdinalIgnoreCase))
            {
                routing = FilePathRouting.Http;

                content = path.Substring(ROUTING_HTTP_STRING.Length);
            }
            else if (path.StartsWith(ROUTING_HTTPS_STRING, StringComparison.OrdinalIgnoreCase))
            {
                routing = FilePathRouting.Https;

                content = path.Substring(ROUTING_HTTPS_STRING.Length);
            }
            else if (path.StartsWith(ROUTING_LOCAL_STRING, StringComparison.OrdinalIgnoreCase))
            {
                routing = FilePathRouting.Local;

                content = path.Substring(ROUTING_LOCAL_STRING.Length);
            }
            else if (path.StartsWith(ROUTING_FILE_STRING, StringComparison.OrdinalIgnoreCase))
            {
                routing = FilePathRouting.File;

                content = path.Substring(ROUTING_FILE_STRING.Length);
            }
            else
            {
                routing = FilePathRouting.Default;

                content = path;
            }

            return content;
        }

        /// <summary>
        /// 获取一个路径的路由类型并返回路由标头之后的实际路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ParseRouting(string path)
        {
            return ParseRouting(path, out _);
        }
    }
}
