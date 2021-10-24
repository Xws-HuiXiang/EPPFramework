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
    [Command("Help", CommandID.Help, CommandType.Console)]
    public class HelpCommand : CommandBase
    {
        private HelpCommand()
        {

        }

        public override string CommandFormat => "Help [CommandName]";
        public override Dictionary<string, string> ParametersTipDict => new Dictionary<string, string>()
        {
            {"CommandName", "查询的命令名称" }
        };

        public override string CommandDescription => "查询某个命令的解释说明和参数、开关说明。若不写命令名称则输出全部可用的命令列表";

        public override CommandExecuteResult Execute(string[] parameters, string[] toggleArray)
        {
            if (parameters == null || parameters.Length <= 0)
            {
                //无参，显示所有可用命令的列表
                foreach (var cmd in Program.CommandDict)
                {
                    System.Console.WriteLine(string.Format("{0}:{1}", cmd.Key.commandName, cmd.Value.CommandDescription));
                }
            }
            else if (parameters.Length > 0)
            {
                //有参数
                string helpCommandName = parameters[0];
                CommandBase cmd = CommandUtil.GetCommandByName(helpCommandName);
                if (cmd != null)
                {
                    cmd.Help();
                }
                else
                {
                    System.Console.WriteLine(string.Format("命令[{0}]不存在", helpCommandName));
                }
            }

            return ReturnSuccessResult();
        }
    }
}
