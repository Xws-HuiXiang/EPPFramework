using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using EPPFServer.Attributes;
using EPPFServer.Network;
using System.Text.RegularExpressions;
using EPPFServer.Log;
using EPPFServer.Common;
using QiLieGuaner;
using EPPFServer.Protocol.Builder;
using EPPFServer.DataBase;
using System.IO;
using Ciphertext;

namespace EPPFServer.Protocol
{
    /// <summary>
    /// 心跳包处理相关协议
    /// </summary>
    [ProtocolHandleAttribute((int)MsgHandleID.Common)]
    public class MsgCommonHandle : ProtocolHandleBase
    {
        /// <summary>
        /// 获取公钥请求
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="data"></param>
        [ProtocolHandleMethodAttribute((int)MsgCommonMethodID.SecretKeyRequest)]
        public static void PublicKey(ClientSocket clientSocket, byte[] data)
        {
            SecretKeyRequest msg = ProtocolHandleBase.Deserialize<SecretKeyRequest>(data);

            clientSocket.SetMsgSecretKey(msg.PublicKey);

            //回复服务端的消息公钥
            string serverPublicKey = DHExchange.GetString(clientSocket.MsgPublicKey);
            byte[] receiveMsg = MsgCommonBuilder.SecretKeyReceiveSerialize(ServerSocket.SecretKey, serverPublicKey, clientSocket.MsgSecretKey);
            ServerSocket.Instance.SendMessage(clientSocket, receiveMsg);
        }

        /// <summary>
        /// 心跳包请求
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="data"></param>
        [ProtocolHandleMethodAttribute((int)MsgCommonMethodID.HeartbeatRequest)]
        public static void PingTime(ClientSocket clientSocket, byte[] data)
        {
            HeartbeatRequest msg = ProtocolHandleBase.Deserialize<HeartbeatRequest>(data);
            clientSocket.SetLastPingTime(msg.PingTime);
        }

        /// <summary>
        /// 跑马灯请求
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="data"></param>
        [ProtocolHandleMethodAttribute((int)MsgCommonMethodID.PaoMaDengRequest)]
        public static void PaoMaDeng(ClientSocket clientSocket, byte[] data)
        {
            //PaoMaDengRequest msg = ProtocolHandleBase.Deserialize<PaoMaDengRequest>(data);

            //跑马灯最多支持30个汉字
            byte[] receiveData = MsgCommonBuilder.PaoMaDengReceiveSerialize("欢迎来到UNO，祝您游戏愉快", -1, clientSocket.MsgSecretKey);
            ServerSocket.Instance.SendMessage(clientSocket, receiveData);
        }

        /// <summary>
        /// 查询个人信息请求
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="data"></param>
        [ProtocolHandleMethodAttribute((int)MsgCommonMethodID.InquirePersonalInfoRequest)]
        public static void InquirePersonalInfo(ClientSocket clientSocket, byte[] data)
        {
            InquirePersonalInfoRequest msg = ProtocolHandleBase.Deserialize<InquirePersonalInfoRequest>(data);

            string playerName = UserDBUtil.Instance.SelectPlayerNameByGUID(msg.PlayerGUID);
            int avatarName = UserDBUtil.Instance.SelectUserAvatarByGUID(msg.PlayerGUID);
            string signature = UserDBUtil.Instance.SelectPlayerSignatureByGUID(msg.PlayerGUID);
            if (string.IsNullOrEmpty(signature))
            {
                signature = "";
            }
            int totalNumberOfGames = UserDBUtil.Instance.SelectTotalNumberOfGamesByGUID(msg.PlayerGUID);
            int numberOfWins = UserDBUtil.Instance.SelectNumberOfWinsByGUID(msg.PlayerGUID);
            float winRate = (float)numberOfWins / totalNumberOfGames;
            int bestRecord = UserDBUtil.Instance.SelectBestRecordByGUID(msg.PlayerGUID);
            byte[] receiveData = MsgCommonBuilder.InquirePersonalInfoReceiveSerialize(
                msg.PlayerGUID,
                playerName,
                signature,
                totalNumberOfGames,
                numberOfWins,
                winRate,
                bestRecord,
                avatarName,
                clientSocket.MsgSecretKey);

            ServerSocket.Instance.SendMessage(clientSocket, receiveData);
        }

        /// <summary>
        /// 游戏版本号查询请求
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="data"></param>
        [ProtocolHandleMethodAttribute((int)MsgCommonMethodID.GameVersionRequest)]
        public static void GameVersion(ClientSocket clientSocket, byte[] data)
        {
            byte[] receiveData = MsgCommonBuilder.GameVersionReceiveSerialize(ServerSocket.GameVersion, clientSocket.MsgSecretKey);

            ServerSocket.Instance.SendMessage(clientSocket, receiveData);
        }

        /// <summary>
        /// 聊天讲话的请求
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="data"></param>
        [ProtocolHandleMethodAttribute((int)MsgCommonMethodID.ChatTalkRequest)]
        public static void ChatTalk(ClientSocket clientSocket, byte[] data)
        {
            ChatTalkRequest msg = ProtocolHandleBase.Deserialize<ChatTalkRequest>(data);

            //直接广播给所有玩家。这里需要加校验（聊天内容是否合法、玩家是否处于黑名单、屏蔽字处理、是否有喇叭等等）
            string playerName = UserDBUtil.Instance.SelectPlayerNameByGUID(msg.PlayerGUID);
            foreach (var client in ServerSocket.ClientSocketDict)
            {
                byte[] receiveData = MsgCommonBuilder.ChatTalkReceiveSerialize(msg.PlayerGUID, playerName, msg.Channel, msg.Text, client.Value.MsgSecretKey);
                ServerSocket.Instance.SendMessage(client.Value, receiveData);
            }
        }

        /// <summary>
        /// 全部默认头像名称请求
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="data"></param>
        [ProtocolHandleMethodAttribute((int)MsgCommonMethodID.AllDefaultAvatarRequest)]
        public static void AllDefaultAvatar(ClientSocket clientSocket, byte[] data)
        {
            AllDefaultAvatarRequest msg = ProtocolHandleBase.Deserialize<AllDefaultAvatarRequest>(data);

            List<string> avatarNameList = new List<string>();
#if DEBUG
            //调试模式中，默认头像列表直接写死是1-50
            for(int i = 1; i <= 50; i++)
            {
                avatarNameList.Add(i.ToString());
            }
#else
            //正式版中，读取配置文件配置的路径中所有png文件的名称
            if (!string.IsNullOrEmpty(ServerSocket.Config.DefaultAvatarPath))
            {
                string[] files = Directory.GetFiles(ServerSocket.Config.DefaultAvatarPath, "*.png", SearchOption.TopDirectoryOnly);
                for(int i = 0; i < files.Length; i++)
                {
                    string avatarName = Path.GetFileNameWithoutExtension(files[i]);
                    avatarNameList.Add(avatarName);
                }
            }
            else
            {
                Debug.L.Warn(string.Format("服务端没有配置默认头像路径"));
            }
#endif
            byte[] allDefaultAvatarReceiveData = MsgCommonBuilder.AllDefaultAvatarReceiveSerialize(avatarNameList, clientSocket.MsgSecretKey);
            ServerSocket.Instance.SendMessage(clientSocket, allDefaultAvatarReceiveData);
        }

        /// <summary>
        /// 更新个人信息请求
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="data"></param>
        [ProtocolHandleMethodAttribute((int)MsgCommonMethodID.UpdatePersonalInfoRequest)]
        public static void UpdatePersonalInfo(ClientSocket clientSocket, byte[] data)
        {
            UpdatePersonalInfoRequest msg = ProtocolHandleBase.Deserialize<UpdatePersonalInfoRequest>(data);

            bool updateSignatureRes = UserDBUtil.Instance.UpdateSignatureByGUID(msg.PlayerGUID, msg.PlayerSignature);
            bool updateAvatarRes = UserDBUtil.Instance.UpdateAvatarByGUID(msg.PlayerGUID, msg.PlayerAvatarName);

            int state;
            if(updateSignatureRes && updateAvatarRes)
            {
                //更新成功
                state = 1;
            }
            else
            {
                //更新失败
                state = 2;
            }

            byte[] receiveData = MsgCommonBuilder.UpdatePersonalInfoReceiveSerialize(state, msg.PlayerAvatarName, clientSocket.MsgSecretKey);
            ServerSocket.Instance.SendMessage(clientSocket, receiveData);
        }
    }
}
