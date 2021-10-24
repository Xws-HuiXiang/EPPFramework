using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPPFramework.ServerConsole.ServerConsoleAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandHandleAttribute : Attribute
    {
        private CommandType id;
        /// <summary>
        /// 当前消息处理类的ID
        /// </summary>
        public CommandType ID { get { return this.id; } }

        /// <summary>
        /// 服务器端处理客户端的消息时，使用哪个类中的方法进行处理
        /// </summary>
        /// <param name="id"></param>
        public CommandHandleAttribute(CommandType id)
        {
            this.id = id;
        }
    }
}
