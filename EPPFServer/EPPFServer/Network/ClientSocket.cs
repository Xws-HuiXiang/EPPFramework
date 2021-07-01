using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Ciphertext;
using EPPFServer.Common;
using EPPFServer.Log;
using EPPFServer.Room;
using EPPFServer.DataBase;

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

        private MessageBuffer messageUtils = new MessageBuffer();
        /// <summary>
        /// 消息处理工具类
        /// </summary>
        public MessageBuffer MessageUtils { get { return messageUtils; } }

        private RoomBase room;
        /// <summary>
        /// 当前玩家所在的房间
        /// </summary>
        public RoomBase Room { get { return room; } }

        private int playerGUID;
        /// <summary>
        /// 当前用户登陆的账号对应的GUID
        /// </summary>
        public int PlayerGUID { get { return playerGUID; } }
        private string playerName;
        /// <summary>
        /// 当前用户登陆的账号对应的玩家名称
        /// </summary>
        public string PlayerName { get { return playerName; } }

        private byte[] msgPublicKey = null;
        /// <summary>
        /// 消息加密用的公钥
        /// </summary>
        public byte[] MsgPublicKey { get { return msgPublicKey; } }
        private byte[] msgPrivateKey = null;
        /// <summary>
        /// 消息加密用的密钥
        /// </summary>
        public byte[] MsgPrivateKey { get { return msgPrivateKey; } }
        private byte[] msgSecretKey = null;
        /// <summary>
        /// 消息加密用的公共密钥
        /// </summary>
        public byte[] MsgSecretKey { get { return msgSecretKey; } }

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

        /// <summary>
        /// 加入一个游戏房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public bool JoinRoom(RoomBase room)
        {
            if (this.room != null)
            {
                Debug.L.Error(string.Format("尝试加入房间失败，本身已经在房间[{0}]中", this.room.Name));

                return false;
            }
            else
            {
                this.room = room;

                return true;
            }
        }

        /// <summary>
        /// 设置当前登陆账号的玩家GUID
        /// </summary>
        /// <param name="playerGUID"></param>
        public void SetPlayerGUID(int playerGUID)
        {
            this.playerGUID = playerGUID;
            SetPlayerName();
        }

        /// <summary>
        /// 设置玩家名称
        /// </summary>
        private void SetPlayerName()
        {
            string playerName = UserDBUtil.Instance.SelectPlayerNameByGUID(this.PlayerGUID);
            if (!string.IsNullOrEmpty(playerName))
            {
                this.playerName = playerName;
            }
            else
            {
                Debug.L.Warn(string.Format("未查询到GUID为[{0}]的玩家的名称", this.PlayerGUID));

                this.playerName = "未知名称";
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="isDisconnection">是否为游戏中的断线</param>
        public void Close(bool isDisconnection)
        {
            if (!isDisconnection)
            {
                if (this.Room != null)
                {
                    ServerSocket.Instance.QuitRoom(this);
                }

                this.room = null;
            }

            if (this.Socket != null)
            {
                this.Socket.Close();
            }
        }

        /// <summary>
        /// 退出UNO游戏房间
        /// </summary>
        public bool QuitUNOGameRoom()
        {
            if (this.Room != null)
            {
                bool res = this.Room.QuitRoom(this);
                this.room = null;

                return res;
            }

            return true;
        }

        /// <summary>
        /// 设置消息加密用的公钥
        /// </summary>
        /// <param name="msgPublicKey"></param>
        public void SetMsgPublicKey(byte[] msgPublicKey)
        {
            this.msgPublicKey = msgPublicKey;
        }
        /// <summary>
        /// 设置消息加密用的密钥
        /// </summary>
        /// <param name="msgPrivateKey"></param>
        public void SetMsgPrivateKey(byte[] msgPrivateKey)
        {
            this.msgPrivateKey = msgPrivateKey;
        }
        /// <summary>
        /// 设置消息加密用的对称密钥
        /// </summary>
        /// <param name="clientMsgPublicKey">客户端公钥</param>
        public void SetMsgSecretKey(string clientMsgPublicKey)
        {
            byte[] publicKeyByteArray = Convert.FromBase64String(clientMsgPublicKey);
            this.msgSecretKey = DHExchange.GenerateKeySecret(this.MsgPrivateKey, publicKeyByteArray);
        }
    }
}
