---发牌请求
MsgSendCardRequestTable = {
    ---玩家GUID
    playerGUID,
};

---发牌响应
MsgSendCardReceiveTable = {
    ---牌的类型
    cardType,
    ---牌的颜色
    cardColor,
};

---出牌请求
MsgPutCardRequestTable = {
    ---玩家GUID
    playerGUID,
    ---牌的类型
    cardType,
    ---牌的颜色
    cardColor,
};

---出牌响应
MsgPutCardReceiveTable = {
    ---出牌结果
    state,
    ---玩家GUID
    playerGUID,
    ---牌的类型
    cardType,
    ---牌的颜色
    cardColor,
};

---轮到下一个玩家出牌的请求
MsgTurnToPutCardPlayerRequestTable = {
    ---玩家GUID
    playerGUID,
};

---轮到下一个玩家出牌的响应
MsgTurnToPutCardPlayerReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---要出牌玩家的椅子ID
    chairID,
    ---牌的类型
    cardType,
    ---牌的颜色
    cardColor,
    ---如果指示牌为功能牌，则客户端是否需要执行功能牌的功能，如果不执行功能则只判断颜色
    executeFunctionCard,
    ---上一个出牌的玩家的座位ID。游戏开始时为-1，即没有上一个玩家
    previousOutCardChairID,
    ---上一个出牌的玩家的手牌数量
    previousHandCardAmount,
    ---客户端中是否增加指引牌（如果摸牌则不加，其他玩家出牌则添加）
    isAddGuideCard,
    ---当前牌堆还有多少牌
    cardHeapCount,
    ---当前出牌方向
    OutCardDirection,
};

---特殊牌的逻辑请求
MsgSpecialCardRequestTable = {
};

---特殊牌的逻辑响应
MsgSpecialCardReceiveTable = {
    ---卡牌类型
    cardType,
    ---卡牌颜色
    cardColor,
    ---需要摸牌的数量
    getCardAmount,
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
    ---当前房间内的玩家手牌数量，对应数组下表为玩家的座位ID列表
    playerHandCardCountList,
    ---自己手牌类型的列表
    myHandCardTypeList,
    ---自己卡牌颜色的列表
    myHandCardColorList,
};

---洗弃牌堆请求
MsgMessUpDiscardHeapRequestTable = {
};

---洗弃牌堆响应
MsgMessUpDiscardHeapReceiveTable = {
    ---时间戳
    time,
};

MsgSendCardRequestTable.__index = MsgSendCardRequestTable;
MsgSendCardReceiveTable.__index = MsgSendCardReceiveTable;
MsgPutCardRequestTable.__index = MsgPutCardRequestTable;
MsgPutCardReceiveTable.__index = MsgPutCardReceiveTable;
MsgTurnToPutCardPlayerRequestTable.__index = MsgTurnToPutCardPlayerRequestTable;
MsgTurnToPutCardPlayerReceiveTable.__index = MsgTurnToPutCardPlayerReceiveTable;
MsgSpecialCardRequestTable.__index = MsgSpecialCardRequestTable;
MsgSpecialCardReceiveTable.__index = MsgSpecialCardReceiveTable;
MsgReconnectionRequestTable.__index = MsgReconnectionRequestTable;
MsgReconnectionReceiveTable.__index = MsgReconnectionReceiveTable;
MsgMessUpDiscardHeapRequestTable.__index = MsgMessUpDiscardHeapRequestTable;
MsgMessUpDiscardHeapReceiveTable.__index = MsgMessUpDiscardHeapReceiveTable;

---创建 发牌请求 协议对象
function MsgSendCardRequestTable:New()
    local o  = {};
    setmetatable(o, MsgSendCardRequestTable);

    return o;
end

---创建 发牌响应 协议对象
function MsgSendCardReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgSendCardReceiveTable);

    return o;
end

---创建 出牌请求 协议对象
function MsgPutCardRequestTable:New()
    local o  = {};
    setmetatable(o, MsgPutCardRequestTable);

    return o;
end

---创建 出牌响应 协议对象
function MsgPutCardReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgPutCardReceiveTable);

    return o;
end

---创建 轮到下一个玩家出牌的请求 协议对象
function MsgTurnToPutCardPlayerRequestTable:New()
    local o  = {};
    setmetatable(o, MsgTurnToPutCardPlayerRequestTable);

    return o;
end

---创建 轮到下一个玩家出牌的响应 协议对象
function MsgTurnToPutCardPlayerReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgTurnToPutCardPlayerReceiveTable);

    return o;
end

---创建 特殊牌的逻辑请求 协议对象
function MsgSpecialCardRequestTable:New()
    local o  = {};
    setmetatable(o, MsgSpecialCardRequestTable);

    return o;
end

---创建 特殊牌的逻辑响应 协议对象
function MsgSpecialCardReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgSpecialCardReceiveTable);

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

---创建 洗弃牌堆请求 协议对象
function MsgMessUpDiscardHeapRequestTable:New()
    local o  = {};
    setmetatable(o, MsgMessUpDiscardHeapRequestTable);

    return o;
end

---创建 洗弃牌堆响应 协议对象
function MsgMessUpDiscardHeapReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgMessUpDiscardHeapReceiveTable);

    return o;
end

