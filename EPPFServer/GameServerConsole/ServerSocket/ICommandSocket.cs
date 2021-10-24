using GameServerConsole.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerConsole.ServerSocket
{
    /// <summary>
    /// 命令对象需要实现的接口。在服务器返回消息时会调用接口方法
    /// </summary>
    public interface ICommandSocket
    {
        /// <summary>
        /// 服务器返回消息的回调
        /// </summary>
        /// <param name="content"></param>
        CommandExecuteResult ServerCallback(byte[] content);
    }
}
