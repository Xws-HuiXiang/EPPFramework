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
    [Command("Quit", CommandID.Quit, CommandType.Console)]
    public class QuitCommand : CommandBase
    {
        public QuitCommand()
        {
        }

        public QuitCommand(string commandName) : base(commandName)
        {
        }

        public override string CommandFormat => "Quit";

        public override string CommandDescription => "退出服务器控制台";

        public override bool NeedConfirm => true;

        public override CommandExecuteResult Execute(string[] parameters, string[] toggleArray)
        {
            Program.Quit();

            return ReturnSuccessResult();
        }
    }
}
