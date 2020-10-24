using System;
using System.Collections.Generic;
using System.Text;

namespace CommonProtocol
{
    /// <summary>
    /// 获取私钥消息。处理方法
    /// </summary>
    public enum MsgSecretKeyHandleMethod
    {
        /// <summary>
        /// 获取密钥
        /// </summary>
        GetSecretKey
    }

    /// <summary>
    /// 获取密钥的消息体
    /// </summary>
    public class MsgSecretKey : MsgBase
    {
        public MsgSecretKey()
        {
            base.ProtocolHandleID = (int)MsgHandle.Secret;
            base.ProtocolHandleMethodID = (int)MsgSecretKeyHandleMethod.GetSecretKey;
        }

        /// <summary>
        /// 密钥
        /// </summary>
        public string secretKey;
    }
}
