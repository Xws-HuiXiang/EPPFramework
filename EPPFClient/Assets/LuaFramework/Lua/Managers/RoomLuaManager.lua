--- 这个类主要是派发房间内的通用协议 准备响应和取消准备响应等等。也包括了一些通用的值
RoomLuaManager = {};
local this = RoomLuaManager;

--- 开始游戏人数
RoomLuaManager.startGamePlayerAmount = 0;
--- 我的椅子ID（服务器的椅子ID，不是本地的）
RoomLuaManager.myChairID = 0;
--- 房间名称
RoomLuaManager.roomName = "";
--- 房间ID
RoomLuaManager.roomID = "";
--- 当前房间出牌倒计时
RoomLuaManager.outCardCountdownTime = -1;
--- 当前房间出牌倒计时的计时器
RoomLuaManager.outCardCountdownTimer = 0;

---发送准备请求
function this.SendReadyRequest()
    local msg = MsgReadyRequestTable:New();
    msg.playerGUID = GameLuaManager.PlayerInfo.GUID;

    local data = NetworkLuaManager.EncodeMsg(MsgRoomMethodStringID.ReadyRequest, msg);
    NetworkManager.Instance:SendMsgFromLua(MsgHandleID.Room, MsgRoomMethodID.ReadyRequest, data);
end

--- 准备响应
function this.OnReadyReceive(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgRoomMethodStringID.ReadyReceive, data);
    local tab = this.GetGameTableByGameType(msg.roomGameType);
    if(tab ~= nil)then
        if(tab.OnReadyReceive ~= nil and type(tab.OnReadyReceive) == "function")then
            tab.OnReadyReceive(msg.state);
        else
            --没有OnReadyReceive方法 或 类型不是function
            FDebugger.LogWarning("没有OnReadyReceive方法 或 类型不是function");
        end
    end
end

--- 发送取消准备请求
function this.SendCancelReadyRequest()
    local msg = MsgCancelReadyRequestTable:New();
    msg.playerGUID = GameLuaManager.PlayerInfo.GUID;

    local data = NetworkLuaManager.EncodeMsg(MsgRoomMethodStringID.CancelReadyRequest, msg);
    NetworkManager.Instance:SendMsgFromLua(MsgHandleID.Room, MsgRoomMethodID.CancelReadyRequest, data);
end

--- 取消准备响应
function this.OnCancelReadyReceive(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgRoomMethodStringID.CancelReadyReceive, data);
    local tab = this.GetGameTableByGameType(msg.roomGameType);
    if(tab ~= nil)then
        if(tab.OnCancelReadyReceive ~= nil and type(tab.OnCancelReadyReceive) == "function")then
            tab.OnCancelReadyReceive(msg.state);
        else
            --没有OnCancelReadyReceive方法 或 类型不是function
            FDebugger.LogWarning("没有OnCancelReadyReceive方法 或 类型不是function");
        end
    end
end

--- 游戏开始响应
function this.OnStartGameReceive(data)
    FDebugger.Log("游戏开始");

    local msg = NetworkLuaManager.DecodeMsg(MsgRoomMethodStringID.StartGameReceive, data);
    local tab = this.GetGameTableByGameType(msg.roomGameType);
    if(tab ~= nil)then
        if(tab.OnStartGameReceive ~= nil and type(tab.OnStartGameReceive) == "function")then
            tab.OnStartGameReceive(msg.time, msg.startGameInfo);
        else
            --没有OnStartGameReceive方法 或 类型不是function
            FDebugger.LogWarning("没有OnStartGameReceive方法 或 类型不是function");
        end
    end
end

--- 退出房间的响应
function this.OnQuitRoomReceive(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgRoomMethodStringID.QuitRoomReceive, data);
    local tab = this.GetGameTableByGameType(msg.roomGameType);
    if(tab ~= nil)then
        if(tab.OnQuitRoomReceive ~= nil and type(tab.OnQuitRoomReceive) == "function")then
            tab.OnQuitRoomReceive(msg.state, msg.playerGUID);
        else
            --没有OnQuitRoomReceive方法 或 类型不是function
            FDebugger.LogWarning("没有OnQuitRoomReceive方法 或 类型不是function");
        end
    end
end

---其他玩家加入房间的请求回调
function this.OnOtherPlayerJoinRoomReceive(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgRoomMethodStringID.OtherPlayerJoinRoomReceive, data);
    local tab = this.GetGameTableByGameType(msg.roomGameType);
    if(tab ~= nil)then
        if(tab.OnOtherPlayerJoinRoomReceive ~= nil and type(tab.OnOtherPlayerJoinRoomReceive) == "function")then
            tab.OnOtherPlayerJoinRoomReceive(msg.playerName, msg.playerGUID, msg.avatarName, msg.playerScore, msg.playerChairID);
        else
            --没有OnOtherPlayerJoinRoomReceive方法 或 类型不是function
            FDebugger.LogWarning("没有OnOtherPlayerJoinRoomReceive方法 或 类型不是function");
        end
    end
end

---其他玩家退出房间的响应
function this.OnOtherPlayerQuitUNORoomReceive(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgRoomMethodStringID.OtherPlayerQuitRoomReceive, data);
    local tab = this.GetGameTableByGameType(msg.roomGameType);
    if(tab ~= nil)then
        if(tab.OnOtherPlayerQuitRoomReceive ~= nil and type(tab.OnOtherPlayerQuitRoomReceive) == "function")then
            tab.OnOtherPlayerQuitRoomReceive(msg.chairID, msg.playerGUID);
        else
            --没有OnOtherPlayerQuitRoomReceive方法 或 类型不是function
            FDebugger.LogWarning("没有OnOtherPlayerQuitRoomReceive方法 或 类型不是function");
        end
    end
end

---游戏结束响应
function this.OnGameOverReceive(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgRoomMethodStringID.GameOverReceive, data);
    local tab = this.GetGameTableByGameType(msg.roomGameType);
    if(tab ~= nil)then
        if(tab.GameOverReceive ~= nil and type(tab.GameOverReceive) == "function")then
            tab.OnGameOverReceive(msg.winPlayer, msg.roomName, msg.roomID, msg.time, msg.playerCount, msg.playerNameList, msg.playerIDList, msg.changeGoldCoinList);
        else
            --没有GameOverReceive方法 或 类型不是function
            FDebugger.LogWarning("没有GameOverReceive方法 或 类型不是function");
        end
    end
end

---其他玩家准备响应
function this.OnOtherPlayerReadyReceive(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgRoomMethodStringID.OtherPlayerReadyReceive, data);
    local tab = this.GetGameTableByGameType(msg.roomGameType);
    if(tab ~= nil)then
        if(tab.OnOtherPlayerReadyReceive ~= nil and type(tab.OnOtherPlayerReadyReceive) == "function")then
            tab.OnOtherPlayerReadyReceive(msg.playerGUID, msg.chairID);
        else
            --没有OnOtherPlayerReadyReceive方法 或 类型不是function
            FDebugger.LogWarning("没有OnOtherPlayerReadyReceive方法 或 类型不是function");
        end
    end
end

---其他玩家取消准备响应
function this.OnOtherPlayerCancelReadyReceive(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgRoomMethodStringID.OtherPlayerCancelReadyReceive, data);
    local tab = this.GetGameTableByGameType(msg.roomGameType);
    if(tab ~= nil)then
        if(tab.OnOtherPlayerCancelReadyReceive ~= nil and type(tab.OnOtherPlayerCancelReadyReceive) == "function")then
            tab.OnOtherPlayerCancelReadyReceive(msg.playerGUID, msg.chairID);
        else
            --没有OnOtherPlayerCancelReadyReceive方法 或 类型不是function
            FDebugger.LogWarning("没有OnOtherPlayerCancelReadyReceive方法 或 类型不是function");
        end
    end
end

--- 根据游戏ID获取游戏对应的lua表
--- @param gameType GameType pb的枚举类型，游戏类型的枚举
--- @returns 游戏类型对应的lua table
function this.GetGameTableByGameType(gameType)
    local enumFullName = NetworkLuaManager.CombineFullPbName("GameType");
    local channelIndex = pb.enum(enumFullName, gameType);

    local res;
    if(channelIndex == 0)then
        --UNO
        res = UNOGameCtrl;
    elseif channelIndex == 1 then
        --翻转棋（黑白棋）
        res = OthelloGameCtrl;
    else
        --未处理的类型
        res = {};

        FDebugger.LogError("根据游戏ID获取游戏对应的lua表方法中，未处理的游戏类型：" .. channelIndex);
    end

    return res;
end

---转换其他人的服务器座位ID为本地座位ID
function this.SwitchChairID(chairID)
    --转换ID = （最大人数 - 我的ID + 目标ID） % 最大人数
    return (RoomLuaManager.startGamePlayerAmount - RoomLuaManager.myChairID + chairID) % RoomLuaManager.startGamePlayerAmount;
end