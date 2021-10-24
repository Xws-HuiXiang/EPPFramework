using GameServerConsole.Common;
using GameServerConsole.ServerSocket;
using GameServerConsole.Utils;
using Google.Protobuf;
using EPPFramework.ServerConsole;
using EPPFramework.ServerConsole.CommandUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerConsole.Command
{
    public abstract class CommandBase : ICommand, ICommandSocket
    {
        /// <summary>
        /// 这个字段会在初始化时使用反射赋值
        /// </summary>
        protected string commandName;
        /// <summary>
        /// 该命令的名称
        /// </summary>
        public string CommandName { get { return commandName; } }
        /// <summary>
        /// 这个字段会在初始化时使用反射赋值
        /// </summary>
        protected CommandType commandType;
        /// <summary>
        /// 该命令的类型
        /// </summary>
        public CommandType CommandType { get { return commandType; } }
        /// <summary>
        /// 这个字段会在初始化时使用反射赋值
        /// </summary>
        protected CommandID commandID;
        /// <summary>
        /// 该命令的ID
        /// </summary>
        public CommandID CommandID { get { return commandID; } }

        public abstract string CommandFormat { get; }

        /// <summary>
        /// 命令的解释说明
        /// </summary>
        public abstract string CommandDescription { get; }

        /// <summary>
        /// 命令执行前是否需要确认
        /// </summary>
        public virtual bool NeedConfirm { get { return false; } }
        /// <summary>
        /// 确认命令时输出的提示内容
        /// </summary>
        public virtual string ConfirmDescription { get { return null; } }

        /// <summary>
        /// Help命令中显示的参数列表说明。key为参数名称，value为解释说明
        /// </summary>
        public virtual Dictionary<string, string> ParametersTipDict { get; }
        /// <summary>
        /// 开关的说明。key为开关的名称，value为解释说明
        /// </summary>
        public virtual Dictionary<string, string> ToggleTipDict { get; }

        //TODO:在基类中实现命令格式

        protected CommandBase()
        {

        }

        public CommandBase(string commandName)
        {
            this.commandName = commandName;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameters">该命令需要的参数</param>
        /// <param name="toggleArray">该命令使用的开关</param>
        /// <returns>命令执行是否成功。仅能指示有没有语法错误</returns>
        public abstract CommandExecuteResult Execute(string[] parameters, string[] toggleArray);

        public virtual CommandExecuteResult Execute(List<string> parameterList, List<string> toggleList)
        {
            return Execute(parameterList.ToArray(), toggleList.ToArray());
        }

        /// <summary>
        /// 在控制台中对该命令使用Help
        /// </summary>
        public virtual void Help()
        {
            //先输出命令格式
            Console.WriteLine(string.Format("命令格式：{0}", CommandFormat));
            //命令的解释说明
            Console.WriteLine(string.Format("解释：{0}", CommandDescription));
            //参数
            Console.WriteLine("参数：");
            if (ParametersTipDict != null && ParametersTipDict.Count > 0)
            {
                //输出参数列表前，先计算缩进
                int[] tabCountArray = new int[ParametersTipDict.Count];
                //找到参数名称最长的长度，除以4后标记tab次数为1，其他参数的缩进值以这个为基准计算
                int maxParameterLength = 0;
                foreach (var item in ParametersTipDict)
                {
                    int length = item.Key.Length + 1;//加一是后面要有个冒号
                    if (length > maxParameterLength)
                    {
                        maxParameterLength = length;
                    }
                }
                //计算tab后的长度
                int tabTempCount = maxParameterLength % 4;
                int realyLength = tabTempCount == 0 ? maxParameterLength : maxParameterLength + 4 - tabTempCount;
                int i = 0;
                //计算各个参数需要tab的个数
                foreach (var item in ParametersTipDict)
                {
                    int subLength = realyLength - item.Key.Length + 1;//加一是后面要有个冒号
                    float t = subLength / 4f;
                    int tabCount = (int)Math.Ceiling(t);
                    tabCountArray[i] = tabCount;

                    i++;
                }

                //打印参数
                i = 0;
                foreach (var item in ParametersTipDict)
                {
                    Console.Write("\t");
                    Console.Write(item.Key);
                    Console.Write(":");
                    for (int j = 0; j < tabCountArray[i]; j++)
                    {
                        Console.Write("\t");
                    }
                    Console.WriteLine(item.Value);

                    i++;
                }
            }
            else
            {
                Console.WriteLine("\t无参数");
            }
            //开关
            Console.WriteLine("开关：");
            if (ToggleTipDict != null && ToggleTipDict.Count > 0)
            {
                //输出参数列表前，先计算缩进
                int[] tabCountArray = new int[ToggleTipDict.Count];
                //找到参数名称最长的长度，除以4后标记tab次数为1，其他参数的缩进值以这个为基准计算
                int maxToggleLength = 0;
                foreach (var item in ToggleTipDict)
                {
                    int length = item.Key.Length + 2;//加二是后面要有个冒号，前面还有个‘-’
                    if (length > maxToggleLength)
                    {
                        maxToggleLength = length;
                    }
                }
                //计算tab后的长度
                int tabTempCount = maxToggleLength % 4;
                int realyLength = tabTempCount == 0 ? maxToggleLength : maxToggleLength + 4 - tabTempCount;
                int i = 0;
                //计算各个参数需要tab的个数
                foreach (var item in ToggleTipDict)
                {
                    int subLength = realyLength - item.Key.Length + 2;//加二是后面要有个冒号，前面还有个‘-’
                    float t = subLength / 4f;
                    int tabCount = (int)Math.Ceiling(t);
                    tabCountArray[i] = tabCount;

                    i++;
                }

                //打印开关
                i = 0;
                foreach (var item in ToggleTipDict)
                {
                    Console.Write("\t-");
                    Console.Write(item.Key);
                    Console.Write(":");
                    for (int j = 0; j < tabCountArray[i]; j++)
                    {
                        Console.Write("\t");
                    }
                    Console.WriteLine(item.Value);

                    i++;
                }
            }
            else
            {
                Console.WriteLine("\t无开关");
            }
        }

        /// <summary>
        /// 发送消息到游戏服务器
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void SendMsg(IMessage obj)
        {
            byte[] data = ProtobufUtil.Serialize(obj);
            GameServerSocket.Instance.SendMsg((int)CommandType, (int)CommandID, data);
        }

        /// <summary>
        /// 返回命令执行成功的结果
        /// </summary>
        /// <returns></returns>
        protected virtual CommandExecuteResult ReturnSuccessResult()
        {
            CommandExecuteResult res = new CommandExecuteResult();
            res.SetResult(true);
            res.SetStateCode(CommandExecuteStateCode.Success);
            res.SetMessage(null);

            return res;
        }

        /// <summary>
        /// 返回命令执行失败的结果
        /// </summary>
        /// <param name="stateCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual CommandExecuteResult ReturnFailedResult(CommandExecuteStateCode stateCode, string message)
        {
            CommandExecuteResult res = new CommandExecuteResult();
            res.SetResult(false);
            res.SetStateCode(stateCode);
            res.SetMessage(message);

            return res;
        }

        /// <summary>
        /// 返回命令执行等待服务器返回消息中的结果。使用等待状态的结果需要重写ServerCallback方法返回执行的具体结果
        /// </summary>
        /// <returns></returns>
        protected virtual CommandExecuteResult ReturnAwaitResult()
        {
            CommandExecuteResult res = new CommandExecuteResult();
            res.SetResult(true);
            res.SetStateCode(CommandExecuteStateCode.Await);
            res.SetMessage(null);

            return res;
        }

        /// <summary>
        /// 服务器返回消息的接口函数
        /// </summary>
        /// <param name="data"></param>
        public virtual CommandExecuteResult ServerCallback(byte[] data)
        {
            return ReturnSuccessResult();
        }
    }
}
