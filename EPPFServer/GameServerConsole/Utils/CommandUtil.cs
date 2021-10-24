using GameServerConsole.Command;
using GameServerConsole.Common;
using GameServerConsole.ServerSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerConsole.Utils
{
    public class CommandUtil
    {
        /// <summary>
        /// 是否处于确认命令的状态
        /// </summary>
        public static bool ConfirmCommandState { get; private set; }

        /// <summary>
        /// 命令确认执行期间的命令对象缓存
        /// </summary>
        private static CommandBase cacheCommand = null;
        /// <summary>
        /// 命令确认执行期间的命令参数缓存
        /// </summary>
        private static string[] cacheParameters = null;
        /// <summary>
        /// 命令确认执行期间的命令开关缓存
        /// </summary>
        private static string[] cacheTogglArray = null;

        /// <summary>
        /// 等待服务器返回中的命令对象
        /// </summary>
        private static CommandBase commandSocketAwait = null;

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="parameters">参数</param>
        /// <param name="toggles">开关</param>
        /// <param name="checkNeedConfirm">检查二次确认</param>
        public static void ExecuteCommand(CommandBase command, string[] parameters, string[] toggles, bool checkNeedConfirm = true)
        {
            if (command.NeedConfirm && checkNeedConfirm)
            {
                ConfirmCommandState = true;

                cacheCommand = command;
                cacheParameters = parameters;
                cacheTogglArray = toggles;

                Console.WriteLine("确认执行该命令吗？");
                if (!string.IsNullOrEmpty(command.ConfirmDescription))
                {
                    Console.WriteLine(command.ConfirmDescription);
                }
                Console.WriteLine("输入'Y'确认，'N'取消");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(string.Format("[{0}]正在执行命令", command.CommandName));
                Console.ForegroundColor = ConsoleColor.White;

                CommandExecuteResult commandResult = command.Execute(parameters, toggles);
                //处理命令执行结果
                HandleCommandExecuteResult(command, commandResult);
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <param name="parameters">参数</param>
        /// <param name="toggles">开关</param>
        /// <param name="checkNeedConfirm">检查二次确认</param>
        public static void ExecuteCommand(string commandName, string[] parameters, string[] toggles, bool checkNeedConfirm = true)
        {
            CommandBase cmd = GetCommandByName(commandName);
            if (cmd != null)
            {
                ExecuteCommand(cmd, parameters, toggles);
            }
            else
            {
                Console.WriteLine("未知的命令名称：" + commandName);
            }
        }

        /// <summary>
        /// 处理命令执行结果
        /// </summary>
        /// <param name="result"></param>
        private static void HandleCommandExecuteResult(CommandBase command, CommandExecuteResult result)
        {
            if (result.StateCode == CommandExecuteStateCode.Await)
            {
                //命令返回了网络等待状态
                commandSocketAwait = command;
            }
            else
            {
                //命令返回不是等待中的状态
                if (result.Result)
                {
                    //命令执行成功
                }
                else
                {
                    //命令执行失败。打印错误信息
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("命令[{0}]执行失败，错误码：{1}|{2}。异常信息：{3}", command.CommandName, (int)result.StateCode, result.StateCode.ToString(), result.Message ?? ""));
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(string.Format("[{0}]命令执行完成", command.CommandName));
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// 命令确认
        /// </summary>
        /// <param name="result">确认的输入结果</param>
        public static void ConfirmCommand(string result)
        {
            if (result.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                //确认
                CommandUtil.ExecuteCommand(cacheCommand, cacheParameters, cacheTogglArray, false);
            }
            else if (result.Equals("N", StringComparison.CurrentCultureIgnoreCase))
            {
                //取消
                Console.WriteLine(string.Format("命令[{0}]取消执行", cacheCommand.CommandName));
            }
            else
            {
                //未知的标志
                Console.WriteLine(string.Format("请输入正确的标志。命令[{0}]已取消执行", cacheCommand.CommandName));
            }
            cacheCommand = null;
            cacheParameters = null;
            cacheTogglArray = null;

            ConfirmCommandState = false;
        }

        /// <summary>
        /// 根据命令名称获取命令对象
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public static CommandBase GetCommandByName(string commandName)
        {
            foreach(var cmd in Program.CommandDict)
            {
                if(cmd.Key.commandName.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                {
                    return cmd.Value as CommandBase;
                }
            }

            Console.WriteLine(string.Format("根据名称获取命令对象失败，名称为：{0}", commandName));

            return null;
        }

        /// <summary>
        /// 从服务器返回了结果，停止命令等待
        /// </summary>
        /// <param name="result"></param>
        public static void StopSocketAwait(byte[] msg)
        {
            Console.WriteLine("正在处理服务器返回的结果");

            CommandExecuteResult result = commandSocketAwait.ServerCallback(msg);

            HandleCommandExecuteResult(commandSocketAwait, result);

            commandSocketAwait = null;
        }

        /// <summary>
        /// 控制台打印欢迎语句
        /// </summary>
        public static void SayHello()
        {
            Console.WriteLine("************************************************************");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("* 欢迎使用服务器控制台                                     *");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("* 控制台相关命令请参考手册（暂时没有手册）                 *");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("* 基本格式为：CommandName [params, -toggles]               *");
            Console.WriteLine("* 命令名称不区分大小写                                     *");
            Console.WriteLine("* 查看某个命令的说明请使用:help [commandName]              *");
            Console.WriteLine("* 注：开关需要以‘-’开头，否则视为参数                    *");
            Console.WriteLine("* help中，以‘[]’包围的参数为选填，以‘<>’包围的参数必填 *");
            Console.WriteLine("************************************************************");
        }
    }
}
