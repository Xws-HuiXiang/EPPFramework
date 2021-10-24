using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPPFramework.ServerConsole.ServerConsoleAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandMethodHandleAttribute : Attribute
    {
        private CommandID methodID;
        /// <summary>
        /// 方法ID
        /// </summary>
        public CommandID MethodID { get { return this.methodID; } }

        /// <summary>
        /// 服务器端处理客户端请求的具体逻辑函数
        /// </summary>
        /// <param name="methodID">当前方法ID</param>
        public CommandMethodHandleAttribute(CommandID methodID)
        {
            this.methodID = methodID;
        }
    }
}
