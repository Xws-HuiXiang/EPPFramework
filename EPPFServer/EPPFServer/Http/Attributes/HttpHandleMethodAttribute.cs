using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFServer.Http.Attributes
{
    /// <summary>
    /// 服务器端处理客户端的消息时，使用哪个类中的方法进行处理。用于标记这个当前方法为处理方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpHandleMethodAttribute : Attribute
    {
        private string url;
        public string URL { get { return url; } }

        public HttpHandleMethodAttribute(string url)
        {
            this.url = url;
        }
    }
}
