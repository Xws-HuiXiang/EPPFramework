---轮到下一个玩家操作的请求
MsgTurnToNextPlayerRequestTable = {
};

---轮到下一个玩家操作的响应
MsgTurnToNextPlayerReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---要出牌玩家的椅子ID
    chairID,
    ---可以选择的位置信息串
    activePosInfo,
};

---放下一个棋子的请求
MsgPutDownPieceRequestTable = {
    ---放下棋子的玩家GUID
    playerGUID,
    ---放下棋子位置的x坐标
    x,
    ---放下棋子位置的y坐标
    y,
};

---放下一个棋子的响应
MsgPutDownPieceReceiveTable = {
    ---放下棋子的玩家GUID
    playerGUID,
    ---放下棋子的结果
    state,
    ---放下棋子位置的x坐标
    x,
    ---放下棋子位置的y坐标
    y,
    ---放下棋子的颜色
    color,
    ---需要翻转的棋子信息串
    translatePiecesInfo,
    ---当前黑色棋子的数量
    blackPieceCount,
    ---当前白色棋子的数量
    whitePieceCount,
};

---断线重连请求
MsgReconnectionRequestTable = {
};

---断线重连响应
MsgReconnectionReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---玩家座位ID
    playerChairID,
    ---当前房间内的玩家数量
    playerAmount,
    ---房间名称
    roomName,
    ---房间ID
    roomID,
    ---出牌倒计时
    outCardCountdown,
    ---当前房间内的玩家名称列表
    playerNameList,
    ---当前房间内的玩家积分列表
    playerScoreList,
    ---当前房间内的玩家头像图片名称列表
    playerAvatarList,
    ---棋盘棋子的信息串
    piecesInfoString,
    ---要出牌玩家的椅子ID
    activeChairID,
    ---可以选择的位置信息串
    activePosInfo,
};

MsgTurnToNextPlayerRequestTable.__index = MsgTurnToNextPlayerRequestTable;
MsgTurnToNextPlayerReceiveTable.__index = MsgTurnToNextPlayerReceiveTable;
MsgPutDownPieceRequestTable.__index = MsgPutDownPieceRequestTable;
MsgPutDownPieceReceiveTable.__index = MsgPutDownPieceReceiveTable;
MsgReconnectionRequestTable.__index = MsgReconnectionRequestTable;
MsgReconnectionReceiveTable.__index = MsgReconnectionReceiveTable;

---创建 轮到下一个玩家操作的请求 协议对象
function MsgTurnToNextPlayerRequestTable:New()
    local o  = {};
    setmetatable(o, MsgTurnToNextPlayerRequestTable);

    return o;
end

---创建 轮到下一个玩家操作的响应 协议对象
function MsgTurnToNextPlayerReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgTurnToNextPlayerReceiveTable);

    return o;
end

---创建 放下一个棋子的请求 协议对象
function MsgPutDownPieceRequestTable:New()
    local o  = {};
    setmetatable(o, MsgPutDownPieceRequestTable);

    return o;
end

---创建 放下一个棋子的响应 协议对象
function MsgPutDownPieceReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgPutDownPieceReceiveTable);

    return o;
end

---创建 断线重连请求 协议对象
function MsgReconnectionRequestTable:New()
    local o  = {};
    setmetatable(o, MsgReconnectionRequestTable);

    return o;
end

---创建 断线重连响应 协议对象
function MsgReconnectionReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgReconnectionReceiveTable);

    return o;
end

