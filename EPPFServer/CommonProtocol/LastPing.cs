using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProtocol
{
    /// <summary>
    /// 获取私钥消息。处理方法
    /// </summary>
    public enum MsgPingMethod
    {
        /// <summary>
        /// 收发心跳包
        /// </summary>
        Ping
    }

    /// <summary>
    /// 心跳包协议
    /// </summary>
    public class LastPing : MsgBase
    {
        public LastPing()
        {
            base.ProtocolHandleID = (int)MsgHandle.Ping;
            base.ProtocolHandleMethodID = (int)MsgPingMethod.Ping;
        }

        public long lastPingTime;
    }
}
