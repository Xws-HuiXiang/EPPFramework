using GameServerConsole.Command;
using EPPFramework.ServerConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerConsole.GameServerConsoleAttribute
{
    /// <summary>
    /// 每个具体命令均需要标记此特性
    /// </summary>
    public class CommandAttribute : Attribute
    {
        private string commandName;
        public string CommandName { get { return commandName; } }
        private CommandType commandType;
        public CommandType CommandType { get { return commandType; } }
        private CommandID commandID;
        public CommandID CommandID { get { return commandID; } }

        /// <summary>
        /// 该类为一个命令
        /// </summary>
        /// <param name="commandName">命令名称</param>
        public CommandAttribute(string commandName, CommandID commandID, CommandType commandType = CommandType.Other)
        {
            this.commandName = commandName;
            this.commandType = commandType;
            this.commandID = commandID;
        }
    }
}
