using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EPPFramework.Common;
using EPPFramework.Network;
using EPPFramework.ServerConsole.CommandUtil;
using EPPFramework.ServerConsole.Handle;
using EPPFramework.ServerConsole.ServerConsoleAttributes;
using Debug = EPPFramework.Log.Debug;

namespace EPPFramework.ServerConsole
{
    /// <summary>
    /// 消息队列结构体
    /// </summary>
    public struct MsgQueue
    {
        /// <summary>
        /// 消息队列结构体
        /// </summary>
        /// <param name="msgByteArray"></param>
        /// <param name="commandType"></param>
        /// <param name="commandID"></param>
        public MsgQueue(byte[] msgByteArray, int commandType, int commandID)
        {
            this.MsgByteArray = msgByteArray;
            this.CommandType = commandType;
            this.CommandID = commandID;
        }

        public byte[] MsgByteArray { get; set; }
        public int CommandType { get; set; }
        public int CommandID { get; set; }
    }

    /// <summary>
    /// 游戏服务器的控制台
    /// </summary>
    public class ControlCommand : Singleton<ControlCommand>
    {
        /// <summary>
        /// 控制台进程的对象
        /// </summary>
        private Process controlCommandProcess = null;

        /// <summary>
        /// 服务器控制台监听的socket对象
        /// </summary>
        private Socket controlCommandSocket = null;
        private Socket controlCommandClientSocket = null;

        private ServerConsoleMessageBuffer messageBuffer = null;

        /// <summary>
        /// 存储所有控制台命令处理方法的字典
        /// </summary>
        private static Dictionary<int, Dictionary<int, Action<byte[]>>> commandHandleDict = new Dictionary<int, Dictionary<int, Action<byte[]>>>();

        /// <summary>
        /// 消息队列
        /// </summary>
        private static Queue<MsgQueue> msgQueue;

        public ControlCommand() 
        {
            messageBuffer = new ServerConsoleMessageBuffer();
            msgQueue = new Queue<MsgQueue>();

            InitCommandHandles();
        }

        /// <summary>
        /// 初始化命令处理方法的字典
        /// </summary>
        private void InitCommandHandles()
        {
            commandHandleDict.Clear();

            //获取带有“消息处理类特性”的所有class
            //首先获取当前程序集
            AssemblyName currentAssemblyName = Assembly.GetCallingAssembly().GetName();
            Assembly asm = Assembly.Load(currentAssemblyName);
            //获取所有自定义类型
            Type[] customTypes = asm.GetExportedTypes();
            //验证当前类型是否含有“消息处理类特性”
            for (int i = 0; i < customTypes.Length; i++)
            {
                //获取当前类的所有特性
                IEnumerable<Attribute> attributeList = customTypes[i].GetCustomAttributes();
                foreach (Attribute attribute in attributeList)
                {
                    //检查是否有标记为“消息处理类”
                    if (attribute is CommandHandleAttribute)
                    {
                        //当前类标记为了“消息处理类”
                        CommandHandleAttribute protocolHandle = attribute as CommandHandleAttribute;
                        Dictionary<int, Action<byte[]>> actionDict = new Dictionary<int, Action<byte[]>>();
                        
                        //创建一个该类的对象
                        dynamic obj = asm.CreateInstance(customTypes[i].FullName, false, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, null, null, null);

                        //查找当前类中所有的标记了“消息处理方法”的方法
                        MethodInfo[] methods = customTypes[i].GetMethods();
                        for (int j = 0; j < methods.Length; j++)
                        {
                            IEnumerable<Attribute> methodAttributeList = methods[j].GetCustomAttributes();
                            foreach (Attribute methodAttribute in methodAttributeList)
                            {
                                if (methodAttribute is CommandMethodHandleAttribute)
                                {
                                    //当前方法标记为了“消息处理方法”
                                    CommandMethodHandleAttribute protocolHandleMethod = methodAttribute as CommandMethodHandleAttribute;

                                    MethodInfo method = methods[j];
                                    actionDict.Add((int)protocolHandleMethod.MethodID, (msg) =>
                                    {
                                        method.Invoke(obj, new object[] { msg });
                                    });
                                }
                            }
                        }

                        commandHandleDict.Add((int)protocolHandle.ID, actionDict);
                    }
                }
            }
        }

        /// <summary>
        /// 开启服务器控制台
        /// </summary>
        public void Start()
        {
            //开启监听。通信使用发消息的方式
            try
            {
                IPAddress ip = IPAddress.Parse(ServerSocket.Config.Command.IP);
                IPEndPoint ipEndPoint = new IPEndPoint(ip, ServerSocket.Config.Command.Port);
                controlCommandSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                controlCommandSocket.Bind(ipEndPoint);
                controlCommandSocket.Listen(0);
                controlCommandSocket.BeginAccept(OnBeginAcceptCallback, null);

                Debug.L.Debug(string.Format("开启服务器控制台的网络监听成功，IP：{0}:{1}", ServerSocket.Config.Command.IP, ServerSocket.Config.Command.Port));
            }
            catch(Exception e)
            {
                Debug.L.Error(string.Format("开启服务器控制台的网络监听失败，错误原因：{0}", e.Message));
            }

            if (ServerSocket.Config.Command.AutoOpenExe)
            {
                //启动服务器控制台的exe程序
                controlCommandProcess = new Process();
                controlCommandProcess.StartInfo.FileName = ServerUtils.ParseRouting(ServerSocket.Config.Command.CommandExePath);
                controlCommandProcess.StartInfo.UseShellExecute = true;
                controlCommandProcess.StartInfo.CreateNoWindow = true;
            }

            try
            {
                if (ServerSocket.Config.Command.AutoOpenExe)
                {
                    controlCommandProcess.Start();
                }

                //循环读取发送的数据
                Task.Run(StartTask);
            }
            catch (Exception e)
            {
                Debug.L.Error(string.Format("启动服务器控制台程序失败，错误信息：{0}", e.Message));
            }
        }

        /// <summary>
        /// 收到控制台连接的回调
        /// </summary>
        /// <param name="ar"></param>
        private void OnBeginAcceptCallback(IAsyncResult ar)
        {
            //Socket socket = ar.AsyncState as Socket;
            controlCommandClientSocket = controlCommandSocket.EndAccept(ar);

            Debug.L.Info("服务器控制台已连接");

            //开始接收消息
            try
            {
                if(controlCommandClientSocket != null && controlCommandClientSocket.Connected)
                {
                    controlCommandClientSocket.BeginReceive(messageBuffer.Data, messageBuffer.WriteIndex, messageBuffer.Remain, SocketFlags.None, OnReceiveCallback, controlCommandClientSocket);
                }
                else
                {
                    Debug.L.Error("开始接收消息失败：controlCommandClientSocket为空或未连接");
                }
            }
            catch(Exception e)
            {
                Debug.L.Error(string.Format("开始接收服务器控制台消息出错：{0}", e.Message));
            }

            controlCommandSocket.BeginAccept(OnBeginAcceptCallback, null);
        }

        /// <summary>
        /// 接收消息回调
        /// </summary>
        /// <param name="ar"></param>
        private void OnReceiveCallback(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;

            if (socket != null && socket.Connected)
            {
                int count = 0;
                try
                {
                    count = socket.EndReceive(ar);
                }
                catch(Exception e)
                {
                    Debug.L.Error("接收服务器控制台控制台消息出错：" + e.Message);
                }
                if (count != 0)
                {
                    messageBuffer.WriteIndex += count;

                    //粘包分包处理
                    while (messageBuffer.Length > 12)
                    {
                        bool decodeRes = ServerConsoleMessageBuffer.DecodeMsg(messageBuffer, out int commandTypeID, out int commandID, out byte[] msg);
                        if (decodeRes)
                        {
                            MsgQueue queue = new MsgQueue(msg, commandTypeID, commandID);
                            lock (msgQueue)
                            {
                                msgQueue.Enqueue(queue);
                            }
                        }
                        else
                        {
                            //消息不完整
                            break;
                        }
                    }

                    //如果缓冲区剩余空间太小则移动数据到开头
                    if (messageBuffer.Remain <= 12)
                    {
                        messageBuffer.CheckAndMoveData();

                        while (messageBuffer.Remain <= 0)
                        {
                            //当消息长度大于1024时，需要扩展缓冲区的大小
                            int expandSize = messageBuffer.Length < ServerConsoleMessageBuffer.DEFAULT_SIZE ? ServerConsoleMessageBuffer.DEFAULT_SIZE : messageBuffer.Length;
                            messageBuffer.ReSize(expandSize * 2);

                            Console.WriteLine("扩展了一次缓冲区大小，当前大小为：" + expandSize * 2);
                        }
                    }

                    //重新开始接收
                    if (socket != null && socket.Connected)
                    {
                        socket.BeginReceive(messageBuffer.Data, messageBuffer.WriteIndex, messageBuffer.Remain, SocketFlags.None, OnReceiveCallback, socket);
                    }
                    else
                    {
                        Console.WriteLine("准备接收信息时连接已关闭");
                    }
                }
                else
                {
                    //服务器关闭了连接
                    Console.WriteLine("服务器控制台关闭了连接");

                    Close();
                }
            }
            else
            {
                //服务器关闭了连接
                Console.WriteLine("服务器控制台关闭了连接");

                Close();
            }
        }

        /// <summary>
        /// 服务器控制台线程方法
        /// </summary>
        private void StartTask()
        {
            while (true)
            {
                if(msgQueue.Count > 0)
                {
                    lock (msgQueue)
                    {
                        MsgQueue msgItem = msgQueue.Dequeue();
                        
                        if(commandHandleDict.TryGetValue(msgItem.CommandType, out Dictionary<int, Action<byte[]>> actionDict))
                        {
                            if(actionDict.TryGetValue(msgItem.CommandID, out Action<byte[]> action))
                            {
                                action.Invoke(msgItem.MsgByteArray);
                            }
                            else
                            {
                                Debug.L.Error("未定义的命令ID：" + msgItem.CommandID);
                            }
                        }
                        else
                        {
                            Debug.L.Error("未定义的命令类型：" + msgItem.CommandType);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 发送消息到游戏服务器控制台
        /// </summary>
        /// <param name="commandTypeID"></param>
        /// <param name="commandID"></param>
        /// <param name="data"></param>
        public void SendMsg(int commandTypeID, int commandID, byte[] data)
        {
            if (data == null)
            {
                Console.WriteLine("尝试发送一条空消息");

                return;
            }

            if (controlCommandClientSocket == null)
            {
                Console.WriteLine("发送消息时socket对象为空，请等待连接服务器后再尝试发送消息");

                return;
            }

            if (!controlCommandClientSocket.Connected)
            {
                Console.WriteLine("socket已断开，发送消息失败");

                return;
            }

            //消息组拼
            byte[] msg = ServerConsoleMessageBuffer.EncodeMsg(commandTypeID, commandID, data);

            controlCommandClientSocket.BeginSend(msg, 0, msg.Length, SocketFlags.None, null, null);
        }

        /// <summary>
        /// 关闭服务器控制台线程
        /// </summary>
        public void Close()
        {
            if(controlCommandProcess != null)
            {
                controlCommandProcess.Close();
                controlCommandProcess.Dispose();
            }
        }
    }
}
