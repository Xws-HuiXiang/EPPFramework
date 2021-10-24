using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPPFramework.ServerConsole.CommandUtil
{
    public class ProtobufUtil
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

        /// <summary>
        /// 序列化一个对象
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>返回以特定格式组拼后的消息字节数组</returns>
        public static byte[] Serialize(IMessage obj)
        {
            //protobuf序列化
            byte[] data = obj.ToByteArray();

            return data;
        }
    }
}
