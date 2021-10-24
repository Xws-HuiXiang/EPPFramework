---Unity广告观看完成请求
MsgUnityAdsWatchFinishRequestTable = {
    ---玩家GUID
    playerGUID,
    ---广告ID
    placementId,
    ---广告观看结果
    result,
};

---Unity广告观看完成响应
MsgUnityAdsWatchFinishReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---当前玩家的金币数量
    goldCoinCount,
};

MsgUnityAdsWatchFinishRequestTable.__index = MsgUnityAdsWatchFinishRequestTable;
MsgUnityAdsWatchFinishReceiveTable.__index = MsgUnityAdsWatchFinishReceiveTable;

---创建 Unity广告观看完成请求 协议对象
function MsgUnityAdsWatchFinishRequestTable:New()
    local o  = {};
    setmetatable(o, MsgUnityAdsWatchFinishRequestTable);

    return o;
end

---创建 Unity广告观看完成响应 协议对象
function MsgUnityAdsWatchFinishReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgUnityAdsWatchFinishReceiveTable);

    return o;
end

