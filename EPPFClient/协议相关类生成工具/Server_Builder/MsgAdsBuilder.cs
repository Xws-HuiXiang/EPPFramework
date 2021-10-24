using QiLieGuaner;
using System.Collections.Generic;

namespace MengHeGameServer.Protocol.Builder
{
    /// <summary>
    /// 广告相关的协议 消息序列化工具类
    /// </summary>
    public class MsgAdsBuilder : MsgBuilderBase
    {
        private static MsgAdsBuilder instance;
        public static MsgAdsBuilder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MsgAdsBuilder();
                }

                return instance;
            }
        }

        private MsgAdsBuilder() : base(MsgHandleID.Ads) { }

        /// <summary>
        /// 序列化 Unity广告观看完成请求
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="placementId">广告ID</param>
        /// <param name="result">广告观看结果</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] UnityAdsWatchFinishRequestSerialize(int playerGUID, string placementId, AdsShowResult result, byte[] msgSecretKey)
        {
            UnityAdsWatchFinishRequest msg = new UnityAdsWatchFinishRequest();

            msg.PlayerGUID = playerGUID;
            msg.PlacementId = placementId;
            msg.Result = result;

            return Instance.SerializeAndEncryption(msg, (int)MsgAdsMethodID.UnityAdsWatchFinishRequest, msgSecretKey);
        }
        /// <summary>
        /// 序列化 Unity广告观看完成响应
        /// </summary>
        /// <param name="playerGUID">玩家GUID</param>
        /// <param name="goldCoinCount">当前玩家的金币数量</param>
        /// <param name="msgSecretKey">消息加密使用的密钥</param>
        /// <returns>使用protobuf序列化后的字节数组</returns>
        public static byte[] UnityAdsWatchFinishReceiveSerialize(int playerGUID, int goldCoinCount, byte[] msgSecretKey)
        {
            UnityAdsWatchFinishReceive msg = new UnityAdsWatchFinishReceive();

            msg.PlayerGUID = playerGUID;
            msg.GoldCoinCount = goldCoinCount;

            return Instance.SerializeAndEncryption(msg, (int)MsgAdsMethodID.UnityAdsWatchFinishReceive, msgSecretKey);
        }
    }
}
