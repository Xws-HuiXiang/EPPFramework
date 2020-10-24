using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using EPPFServer.Common;

namespace EPPFServer.Network
{
    public class ClientSocket
    {
        private Socket socket;
        /// <summary>
        /// 当前客户端的Socket套接字
        /// </summary>
        public Socket Socket { get { return socket; } }

        private long lastPingTime;
        /// <summary>
        /// 最后发送的心跳包时间戳
        /// </summary>
        public long LastPingTime { get { return lastPingTime; } }

        private MessageUtils messageUtils = new MessageUtils();
        /// <summary>
        /// 消息处理工具类
        /// </summary>
        public MessageUtils MessageUtils { get { return messageUtils; } }

        public ClientSocket(Socket socket)
        {
            this.socket = socket;
        }

        /// <summary>
        /// 设置最后发送的心跳包时间戳
        /// </summary>
        /// <param name="time"></param>
        public void SetLastPingTime(long time)
        {
            this.lastPingTime = time;
        }
    }
}
