using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServerConsole.Common;

namespace GameServerConsole.Utils
{
    public class CommandExecuteResult
    {
        private bool result;
        /// <summary>
        /// 命令执行是否成功
        /// </summary>
        public bool Result { get { return result; } }
        private CommandExecuteStateCode stateCode;
        /// <summary>
        /// 执行结果的状态码
        /// </summary>
        public CommandExecuteStateCode StateCode { get { return stateCode; } }
        private string message;
        /// <summary>
        /// 命令执行失败时的提示消息
        /// </summary>
        public string Message { get { return message; } }

        public bool IsUsing { get; set; }

        public CommandExecuteResult() : this(true, CommandExecuteStateCode.Success, null) { }

        public CommandExecuteResult(bool result, CommandExecuteStateCode stateCode, string message)
        {
            this.result = result;
            this.stateCode = stateCode;
            this.message = message;
            this.IsUsing = false;
        }

        /// <summary>
        /// 设置执行结果的标志位
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(bool result)
        {
            this.result = result;
        }

        /// <summary>
        /// 设置执行结果的状态码
        /// </summary>
        /// <param name="stateCode"></param>
        public void SetStateCode(CommandExecuteStateCode stateCode)
        {
            this.stateCode = stateCode;
        }

        /// <summary>
        /// 设置错误时的消息提示
        /// </summary>
        /// <param name="message"></param>
        public void SetMessage(string message)
        {
            this.message = message;
        }
    }
}
