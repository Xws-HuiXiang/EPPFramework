using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFramework.Http.Data
{
    /// <summary>
    /// 请求时，返回错误的数据对象
    /// </summary>
    public class ErrorResponseData
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// 错误消息具体内容
        /// </summary>
        public string Message { get; set; }
    }
}
