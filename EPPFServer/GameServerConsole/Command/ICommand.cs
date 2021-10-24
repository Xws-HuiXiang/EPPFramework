using GameServerConsole.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerConsole.Command
{
    public interface ICommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameters">该命令需要的参数</param>
        /// <param name="toggleList">该命令的开关列表</param>
        /// <returns>命令执行是否成功</returns>
        CommandExecuteResult Execute(string[] parameters, string[] toggleArray);

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameterList">该命令需要的参数</param>
        /// <param name="toggleList">该命令的开关列表</param>
        /// <returns>命令执行是否成功。仅能指示有没有语法错误</returns>
        CommandExecuteResult Execute(List<string> parameterList, List<string> toggleList);

        /// <summary>
        /// 命令格式提示
        /// </summary>
        string CommandFormat { get; }
        /// <summary>
        /// 命令的解释说明
        /// </summary>
        string CommandDescription { get; }

        /// <summary>
        /// 执行命令前是否需要确认（Y/N）
        /// </summary>
        bool NeedConfirm { get; }
    }
}
