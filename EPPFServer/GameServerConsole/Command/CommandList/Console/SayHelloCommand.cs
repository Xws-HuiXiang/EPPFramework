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
    [Command("SayHello", CommandID.SayHello, CommandType.Console)]
    public class SayHelloCommand : CommandBase
    {
        private SayHelloCommand()
        {
        }

        public SayHelloCommand(string commandName) : base(commandName)
        {
        }

        public override string CommandFormat => "SayHello";

        public override string CommandDescription => "控制台打印欢迎语句";

        public override CommandExecuteResult Execute(string[] parameters, string[] toggleArray)
        {
            CommandUtil.SayHello();

            return ReturnSuccessResult();
        }
    }
}
