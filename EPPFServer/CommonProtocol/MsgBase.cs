using System;
using System.Collections.Generic;
using System.Text;

namespace CommonProtocol
{
    /// <summary>
    /// 相关的处理类
    /// </summary>
    public enum MsgHandle
    {
        /// <summary>
        /// 密钥相关
        /// </summary>
        Secret,
        /// <summary>
        /// 游戏版本相关
        /// </summary>
        Version,
        /// <summary>
        /// 心跳包相关
        /// </summary>
        Ping
    }

    /// <summary>
    /// 协议基类
    /// </summary>
    public class MsgBase
    {
        /// <summary>
        /// 协议处理类ID
        /// </summary>
        public int ProtocolHandleID { get; set; }
        /// <summary>
        /// 协议处理类中方法ID
        /// </summary>
        public int ProtocolHandleMethodID { get; set; }
    }
}
