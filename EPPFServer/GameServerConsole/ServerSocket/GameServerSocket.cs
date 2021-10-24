using GameServerConsole.Utils;
using EPPFramework.Common;
using EPPFramework.ServerConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServerConsole.ServerSocket
{
    public class GameServerSocket : Singleton<GameServerSocket>
    {
        private Socket socket;

        ServerConsoleMessageBuffer messageBuffer = new ServerConsoleMessageBuffer();

        /// <summary>
        /// 消息队列
        /// </summary>
        private static Queue<MsgQueue> msgQueue;

        public GameServerSocket()
        {
            msgQueue = new Queue<MsgQueue>();
            Task.Run(StartTask);
        }

        /// <summary>
        /// 服务器控制台线程方法
        /// </summary>
        private void StartTask()
        {
            while (true)
            {
                if (msgQueue.Count > 0)
                {
                    lock (msgQueue)
                    {
                        MsgQueue msgItem = msgQueue.Dequeue();
                        
                        CommandUtil.StopSocketAwait(msgItem.MsgByteArray);
                    }
                }
            }
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public bool Connection(string ip, int port)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ip);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.BeginConnect(ipAddress, port, OnConnectCallback, socket);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("解析IP地址和端口号异常，请重新尝试。" + e.Message);
            }

            return false;
        }

        /// <summary>
        /// 连接服务器的回调
        /// </summary>
        /// <param name="ar"></param>
        private void OnConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = ar.AsyncState as Socket;
                socket.EndConnect(ar);

                Console.WriteLine("连接服务器成功");

                socket.BeginReceive(messageBuffer.Data, messageBuffer.WriteIndex, messageBuffer.Remain, SocketFlags.None, OnReceiveCallback, socket);
            }
            catch(Exception e)
            {
                Console.WriteLine("连接服务器失败：" + e.Message);
            }
        }

        /// <summary>
        /// 发送消息到游戏服务器
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

            if(socket == null)
            {
                Console.WriteLine("发送消息时socket对象为空，请等待连接服务器后再尝试发送消息");

                return;
            }

            if (!socket.Connected)
            {
                Console.WriteLine("socket已断开，发送消息失败");

                return;
            }

            //消息组拼
            byte[] msg = ServerConsoleMessageBuffer.EncodeMsg(commandTypeID, commandID, data);

            socket.BeginSend(msg, 0, msg.Length, SocketFlags.None, null, null);
        }

        /// <summary>
        /// 接收服务器信息回调
        /// </summary>
        /// <param name="ar"></param>
        private void OnReceiveCallback(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;

            if (socket != null && socket.Connected)
            {
                try
                {
                    int count = socket.EndReceive(ar);
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
                        Console.WriteLine("服务器关闭了连接");
                        Close();
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("服务器关闭了连接。" + e.Message);

                    Close();
                }
            }
            else
            {
                //服务器关闭了连接
                Console.WriteLine("Socket为空，服务器关闭了连接");

                Close();
            }
        }

        private void Close()
        {
            if(socket != null)
            {
                socket.Close();
            }

            Program.Quit();
        }
    }
}
