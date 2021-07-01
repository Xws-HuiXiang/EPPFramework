using QiLieGuaner;
using System.Collections.Generic;

namespace EPPFServer.Protocol.Builder
{
    /// <summary>
    /// 游戏房间相关逻辑 消息序列化工具类
    /// </summary>
    public class MsgRoomBuilder : MsgBuilderBase
    {
        private static MsgRoomBuilder instance;
        public static MsgRoomBuilder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MsgRoomBuilder();
                }

                return instance;
            }
        }

        private MsgRoomBuilder() : base(MsgHandleID.Room) { }

        /// <summary>
        /// 序列化 创建房间请求
        /// </summary>
        /// <param name="playerGUID">玩家guid</param>
        /// <param name="roomName">房间名称</param>
        /// <param name="roomPassword">房间密码</param>
        /// <param name="gameTypeID">游戏类型ID</param>
        /// <param name="gameRule">创建房间时，指定的游戏规则串。配置项顺序与客户端中表顺序一致；每个大项规则之间用‘|’分割，如果是多选则选项之间用‘#’分割</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] CreateRoomRequestSerialize(int playerGUID, string roomName, string roomPassword, GameType gameTypeID, string gameRule, byte[] msgSecretKey)
        {
            CreateRoomRequest msg = new CreateRoomRequest();

            msg.PlayerGUID = playerGUID;
            msg.RoomName = roomName;
            msg.RoomPassword = roomPassword;
            msg.GameTypeID = gameTypeID;
            msg.GameRule = gameRule;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.CreateRoomRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 创建房间响应
        /// </summary>
        /// <param name="state">创建房间结果</param>
        /// <param name="playerGUID">创建房间的玩家guid</param>
        /// <param name="roomName">房间名称</param>
        /// <param name="roomID">房间ID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] CreateRoomReceiveSerialize(int state, int playerGUID, string roomName, string roomID, byte[] msgSecretKey)
        {
            CreateRoomReceive msg = new CreateRoomReceive();

            msg.State = state;
            msg.PlayerGUID = playerGUID;
            msg.RoomName = roomName;
            msg.RoomID = roomID;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.CreateRoomReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 房间列表请求
        /// </summary>
        /// <param name="playerGUID">发起查询房间列表的玩家guid</param>
        /// <param name="gameType">游戏类型</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] RoomListRequestSerialize(int playerGUID, GameType gameType, byte[] msgSecretKey)
        {
            RoomListRequest msg = new RoomListRequest();

            msg.PlayerGUID = playerGUID;
            msg.GameType = gameType;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.RoomListRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 房间列表响应
        /// </summary>
        /// <param name="playerGUID">发起查询房间列表的玩家guid</param>
        /// <param name="roomInfoList">房间信息列表</param>
        /// <param name="roomHasLockList">房间是否有锁的列表</param>
        /// <param name="gameType">游戏类型</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] RoomListReceiveSerialize(int playerGUID, List<string> roomInfoList, List<bool> roomHasLockList, GameType gameType, byte[] msgSecretKey)
        {
            RoomListReceive msg = new RoomListReceive();

            msg.PlayerGUID = playerGUID;
            if (roomInfoList != null)
            {
                foreach (var item in roomInfoList)
                {
                    msg.RoomInfoList.Add(item);
                }
            }
            if (roomHasLockList != null)
            {
                foreach (var item in roomHasLockList)
                {
                    msg.RoomHasLockList.Add(item);
                }
            }
            msg.GameType = gameType;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.RoomListReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 加入房间请求
        /// </summary>
        /// <param name="playerGUID">玩家guid</param>
        /// <param name="roomID">房间ID</param>
        /// <param name="roomPassword">房间密码</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] JoinRoomRequestSerialize(int playerGUID, string roomID, string roomPassword, byte[] msgSecretKey)
        {
            JoinRoomRequest msg = new JoinRoomRequest();

            msg.PlayerGUID = playerGUID;
            msg.RoomID = roomID;
            msg.RoomPassword = roomPassword;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.JoinRoomRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 加入房间响应
        /// </summary>
        /// <param name="state">加入UNO房间的结果</param>
        /// <param name="gameTypeID">游戏类型ID</param>
        /// <param name="roomID">房间ID</param>
        /// <param name="roomName">房间名称</param>
        /// <param name="playerAmount">开始游戏需要的玩家数量</param>
        /// <param name="playerChairID">椅子ID</param>
        /// <param name="outCardCountdown">出牌倒计时</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] JoinRoomReceiveSerialize(int state, GameType gameTypeID, string roomID, string roomName, int playerAmount, int playerChairID, int outCardCountdown, byte[] msgSecretKey)
        {
            JoinRoomReceive msg = new JoinRoomReceive();

            msg.State = state;
            msg.GameTypeID = gameTypeID;
            msg.RoomID = roomID;
            msg.RoomName = roomName;
            msg.PlayerAmount = playerAmount;
            msg.PlayerChairID = playerChairID;
            msg.OutCardCountdown = outCardCountdown;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.JoinRoomReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 退出房间请求
        /// </summary>
        /// <param name="playerGUID">玩家guid</param>
        /// <param name="roomID">房间ID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] QuitRoomRequestSerialize(int playerGUID, string roomID, byte[] msgSecretKey)
        {
            QuitRoomRequest msg = new QuitRoomRequest();

            msg.PlayerGUID = playerGUID;
            msg.RoomID = roomID;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.QuitRoomRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 退出房间响应
        /// </summary>
        /// <param name="state">退出UNO房间的结果</param>
        /// <param name="playerGUID">退出房间的玩家ID</param>
        /// <param name="roomGameType">游戏类型ID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] QuitRoomReceiveSerialize(int state, int playerGUID, GameType roomGameType, byte[] msgSecretKey)
        {
            QuitRoomReceive msg = new QuitRoomReceive();

            msg.State = state;
            msg.PlayerGUID = playerGUID;
            msg.RoomGameType = roomGameType;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.QuitRoomReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 其他玩家加入房间请求
        /// </summary>
        /// <param name="playerGUID">玩家guid</param>
        /// <param name="roomID">房间ID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] OtherPlayerJoinRoomRequestSerialize(int playerGUID, string roomID, byte[] msgSecretKey)
        {
            OtherPlayerJoinRoomRequest msg = new OtherPlayerJoinRoomRequest();

            msg.PlayerGUID = playerGUID;
            msg.RoomID = roomID;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.OtherPlayerJoinRoomRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 其他玩家加入房间响应
        /// </summary>
        /// <param name="playerName">加入房间的玩家名称</param>
        /// <param name="playerGUID">加入房间的玩家GUID</param>
        /// <param name="avatarName">加入的玩家头像名称</param>
        /// <param name="playerScore">加入房间的玩家分数（金币）</param>
        /// <param name="playerChairID">加入的玩家座位号</param>
        /// <param name="roomGameType">游戏类型ID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] OtherPlayerJoinRoomReceiveSerialize(string playerName, int playerGUID, string avatarName, int playerScore, int playerChairID, GameType roomGameType, byte[] msgSecretKey)
        {
            OtherPlayerJoinRoomReceive msg = new OtherPlayerJoinRoomReceive();

            msg.PlayerName = playerName;
            msg.PlayerGUID = playerGUID;
            msg.AvatarName = avatarName;
            msg.PlayerScore = playerScore;
            msg.PlayerChairID = playerChairID;
            msg.RoomGameType = roomGameType;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.OtherPlayerJoinRoomReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 其他玩家退出房间请求
        /// </summary>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] OtherPlayerQuitRoomRequestSerialize(byte[] msgSecretKey)
        {
            OtherPlayerQuitRoomRequest msg = new OtherPlayerQuitRoomRequest();


            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.OtherPlayerQuitRoomRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 其他玩家退出房间响应
        /// </summary>
        /// <param name="chairID">退出房间的玩家椅子ID</param>
        /// <param name="playerGUID">退出UNO房间的玩家GUID</param>
        /// <param name="roomGameType">游戏类型ID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] OtherPlayerQuitRoomReceiveSerialize(int chairID, int playerGUID, GameType roomGameType, byte[] msgSecretKey)
        {
            OtherPlayerQuitRoomReceive msg = new OtherPlayerQuitRoomReceive();

            msg.ChairID = chairID;
            msg.PlayerGUID = playerGUID;
            msg.RoomGameType = roomGameType;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.OtherPlayerQuitRoomReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 快速加入房间请求
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] QuickJoinRoomRequestSerialize(int playerGUID, byte[] msgSecretKey)
        {
            QuickJoinRoomRequest msg = new QuickJoinRoomRequest();

            msg.PlayerGUID = playerGUID;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.QuickJoinRoomRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 快速加入房间响应
        /// </summary>
        /// <param name="state">快速加入房间结果</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] QuickJoinRoomReceiveSerialize(int state, byte[] msgSecretKey)
        {
            QuickJoinRoomReceive msg = new QuickJoinRoomReceive();

            msg.State = state;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.QuickJoinRoomReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 玩家准备请求
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] ReadyRequestSerialize(int playerGUID, byte[] msgSecretKey)
        {
            ReadyRequest msg = new ReadyRequest();

            msg.PlayerGUID = playerGUID;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.ReadyRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 玩家准备响应
        /// </summary>
        /// <param name="state">准备请求的结果</param>
        /// <param name="roomGameType">房间游戏类型</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] ReadyReceiveSerialize(int state, GameType roomGameType, byte[] msgSecretKey)
        {
            ReadyReceive msg = new ReadyReceive();

            msg.State = state;
            msg.RoomGameType = roomGameType;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.ReadyReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 玩家取消准备请求
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] CancelReadyRequestSerialize(int playerGUID, byte[] msgSecretKey)
        {
            CancelReadyRequest msg = new CancelReadyRequest();

            msg.PlayerGUID = playerGUID;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.CancelReadyRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 玩家取消准备响应
        /// </summary>
        /// <param name="state">取消准备请求的结果</param>
        /// <param name="roomGameType">房间游戏类型</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] CancelReadyReceiveSerialize(int state, GameType roomGameType, byte[] msgSecretKey)
        {
            CancelReadyReceive msg = new CancelReadyReceive();

            msg.State = state;
            msg.RoomGameType = roomGameType;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.CancelReadyReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 游戏开始请求
        /// </summary>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] StartGameRequestSerialize(byte[] msgSecretKey)
        {
            StartGameRequest msg = new StartGameRequest();


            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.StartGameRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 游戏开始响应
        /// </summary>
        /// <param name="time">游戏开始时的时间</param>
        /// <param name="startGameInfo">游戏开始时提供的参数串</param>
        /// <param name="roomGameType">房间游戏类型</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] StartGameReceiveSerialize(long time, string startGameInfo, GameType roomGameType, byte[] msgSecretKey)
        {
            StartGameReceive msg = new StartGameReceive();

            msg.Time = time;
            msg.StartGameInfo = startGameInfo;
            msg.RoomGameType = roomGameType;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.StartGameReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 游戏结束请求
        /// </summary>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] GameOverRequestSerialize(byte[] msgSecretKey)
        {
            GameOverRequest msg = new GameOverRequest();


            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.GameOverRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 游戏结束响应
        /// </summary>
        /// <param name="winPlayer">玩家GUID</param>
        /// <param name="roomName">房间名称</param>
        /// <param name="roomID">房间ID</param>
        /// <param name="time">对战时间</param>
        /// <param name="playerCount">房间内玩家数量</param>
        /// <param name="playerNameList">玩家名称列表</param>
        /// <param name="playerIDList">玩家ID列表</param>
        /// <param name="changeGoldCoinList">金币变动列表</param>
        /// <param name="roomGameType">房间游戏类型</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] GameOverReceiveSerialize(int winPlayer, string roomName, string roomID, long time, int playerCount, List<string> playerNameList, List<int> playerIDList, List<int> changeGoldCoinList, GameType roomGameType, byte[] msgSecretKey)
        {
            GameOverReceive msg = new GameOverReceive();

            msg.WinPlayer = winPlayer;
            msg.RoomName = roomName;
            msg.RoomID = roomID;
            msg.Time = time;
            msg.PlayerCount = playerCount;
            if (playerNameList != null)
            {
                foreach (var item in playerNameList)
                {
                    msg.PlayerNameList.Add(item);
                }
            }
            if (playerIDList != null)
            {
                foreach (var item in playerIDList)
                {
                    msg.PlayerIDList.Add(item);
                }
            }
            if (changeGoldCoinList != null)
            {
                foreach (var item in changeGoldCoinList)
                {
                    msg.ChangeGoldCoinList.Add(item);
                }
            }
            msg.RoomGameType = roomGameType;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.GameOverReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 其他玩家准备请求
        /// </summary>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] OtherPlayerReadyRequestSerialize(byte[] msgSecretKey)
        {
            OtherPlayerReadyRequest msg = new OtherPlayerReadyRequest();


            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.OtherPlayerReadyRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 其他玩家准备响应
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="chairID">玩家座位ID</param>
        /// <param name="roomGameType">房间游戏类型</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] OtherPlayerReadyReceiveSerialize(int playerGUID, int chairID, GameType roomGameType, byte[] msgSecretKey)
        {
            OtherPlayerReadyReceive msg = new OtherPlayerReadyReceive();

            msg.PlayerGUID = playerGUID;
            msg.ChairID = chairID;
            msg.RoomGameType = roomGameType;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.OtherPlayerReadyReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 其他玩家取消准备请求
        /// </summary>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] OtherPlayerCancelReadyRequestSerialize(byte[] msgSecretKey)
        {
            OtherPlayerCancelReadyRequest msg = new OtherPlayerCancelReadyRequest();


            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.OtherPlayerCancelReadyRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 其他玩家取消准备响应
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="chairID">玩家座位ID</param>
        /// <param name="roomGameType">房间游戏类型</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] OtherPlayerCancelReadyReceiveSerialize(int playerGUID, int chairID, GameType roomGameType, byte[] msgSecretKey)
        {
            OtherPlayerCancelReadyReceive msg = new OtherPlayerCancelReadyReceive();

            msg.PlayerGUID = playerGUID;
            msg.ChairID = chairID;
            msg.RoomGameType = roomGameType;

            return Instance.SerializeAndEncryption(msg, (int)MsgRoomMethodID.OtherPlayerCancelReadyReceive, msgSecretKey);
        }
    }
}
