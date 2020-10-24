using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFServer.Common
{
    public static class ServerUtils
    {
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
    }
}
