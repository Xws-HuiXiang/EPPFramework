using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFServer.Protocol
{
    public class ProtocolHandleBase
    {
        /// <summary>
        /// 反序列化一个protobuf消息对象
        /// </summary>
        /// <typeparam name="T">消息对象类型</typeparam>
        /// <param name="data">protobuf对象序列化后的字节数组</param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] data) where T : class, IMessage, new()
        {
            T obj = new T();
            IMessage message = obj.Descriptor.Parser.ParseFrom(data);

            return message as T;
        }
    }
}
