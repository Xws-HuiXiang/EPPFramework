using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFServer.Attributes
{
    /// <summary>
    /// 服务器端处理客户端请求的具体逻辑函数
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    class ProtocolHandleMethodAttribute : Attribute
    {
        private int methodID;
        /// <summary>
        /// 方法ID
        /// </summary>
        public int MethodID { get { return this.methodID; } }

        /// <summary>
        /// 服务器端处理客户端请求的具体逻辑函数
        /// </summary>
        /// <param name="methodID">当前方法ID</param>
        public ProtocolHandleMethodAttribute(int methodID)
        {
            this.methodID = methodID;
        }
    }
}
