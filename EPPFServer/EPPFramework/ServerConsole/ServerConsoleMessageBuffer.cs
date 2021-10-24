using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPPFramework.Log;

namespace EPPFramework.ServerConsole
{
    public class ServerConsoleMessageBuffer
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
        public ServerConsoleMessageBuffer()
        {
            this.data = new byte[ServerConsoleMessageBuffer.DEFAULT_SIZE];
            this.size = ServerConsoleMessageBuffer.DEFAULT_SIZE;
            this.ReadIndex = 0;
            this.WriteIndex = 0;
            this.Capacity = ServerConsoleMessageBuffer.DEFAULT_SIZE;
        }

        /// <summary>
        /// 检查剩余容量是否小于12，小于则移动数据到0的位置
        /// </summary>
        public void CheckAndMoveData()
        {
            if (this.Length < 12)
            {
                if (this.ReadIndex < 0)
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
            if (this.ReadIndex < 0)
            {
                return;
            }

            if (size < Length)
            {
                return;
            }

            if (size < this.size)
            {
                return;
            }

            int n = 1024;
            while (n < size)
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
        /// 以格式：‘[消息长度][命令类型ID][命令ID][消息体]’进行消息组拼
        /// </summary>
        /// <param name="commandTypeID"></param>
        /// <param name="commandID"></param>
        /// <param name="msgBody"></param>
        /// <returns></returns>
        public static byte[] EncodeMsg(int commandTypeID, int commandID, byte[] msgBody)
        {
            if (msgBody == null || msgBody.Length <= 0)
            {
                Debug.L.Warn("组拼消息时，消息体msgBody为空或空串");

                return msgBody;
            }

            //消息总长度 = 消息长度的int长度 + 处理类ID的int长度 + 处理方法ID的int长度 + 消息体长度
            int totalLength = 12 + msgBody.Length;
            byte[] data = new byte[totalLength];

            byte[] totalLengthByteArray = BitConverter.GetBytes(totalLength);
            byte[] protocolHandleByteArray = BitConverter.GetBytes(commandTypeID);
            byte[] protocolHandleMethodByteArray = BitConverter.GetBytes(commandID);

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
        /// 消息解析
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="protocolHandleID"></param>
        /// <param name="protocolHandleMethodID"></param>
        /// <param name="msg"></param>
        /// <returns>是否解析出一条消息</returns>
        public static bool DecodeMsg(ServerConsoleMessageBuffer mb, out int protocolHandleID, out int protocolHandleMethodID, out byte[] msg)
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
                Debug.L.Error("消息解析“处理类ID”时出错：" + e.Message);

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
                Debug.L.Error("消息解析“处理类中方法的ID”时出错：" + e.Message);

                return false;
            }

            //消息内容
            try
            {
                int contentLength = bodyLength - 12;

                byte[] data = new byte[contentLength];
                Array.Copy(mb.Data, mb.ReadIndex, data, 0, contentLength);

                msg = data;

                mb.ReadIndex += contentLength;
            }
            catch (Exception e)
            {
                Debug.L.Error("消息解析“消息内容”时出错：" + e.Message);

                return false;
            }

            return true;
        }
    }
}
