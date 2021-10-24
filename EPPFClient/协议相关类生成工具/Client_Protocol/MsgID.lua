---协议处理ID
MsgHandleID = {
    ---广告相关的协议
    Ads = 8,
    ---配置协议的例子
    Common = 0,
}

---广告相关的协议 的处理方法ID
MsgAdsMethodID = {
    ---Unity广告观看完成请求
    UnityAdsWatchFinishRequest = 0,
    ---Unity广告观看完成响应
    UnityAdsWatchFinishReceive = 1,
}

---配置协议的例子 的处理方法ID
MsgCommonMethodID = {
    ---获取密钥请求
    SecretKeyRequest = 0,
    ---获取密钥响应
    SecretKeyReceive = 1,
    ---心跳包请求
    HeartbeatRequest = 2,
    ---心跳包响应
    HeartbeatReceive = 3,
}

