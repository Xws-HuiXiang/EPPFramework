using EPPFramework.ServerConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerConsole.Command
{
    public struct CommandInfo
    {
        public string commandName;
        public CommandType commandType;
        public CommandID commandID;

        public CommandInfo(string commandName, CommandType commandType, CommandID commandID)
        {
            this.commandName = commandName;
            this.commandType = commandType;
            this.commandID = commandID;
        }
    }
}
