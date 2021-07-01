---创建房间请求
MsgCreateRoomRequestTable = {
    ---玩家guid
    playerGUID,
    ---房间名称
    roomName,
    ---房间密码
    roomPassword,
    ---游戏类型ID
    gameTypeID,
    ---创建房间时，指定的游戏规则串。配置项顺序与客户端中表顺序一致；每个大项规则之间用‘|’分割，如果是多选则选项之间用‘#’分割
    gameRule,
};

---创建房间响应
MsgCreateRoomReceiveTable = {
    ---创建房间结果
    state,
    ---创建房间的玩家guid
    playerGUID,
    ---房间名称
    roomName,
    ---房间ID
    roomID,
};

---房间列表请求
MsgRoomListRequestTable = {
    ---发起查询房间列表的玩家guid
    playerGUID,
    ---游戏类型
    gameType,
};

---房间列表响应
MsgRoomListReceiveTable = {
    ---发起查询房间列表的玩家guid
    playerGUID,
    ---房间信息列表
    roomInfoList,
    ---房间是否有锁的列表
    roomHasLockList,
    ---游戏类型
    gameType,
};

---加入房间请求
MsgJoinRoomRequestTable = {
    ---玩家guid
    playerGUID,
    ---房间ID
    roomID,
    ---房间密码
    roomPassword,
};

---加入房间响应
MsgJoinRoomReceiveTable = {
    ---加入UNO房间的结果
    state,
    ---游戏类型ID
    gameTypeID,
    ---房间ID
    roomID,
    ---房间名称
    roomName,
    ---开始游戏需要的玩家数量
    playerAmount,
    ---椅子ID
    playerChairID,
    ---出牌倒计时
    outCardCountdown,
};

---退出房间请求
MsgQuitRoomRequestTable = {
    ---玩家guid
    playerGUID,
    ---房间ID
    roomID,
};

---退出房间响应
MsgQuitRoomReceiveTable = {
    ---退出UNO房间的结果
    state,
    ---退出房间的玩家ID
    playerGUID,
    ---游戏类型ID
    roomGameType,
};

---其他玩家加入房间请求
MsgOtherPlayerJoinRoomRequestTable = {
    ---玩家guid
    playerGUID,
    ---房间ID
    roomID,
};

---其他玩家加入房间响应
MsgOtherPlayerJoinRoomReceiveTable = {
    ---加入房间的玩家名称
    playerName,
    ---加入房间的玩家GUID
    playerGUID,
    ---加入的玩家头像名称
    avatarName,
    ---加入房间的玩家分数（金币）
    playerScore,
    ---加入的玩家座位号
    playerChairID,
    ---游戏类型ID
    roomGameType,
};

---其他玩家退出房间请求
MsgOtherPlayerQuitRoomRequestTable = {
};

---其他玩家退出房间响应
MsgOtherPlayerQuitRoomReceiveTable = {
    ---退出房间的玩家椅子ID
    chairID,
    ---退出UNO房间的玩家GUID
    playerGUID,
    ---游戏类型ID
    roomGameType,
};

---快速加入房间请求
MsgQuickJoinRoomRequestTable = {
    ---玩家GUID
    playerGUID,
};

---快速加入房间响应
MsgQuickJoinRoomReceiveTable = {
    ---快速加入房间结果
    state,
};

---玩家准备请求
MsgReadyRequestTable = {
    ---玩家GUID
    playerGUID,
};

---玩家准备响应
MsgReadyReceiveTable = {
    ---准备请求的结果
    state,
    ---房间游戏类型
    roomGameType,
};

---玩家取消准备请求
MsgCancelReadyRequestTable = {
    ---玩家GUID
    playerGUID,
};

---玩家取消准备响应
MsgCancelReadyReceiveTable = {
    ---取消准备请求的结果
    state,
    ---房间游戏类型
    roomGameType,
};

---游戏开始请求
MsgStartGameRequestTable = {
};

---游戏开始响应
MsgStartGameReceiveTable = {
    ---游戏开始时的时间
    time,
    ---游戏开始时提供的参数串
    startGameInfo,
    ---房间游戏类型
    roomGameType,
};

---游戏结束请求
MsgGameOverRequestTable = {
};

---游戏结束响应
MsgGameOverReceiveTable = {
    ---玩家GUID
    winPlayer,
    ---房间名称
    roomName,
    ---房间ID
    roomID,
    ---对战时间
    time,
    ---房间内玩家数量
    playerCount,
    ---玩家名称列表
    playerNameList,
    ---玩家ID列表
    playerIDList,
    ---金币变动列表
    changeGoldCoinList,
    ---房间游戏类型
    roomGameType,
};

---其他玩家准备请求
MsgOtherPlayerReadyRequestTable = {
};

---其他玩家准备响应
MsgOtherPlayerReadyReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---玩家座位ID
    chairID,
    ---房间游戏类型
    roomGameType,
};

---其他玩家取消准备请求
MsgOtherPlayerCancelReadyRequestTable = {
};

---其他玩家取消准备响应
MsgOtherPlayerCancelReadyReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---玩家座位ID
    chairID,
    ---房间游戏类型
    roomGameType,
};

MsgCreateRoomRequestTable.__index = MsgCreateRoomRequestTable;
MsgCreateRoomReceiveTable.__index = MsgCreateRoomReceiveTable;
MsgRoomListRequestTable.__index = MsgRoomListRequestTable;
MsgRoomListReceiveTable.__index = MsgRoomListReceiveTable;
MsgJoinRoomRequestTable.__index = MsgJoinRoomRequestTable;
MsgJoinRoomReceiveTable.__index = MsgJoinRoomReceiveTable;
MsgQuitRoomRequestTable.__index = MsgQuitRoomRequestTable;
MsgQuitRoomReceiveTable.__index = MsgQuitRoomReceiveTable;
MsgOtherPlayerJoinRoomRequestTable.__index = MsgOtherPlayerJoinRoomRequestTable;
MsgOtherPlayerJoinRoomReceiveTable.__index = MsgOtherPlayerJoinRoomReceiveTable;
MsgOtherPlayerQuitRoomRequestTable.__index = MsgOtherPlayerQuitRoomRequestTable;
MsgOtherPlayerQuitRoomReceiveTable.__index = MsgOtherPlayerQuitRoomReceiveTable;
MsgQuickJoinRoomRequestTable.__index = MsgQuickJoinRoomRequestTable;
MsgQuickJoinRoomReceiveTable.__index = MsgQuickJoinRoomReceiveTable;
MsgReadyRequestTable.__index = MsgReadyRequestTable;
MsgReadyReceiveTable.__index = MsgReadyReceiveTable;
MsgCancelReadyRequestTable.__index = MsgCancelReadyRequestTable;
MsgCancelReadyReceiveTable.__index = MsgCancelReadyReceiveTable;
MsgStartGameRequestTable.__index = MsgStartGameRequestTable;
MsgStartGameReceiveTable.__index = MsgStartGameReceiveTable;
MsgGameOverRequestTable.__index = MsgGameOverRequestTable;
MsgGameOverReceiveTable.__index = MsgGameOverReceiveTable;
MsgOtherPlayerReadyRequestTable.__index = MsgOtherPlayerReadyRequestTable;
MsgOtherPlayerReadyReceiveTable.__index = MsgOtherPlayerReadyReceiveTable;
MsgOtherPlayerCancelReadyRequestTable.__index = MsgOtherPlayerCancelReadyRequestTable;
MsgOtherPlayerCancelReadyReceiveTable.__index = MsgOtherPlayerCancelReadyReceiveTable;

---创建 创建房间请求 协议对象
function MsgCreateRoomRequestTable:New()
    local o  = {};
    setmetatable(o, MsgCreateRoomRequestTable);

    return o;
end

---创建 创建房间响应 协议对象
function MsgCreateRoomReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgCreateRoomReceiveTable);

    return o;
end

---创建 房间列表请求 协议对象
function MsgRoomListRequestTable:New()
    local o  = {};
    setmetatable(o, MsgRoomListRequestTable);

    return o;
end

---创建 房间列表响应 协议对象
function MsgRoomListReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgRoomListReceiveTable);

    return o;
end

---创建 加入房间请求 协议对象
function MsgJoinRoomRequestTable:New()
    local o  = {};
    setmetatable(o, MsgJoinRoomRequestTable);

    return o;
end

---创建 加入房间响应 协议对象
function MsgJoinRoomReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgJoinRoomReceiveTable);

    return o;
end

---创建 退出房间请求 协议对象
function MsgQuitRoomRequestTable:New()
    local o  = {};
    setmetatable(o, MsgQuitRoomRequestTable);

    return o;
end

---创建 退出房间响应 协议对象
function MsgQuitRoomReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgQuitRoomReceiveTable);

    return o;
end

---创建 其他玩家加入房间请求 协议对象
function MsgOtherPlayerJoinRoomRequestTable:New()
    local o  = {};
    setmetatable(o, MsgOtherPlayerJoinRoomRequestTable);

    return o;
end

---创建 其他玩家加入房间响应 协议对象
function MsgOtherPlayerJoinRoomReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgOtherPlayerJoinRoomReceiveTable);

    return o;
end

---创建 其他玩家退出房间请求 协议对象
function MsgOtherPlayerQuitRoomRequestTable:New()
    local o  = {};
    setmetatable(o, MsgOtherPlayerQuitRoomRequestTable);

    return o;
end

---创建 其他玩家退出房间响应 协议对象
function MsgOtherPlayerQuitRoomReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgOtherPlayerQuitRoomReceiveTable);

    return o;
end

---创建 快速加入房间请求 协议对象
function MsgQuickJoinRoomRequestTable:New()
    local o  = {};
    setmetatable(o, MsgQuickJoinRoomRequestTable);

    return o;
end

---创建 快速加入房间响应 协议对象
function MsgQuickJoinRoomReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgQuickJoinRoomReceiveTable);

    return o;
end

---创建 玩家准备请求 协议对象
function MsgReadyRequestTable:New()
    local o  = {};
    setmetatable(o, MsgReadyRequestTable);

    return o;
end

---创建 玩家准备响应 协议对象
function MsgReadyReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgReadyReceiveTable);

    return o;
end

---创建 玩家取消准备请求 协议对象
function MsgCancelReadyRequestTable:New()
    local o  = {};
    setmetatable(o, MsgCancelReadyRequestTable);

    return o;
end

---创建 玩家取消准备响应 协议对象
function MsgCancelReadyReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgCancelReadyReceiveTable);

    return o;
end

---创建 游戏开始请求 协议对象
function MsgStartGameRequestTable:New()
    local o  = {};
    setmetatable(o, MsgStartGameRequestTable);

    return o;
end

---创建 游戏开始响应 协议对象
function MsgStartGameReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgStartGameReceiveTable);

    return o;
end

---创建 游戏结束请求 协议对象
function MsgGameOverRequestTable:New()
    local o  = {};
    setmetatable(o, MsgGameOverRequestTable);

    return o;
end

---创建 游戏结束响应 协议对象
function MsgGameOverReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgGameOverReceiveTable);

    return o;
end

---创建 其他玩家准备请求 协议对象
function MsgOtherPlayerReadyRequestTable:New()
    local o  = {};
    setmetatable(o, MsgOtherPlayerReadyRequestTable);

    return o;
end

---创建 其他玩家准备响应 协议对象
function MsgOtherPlayerReadyReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgOtherPlayerReadyReceiveTable);

    return o;
end

---创建 其他玩家取消准备请求 协议对象
function MsgOtherPlayerCancelReadyRequestTable:New()
    local o  = {};
    setmetatable(o, MsgOtherPlayerCancelReadyRequestTable);

    return o;
end

---创建 其他玩家取消准备响应 协议对象
function MsgOtherPlayerCancelReadyReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgOtherPlayerCancelReadyReceiveTable);

    return o;
end

