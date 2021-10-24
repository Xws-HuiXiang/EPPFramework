using GameServerConsole.Command;
using GameServerConsole.GameServerConsoleAttribute;
using GameServerConsole.ServerSocket;
using GameServerConsole.Utils;
using EPPFramework.ServerConsole;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GameServerConsole
{
    public class Program
    {
        private static Dictionary<CommandInfo, ICommand> commandDict = new Dictionary<CommandInfo, ICommand>();
        /// <summary>
        /// 所有支持的命令的字典
        /// </summary>
        public static Dictionary<CommandInfo, ICommand> CommandDict { get { return commandDict; } }
        private static Dictionary<CommandID, ICommandSocket> commandSocketDict = new Dictionary<CommandID, ICommandSocket>();
        /// <summary>
        /// 命令ID与socket接口对象的字典。用于服务器返回消息
        /// </summary>
        public static Dictionary<CommandID, ICommandSocket> CommandSocketDict { get { return commandSocketDict; } }

        /// <summary>
        /// 标识了控制台程序是否应该退出
        /// </summary>
        private static bool isQuit = false;

        private static void Main(string[] args)
        {
            InitCommandDict();

            //打印欢迎语句
            CommandUtil.SayHello();

            while (true)
            {
                //先输入连接的地址，输入0为本机IP
                Console.WriteLine("请输入服务器所在IP，不输入则使用默认本机127.0.0.1");
                string ip = Console.ReadLine();
                if (string.IsNullOrEmpty(ip))
                {
                    ip = "127.0.0.1";
                }
                Console.WriteLine("请输入连接服务器的端口号，不输入则使用默认端口1132：");
                int port = 0;
                try
                {
                    string portString = Console.ReadLine().Trim();
                    if (string.IsNullOrEmpty(portString))
                    {
                        port = 1132;
                    }
                    else
                    {
                        port = int.Parse(Console.ReadLine().Trim());
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("端口号输入有误，请重新输入。" + e.Message);
                }

                //根据ip和端口连接主游戏服务器
                Console.WriteLine(string.Format("正在连接：{0}:{1}", ip, port));
                bool connectRes = GameServerSocket.Instance.Connection(ip, port);
                if (connectRes)
                {
                    break;
                }
            }

            //监听控制台输入
            StartCheckConsoleInput();
        }

        /// <summary>
        /// 初始化命令字典
        /// </summary>
        private static void InitCommandDict()
        {
            commandDict.Clear();

            //获取带有“命令”的所有class
            //首先获取当前程序集
            AssemblyName currentAssemblyName = Assembly.GetCallingAssembly().GetName();
            Assembly asm = Assembly.Load(currentAssemblyName);
            //获取所有自定义类型
            Type[] customTypes = asm.GetExportedTypes();
            //验证当前类型是否含有“命令”
            for (int i = 0; i < customTypes.Length; i++)
            {
                //获取当前类的所有特性
                IEnumerable<Attribute> attributeList = customTypes[i].GetCustomAttributes();
                foreach (Attribute attribute in attributeList)
                {
                    //检查是否有标记为“命令”
                    if (attribute is CommandAttribute)
                    {
                        //当前类标记为了“命令”
                        CommandAttribute commandAttribute = attribute as CommandAttribute;
                        //命令名称为全小写
                        string commandName = commandAttribute.CommandName.ToLower();
                        //创建命令对象
                        dynamic commandObj = asm.CreateInstance(customTypes[i].FullName, false, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, null, null, null);
                        //设置命令名称
                        FieldInfo commandNameField = customTypes[i].BaseType.GetField("commandName", BindingFlags.NonPublic | BindingFlags.Instance);
                        commandNameField.SetValue(commandObj, commandAttribute.CommandName);
                        //设置命令类型
                        FieldInfo commandTypeField = customTypes[i].BaseType.GetField("commandType", BindingFlags.NonPublic | BindingFlags.Instance);
                        commandTypeField.SetValue(commandObj, commandAttribute.CommandType);
                        //设置命令ID
                        FieldInfo commandIDField = customTypes[i].BaseType.GetField("commandID", BindingFlags.NonPublic | BindingFlags.Instance);
                        commandIDField.SetValue(commandObj, commandAttribute.CommandID);

                        CommandInfo commandInfo = new CommandInfo(commandName, commandAttribute.CommandType, commandAttribute.CommandID);
                        //添加到全部命令列表
                        if (!commandDict.ContainsKey(commandInfo))
                        {
                            commandDict.Add(commandInfo, commandObj);
                        }
                        else
                        {
                            //命令名称重复
                            Console.WriteLine("命令名称重复：" + commandName);
                        }

                        //命令网络连接对象存储
                        commandSocketDict.Add(commandAttribute.CommandID, commandObj);
                    }
                }
            }
        }

        /// <summary>
        /// 检测控制台输入
        /// </summary>
        private static void StartCheckConsoleInput()
        {
            while (true && !isQuit)
            {
                string consoleInput = Console.ReadLine();
                consoleInput = consoleInput.Trim();//去前后空格
                if (CommandUtil.ConfirmCommandState)
                {
                    CommandUtil.ConfirmCommand(consoleInput);

                    continue;
                }
                if (!string.IsNullOrEmpty(consoleInput))
                {
                    int spaceIndex = consoleInput.IndexOf(' ');//获取第一个空格的位置，如果有空格则有参数否则无参数
                    string commandName;
                    string[] parameterArray;
                    string[] toggleArray;
                    List<string> parameterList = new List<string>();
                    List<string> toggleList = new List<string>();
                    if (spaceIndex != -1)
                    {
                        //有参数
                        commandName = consoleInput.Substring(0, spaceIndex);
                        //拆分参数
                        string allParameterString = consoleInput.Substring(spaceIndex + 1);
                        bool isString = false;
                        StringBuilder parameterBuilder = new StringBuilder();
                        StringBuilder stringBuilder = new StringBuilder();
                        for (int i = 0; i < allParameterString.Length; i++)
                        {
                            char c = allParameterString[i];
                            if (isString)
                            {
                                if (c.Equals('\"'))
                                {
                                    string paramString = stringBuilder.ToString();
                                    if (paramString.StartsWith("-"))
                                    {
                                        toggleList.Add(paramString.Substring(1));
                                    }
                                    else
                                    {
                                        parameterList.Add(paramString);
                                    }
                                    stringBuilder.Clear();

                                    isString = false;
                                }
                                else if (c.Equals('\\'))
                                {
                                    //字符串里面的转义字符
                                    if ((i + 1) < allParameterString.Length)
                                    {
                                        char translateChar = allParameterString[i + 1];
                                        if (translateChar.Equals('\"'))
                                        {
                                            stringBuilder.Append('"');
                                            i++;
                                        }
                                        else if (translateChar.Equals('\\'))
                                        {
                                            stringBuilder.Append('\\');
                                            i++;
                                        }
                                        else
                                        {
                                            //未知的转义字符，全部输出
                                            stringBuilder.Append('\\');
                                        }
                                    }
                                    else
                                    {
                                        //转义字符后面没有东西了，直接输出一个反斜杠
                                        stringBuilder.Append('\\');
                                    }
                                }
                                else
                                {
                                    stringBuilder.Append(c);
                                }
                            }
                            else
                            {
                                if (c.Equals(' '))
                                {
                                    if (parameterBuilder.Length > 0)
                                    {
                                        string parameter = parameterBuilder.ToString();
                                        parameter = parameter.Trim();
                                        if (!string.IsNullOrEmpty(parameter))
                                        {
                                            string paramString = stringBuilder.ToString();
                                            if (paramString.StartsWith("-"))
                                            {
                                                toggleList.Add(paramString.Substring(1));
                                            }
                                            else
                                            {
                                                parameterList.Add(paramString);
                                            }
                                        }

                                        parameterBuilder.Clear();
                                    }
                                }
                                else if (c.Equals('\"'))
                                {
                                    isString = !isString;
                                }
                                else
                                {
                                    parameterBuilder.Append(c);
                                }
                            }
                        }

                        //处理循环结束后 最后一个参数
                        if (parameterBuilder.Length > 0)
                        {
                            string parameter = parameterBuilder.ToString();
                            parameter = parameter.Trim();
                            if (!string.IsNullOrEmpty(parameter))
                            {
                                if (parameter.StartsWith("-"))
                                {
                                    toggleList.Add(parameter.Substring(1));
                                }
                                else
                                {
                                    parameterList.Add(parameter);
                                }
                            }
                        }
                    }
                    else
                    {
                        //无参数
                        commandName = consoleInput;
                    }
                    parameterArray = parameterList.ToArray();
                    toggleArray = toggleList.ToArray();

                    //修改命令名称为全部小写
                    commandName = commandName.ToLower();
                    bool hasCommand = false;
                    foreach(var command in CommandDict)
                    {
                        if(command.Key.commandName.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                        {
                            CommandBase cmd = command.Value as CommandBase;
                            CommandUtil.ExecuteCommand(cmd, parameterArray, toggleArray);
                            hasCommand = true;

                            break;
                        }
                    }

                    if (!hasCommand)
                    {
                        //未知的命令
                        Console.WriteLine("未定义的命令：" + commandName);
                    }
                }
            }
        }

        /// <summary>
        /// 退出服务器控制台
        /// </summary>
        public static void Quit()
        {
            isQuit = true;
        }
    }
}
