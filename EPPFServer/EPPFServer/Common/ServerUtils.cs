using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFServer.Common
{
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
                return "https://www.qinghuixiang.com:443/File/UNOHotfixFile";
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
    }
}
