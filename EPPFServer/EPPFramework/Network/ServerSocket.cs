//TODO:宏定义以后改 改成配置文件的格式
//如果开启这个宏定义，则走调试相关的逻辑，如果是线上则需要关闭这个宏
#define DEBUG_MOD
//是否为开发模式。开发模式中将开启不同的端口监听
#define DEV_MOD

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using EPPFramework.Attributes;
using EPPFramework.Common;
using EPPFramework.Log;
using System.IO;
using LitJson;
using EPPFramework.DataBase;
using System.Linq;
using EPPFramework.Protocol.Builder;
using System.Threading;
using EPPFramework.Http;
using Ciphertext;
using EPPFramework.Factory;
using EPPFramework.ServerConsole;

namespace EPPFramework.Network
{
    /// <summary>
    /// 服务器端的Socket类。使用非阻塞方式的多路复用方式开发，为消息驱动式思想，提高了服务器性能。主要实现为Socket的Select方法。
    /// </summary>
    public class ServerSocket : Singleton<ServerSocket>
    {
        private static string publicKey = "EPPFrameworkServer";
        /// <summary>
        /// 公钥
        /// </summary>
        public static string PublicKey { get { return publicKey; } }

        private static string secretKey = "EPPFrameworkServerKey";
        /// <summary>
        /// 密钥
        /// </summary>
        public static string SecretKey { get { return secretKey; } }
        
        /// <summary>
        /// 服务器IP
        /// </summary>
#if DEBUG_MOD
        private const string ipString = "127.0.0.1";
        private const string httpIPString = "http://127.0.0.1";
#else
        private const string ipString = "";
        private const string httpIPString = "";
#endif
#if DEV_MOD
        /// <summary>
        /// 端口号
        /// </summary>
        private const int port = 1129;
        /// <summary>
        /// http请求监听的端口号
        /// </summary>
        private const int httpPort = 1128;
#else
        /// <summary>
        /// 端口号
        /// </summary>
        private const int port = 1130;
        /// <summary>
        /// http请求监听的端口号
        /// </summary>
        private const int httpPort = 1131;
#endif
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
        /// 当前服务器中所有连接的客户端字典
        /// </summary>
        public static Dictionary<Socket, ClientSocket> ClientSocketDict { get { return clientSocketDict; } }

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
            //socket连接初始化
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

                return;
            }

            //添加工厂
            FactoryManager.Instance.Init();

            //MySQL数据库连接
            bool mySQLInitRes = MySQLDataBase.Instance.Init(ServerSocket.Config.DataBaseConnectionString);

            if (mySQLInitRes)
            {
                //避免MySQL的连接空置八小时后自动断开，这里添加一个线程定时向MySQL发送一个操作
                Thread mySQLRefresh = new Thread(MySQLDataBase.Instance.MySQLRefreshThread);
                mySQLRefresh.Start();
            }
            else
            {
                Debug.L.Warn("因MySQL服务器初始化失败，所以定时更新MySQL连接时间戳的线程未开启");
            }

            //启动http监听
            HttpServerSocket.Instance.Init(httpIPString, httpPort);

            //如果配置中选择了开启控制台，则开启
            if (Config.Command.Active)
            {
                ControlCommand.Instance.Start();
            }

            Debug.L.Info("服务器启动完成");
            
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

                Debug.L.Info(string.Format("一个客户端[{0}]连接，当前总连接数为：{1}", client.RemoteEndPoint.ToString(), clientSocketDict.Count));

                //生成密钥对
                byte[] clientSocketPrivateKey = new byte[DHExchange.DH_KEY_LENGTH];
                byte[] clientSocketPublicKey = new byte[DHExchange.DH_KEY_LENGTH];
                DHExchange.GenerateKeyPair(clientSocketPublicKey, clientSocketPrivateKey);
                //将公共密钥和私钥保存到对象中
                clientSocket.SetMsgPrivateKey(clientSocketPrivateKey);
                clientSocket.SetMsgPublicKey(clientSocketPublicKey);
            }
            catch (Exception e)
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
                MessageBuffer messageBuffer = clientSocket.MessageUtils;

                try
                {
                    int count = socket.Receive(messageBuffer.Data, messageBuffer.WriteIndex, messageBuffer.Remain, SocketFlags.None);
                    if(count == 0)
                    {
                        //客户端断开连接
                        CloseClientSocket(clientSocket);
                    }
                    else
                    {
                        messageBuffer.WriteIndex += count;

                        //粘包分包处理
                        while (messageBuffer.Length > 12)
                        {
                            bool decodeRes = MessageBuffer.DecodeMsg(messageBuffer, clientSocket.MsgSecretKey, out int protocolHandleID, out int protocolHandleMethodID, out byte[] msg);
                            if (decodeRes)
                            {
                                //Debug.L.Debug(string.Format("接收客户端发送的消息：处理ID为{0}，处理方法ID为{1}", protocolHandleID, protocolHandleMethodID));
                                //执行对应的协议业务逻辑
                                this.ExecuteProtocolHandle(clientSocket, protocolHandleID, protocolHandleMethodID, msg);
                            }
                            else
                            {
                                //没有解析出消息
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
                                int expandSize = messageBuffer.Length < MessageBuffer.DEFAULT_SIZE ? MessageBuffer.DEFAULT_SIZE : messageBuffer.Length;
                                messageBuffer.ReSize(expandSize * 2);

                                Debug.L.Debug("扩展了一次缓冲区大小，当前大小为：" + expandSize * 2);
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    try
                    {
                        Debug.L.Info(string.Format("客户端[{0}]强制断开连接。连接将关闭：{1}", socket.RemoteEndPoint, e.Message));
                    }
                    catch(Exception ex)
                    {
                        Debug.L.Error(string.Format("未知错误：{0}", ex.Message));
                    }

                    CloseClientSocket(clientSocket);

                    return;
                }
            }
            else
            {
                Debug.L.Warn(string.Format("客户端[{0}]发送了消息，但在客户端字典中没有该客户端", socket.RemoteEndPoint));
            }
        }

        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        /// <param name="clientSocket">接收消息的客户端</param>
        /// <param name="msgData">要发送的消息内容</param>
        public void SendMessage(ClientSocket clientSocket, byte[] msgData)
        {
            //如果客户端为空或者处于未连接状态则不发送消息
            if(clientSocket == null || !clientSocket.Socket.Connected)
            {
                Debug.L.Warn(string.Format("发送消息时指定的客户端为空或客户端处于未连接状态"));

                return;
            }
            if(msgData == null || msgData.Length <= 0)
            {
                //组拼消息异常，要发送的内容为空或空串
                Debug.L.Warn("组拼消息异常，要发送的内容为空或空串");

                return;
            }

            try
            {
                clientSocket.Socket.BeginSend(msgData, 0, msgData.Length, SocketFlags.None, null, null);
            }
            catch(Exception e)
            {
                Debug.L.Error(string.Format("向客户端[{0}]发送消息失败：{1}", clientSocket.Socket.RemoteEndPoint.ToString(), e.Message));
            }
        }

        /// <summary>
        /// 关闭一个客户端连接
        /// </summary>
        /// <param name="clientSocket"></param>
        public void CloseClientSocket(ClientSocket clientSocket)
        {
            //是否为游戏中的断线
            bool isDisconnection = false;

            clientSocketDict.Remove(clientSocket.Socket);

            try
            {
                Debug.L.Info(string.Format("客户端[{0}]断开连接，当前连接的客户端数量：{1}", clientSocket.Socket.RemoteEndPoint, clientSocketDict.Count));
            }
            catch(Exception e)
            {
                Debug.L.Error(string.Format("一个客户端断开连接并触发了一个未知的错误：{0}", e.Message));
            }

            clientSocket.Close(isDisconnection);
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
            string configFolder = Path.Combine(exePath, "Config");
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }
            string configPath = Path.Combine(configFolder, "Config.json");
            ConfigData configData;
            if (!File.Exists(configPath))
            {
                Debug.L.Warn(string.Format("配置文件[{0}]不存在，将创建默认配置文件", configPath));

                FileStream fileStream = File.Create(configPath);

                //创建默认配置文件内容
                configData = ConfigData.CreateDefaultConfig();
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

        /// <summary>
        /// 提供处理类ID、处理方法ID和消息内容，执行某个协议具体逻辑
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="protocolHandleID"></param>
        /// <param name="protocolHandleMethodID"></param>
        /// <param name="msg"></param>
        public void ExecuteProtocolHandle(ClientSocket clientSocket, int protocolHandleID, int protocolHandleMethodID, byte[] msg)
        {
            if (protocolHandleDict.ContainsKey(protocolHandleID))
            {
                Dictionary<int, Action<ClientSocket, byte[]>> dict = protocolHandleDict[protocolHandleID];
                if (dict.ContainsKey(protocolHandleMethodID))
                {
                    Action<ClientSocket, byte[]> action = dict[protocolHandleMethodID];
                    action.Invoke(clientSocket, msg);
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

        ~ServerSocket()
        {
            if(clientSocketDict != null)
            {
                foreach(var clientSocket in clientSocketDict)
                {
                    clientSocket.Value.Socket.Close();
                }
            }

            //尝试关闭服务器控制台
            ControlCommand.Instance.Close();

            //关闭数据库连接
            MySQLDataBase.Instance.CloseDataBase();
        }
    }
}
