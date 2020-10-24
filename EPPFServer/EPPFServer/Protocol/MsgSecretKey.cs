using CommonProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using EPPFServer.Attributes;
using EPPFServer.Network;

namespace EPPFServer.Protocol
{
    /// <summary>
    /// 获取秘钥协议
    /// </summary>
    [ProtocolHandleAttribute((int)MsgHandle.Secret)]
    public class MsgSecretKey
    {
        [ProtocolHandleMethodAttribute((int)MsgHandle.Secret, (int)MsgSecretKeyHandleMethod.GetSecretKey)]
        public static void GetSecretKey(ClientSocket clientSocket, byte[] msg)
        {
            Console.WriteLine("接到了消息：" + msg.ToString());
        }
    }
}
