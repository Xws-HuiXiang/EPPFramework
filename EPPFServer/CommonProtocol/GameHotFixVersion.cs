using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProtocol
{
    /// <summary>
    /// 游戏版本相关处理方法
    /// </summary>
    public enum MsgGameVersionHandleMethod
    {
        /// <summary>
        /// 获取服务器热更资源版本
        /// </summary>
        GetGameHotFixVersion
    }

    /// <summary>
    /// 获取游戏热更版本
    /// </summary>
    public class GameHotFixVersion : MsgBase
    {
        public GameHotFixVersion()
        {
            base.ProtocolHandleID = (int)MsgHandle.Version;
            base.ProtocolHandleMethodID = (int)MsgGameVersionHandleMethod.GetGameHotFixVersion;
        }

        public UnityEngine.RuntimePlatform platform;
        public int resVersion;
        public int luaVersion;
    }
}
