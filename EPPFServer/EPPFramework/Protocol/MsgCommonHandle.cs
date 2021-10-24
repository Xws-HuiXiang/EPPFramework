using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using EPPFramework.Attributes;
using EPPFramework.Network;
using System.Text.RegularExpressions;
using EPPFramework.Log;
using EPPFramework.Common;
using EPPFramework.Protocol.Builder;
using EPPFramework.DataBase;
using System.IO;
using Ciphertext;
using Example;

namespace EPPFramework.Protocol
{
    /// <summary>
    /// 心跳包处理相关协议
    /// </summary>
    [ProtocolHandleAttribute((int)MsgHandleID.Common)]
    public class MsgCommonHandle : ProtocolHandleBase
    {
        /// <summary>
        /// 获取公钥请求
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="data"></param>
        [ProtocolHandleMethodAttribute((int)MsgCommonMethodID.SecretKeyRequest)]
        public static void PublicKey(ClientSocket clientSocket, byte[] data)
        {
            SecretKeyRequest msg = ProtocolHandleBase.Deserialize<SecretKeyRequest>(data);

            clientSocket.SetMsgSecretKey(msg.PublicKey);

            //回复服务端的消息公钥
            string serverPublicKey = DHExchange.GetString(clientSocket.MsgPublicKey);
            byte[] receiveMsg = MsgCommonBuilder.SecretKeyReceiveSerialize(ServerSocket.SecretKey, serverPublicKey, clientSocket.MsgSecretKey);
            ServerSocket.Instance.SendMessage(clientSocket, receiveMsg);
        }

        /// <summary>
        /// 心跳包请求
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="data"></param>
        [ProtocolHandleMethodAttribute((int)MsgCommonMethodID.HeartbeatRequest)]
        public static void PingTime(ClientSocket clientSocket, byte[] data)
        {
            HeartbeatRequest msg = ProtocolHandleBase.Deserialize<HeartbeatRequest>(data);
            clientSocket.SetLastPingTime(msg.PingTime);
        }
    }
}
