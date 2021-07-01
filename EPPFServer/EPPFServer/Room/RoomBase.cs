using QiLieGuaner;
using System;
using System.Collections.Generic;
using System.Text;
using EPPFServer.Common;
using EPPFServer.DataBase;
using EPPFServer.GameRoomRule;
using EPPFServer.Log;
using EPPFServer.Network;
using EPPFServer.Protocol.Builder;

namespace EPPFServer.Room
{
    /// <summary>
    /// 出牌方向
    /// </summary>
    public enum OutCardDirection
    {
        /// <summary>
        /// 顺时针
        /// </summary>
        Clockwise,
        /// <summary>
        /// 逆时针
        /// </summary>
        Counterclockwise
    }

    /// <summary>
    /// 游戏房间基类
    /// </summary>
    public abstract class RoomBase
    {
        protected string name;
        /// <summary>
        /// 当前房间的名称
        /// </summary>
        public string Name { get { return name; } }
        protected string roomID;
        /// <summary>
        /// 当前的房间ID
        /// </summary>
        public string RoomID { get { return roomID; } }
        protected int houseOwnerPlayerGUID;
        /// <summary>
        /// 当前房间的房主的GUID
        /// </summary>
        public int HouseOwnerPlayerGUID { get { return this.houseOwnerPlayerGUID; } }

        protected ClientSocket[] playersInRoom;
        /// <summary>
        /// 当前房间内的玩家
        /// </summary>
        public ClientSocket[] PlayersInRoom { get { return playersInRoom; } }
        protected int maxPlayerAmount;
        /// <summary>
        /// 最大玩家数量
        /// </summary>
        public int MaxPlayerAmount { get { return maxPlayerAmount; } }
        protected int nowPlayerAmount;
        /// <summary>
        /// 当前房间玩家数量
        /// </summary>
        public int NowPlayerAmount { get { return nowPlayerAmount; } }

        protected bool playing = false;
        /// <summary>
        /// 当前房间是否在游戏中
        /// </summary>
        public bool Playing { get { return playing; } }

        protected string houseOwnerPlayerName;
        /// <summary>
        /// 当前房间的房主的名字
        /// </summary>
        public string HouseOwnerPlayerName { get { return this.houseOwnerPlayerName; } }

        protected bool[] playersReadyState;
        /// <summary>
        /// 房间玩家的准备状态
        /// </summary>
        public bool[] PlayersReadyState { get { return playersReadyState; } }

        protected ClientSocket nowActivePlayer;
        /// <summary>
        /// 当前正在执行操作的玩家
        /// </summary>
        public ClientSocket NowActivelayer { get { return nowActivePlayer; } }

        protected ClientSocket previousOutCardPlayer;
        /// <summary>
        /// 上一个出牌玩家
        /// </summary>
        public ClientSocket PreviousOutCardPlayer { get { return previousOutCardPlayer; } }

        protected OutCardDirection outCardDirection;
        /// <summary>
        /// 当前出牌方向
        /// </summary>
        public OutCardDirection OutCardDirection { get { return outCardDirection; } }

        /// <summary>
        /// 当前房间内的玩家是否全部准备
        /// </summary>
        public bool AllPlayersIsReady
        {
            get
            {
                for (int i = 0; i < PlayersReadyState.Length; i++)
                {
                    if (PlayersReadyState[i] == false)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private string roomPassword;
        /// <summary>
        /// 当前房间的密码
        /// </summary>
        public string RoomPassword { get { return roomPassword; } }

        /// <summary>
        /// 房间是否有密码
        /// </summary>
        public bool HasPassword
        {
            get
            {
                return !string.IsNullOrEmpty(RoomPassword);
            }
        }

        /// <summary>
        /// 设置房间名称
        /// </summary>
        /// <param name="roomName"></param>
        public void SetRoomName(string roomName)
        {
            this.name = roomName;
        }

        /// <summary>
        /// 设置最大玩家数量
        /// </summary>
        /// <param name="amount"></param>
        public void SetMaxPlayerAmount(int amount)
        {
            this.maxPlayerAmount = amount;
        }

        /// <summary>
        /// 玩家准备
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <returns></returns>
        public bool PlayerReady(ClientSocket clientSocket)
        {
            bool res = false;
            for (int i = 0; i < PlayersInRoom.Length; i++)
            {
                if (PlayersInRoom[i].Equals(clientSocket))
                {
                    PlayersReadyState[i] = true;

                    res = true;
                    break;
                }
            }

            bool playingTemp = true;
            for (int j = 0; j < PlayersReadyState.Length; j++)
            {
                if (PlayersReadyState[j] == false)
                {
                    playingTemp = false;
                    break;
                }
            }
            playing = playingTemp;

            return res;
        }

        /// <summary>
        /// 加入本房间 加入成功返回座位号 加入失败返回-1，密码错误返回-2
        /// </summary>
        /// <param name="clientSocket">加入房间的玩家</param>
        /// <param name="roomPassword">房间密码</param>
        /// <returns></returns>
        public bool JoinRoom(ClientSocket clientSocket, string roomPassword, out int chairID)
        {
            //int res = -1;
            chairID = -1;

            if (NowPlayerAmount > MaxPlayerAmount)
            {
                //房间已满
                chairID = -1;

                return false;
            }

            if (HasPassword && !RoomPassword.Equals(roomPassword))
            {
                //密码错误
                chairID = -2;

                return false;
            }

            bool isJoinRoom = false;
            for (int i = 0; i < playersInRoom.Length; i++)
            {
                if (playersInRoom[i] == null)
                {
                    playersInRoom[i] = clientSocket;
                    isJoinRoom = true;
                    chairID = i;
                    break;
                }
            }
            if (!isJoinRoom)
            {
                Debug.L.Warn(string.Format("加入房间失败，房间[{0}]中没有空座位", this.RoomID));
                chairID = -1;

                return false;
            }

            nowPlayerAmount++;

            return true;
        }

        /// <summary>
        /// 退出房间。如果房间内没有玩家则返回true，否则为false
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <returns></returns>
        public bool QuitRoom(ClientSocket clientSocket)
        {
            for (int i = 0; i < PlayersInRoom.Length; i++)
            {
                ClientSocket client = PlayersInRoom[i];
                if (client != null && client.Equals(clientSocket))
                {
                    PlayersInRoom[i] = null;
                    nowPlayerAmount--;
                    break;
                }
            }

            if (NowPlayerAmount <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取玩家座位ID。以房主为0号座位
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <returns></returns>
        public int GetChairID(ClientSocket clientSocket)
        {
            int res = -1;
            for (int i = 0; i < PlayersInRoom.Length; i++)
            {
                ClientSocket client = PlayersInRoom[i];
                if (clientSocket.Equals(client))
                {
                    res = i;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// 玩家取消准备
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <returns></returns>
        public bool PlayerCancelReady(ClientSocket clientSocket)
        {
            bool res = false;
            for (int i = 0; i < PlayersInRoom.Length; i++)
            {
                if (PlayersInRoom[i].Equals(clientSocket))
                {
                    PlayersReadyState[i] = false;

                    res = true;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// 游戏开始时间
        /// </summary>
        protected long startGameTime;

        private GameType gameType;
        /// <summary>
        /// 当前房间的游戏类型
        /// </summary>
        public GameType GameType { get { return gameType; } }

        public RoomBase(GameType gameType, ClientSocket clientSocket, string roomName, GameRuleDataBase ruleBase) : this(gameType, clientSocket, roomName, null, ruleBase) { }

        /// <summary>
        /// 实例化一个房间对象
        /// </summary>
        /// <param name="gameType"></param>
        /// <param name="clientSocket">创建房间的玩家连接对象（将作为默认房主）</param>
        /// <param name="roomName"></param>
        /// <param name="roomPassword"></param>
        public RoomBase(GameType gameType, ClientSocket clientSocket, string roomName, string roomPassword, GameRuleDataBase ruleBase)
        {
            this.gameType = gameType;
            this.name = roomName;
            this.roomPassword = roomPassword;

            if(ruleBase != null)
            {
                //UNO游戏人数
                SetMaxPlayerAmount(ruleBase.PlayerCount);
                playersInRoom = new ClientSocket[ruleBase.PlayerCount];
                playersReadyState = new bool[ruleBase.PlayerCount];
            }

            this.houseOwnerPlayerGUID = clientSocket.PlayerGUID;
            this.roomID = CreateNotRepeatingRoomStringID();
            string playerName = UserDBUtil.Instance.SelectPlayerNameByGUID(this.houseOwnerPlayerGUID);
            if (string.IsNullOrEmpty(playerName))
            {
                Debug.L.Error(string.Format("没有查询到GUID为[{0}]的玩家名称信息", this.houseOwnerPlayerGUID));

                playerName = "Error";
            }
            this.houseOwnerPlayerName = playerName;

            //房间内玩家
            for (int i = 0; i < playersInRoom.Length; i++)
            {
                if (playersInRoom[i] == null)
                {
                    playersInRoom[i] = clientSocket;
                    break;
                }
            }
            nowPlayerAmount = 1;
        }

        /// <summary>
        /// 获取自游戏开始到现在的时间
        /// </summary>
        /// <returns></returns>
        public long GetPlayingTime()
        {
            return ServerUtils.GetTimeStamp() - this.startGameTime;
        }

        /// <summary>
        /// 轮到下一个玩家进行操作
        /// </summary>
        /// <returns></returns>
        public virtual void TurnToActivePlayer()
        {
            int nowOutCardPlayerChairID = GetChairID(NowActivelayer);
            previousOutCardPlayer = PlayersInRoom[nowOutCardPlayerChairID];
            if (OutCardDirection == OutCardDirection.Clockwise)
            {
                //逆时针方向 索引递增
                nowOutCardPlayerChairID++;
                nowOutCardPlayerChairID %= PlayersInRoom.Length;
            }
            else
            {
                //顺时针方向 索引递减
                nowOutCardPlayerChairID--;
                if (nowOutCardPlayerChairID < 0)
                {
                    nowOutCardPlayerChairID = PlayersInRoom.Length - 1;
                }
            }

            nowActivePlayer = PlayersInRoom[nowOutCardPlayerChairID];
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public virtual void StartGame() 
        {
            startGameTime = ServerUtils.GetTimeStamp();

            //默认房主为第一个进行操作的玩家
            nowActivePlayer = PlayersInRoom[0];
        }

        /// <summary>
        /// 游戏开始时提供的字符串。给游戏开始协议用的
        /// </summary>
        /// <returns></returns>
        public virtual string GetStartGameInfoString()
        {
            return "";
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        public virtual void GameOver()
        {
            playing = false;

            //恢复标志位
            for (int i = 0; i < playersReadyState.Length; i++)
            {
                playersReadyState[i] = false;
            }
        }

        /// <summary>
        /// 玩家重连游戏
        /// </summary>
        /// <param name="oldClientSocket">旧的连接对象</param>
        /// <param name="newClientSocket">新的连接对象</param>
        public virtual void PlayerReconnection(ClientSocket oldClientSocket, ClientSocket newClientSocket)
        {
            ClientSocket client = null;
            int playerIndex = -1;
            for (int i = 0; i < PlayersInRoom.Length; i++)
            {
                if (PlayersInRoom[i].Equals(oldClientSocket))
                {
                    client = PlayersInRoom[i];
                    playerIndex = i;

                    break;
                }
            }
            if (client == null)
            {
                Debug.L.Error(string.Format("玩家重连游戏，但是在房间[{0}]中没有找到对应玩家[{1}]", this.RoomID, newClientSocket.Socket.RemoteEndPoint.ToString()));

                return;
            }

            playersInRoom[playerIndex] = newClientSocket;
            if (NowActivelayer.Equals(oldClientSocket))
            {
                nowActivePlayer = newClientSocket;
            }

            ServerSocket.DisconnectedDict.Remove(newClientSocket.PlayerGUID);

            PlayerReconnectionAction(newClientSocket);
        }

        /// <summary>
        /// 发送重连消息到客户端
        /// </summary>
        /// <param name="clientSocket"></param>
        public abstract void PlayerReconnectionAction(ClientSocket clientSocket);

        /// <summary>
        /// 出牌倒计时
        /// </summary>
        public abstract int OutCardCountdown { get; }

        /// <summary>
        /// 游戏是否结束
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="playerNameList"></param>
        /// <param name="playerGUIDList"></param>
        /// <param name="scoreList"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract bool IsGameOver(out ClientSocket clientSocket, out List<string> playerNameList, out List<int> playerGUIDList, out List<int> scoreList, out object obj);

        /// <summary>
        /// 给房间内所有玩家发送游戏结束的消息，并更新记录到数据库
        /// </summary>
        /// <param name="winPlayer"></param>
        /// <param name="playerNameList"></param>
        /// <param name="playerGUIDList"></param>
        /// <param name="scoreList"></param>
        /// <param name="obj"></param>
        public virtual void SendGameOverMessage(ClientSocket winPlayer, List<string> playerNameList, List<int> playerGUIDList, List<int> scoreList, object obj = null)
        {
            long time = GetPlayingTime();
            for (int i = 0; i < PlayersInRoom.Length; i++)
            {
                ClientSocket client = PlayersInRoom[i];

                if (client != null)
                {
                    byte[] gameOverReceiveData = MsgRoomBuilder.GameOverReceiveSerialize(
                        winPlayer.PlayerGUID,
                        Name,
                        RoomID,
                        time,
                        MaxPlayerAmount,
                        playerNameList,
                        playerGUIDList,
                        scoreList,
                        GameType,
                        client.MsgSecretKey);
                    ServerSocket.Instance.SendMessage(client, gameOverReceiveData);
                }
            }

            //更新记录到数据库
            for (int i = 0; i < playerGUIDList.Count; i++)
            {
                int changeGoldCoin = scoreList[i];
                UserDBUtil.Instance.ChangeGoldCoin(playerGUIDList[i], changeGoldCoin);
            }
            //将所有玩家的对战次数+1
            for (int i = 0; i < PlayersInRoom.Length; i++)
            {
                UserDBUtil.Instance.AddTotalNumOfGames(PlayersInRoom[i].PlayerGUID);
            }
            //胜利的玩家赢游戏的次数+1
            UserDBUtil.Instance.AddNumOfWins(winPlayer.PlayerGUID);
            //胜利的玩家最高战绩尝试更新
            int winPlayerBestRecord = UserDBUtil.Instance.SelectBestRecordByGUID(winPlayer.PlayerGUID);
            if (scoreList[0] > winPlayerBestRecord)
            {
                UserDBUtil.Instance.UpdateBestRecord(winPlayer.PlayerGUID, scoreList[0]);
            }

            //TODO:更新当前房间内所有玩家显示分数

            //更新大厅金币数量
            for (int i = 0; i < PlayersInRoom.Length; i++)
            {
                ClientSocket client = PlayersInRoom[i];
                if (client != null)
                {
                    int coinCount = UserDBUtil.Instance.SelectUserCoinByGUID(client.PlayerGUID);
                    byte[] updateGoldCoinReceiveData = MsgCommonBuilder.UpdateGoldCoinReceiveSerialize(client.PlayerGUID, coinCount, client.MsgSecretKey);
                    ServerSocket.Instance.SendMessage(client, updateGoldCoinReceiveData);
                }
            }
        }

        #region 生成房间ID
        private readonly static List<char> ROOM_STRING_ID_INIT_CHAR_NORMAL = new List<char>() {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                                                                                               'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                                                                                               'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
                                                                                               'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                                                                                               'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                                                                                               'y', 'z'};
        private readonly static List<char> ROOM_STRING_ID_INIT_CHAR_SPECIAL = new List<char>() { '!', '#', '$', '%', '&', '+', '=', '?', '@' };
        private readonly static List<char> ROOM_STRING_ID_INIT_CHAR_NUMBER = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// 创建12位字符串房间ID 不与服务器已经存在的房间ID重复
        /// </summary>
        /// <returns></returns>
        public static string CreateNotRepeatingRoomStringID()
        {
            string id = CreateRoomStringID();
            while (ServerSocket.Instance.RoomIDList.Contains(id))
            {
                id = CreateRoomStringID();
            }

            return id;
        }

        /// <summary>
        /// 创建6位数字房间ID 不与服务器已经存在的房间ID重复
        /// </summary>
        /// <returns></returns>
        public static int CreateNotRepeatingRoomNumberID()
        {
            int id = CreateRoomNumberID();
            while (ServerSocket.Instance.RoomIDList.Contains(id.ToString()))
            {
                id = CreateRoomNumberID();
            }

            return id;
        }

        /// <summary>
        /// 创建6位数字房间ID
        /// </summary>
        /// <returns></returns>
        public static int CreateRoomNumberID()
        {
            Random r = new Random();
            int number = r.Next(0, 10);
            for (int i = 0; i < 5; i++)
            {
                int n = r.Next(0, 10);
                number = number * 10 + n;
            }

            return number;
        }

        /// <summary>
        /// 创建12位字符串房间ID
        /// </summary>
        /// <returns></returns>
        public static string CreateRoomStringID()
        {
            StringBuilder builder = new StringBuilder();
            Random r = new Random();
            char c = ' ';
            //上一个生成的字符
            //四个字符为一小组 每一小组的起始字符固定为普通字符 字符不能连续出现
            char lastChar;//上一个生成的字符
            for (int i = 0; i < 3; i++)
            {
                int index = r.Next(0, ROOM_STRING_ID_INIT_CHAR_NORMAL.Count);
                lastChar = ROOM_STRING_ID_INIT_CHAR_NORMAL[index];
                while (lastChar == c)
                {
                    index = r.Next(0, ROOM_STRING_ID_INIT_CHAR_NORMAL.Count);
                    lastChar = ROOM_STRING_ID_INIT_CHAR_NORMAL[index];
                }
                c = lastChar;
                builder.Append(c);
                for (int j = 0; j < 3; j++)
                {
                    int typeIndex = r.Next(0, 10);
                    //降低特殊字符的出现概率
                    if (typeIndex < 3)
                    {
                        index = r.Next(0, ROOM_STRING_ID_INIT_CHAR_SPECIAL.Count);
                        lastChar = ROOM_STRING_ID_INIT_CHAR_SPECIAL[index];
                        while (lastChar == c)
                        {
                            index = r.Next(0, ROOM_STRING_ID_INIT_CHAR_SPECIAL.Count);
                            lastChar = ROOM_STRING_ID_INIT_CHAR_SPECIAL[index];
                        }
                        c = lastChar;
                    }
                    else
                    {
                        if (typeIndex > 6)
                        {
                            //数字
                            index = r.Next(0, ROOM_STRING_ID_INIT_CHAR_NUMBER.Count);
                            lastChar = ROOM_STRING_ID_INIT_CHAR_NUMBER[index];
                            while (lastChar == c)
                            {
                                index = r.Next(0, ROOM_STRING_ID_INIT_CHAR_NUMBER.Count);
                                lastChar = ROOM_STRING_ID_INIT_CHAR_NUMBER[index];
                            }
                            c = lastChar;
                        }
                        else
                        {
                            //字母
                            index = r.Next(0, ROOM_STRING_ID_INIT_CHAR_NORMAL.Count);
                            lastChar = ROOM_STRING_ID_INIT_CHAR_NORMAL[index];
                            while (lastChar == c)
                            {
                                index = r.Next(0, ROOM_STRING_ID_INIT_CHAR_NORMAL.Count);
                                lastChar = ROOM_STRING_ID_INIT_CHAR_NORMAL[index];
                            }
                            c = lastChar;
                        }
                    }
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }
        #endregion
    }
}
