using Ciphertext;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;
using EPPFramework.Common;
using EPPFramework.Log;
using EPPFramework.Network;

namespace EPPFramework.Protocol.Builder
{
    /// <summary>
    /// 序列化各种消息的基类
    /// </summary>
    public class MsgBuilderBase
    {
        private MsgHandleID handleID;
        /// <summary>
        /// 消息处理类ID
        /// </summary>
        public MsgHandleID HandleID { get { return handleID; } }

        protected MsgBuilderBase(MsgHandleID handleID) 
        {
            this.handleID = handleID;
        }

        /// <summary>
        /// 序列化一个对象并且使用AES加密
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="handleMethodID">消息处理方法ID</param>
        /// <param name="msgSecretKey">消息加密用的密钥。每个客户端每次连接的密钥均不同，使用DH算法生成</param>
        /// <param name="usePublicKey">是否使用公钥加密</param>
        /// <returns>返回以特定格式组拼后的消息字节数组</returns>
        protected byte[] SerializeAndEncryption(IMessage obj, int handleMethodID, byte[] msgSecretKey, bool usePublicKey)
        {
            //protobuf序列化
            byte[] data = obj.ToByteArray();

            //加密
            try
            {
                if (usePublicKey)
                {
                    data = AES.AESEncrypt(data, ServerSocket.PublicKey);
                }
                else
                {
                    string msgSecretKeyString = Convert.ToBase64String(msgSecretKey);
                    data = AES.AESEncrypt(data, msgSecretKeyString);
                }
            }
            catch(Exception e)
            {
                Debug.L.Warn(string.Format("消息加密失败，要加密的内容为空。错误信息：{0}", e.Message));
            }

            //消息以特定格式组拼
            data = MessageBuffer.EncodeMsg((int)this.HandleID, handleMethodID, data);

            return data;
        }

        /// <summary>
        /// 序列化一个对象并且使用私钥进行AES加密。使用密钥加密
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="handleMethodID">消息处理方法ID</param>
        /// <param name="msgSecretKey">消息加密用的密钥。每个客户端每次连接的密钥均不同，使用DH算法生成</param>
        /// <returns>返回以特定格式组拼后的消息字节数组</returns>
        protected byte[] SerializeAndEncryption(IMessage obj, int handleMethodID, byte[] msgSecretKey)
        {
            return SerializeAndEncryption(obj, handleMethodID, msgSecretKey, false);
        }

        public override string ToString()
        {
            return "MsgBuilderBase的处理类名称为：" + Enum.GetName(typeof(MsgHandleID), this.HandleID);
        }
    }
}
