using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ciphertext;
using LuaInterface;
using ProtoBuf;

namespace UNOServer.Common
{
    /// <summary>
    /// 用于读取消息缓冲区中的数据
    /// </summary>
    public class MessageBuffer
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

        /// <summary>
        /// 在缓冲区中读取数据时的开始索引
        /// </summary>
        public int ReadIndex { get; set; }

        /// <summary>
        /// 向缓冲区中写入数据时的索引位置
        /// </summary>
        public int WriteIndex { get; set; }

        /// <summary>
        /// 当前缓冲区的容量
        /// </summary>
        public int Capacity { get; set; }

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
        public MessageBuffer()
        {
            this.data = new byte[MessageBuffer.DEFAULT_SIZE];
            this.size = MessageBuffer.DEFAULT_SIZE;
            this.ReadIndex = 0;
            this.WriteIndex = 0;
            this.Capacity = MessageBuffer.DEFAULT_SIZE;
        }

        /// <summary>
        /// 检查剩余容量是否小于12，小于则移动数据到0的位置
        /// </summary>
        public void CheckAndMoveData()
        {
            if(this.Length < 12)
            {
                if(this.ReadIndex < 0)
                {
                    return;
                }

                Array.Copy(this.data, this.ReadIndex, this.data, 0, Length);
                this.WriteIndex = this.Length;
                this.ReadIndex = 0;
            }
        }

        /// <summary>
        /// 重设缓冲区大小
        /// </summary>
        /// <param name="size"></param>
        public void ReSize(int size)
        {
            if(this.ReadIndex < 0)
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

            int n = MessageBuffer.DEFAULT_SIZE;
            while(n < size)
            {
                //容量按照原本大小乘2扩大
                n *= 2;
            }

            this.Capacity = n;
            byte[] newData = new byte[this.Capacity];
            Array.Copy(this.data, ReadIndex, newData, 0, Length);
            this.data = newData;
            this.WriteIndex = Length;
            this.ReadIndex = 0;
        }

        /// <summary>
        /// 以格式：‘[消息长度][客户端处理消息类ID][客户端处理消息类中方法ID][消息体]’进行消息组拼
        /// </summary>
        /// <param name="protocolHandleID"></param>
        /// <param name="protocolHandleMethodID"></param>
        /// <param name="msgBody"></param>
        /// <returns></returns>
        public static byte[] EncodeMsg(int protocolHandleID, int protocolHandleMethodID, byte[] msgBody)
        {
            //消息总长度 = 消息长度的int长度 + 处理类ID的int长度 + 处理方法ID的int长度 + 消息体长度
            int totalLength = 12 + msgBody.Length;
            byte[] data = new byte[totalLength];

            byte[] totalLengthByteArray = BitConverter.GetBytes(totalLength);
            byte[] protocolHandleByteArray = BitConverter.GetBytes(protocolHandleID);
            byte[] protocolHandleMethodByteArray = BitConverter.GetBytes(protocolHandleMethodID);

            int partOneLength = 0;//0
            int partTwoLength = partOneLength + totalLengthByteArray.Length;//4
            int partThreeLength = partTwoLength + protocolHandleByteArray.Length;//8
            int partFourLength = partThreeLength + protocolHandleMethodByteArray.Length;//12

            Array.Copy(totalLengthByteArray, 0, data, partOneLength, totalLengthByteArray.Length);
            Array.Copy(protocolHandleByteArray, 0, data, partTwoLength, protocolHandleByteArray.Length);
            Array.Copy(protocolHandleMethodByteArray, 0, data, partThreeLength, protocolHandleMethodByteArray.Length);
            Array.Copy(msgBody, 0, data, partFourLength, msgBody.Length);

            return data;
        }

        /// <summary>
        /// 消息解析 protobuf版
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="secretKey"></param>
        /// <param name="protocolHandleID"></param>
        /// <param name="protocolHandleMethodID"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool DecodeMsg(MessageBuffer mb, byte[] secretKey, out int protocolHandleID, out int protocolHandleMethodID, out byte[] msg)
        {
            protocolHandleID = 0;
            protocolHandleMethodID = 0;
            msg = null;

            //安全判断。消息长度若只包含请求头或读取索引为负数则不执行逻辑
            if (mb.Length <= 12 || mb.ReadIndex < 0)
            {
                return false;
            }

            //一个消息的总长度
            int bodyLength = BitConverter.ToInt32(mb.Data, mb.ReadIndex);

            //判断接收到的消息长度是否小于‘消息长度+处理类ID长度+处理类中方法ID长度+消息内容长度’，如果小于则信息不全，如果大于则为消息为全部或为粘包
            if (mb.Length < bodyLength)
            {
                return false;
            }

            //先把消息头的长度加上
            mb.ReadIndex += 4;

            //处理类的ID
            try
            {
                protocolHandleID = BitConverter.ToInt32(mb.Data, mb.ReadIndex);
                mb.ReadIndex += 4;
            }
            catch (Exception e)
            {
                FDebugger.LogError("消息解析“处理类ID”时出错：" + e.Message);

                return false;
            }

            //处理类中函数的ID
            try
            {
                protocolHandleMethodID = BitConverter.ToInt32(mb.Data, mb.ReadIndex);
                mb.ReadIndex += 4;
            }
            catch (Exception e)
            {
                FDebugger.LogError("消息解析“处理类中方法的ID”时出错：" + e.Message);

                return false;
            }

            //消息内容
            try
            {
                int contentLength = bodyLength - 12;

                byte[] data = new byte[contentLength];
                Array.Copy(mb.Data, mb.ReadIndex, data, 0, contentLength);

                //消息体解密
                if (protocolHandleID == 0 && protocolHandleMethodID == 1)
                {
                    //消息是“密钥相关”且为“获取密钥响应”，使用公钥解密
                    msg = AES.AESDecrypt(data, NetworkManager.PublicKey);
                }
                else
                {
                    //使用私钥解密
                    string msgSecretKeyString = DHExchange.GetString(secretKey);
                    msg = AES.AESDecrypt(data, msgSecretKeyString);
                }

                mb.ReadIndex += contentLength;
            }
            catch (Exception e)
            {
                FDebugger.LogError("消息解析“消息内容”时出错：" + e.Message);

                return false;
            }

            return true;
        }
    }
}
