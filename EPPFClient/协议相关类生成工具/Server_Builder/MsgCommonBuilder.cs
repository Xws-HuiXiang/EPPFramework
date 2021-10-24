using Example;
using System.Collections.Generic;

namespace EPPFramework.Protocol.Builder
{
    /// <summary>
    /// 配置协议的例子 消息序列化工具类
    /// </summary>
    public class MsgCommonBuilder : MsgBuilderBase
    {
        private static MsgCommonBuilder instance;
        public static MsgCommonBuilder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MsgCommonBuilder();
                }

                return instance;
            }
        }

        private MsgCommonBuilder() : base(MsgHandleID.Common) { }

        /// <summary>
        /// 序列化 获取密钥请求
        /// </summary>
        /// <param name="publicKey">客户端的消息公钥</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] SecretKeyRequestSerialize(string publicKey, byte[] msgSecretKey)
        {
            SecretKeyRequest msg = new SecretKeyRequest();

            msg.PublicKey = publicKey;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.SecretKeyRequest, msgSecretKey, true);
        }
        /// <summary>
        /// 序列化 获取密钥响应
        /// </summary>
        /// <param name="secretKey">密钥内容</param>
        /// <param name="msgPublicKey">服务端消息公钥</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] SecretKeyReceiveSerialize(string secretKey, string msgPublicKey, byte[] msgSecretKey)
        {
            SecretKeyReceive msg = new SecretKeyReceive();

            msg.SecretKey = secretKey;
            msg.MsgPublicKey = msgPublicKey;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.SecretKeyReceive, msgSecretKey, true);
        }
        /// <summary>
        /// 序列化 心跳包请求
        /// </summary>
        /// <param name="pingTime">时间戳</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] HeartbeatRequestSerialize(long pingTime, byte[] msgSecretKey)
        {
            HeartbeatRequest msg = new HeartbeatRequest();

            msg.PingTime = pingTime;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.HeartbeatRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 心跳包响应
        /// </summary>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] HeartbeatReceiveSerialize(byte[] msgSecretKey)
        {
            HeartbeatReceive msg = new HeartbeatReceive();


            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.HeartbeatReceive, msgSecretKey);
        }
    }
}
