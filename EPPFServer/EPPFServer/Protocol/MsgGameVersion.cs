using CommonProtocol;
using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using EPPFServer.Attributes;
using EPPFServer.Network;

namespace EPPFServer.Protocol
{
    /// <summary>
    /// 游戏版本相关协议
    /// </summary>
    [ProtocolHandleAttribute((int)MsgHandle.Version)]
    public class MsgGameVersion
    {
        [ProtocolHandleMethodAttribute((int)MsgHandle.Version, (int)MsgGameVersionHandleMethod.GetGameHotFixVersion)]
        public static void GetHotfixVersion(ClientSocket clientSocket, byte[] msg)
        {
            string jsonString = Encoding.UTF8.GetString(msg);
            GameHotFixVersion data = JsonMapper.ToObject<GameHotFixVersion>(jsonString);
            data.platform = UnityEngine.RuntimePlatform.WindowsPlayer;
            data.resVersion = ServerSocket.Config.ResVersion;
            data.luaVersion = ServerSocket.Config.LuaVersion;

            ServerSocket.Instance.SendMessage(clientSocket, data);
        }
    }
}
