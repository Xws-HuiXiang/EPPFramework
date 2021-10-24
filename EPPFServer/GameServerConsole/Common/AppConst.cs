using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerConsole.Common
{
    public class AppConst
    {

    }

    public enum CommandExecuteStateCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 正在等待服务器返回消息。使用等待状态的结果需要重写ServerCallback方法返回执行的具体结果
        /// </summary>
        Await = 1,
        /// <summary>
        /// 参数个数异常
        /// </summary>
        ParametersCountException = 2,
        /// <summary>
        /// 空参数异常
        /// </summary>
        NullParameterException = 3,
        /// <summary>
        /// 服务器处理命令时出现异常
        /// </summary>
        ServerHandleException = 4,
        /// <summary>
        /// 参数类型异常
        /// </summary>
        ParameterTypeExecption = 5
    }
}
