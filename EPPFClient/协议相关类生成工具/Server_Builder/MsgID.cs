namespace MengHeGameServer.Protocol.Builder
{
    public class MsgID { }

    /// <summary>
    /// 协议处理ID
    /// </summary>
    public enum MsgHandleID
    {
        /// <summary>
        /// 广告相关的协议
        /// </summary>
        Ads = 8,
        /// <summary>
        /// 配置协议的例子
        /// </summary>
        Common = 0,
    }

    /// <summary>
    /// 广告相关的协议
    /// </summary>
    public enum MsgAdsMethodID
    {
        /// <summary>
        /// Unity广告观看完成请求
        /// </summary>
        UnityAdsWatchFinishRequest = 0,
        /// <summary>
        /// Unity广告观看完成响应
        /// </summary>
        UnityAdsWatchFinishReceive = 1,
    }

    /// <summary>
    /// 配置协议的例子
    /// </summary>
    public enum MsgCommonMethodID
    {
        /// <summary>
        /// 获取密钥请求
        /// </summary>
        SecretKeyRequest = 0,
        /// <summary>
        /// 获取密钥响应
        /// </summary>
        SecretKeyReceive = 1,
        /// <summary>
        /// 心跳包请求
        /// </summary>
        HeartbeatRequest = 2,
        /// <summary>
        /// 心跳包响应
        /// </summary>
        HeartbeatReceive = 3,
    }

}