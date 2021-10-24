using GameServerConsole.GameServerConsoleAttribute;
using GameServerConsole.Utils;
using EPPFramework.ServerConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerConsole.Command.CommandList.Console
{
    [Command("Clear", CommandID.Clear, CommandType.Console)]
    public class ClearCommand : CommandBase
    {
        private ClearCommand()
        {

        }

        public override string CommandFormat => "Clear";

        public override string CommandDescription => "清空当前控制台的文字，同cmd中的‘clear’命令";

        public override CommandExecuteResult Execute(string[] parameters, string[] toggleArray)
        {
            System.Console.Clear();

            return ReturnSuccessResult();
        }
    }
}
