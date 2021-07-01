require "Protocol/MsgID"
require "Protocol/MsgStringID"
require "Protocol/Tables/CommonTable"
require "Ctrl/LoginCtrl"
require "Ctrl/CreateRoomCtrl"
require "Common/LuaTimer"
require "Ctrl/ChatCtrl"
require "Managers/RoomLuaManager"
require "Ctrl/Game/UNOGameCtrl"
require "Ctrl/Game/OthelloGameCtrl"

NetworkLuaManager = {};
local this = NetworkLuaManager;

---每个消息的包名
this.msgPackageName = "QiLieGuaner.";

---加载全部.pb文件的协议
function this.LoadProtocolSchema()
    local path = AppConst.ProtoFilePath;
    FDebugger.Log("proto文件路径：" .. path);
    local pbFileArray = Utils.GetFilesByDirectory(path, "*.*", {".meta"});
    --FDebugger.Log("proto文件数量：" .. pbFileArray.Length);
    for i = 0, pbFileArray.Length - 1 do
        --FDebugger.Log("加载proto文件：" .. pbFileArray[i]);
        assert(pb.loadfile(pbFileArray[i]));
    end
end

---添加所有的协议处理方法
function this.AddProtocolHandles()
    --登陆相关
    NetworkManager.AddProtocolHandle(MsgHandleID.Login, MsgLoginMethodID.LoginReceive, LoginCtrl.OnMsgLoginHandleMethodCallback);
    NetworkManager.AddProtocolHandle(MsgHandleID.Login, MsgLoginMethodID.RegisterReceive, RegisterCtrl.OnMsgRegisterBackHandleMethodCallback);
    NetworkManager.AddProtocolHandle(MsgHandleID.Login, MsgLoginMethodID.LogOutReceive, NetworkLuaManager.OnLogOutReceive);

    --更新玩家信息
    NetworkManager.AddProtocolHandle(MsgHandleID.Common, MsgCommonMethodID.UpdateGoldCoinReceive, MainPageCtrl.OnUpdateGoldCoinReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Common, MsgCommonMethodID.InquirePersonalInfoReceive, PersonalInfoCtrl.OnInquirePersonalInfoReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Common, MsgCommonMethodID.AllDefaultAvatarReceive, AvatarSelectCtrl.OnAllDefaultAvatarReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Common, MsgCommonMethodID.UpdatePersonalInfoReceive, PersonalInfoCtrl.OnUpdatePersonalInfoReceive);

    --跑马灯
    NetworkManager.AddProtocolHandle(MsgHandleID.Common, MsgCommonMethodID.PaoMaDengReceive, PaoMaDengCtrl.OnPaoMaDengReceive);

    --房间相关
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.CreateRoomReceive, CreateRoomCtrl.OnCreateRoomRequestCallback);
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.RoomListReceive, RoomListCtrl.OnGetRoomListReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.JoinRoomReceive, RoomListCtrl.OnJoinRoomReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.OtherPlayerJoinRoomReceive, RoomLuaManager.OnOtherPlayerJoinRoomReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.QuitRoomReceive, RoomLuaManager.OnQuitRoomReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.OtherPlayerQuitRoomReceive, RoomLuaManager.OnOtherPlayerQuitUNORoomReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.QuickJoinRoomReceive, MainPageCtrl.OnQuickJoinRoomReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.ReadyReceive, RoomLuaManager.OnReadyReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.CancelReadyReceive, RoomLuaManager.OnCancelReadyReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.StartGameReceive, RoomLuaManager.OnStartGameReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.GameOverReceive, RoomLuaManager.OnGameOverReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.OtherPlayerReadyReceive, RoomLuaManager.OnOtherPlayerReadyReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Room, MsgRoomMethodID.OtherPlayerCancelReadyReceive, RoomLuaManager.OnOtherPlayerCancelReadyReceive);

    --UNO游戏相关
    NetworkManager.AddProtocolHandle(MsgHandleID.UNOGame, MsgUNOGameMethodID.SendCardReceive, UNOGameCtrl.OnSendCardReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.UNOGame, MsgUNOGameMethodID.PutCardReceive, UNOGameCtrl.OnPutCardBtnClickReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.UNOGame, MsgUNOGameMethodID.TurnToPutCardPlayerReceive, UNOGameCtrl.TurnToPutCardPlayerReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.UNOGame, MsgUNOGameMethodID.SpecialCardReceive, UNOGameCtrl.SpecialCardReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.UNOGame, MsgUNOGameMethodID.ReconnectionReceive, UNOGameCtrl.OnReconnectionReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.UNOGame, MsgUNOGameMethodID.MessUpDiscardHeapReceive, UNOGameCtrl.OnMessUpDiscardHeapReceive);

    --翻转棋相关
    NetworkManager.AddProtocolHandle(MsgHandleID.OthelloGame, MsgOthelloGameMethodID.TurnToNextPlayerReceive, OthelloGameCtrl.OnTurnToNextPlayerReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.OthelloGame, MsgOthelloGameMethodID.PutDownPieceReceive, OthelloGameCtrl.OnPutDownPieceReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.OthelloGame, MsgOthelloGameMethodID.ReconnectionReceive, OthelloGameCtrl.OnReconnectionReceive);

    --排行榜
    NetworkManager.AddProtocolHandle(MsgHandleID.Ranking, MsgRankingMethodID.RankingReceive, RankingCtrl.OnRankingReceive);

    --商城
    NetworkManager.AddProtocolHandle(MsgHandleID.Mall, MsgMallMethodID.MallGoodsReceive, MallCtrl.OnMallGoodsReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Mall, MsgMallMethodID.AddToShoppingCardReceive, MallCtrl.OnAddToShoppingCardReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Mall, MsgMallMethodID.ShoppingCardReceive, MallCtrl.OnShoppingCardReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Mall, MsgMallMethodID.UpdateShoppingCardGoodsAmountReceive, MallCtrl.OnUpdateShoppingCardGoodsAmountReceive);
    NetworkManager.AddProtocolHandle(MsgHandleID.Mall, MsgMallMethodID.SettlementReceive, MallCtrl.OnSettlementReceive);

    --游戏版本号
    NetworkManager.AddProtocolHandle(MsgHandleID.Common, MsgCommonMethodID.GameVersionReceive, SettingCtrl.OnGameVersionReceive);

    --公屏聊天
    NetworkManager.AddProtocolHandle(MsgHandleID.Common, MsgCommonMethodID.ChatTalkReceive, ChatCtrl.OnChatTalkReceive);

    --活动
    NetworkManager.AddProtocolHandle(MsgHandleID.Activity, MsgActivityMethodID.ActivityListReceive, ActivityCtrl.OnActivityListReceive);

    --广告相关
    NetworkManager.AddProtocolHandle(MsgHandleID.Ads, MsgAdsMethodID.UnityAdsWatchFinishReceive, UnityAdsLuaManager.OnUnityAdsWatchFinishedReceive);
end

---获取私钥响应
function this.OnSecretKeyReceive(data)
    --注意decode的第一个参数，如果proto带有package，则需要写package.message
    local msg = NetworkLuaManager.DecodeMsg(MsgCommonMethodStringID.SecretKeyReceive, data);
    NetworkManager.SetSecretKey(msg.secretKey);
    NetworkManager.SetMsgSecretKey(msg.msgPublicKey);

    --收到密钥以后，持续发送心跳包
    LuaTimer.AddLoopTimer(NetworkManager.PING_INTERVAL, this.SendLastPingMsg, true);

    --因为配置信息是用私钥加密，所以等待私钥返回时再读取和设置
    --根据配置信息设置值
    AudioManager.Instance.BGMVolume = tonumber(SaveConfig.GetConfigValue(SaveConfig.ConfigListField.AudioBGMVolume, 0.4));
    AudioManager.Instance.EffectVolume = tonumber(SaveConfig.GetConfigValue(SaveConfig.ConfigListField.AudioEffectVolume, 1));
    AudioManager.Instance.EffectMute = Utils.ParseBoolean(SaveConfig.GetConfigValue(SaveConfig.ConfigListField.AudioEffectIsMute, false));
    AudioManager.Instance.BGMMute = Utils.ParseBoolean(SaveConfig.GetConfigValue(SaveConfig.ConfigListField.AudioBGMIsMute, false));

    --尝试自动登陆
    LoginCtrl.TryAutoLogin();
end

---发送心跳包
function this.SendLastPingMsg()
    local msg = MsgHeartbeatRequestTable:New();
    --用C#传回string，再转为number。直接传long貌似不支持这个类型
    msg.pingTime = tonumber(Utils.GetTimeStampString());

    --local data = assert(pb.encode('QiLieGuaner.HeartbeatRequest', msg));
    local data = NetworkLuaManager.EncodeMsg(MsgCommonMethodStringID.HeartbeatRequest, msg);
    NetworkManager.Instance:SendMsgFromLua(MsgHandleID.Common, MsgCommonMethodID.HeartbeatRequest, data);

    FDebugger.Log("发送心跳包");
end

--- 使用pb.encode序列化一个消息
--- @param msgStringID string 消息的字符串标识
--- @param msg string 消息表对象
--- @returns 返回值：使用protobuf序列化后的字节数组
function this.EncodeMsg(msgStringID, msg)
    local fullMsgName = this.msgPackageName .. msgStringID;
    local data = assert(pb.encode(fullMsgName, msg));

    return data;
end

--- 使用pb.decode反序列化一个消息
--- @param msgStringID string 消息的字符串标识
--- @param data any 消息字节数组，参数为字节数组类型
--- @returns 返回值：使用protobuf反序列化后的消息对象
function this.DecodeMsg(msgStringID, data)
    local fullMsgName = this.msgPackageName .. msgStringID;
    local msg = assert(pb.decode(fullMsgName, data));

    return msg;
end

---组合包名和提供的类型名 返回完整的类型名字符串
function this.CombineFullPbName(name)
    return this.msgPackageName .. name;
end

---NetworkManager的异常捕获
function this.SocketException(exceptionTipString)
    FDebugger.LogError("网络异常，错误信息：" .. exceptionTipString);

    TipWindowCtrl.ShowTipWindow(exceptionTipString, TipWindowCtrl.TipWindowStyle.OnlyOK, nil, nil);
end

---NetworkManager的异常捕获
function this.Exception(exceptionTipString)
    FDebugger.LogError("捕获了一个异常，错误信息：" .. exceptionTipString);

    TipWindowCtrl.ShowTipWindow(exceptionTipString, TipWindowCtrl.TipWindowStyle.OnlyOK, nil, nil);
end

---服务器关闭了与客户端的连接
function this.OnLogOutReceive(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgLoginMethodStringID.LogOutReceive, data);

    TipWindowCtrl.ShowTipWindow("该账号在另一设备上登陆。如果这不是您自己操作，请立即修改密码", TipWindowCtrl.TipWindowStyle.OnlyOK, nil, nil);
end