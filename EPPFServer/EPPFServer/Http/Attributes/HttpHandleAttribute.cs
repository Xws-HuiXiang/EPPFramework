using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFServer.Http.Attributes
{
    /// <summary>
    /// 标记该类为http请求的处理类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpHandleAttribute : Attribute 
    {
        private string url;
        public string URL { get { return url; } }

        public HttpHandleAttribute(string url)
        {
            this.url = url;
        }
    }
}
