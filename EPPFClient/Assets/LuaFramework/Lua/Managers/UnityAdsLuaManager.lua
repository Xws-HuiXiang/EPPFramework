require "Protocol/Tables/AdsTable"

UnityAdsLuaManager = {};
local this = UnityAdsLuaManager;

--- UnityAds
this.Ads = nil;

function this.Init()
    this.Ads = UnityAds.New("");--这里传UnityAds后台的项目ID

    --清空事件监听
    this.RemoveAllEventFunc();
    --添加事件监听
    this.AddAllEventFunc();
end

--- 添加全部的事件方法监听
function this.AddAllEventFunc()
    this.Ads:AddUnityAdsReadyEvent(this.OnUnityAdsReady);
    this.Ads:AddUnityAdsDidErrorEvent(this.OnUnityAdsDidError);
    this.Ads:AddUnityAdsDidStartEvent(this.OnUnityAdsDidStart);
    this.Ads:AddUnityAdsDidFinishFailedEvent(this.OnUnityAdsDidFinishFailed);
    this.Ads:AddUnityAdsDidFinishSkippedEvent(this.OnUnityAdsDidFinishSkipped);
    this.Ads:AddUnityAdsDidFinishFinishedEvent(this.OnUnityAdsDidFinishFinished);
    this.Ads:AddAdsIsReadyEvent(this.AdsIsReady);
    this.Ads:AddAdsIsNotReadyEvent(this.AdsIsNotReady);
end

--- 移除全部事件监听
function this.RemoveAllEventFunc()
    this.Ads:RemoveUnityAdsReadyEvent(this.OnUnityAdsReady);
    this.Ads:RemoveUnityAdsDidErrorEvent(this.OnUnityAdsDidError);
    this.Ads:RemoveUnityAdsDidStartEvent(this.OnUnityAdsDidStart);
    this.Ads:RemoveUnityAdsDidFinishFailedEvent(this.OnUnityAdsDidFinishFailed);
    this.Ads:RemoveUnityAdsDidFinishSkippedEvent(this.OnUnityAdsDidFinishSkipped);
    this.Ads:RemoveUnityAdsDidFinishFinishedEvent(this.OnUnityAdsDidFinishFinished);
    this.Ads:RemoveIsReadyEvent(this.AdsIsReady);
    this.Ads:RemoveAdsIsNotReadyEvent(this.AdsIsNotReady);
end

--- 广告准备完成
function this.OnUnityAdsReady(placementId)
    --FDebugger.Log("广告准备完成");
end

--- 广告播放出错
function this.OnUnityAdsDidError(message)
    TipText.ShowTipText("广告播放出错，请稍后重试");
end

--- 广告开始播放
function this.OnUnityAdsDidStart(placementId)
    --FDebugger.Log("广告开始播放");
end

--- 广告播放失败
function this.OnUnityAdsDidFinishFailed(placementId, showResult)
    TipText.ShowTipText("广告播放失败，请稍后重试");

    this.SendUnityAdsWatchFinishedRequest(placementId, 0);
end

--- 广告播放被跳过
function this.OnUnityAdsDidFinishSkipped(placementId, showResult)
    TipText.ShowTipText("广告被跳过，将无法得到奖励呦");

    this.SendUnityAdsWatchFinishedRequest(placementId, 1);
end

--- 广告播放完成
function this.OnUnityAdsDidFinishFinished(placementId, showResult)
    this.SendUnityAdsWatchFinishedRequest(placementId, 2);
end

--- 调用Show方法时，广告已经播放时调用的事件
function this.AdsIsReady()
    --FDebugger.Log("调用Show方法时，广告已经播放 时调用的事件");
end

--- 调用Show方法时，广告还没有准备好的事件
function this.AdsIsNotReady()
    --FDebugger.Log("调用Show方法时，广告还没有准备好的事件");
end

--- 显示一个广告
function this.Show()
    this.Ads:Show();
end

--- 发送广告观看结果
function this.SendUnityAdsWatchFinishedRequest(placementId, result)
    local msg = MsgUnityAdsWatchFinishRequestTable:New();

    msg.playerGUID = GameLuaManager.PlayerInfo.GUID;
    msg.placementId = placementId;
    local enumFullName = NetworkLuaManager.CombineFullPbName("AdsShowResult");
    msg.result = pb.enum(enumFullName, result);

    local data = NetworkLuaManager.EncodeMsg(MsgAdsMethodStringID.UnityAdsWatchFinishRequest, msg);
    NetworkManager.Instance:SendMsgFromLua(MsgHandleID.Ads, MsgAdsMethodID.UnityAdsWatchFinishRequest, data);
end

--- 观看广告结果返回
function this.OnUnityAdsWatchFinishedReceive(data)
    TipText.ShowTipText("获得40金币奖励");

    local msg = NetworkLuaManager.DecodeMsg(MsgAdsMethodStringID.UnityAdsWatchFinishReceive, data);

    GameLuaManager.PlayerInfo.Score = msg.goldCoinCount;

    MainPageCtrl.UpdatePlayerInfoShow();
end