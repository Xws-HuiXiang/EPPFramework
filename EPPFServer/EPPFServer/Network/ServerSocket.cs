using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using EPPFServer.Attributes;
using EPPFServer.Common;
using EPPFServer.Log;
using CommonProtocol;
using System.IO;
using LitJson;

namespace EPPFServer.Network
{
    /// <summary>
    /// 服务器端的Socket类。使用非阻塞方式的多路复用方式开发，为消息驱动式思想，提高了服务器性能。主要实现为Socket的Select方法。
    /// </summary>
    public class ServerSocket : Singleton<ServerSocket>
    {
        private static string publicKey = "EPPFServer";
        /// <summary>
        /// 公钥
        /// </summary>
        public static string PublicKey { get { return publicKey; } }

        private static string secretKey = "HuiXiangEPPFServer";
        /// <summary>
        /// 密钥
        /// </summary>
        public static string SecretKey { get { return secretKey; } }

#if DEBUG
        /// <summary>
        /// 服务器IP
        /// </summary>
        private const string ipString = "127.0.0.1";
#else
        /// <summary>
        /// 服务器IP
        /// </summary>
        private const string ipString = "";
#endif
        /// <summary>
        /// 端口号
        /// </summary>
        private const int port = 1130;

        /// <summary>
        /// 服务器监听客户端连接的Socket
        /// </summary>
        private Socket serverListenSocket;

        /// <summary>
        /// 心跳包超时时间
        /// </summary>
        private const long pingIntervalTimeOut = 120;

        /// <summary>
        /// Select方法使用的检测列表
        /// </summary>
        private static List<Socket> checkReadList = new List<Socket>();
        /// <summary>
        /// 当前服务器中所有连接的客户端字典
        /// </summary>
        private static Dictionary<Socket, ClientSocket> clientSocketDict = new Dictionary<Socket, ClientSocket>();

        /// <summary>
        /// 心跳包超时的客户端临时列表
        /// </summary>
        private static List<ClientSocket> timeOutClientSocketList = new List<ClientSocket>();

        /// <summary>
        /// 保存所有消息处理类和消息处理类中的方法字典
        /// </summary>
        private static Dictionary<int, Dictionary<int, Action<ClientSocket, byte[]>>> protocolHandleDict = new Dictionary<int, Dictionary<int, Action<ClientSocket, byte[]>>>();

        /// <summary>
        /// 配置信息
        /// </summary>
        private static ConfigData config;
        /// <summary>
        /// 当前配置的信息
        /// </summary>
        public static ConfigData Config { get { return config; } }

        public ServerSocket()
        {

        }

        /// <summary>
        /// 初始化Server
        /// </summary>
        public void Init()
        {
            try
            {
                ReadConfig();
                InitProtocolHandleDict();

                IPAddress ip = IPAddress.Parse(ipString);
                IPEndPoint ipEndPoint = new IPEndPoint(ip, port);
                serverListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverListenSocket.Bind(ipEndPoint);
                serverListenSocket.Listen(0);

                Debug.L.Debug(string.Format("初始化Server成功，监听IP地址为：{0}", ipEndPoint.ToString()));
            }
            catch(Exception e)
            {
                Debug.L.Error(string.Format("初始化Server失败：{0}", e.Message));
            }

            while (true)
            {
                ResetCheckReadList();

                try
                {
                    //非阻塞方式的多路复用。最后的参数单位为微秒
                    Socket.Select(checkReadList, null, null, 1000);
                }
                catch(Exception e)
                {
                    Debug.L.Error(string.Format("非阻塞方式的多路复用的方法Select执行出错：{0}", e.Message));
                }

                //遍历连接列表，判断是否可读消息或者为连接服务器
                for(int i = checkReadList.Count - 1; i >= 0; i--)
                {
                    Socket socket = checkReadList[i];
                    if (socket == serverListenSocket)
                    {
                        //说明有客户端连接到服务器，所以服务器的Socket可读
                        ReadServerListen(socket);
                    }
                    else
                    {
                        //说明连接的客户端可读，所以是客户端发送了消息到服务器
                        ReadClient(socket);
                    }
                }

                //心跳包是否超时的判断
                long timeNow = ServerUtils.GetTimeStamp();
                timeOutClientSocketList.Clear();
                foreach (ClientSocket clientSocket in clientSocketDict.Values)
                {
                    if(timeNow - clientSocket.LastPingTime > pingIntervalTimeOut)
                    {
                        //心跳包超时，断开连接
                        Debug.L.Debug(string.Format("客户端[{0}]的心跳包超时", clientSocket.Socket.RemoteEndPoint.ToString()));
                        timeOutClientSocketList.Add(clientSocket);
                    }
                }
                foreach(ClientSocket clientSocket in timeOutClientSocketList)
                {
                    CloseClientSocket(clientSocket);
                }
                timeOutClientSocketList.Clear();
            }
        }

        /// <summary>
        /// 初始化消息处理类和消息处理类中的方法
        /// </summary>
        private void InitProtocolHandleDict()
        {
            protocolHandleDict.Clear();

            //获取带有“消息处理类特性”的所有class
            //首先获取当前程序集
            AssemblyName currentAssemblyName = Assembly.GetCallingAssembly().GetName();
            Assembly asm = Assembly.Load(currentAssemblyName);
            //获取所有自定义类型
            Type[] customTypes = asm.GetExportedTypes();
            //验证当前类型是否含有“消息处理类特性”
            for(int i = 0; i < customTypes.Length; i++)
            {
                //获取当前类的所有特性
                IEnumerable<Attribute> attributeList = customTypes[i].GetCustomAttributes();
                foreach(Attribute attribute in attributeList)
                {
                    //检查是否有标记为“消息处理类”
                    if(attribute is ProtocolHandleAttribute)
                    {
                        //当前类标记为了“消息处理类”
                        ProtocolHandleAttribute protocolHandle = attribute as ProtocolHandleAttribute;
                        Dictionary<int, Action<ClientSocket, byte[]>> actionDict = new Dictionary<int, Action<ClientSocket, byte[]>>();

                        //查找当前类中所有的标记了“消息处理方法”的方法
                        MethodInfo[] methods = customTypes[i].GetMethods();
                        for(int j = 0; j < methods.Length; j++)
                        {
                            IEnumerable<Attribute> methodAttributeList = methods[j].GetCustomAttributes();
                            foreach(Attribute methodAttribute in methodAttributeList)
                            {
                                if(methodAttribute is ProtocolHandleMethodAttribute)
                                {
                                    //当前方法标记为了“消息处理方法”
                                    ProtocolHandleMethodAttribute protocolHandleMethod = methodAttribute as ProtocolHandleMethodAttribute;

                                    MethodInfo method = methods[j];
                                    actionDict.Add(protocolHandleMethod.MethodID, (client, msg) =>
                                    {
                                        method.Invoke(null, new object[] { client, msg });
                                    });
                                }
                            }
                        }

                        protocolHandleDict.Add(protocolHandle.ID, actionDict);
                    }
                }
            }
        }

        /// <summary>
        /// 重设CheckReadList列表
        /// </summary>
        private void ResetCheckReadList()
        {
            checkReadList.Clear();
            checkReadList.Add(serverListenSocket);
            foreach(Socket socket in clientSocketDict.Keys)
            {
                checkReadList.Add(socket);
            }
        }

        /// <summary>
        /// 有客户端连接到服务器
        /// </summary>
        /// <param name="socket"></param>
        private void ReadServerListen(Socket socket)
        {
            try
            {
                Socket client = socket.Accept();
                ClientSocket clientSocket = new ClientSocket(client);
                clientSocket.SetLastPingTime(ServerUtils.GetTimeStamp());
                clientSocketDict.Add(client, clientSocket);

                Debug.L.Info(string.Format("一个客户端[{0}]连接，当前总连接数为：{1}", client.LocalEndPoint.ToString(), clientSocketDict.Count));

                //连接成功后向客户端发送密钥
                MsgSecretKey msg = new MsgSecretKey();
                msg.secretKey = ServerSocket.SecretKey;
                SendMessage(clientSocket, msg);
            }
            catch(Exception e)
            {
                Debug.L.Error(string.Format("客户端连接到服务器时出错：" + e.Message));
            }
        }

        /// <summary>
        /// 客户端发送了消息到服务器
        /// </summary>
        /// <param name="socket"></param>
        private void ReadClient(Socket socket)
        {
            if(clientSocketDict.TryGetValue(socket, out ClientSocket clientSocket))
            {
                MessageUtils messageUtils = clientSocket.MessageUtils;
                if(messageUtils.Remain <= 0)
                {
                    //如果缓冲区刚刚好被沾满，则数据填充到索引为0的位置
                    OnReceiveData(clientSocket);
                    messageUtils.CheckAndMoveData();
                    while(messageUtils.Remain <= 0)
                    {
                        //当消息长度大于1024时，需要扩展缓冲区的大小
                        int expandSize = messageUtils.Length < MessageUtils.DEFAULT_SIZE ? MessageUtils.DEFAULT_SIZE : messageUtils.Length;
                        messageUtils.ReSize(expandSize * 2);
                    }
                }
                try
                {
                    int count = socket.Receive(messageUtils.Data, messageUtils.WriteIndex, messageUtils.Remain, SocketFlags.None);
                    if(count == 0)
                    {
                        //客户端断开连接
                        CloseClientSocket(clientSocket);
                    }
                    else
                    {
                        messageUtils.WriteIndex += count;

                        MessageUtils.DecodeMsg(messageUtils, out int protocolHandleID, out int protocolHandleMethodID, out byte[] msgByteArray);
                        while(messageUtils.Length > 12)
                        {
                            MessageUtils.DecodeMsg(messageUtils, out protocolHandleID, out protocolHandleMethodID, out msgByteArray);
                        }
                        messageUtils.CheckAndMoveData();
                        Debug.L.Debug(string.Format("接收客户端发送的消息：处理ID为{0}，处理方法ID为{1}", protocolHandleID, protocolHandleMethodID));
                        if (protocolHandleDict.ContainsKey(protocolHandleID))
                        {
                            Dictionary<int, Action<ClientSocket, byte[]>> dict = protocolHandleDict[protocolHandleID];
                            if (dict.ContainsKey(protocolHandleMethodID))
                            {
                                Action<ClientSocket, byte[]> action = dict[protocolHandleMethodID];
                                action.Invoke(clientSocket, msgByteArray);
                            }
                            else
                            {
                                Debug.L.Warn(string.Format("在ID为[{0}]的处理类中未找到ID为[{1}]的消息处理方法", protocolHandleID, protocolHandleMethodID));
                            }
                        }
                        else
                        {
                            Debug.L.Warn(string.Format("未找到ID为[{0}]的协议处理类", protocolHandleID));
                        }
                    }
                }
                catch(Exception e)
                {
                    Debug.L.Info(string.Format("客户端[{0}]强制断开连接。连接将关闭：{1}", socket.LocalEndPoint, e.Message));

                    CloseClientSocket(clientSocket);

                    return;
                }
            }
            else
            {
                Debug.L.Warn(string.Format("客户端[{0}]发送了消息，但在客户端字典中没有该客户端", socket.LocalEndPoint));
            }
        }

        /// <summary>
        /// 从客户端接收数据
        /// </summary>
        /// <param name="clientSocket"></param>
        private void OnReceiveData(ClientSocket clientSocket)
        {
            MessageUtils.DecodeMsg(clientSocket.MessageUtils, out int protocolHandleID, out int protocolHandleMethodID, out byte[] msgByteArray);

            //根据处理类ID和方法ID进行消息分发
            Dictionary<int, Action<ClientSocket, byte[]>> handleActionDict;
            if (protocolHandleDict.TryGetValue(protocolHandleID, out handleActionDict))
            {
                Action<ClientSocket, byte[]> action;
                if(handleActionDict.TryGetValue(protocolHandleMethodID, out action))
                {
                    action.Invoke(clientSocket, msgByteArray);
                }
                else
                {
                    Debug.L.Error(string.Format("在ID为[{0}]的消息处理类中，未找到ID为[{1}]的消息处理方法", protocolHandleID, protocolHandleMethodID));
                }
            }
            else
            {
                Debug.L.Error(string.Format("未找到ID为[{0}]的消息处理类", protocolHandleID));
            }

            //如果有粘包，继续读消息
            if(clientSocket.MessageUtils.Length > 4)
            {
                OnReceiveData(clientSocket);
            }
        }

        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        /// <param name="clientSocket">接收消息的客户端</param>
        /// <param name="msg">消息内容</param>
        public void SendMessage(ClientSocket clientSocket, MsgBase msg)
        {
            //如果客户端为空或者处于未连接状态则不发送消息
            if(clientSocket == null || !clientSocket.Socket.Connected)
            {
                Debug.L.Warn(string.Format("发送消息时指定的客户端为空或客户端处于未连接状态"));

                return;
            }

            try
            {
                //以特定的格式组拼消息
                byte[] data = MessageUtils.EncodeMsg(msg.ProtocolHandleID, msg.ProtocolHandleMethodID, msg);

                try
                {
                    clientSocket.Socket.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
                }
                catch(Exception e)
                {
                    Debug.L.Error(string.Format("开始发送数据到[{0}]客户端失败：{1}", clientSocket.Socket.LocalEndPoint.ToString(), e.Message));
                }
            }
            catch(Exception e)
            {
                Debug.L.Error(string.Format("向客户端[{0}]发送消息前失败：{1}", clientSocket.Socket.LocalEndPoint.ToString(), e.Message));
            }
        }

        /// <summary>
        /// 关闭一个客户端连接
        /// </summary>
        /// <param name="clientSocket"></param>
        public void CloseClientSocket(ClientSocket clientSocket)
        {
            clientSocketDict.Remove(clientSocket.Socket);

            Debug.L.Info(string.Format("客户端[{0}]断开连接，当前连接的客户端数量：{1}", clientSocket.Socket.LocalEndPoint, clientSocketDict.Count));

            clientSocket.Socket.Close();
        }

        /// <summary>
        /// 修改公钥
        /// </summary>
        /// <param name="key"></param>
        public void SetPublicKey(string key)
        {
            ServerSocket.publicKey = key;
        }

        /// <summary>
        /// 修改密钥
        /// </summary>
        /// <param name="key"></param>
        public void SetSecretKey(string key)
        {
            ServerSocket.secretKey = key;
        }

        /// <summary>
        /// 读取服务器配置文件
        /// </summary>
        private void ReadConfig()
        {
            string exePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //如果不存在目录则创建
            string configFolder = Path.Combine(exePath, "config");
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }
            string configPath = Path.Combine(configFolder, "Config.cfg");
            ConfigData configData = null;
            if (!File.Exists(configPath))
            {
                FileStream fileStream = File.Create(configPath);

                //创建默认配置文件内容
                configData = new ConfigData();
                configData.ResVersion = 1;
                configData.LuaVersion = 1;
                string jsonString = JsonMapper.ToJson(configData);
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
                fileStream.Write(jsonBytes, 0, jsonBytes.Length);

                fileStream.Close();
            }
            else
            {
                //文件存在 读取配置文件内容
                string configJsonString = File.ReadAllText(configPath);
                configData = JsonMapper.ToObject<ConfigData>(configJsonString);
            }

            ServerSocket.config = configData;
        }

        ~ServerSocket()
        {
            if(clientSocketDict != null)
            {
                foreach(var clientSocket in clientSocketDict)
                {
                    clientSocket.Value.Socket.Close();
                }
            }
        }
    }
}
