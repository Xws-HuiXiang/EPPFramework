using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFramework.Attributes
{
    /// <summary>
    /// 服务器端处理客户端的消息时，使用哪个类中的方法进行处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ProtocolHandleAttribute : Attribute
    {
        private int id;
        /// <summary>
        /// 当前消息处理类的ID
        /// </summary>
        public int ID { get { return this.id; } }

        /// <summary>
        /// 服务器端处理客户端的消息时，使用哪个类中的方法进行处理
        /// </summary>
        /// <param name="id"></param>
        public ProtocolHandleAttribute(int id)
        {
            this.id = id;
        }
    }
}
