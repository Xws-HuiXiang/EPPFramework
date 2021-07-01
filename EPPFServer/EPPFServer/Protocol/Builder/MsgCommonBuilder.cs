using QiLieGuaner;
using System.Collections.Generic;

namespace EPPFServer.Protocol.Builder
{
    /// <summary>
    /// 通用协议 消息序列化工具类
    /// </summary>
    public class MsgCommonBuilder : MsgBuilderBase
    {
        private static MsgCommonBuilder instance;
        public static MsgCommonBuilder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MsgCommonBuilder();
                }

                return instance;
            }
        }

        private MsgCommonBuilder() : base(MsgHandleID.Common) { }

        /// <summary>
        /// 序列化 获取密钥请求
        /// </summary>
        /// <param name="publicKey">客户端的消息公钥</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] SecretKeyRequestSerialize(string publicKey, byte[] msgSecretKey)
        {
            SecretKeyRequest msg = new SecretKeyRequest();

            msg.PublicKey = publicKey;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.SecretKeyRequest, msgSecretKey, true);
        }
        /// <summary>
        /// 序列化 获取密钥响应
        /// </summary>
        /// <param name="secretKey">密钥内容</param>
        /// <param name="msgPublicKey">服务端消息公钥</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] SecretKeyReceiveSerialize(string secretKey, string msgPublicKey, byte[] msgSecretKey)
        {
            SecretKeyReceive msg = new SecretKeyReceive();

            msg.SecretKey = secretKey;
            msg.MsgPublicKey = msgPublicKey;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.SecretKeyReceive, msgSecretKey, true);
        }
        /// <summary>
        /// 序列化 心跳包请求
        /// </summary>
        /// <param name="pingTime">时间戳</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] HeartbeatRequestSerialize(long pingTime, byte[] msgSecretKey)
        {
            HeartbeatRequest msg = new HeartbeatRequest();

            msg.PingTime = pingTime;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.HeartbeatRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 心跳包响应
        /// </summary>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] HeartbeatReceiveSerialize(byte[] msgSecretKey)
        {
            HeartbeatReceive msg = new HeartbeatReceive();


            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.HeartbeatReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 跑马灯请求
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] PaoMaDengRequestSerialize(int playerGUID, byte[] msgSecretKey)
        {
            PaoMaDengRequest msg = new PaoMaDengRequest();

            msg.PlayerGUID = playerGUID;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.PaoMaDengRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 跑马灯响应
        /// </summary>
        /// <param name="content">跑马灯内容</param>
        /// <param name="count">跑马灯滚动次数</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] PaoMaDengReceiveSerialize(string content, int count, byte[] msgSecretKey)
        {
            PaoMaDengReceive msg = new PaoMaDengReceive();

            msg.Content = content;
            msg.Count = count;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.PaoMaDengReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 更新金币请求
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] UpdateGoldCoinRequestSerialize(int playerGUID, byte[] msgSecretKey)
        {
            UpdateGoldCoinRequest msg = new UpdateGoldCoinRequest();

            msg.PlayerGUID = playerGUID;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.UpdateGoldCoinRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 更新金币响应
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="goldCoin">金币数量</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] UpdateGoldCoinReceiveSerialize(int playerGUID, int goldCoin, byte[] msgSecretKey)
        {
            UpdateGoldCoinReceive msg = new UpdateGoldCoinReceive();

            msg.PlayerGUID = playerGUID;
            msg.GoldCoin = goldCoin;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.UpdateGoldCoinReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 查询个人信息请求
        /// </summary>
        /// <param name="playerGUID">要查询的玩家GUID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] InquirePersonalInfoRequestSerialize(int playerGUID, byte[] msgSecretKey)
        {
            InquirePersonalInfoRequest msg = new InquirePersonalInfoRequest();

            msg.PlayerGUID = playerGUID;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.InquirePersonalInfoRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 查询个人信息响应
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="playerName">玩家昵称</param>
        /// <param name="signature">个性签名</param>
        /// <param name="total">总场次</param>
        /// <param name="win">胜利场次</param>
        /// <param name="winRate">胜率</param>
        /// <param name="best">最佳战绩</param>
        /// <param name="avatarName">玩家头像图片的名称</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] InquirePersonalInfoReceiveSerialize(int playerGUID, string playerName, string signature, int total, int win, float winRate, int best, int avatarName, byte[] msgSecretKey)
        {
            InquirePersonalInfoReceive msg = new InquirePersonalInfoReceive();

            msg.PlayerGUID = playerGUID;
            msg.PlayerName = playerName;
            msg.Signature = signature;
            msg.Total = total;
            msg.Win = win;
            msg.WinRate = winRate;
            msg.Best = best;
            msg.AvatarName = avatarName;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.InquirePersonalInfoReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 版本号查询请求
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] GameVersionRequestSerialize(int playerGUID, byte[] msgSecretKey)
        {
            GameVersionRequest msg = new GameVersionRequest();

            msg.PlayerGUID = playerGUID;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.GameVersionRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 版本号查询响应
        /// </summary>
        /// <param name="version">版本号</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] GameVersionReceiveSerialize(string version, byte[] msgSecretKey)
        {
            GameVersionReceive msg = new GameVersionReceive();

            msg.Version = version;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.GameVersionReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 聊天讲话的请求
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="channel">聊天的频道</param>
        /// <param name="text">发送的聊天内容</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] ChatTalkRequestSerialize(int playerGUID, ChatChannel channel, string text, byte[] msgSecretKey)
        {
            ChatTalkRequest msg = new ChatTalkRequest();

            msg.PlayerGUID = playerGUID;
            msg.Channel = channel;
            msg.Text = text;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.ChatTalkRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 聊天讲话的响应
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="playerName">玩家名称</param>
        /// <param name="channel">聊天的频道</param>
        /// <param name="text">聊天内容</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] ChatTalkReceiveSerialize(int playerGUID, string playerName, ChatChannel channel, string text, byte[] msgSecretKey)
        {
            ChatTalkReceive msg = new ChatTalkReceive();

            msg.PlayerGUID = playerGUID;
            msg.PlayerName = playerName;
            msg.Channel = channel;
            msg.Text = text;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.ChatTalkReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 全部默认头像名称请求
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] AllDefaultAvatarRequestSerialize(int playerGUID, byte[] msgSecretKey)
        {
            AllDefaultAvatarRequest msg = new AllDefaultAvatarRequest();

            msg.PlayerGUID = playerGUID;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.AllDefaultAvatarRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 全部默认头像响应
        /// </summary>
        /// <param name="defaultAvatarList">默认头像名称列表</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] AllDefaultAvatarReceiveSerialize(List<string> defaultAvatarList, byte[] msgSecretKey)
        {
            AllDefaultAvatarReceive msg = new AllDefaultAvatarReceive();

            if (defaultAvatarList != null)
            {
                foreach (var item in defaultAvatarList)
                {
                    msg.DefaultAvatarList.Add(item);
                }
            }

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.AllDefaultAvatarReceive, msgSecretKey);
        }
        /// <summary>
        /// 序列化 更新个人信息请求。不更新玩家名称，玩家名称的修改是单独的消息
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="playerAvatarName">玩家头像名称</param>
        /// <param name="playerSignature">玩家个性签名</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] UpdatePersonalInfoRequestSerialize(int playerGUID, string playerAvatarName, string playerSignature, byte[] msgSecretKey)
        {
            UpdatePersonalInfoRequest msg = new UpdatePersonalInfoRequest();

            msg.PlayerGUID = playerGUID;
            msg.PlayerAvatarName = playerAvatarName;
            msg.PlayerSignature = playerSignature;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.UpdatePersonalInfoRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 更新个人信息响应。不更新玩家名称，玩家名称的修改是单独的消息
        /// </summary>
        /// <param name="state">更新结果</param>
        /// <param name="playerAvatarName">玩家头像名称</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] UpdatePersonalInfoReceiveSerialize(int state, string playerAvatarName, byte[] msgSecretKey)
        {
            UpdatePersonalInfoReceive msg = new UpdatePersonalInfoReceive();

            msg.State = state;
            msg.PlayerAvatarName = playerAvatarName;

            return Instance.SerializeAndEncryption(msg, (int)MsgCommonMethodID.UpdatePersonalInfoReceive, msgSecretKey);
        }
    }
}
