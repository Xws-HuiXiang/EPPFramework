using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ciphertext;
using CommonProtocol;
using LitJson;
using UnityEngine;

namespace UNOServer.Common
{
    /// <summary>
    /// 用于读取消息缓冲区中的数据
    /// </summary>
    public class MessageUtils
    {
        /// <summary>
        /// 消息缓冲区的默认大小
        /// </summary>
        public const int DEFAULT_SIZE = 1024;
        /// <summary>
        /// 当前缓冲区的大小
        /// </summary>
        private int size;

        private byte[] data;
        /// <summary>
        /// 缓冲区
        /// </summary>
        public byte[] Data { get { return data; } }

        private int readIndex;
        /// <summary>
        /// 在缓冲区中读取数据时的开始索引
        /// </summary>
        public int ReadIndex { get { return readIndex; } }

        /// <summary>
        /// 向缓冲区中写入数据时的索引位置
        /// </summary>
        public int WriteIndex { get; set; }

        private int capacity;
        /// <summary>
        /// 当前缓冲区的容量
        /// </summary>
        public int Capacity { get { return capacity; } }

        /// <summary>
        /// 当前缓冲区剩余空间
        /// </summary>
        public int Remain
        {
            get
            {
                return Capacity - WriteIndex;
            }
        }

        /// <summary>
        /// 接收的数据长度
        /// </summary>
        public int Length
        {
            get
            {
                return WriteIndex - ReadIndex;
            }
        }

        /// <summary>
        /// 用于读取消息缓冲区中的数据
        /// </summary>
        public MessageUtils()
        {
            this.data = new byte[MessageUtils.DEFAULT_SIZE];
            this.size = MessageUtils.DEFAULT_SIZE;
            this.readIndex = 0;
            this.WriteIndex = 0;
            this.capacity = MessageUtils.DEFAULT_SIZE;
        }

        /// <summary>
        /// 检查是否需要移动数据（当消息内容小于8字节时移动数据，即小于两个int32的整数时会移动）
        /// </summary>
        public void CheckAndMoveData()
        {
            if(Length < 8)
            {
                MoveData();
            }
        }

        /// <summary>
        /// 移动数据
        /// </summary>
        public void MoveData()
        {
            if(this.readIndex < 0)
            {
                return;
            }

            Array.Copy(this.data, readIndex, this.data, 0, Length);
            WriteIndex = Length;
            SetReadIndex(0);
        }

        /// <summary>
        /// 重设缓冲区大小
        /// </summary>
        /// <param name="size"></param>
        public void ReSize(int size)
        {
            if(this.readIndex < 0)
            {
                return;
            }

            if(size < Length)
            {
                return;
            }

            if(size < this.size)
            {
                return;
            }

            int n = 1024;
            while(n < size)
            {
                //容量按照原本大小乘2扩大
                n *= 2;
            }

            this.capacity = n;
            byte[] newData = new byte[this.Capacity];
            Array.Copy(this.data, ReadIndex, newData, 0, Length);
            this.data = newData;
            this.WriteIndex = Length;
            this.readIndex = 0;
        }

        /// <summary>
        /// 设置ReadIndex
        /// </summary>
        /// <param name="readIndex"></param>
        public void SetReadIndex(int readIndex)
        {
            this.readIndex = readIndex;
        }

        /// <summary>
        /// 以格式：‘[消息长度][客户端处理消息类ID][客户端处理消息类中方法ID][消息体]’进行消息组拼
        /// </summary>
        /// <param name="protocolHandleID"></param>
        /// <param name="protocolHandleMethodID"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] EncodeMsg(int protocolHandleID, int protocolHandleMethodID, object msg)
        {
            byte[] protocolHandleByteArray = BitConverter.GetBytes(protocolHandleID);
            byte[] protocolHandleMethodByteArray = BitConverter.GetBytes(protocolHandleMethodID);
            byte[] msgByteArray = null;

            //使用LitJson序列化 
            string msgJsonString = JsonMapper.ToJson(msg);
            byte[] array = Encoding.UTF8.GetBytes(msgJsonString);

            //使用AES加密。如果是获取密钥的协议则使用公钥加密，否则使用密钥加密
            if (protocolHandleID == (int)CommonProtocol.MsgHandle.Secret && protocolHandleMethodID == (int)MsgSecretKeyHandleMethod.GetSecretKey)
            {
                msgByteArray = AES.AESEncrypt(array, NetworkManager.PublicKey);
            }
            else
            {
                msgByteArray = AES.AESEncrypt(array, NetworkManager.SecretKey);
            }
            //消息总长度 = 消息体长度 + 处理类的id长度 + 处理类中的方法id长度 + 消息体长度
            int msgTotalLength = 4 + protocolHandleByteArray.Length + protocolHandleMethodByteArray.Length + msgByteArray.Length;
            byte[] msgTotalLengthByteArray = BitConverter.GetBytes(msgTotalLength);

            //完整消息内容
            byte[] data = new byte[msgTotalLength];

            //按照规定的格式进行数组拷贝
            Array.Copy(msgTotalLengthByteArray, 0, data, 0, msgTotalLengthByteArray.Length);
            Array.Copy(protocolHandleByteArray, 0, data, protocolHandleByteArray.Length, protocolHandleByteArray.Length);
            Array.Copy(protocolHandleMethodByteArray, 0, data, protocolHandleByteArray.Length + protocolHandleMethodByteArray.Length, protocolHandleMethodByteArray.Length);
            Array.Copy(msgByteArray, 0, data, protocolHandleByteArray.Length + protocolHandleMethodByteArray.Length + msgTotalLengthByteArray.Length, msgByteArray.Length);

            return data;
        }

        /// <summary>
        /// 消息解析
        /// </summary>
        public static void DecodeMsg(MessageUtils messageUtils, out int protocolHandleID, out int protocolHandleMethodID, out byte[] msgByteArray)
        {
            protocolHandleID = 0;
            protocolHandleMethodID = 0;
            msgByteArray = null;

            //安全判断。消息长度若只包含请求头或读取索引为负数则不执行逻辑
            if (messageUtils.Length <= 12 || messageUtils.ReadIndex < 0)
            {
                return;
            }

            int readIndex = messageUtils.ReadIndex;
            byte[] data = messageUtils.Data;
            int bodyLength = BitConverter.ToInt32(data, readIndex);

            //判断接收到的消息长度是否小于‘消息长度+处理类ID长度+处理类中方法ID长度+消息内容长度’，如果小于则信息不全，如果大于则为消息为全部或为粘包
            if (messageUtils.Length < bodyLength)
            {
                return;
            }

            //先把消息头的长度加上
            messageUtils.SetReadIndex(messageUtils.ReadIndex + 4);

            //处理类的ID
            try
            {
                protocolHandleID = BitConverter.ToInt32(data, messageUtils.ReadIndex);
                messageUtils.SetReadIndex(messageUtils.ReadIndex + 4);
            }
            catch (Exception e)
            {
                FDebugger.LogError("消息解析“处理类ID”时出错：" + e.Message);

                return;
            }

            //处理类中函数的ID
            try
            {
                protocolHandleMethodID = BitConverter.ToInt32(data, messageUtils.ReadIndex);
                messageUtils.SetReadIndex(messageUtils.ReadIndex + 4);
            }
            catch (Exception e)
            {
                FDebugger.LogError("消息解析“处理类中方法的ID”时出错：" + e.Message);

                return;
            }

            //消息内容
            try
            {
                int contentLength = bodyLength - 12;
                //协议体解密
                msgByteArray = new byte[contentLength];
                Array.Copy(data, messageUtils.readIndex, msgByteArray, 0, contentLength);
                //如果是获取密钥的协议，则使用公钥解密，否则使用密钥解密
                string key = NetworkManager.SecretKey;
                if(protocolHandleID == (int)CommonProtocol.MsgHandle.Secret && protocolHandleMethodID == (int)MsgSecretKeyHandleMethod.GetSecretKey)
                {
                    key = NetworkManager.PublicKey;
                }
                msgByteArray = AES.AESDecrypt(msgByteArray, key);

                messageUtils.SetReadIndex(messageUtils.ReadIndex + contentLength);
            }
            catch (Exception e)
            {
                FDebugger.LogError("消息解析“消息内容”时出错：" + e.Message);

                return;
            }
        }

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
