using CommonProtocol;
using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using EPPFServer.Attributes;
using EPPFServer.Network;

namespace EPPFServer.Protocol
{
    /// <summary>
    /// 心跳包处理相关协议
    /// </summary>
    [ProtocolHandleAttribute((int)MsgHandle.Ping)]
    public class MsgPingHandle
    {
        [ProtocolHandleMethodAttribute((int)MsgHandle.Ping, (int)MsgPingMethod.Ping)]
        public static void PingTime(ClientSocket clientSocket, byte[] msg)
        {
            string jsonString = Encoding.UTF8.GetString(msg);
            LastPing lastPing = JsonMapper.ToObject<LastPing>(jsonString);

            clientSocket.SetLastPingTime(lastPing.lastPingTime);
        }
    }
}
