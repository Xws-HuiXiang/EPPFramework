---获取密钥请求
MsgSecretKeyRequestTable = {
    ---客户端的消息公钥
    publicKey,
};

---获取密钥响应
MsgSecretKeyReceiveTable = {
    ---密钥内容
    secretKey,
    ---服务端消息公钥
    msgPublicKey,
};

---心跳包请求
MsgHeartbeatRequestTable = {
    ---时间戳
    pingTime,
};

---心跳包响应
MsgHeartbeatReceiveTable = {
};

---跑马灯请求
MsgPaoMaDengRequestTable = {
    ---玩家GUID
    playerGUID,
};

---跑马灯响应
MsgPaoMaDengReceiveTable = {
    ---跑马灯内容
    content,
    ---跑马灯滚动次数
    count,
};

---更新金币请求
MsgUpdateGoldCoinRequestTable = {
    ---玩家GUID
    playerGUID,
};

---更新金币响应
MsgUpdateGoldCoinReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---金币数量
    goldCoin,
};

---查询个人信息请求
MsgInquirePersonalInfoRequestTable = {
    ---要查询的玩家GUID
    playerGUID,
};

---查询个人信息响应
MsgInquirePersonalInfoReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---玩家昵称
    playerName,
    ---个性签名
    signature,
    ---总场次
    total,
    ---胜利场次
    win,
    ---胜率
    winRate,
    ---最佳战绩
    best,
    ---玩家头像图片的名称
    avatarName,
};

---版本号查询请求
MsgGameVersionRequestTable = {
    ---玩家GUID
    playerGUID,
};

---版本号查询响应
MsgGameVersionReceiveTable = {
    ---版本号
    version,
};

---聊天讲话的请求
MsgChatTalkRequestTable = {
    ---玩家GUID
    playerGUID,
    ---聊天的频道
    channel,
    ---发送的聊天内容
    text,
};

---聊天讲话的响应
MsgChatTalkReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---玩家名称
    playerName,
    ---聊天的频道
    channel,
    ---聊天内容
    text,
};

---全部默认头像名称请求
MsgAllDefaultAvatarRequestTable = {
    ---玩家GUID
    playerGUID,
};

---全部默认头像响应
MsgAllDefaultAvatarReceiveTable = {
    ---默认头像名称列表
    defaultAvatarList,
};

---更新个人信息请求。不更新玩家名称，玩家名称的修改是单独的消息
MsgUpdatePersonalInfoRequestTable = {
    ---玩家GUID
    playerGUID,
    ---玩家头像名称
    playerAvatarName,
    ---玩家个性签名
    playerSignature,
};

---更新个人信息响应。不更新玩家名称，玩家名称的修改是单独的消息
MsgUpdatePersonalInfoReceiveTable = {
    ---更新结果
    state,
    ---玩家头像名称
    playerAvatarName,
};

MsgSecretKeyRequestTable.__index = MsgSecretKeyRequestTable;
MsgSecretKeyReceiveTable.__index = MsgSecretKeyReceiveTable;
MsgHeartbeatRequestTable.__index = MsgHeartbeatRequestTable;
MsgHeartbeatReceiveTable.__index = MsgHeartbeatReceiveTable;
MsgPaoMaDengRequestTable.__index = MsgPaoMaDengRequestTable;
MsgPaoMaDengReceiveTable.__index = MsgPaoMaDengReceiveTable;
MsgUpdateGoldCoinRequestTable.__index = MsgUpdateGoldCoinRequestTable;
MsgUpdateGoldCoinReceiveTable.__index = MsgUpdateGoldCoinReceiveTable;
MsgInquirePersonalInfoRequestTable.__index = MsgInquirePersonalInfoRequestTable;
MsgInquirePersonalInfoReceiveTable.__index = MsgInquirePersonalInfoReceiveTable;
MsgGameVersionRequestTable.__index = MsgGameVersionRequestTable;
MsgGameVersionReceiveTable.__index = MsgGameVersionReceiveTable;
MsgChatTalkRequestTable.__index = MsgChatTalkRequestTable;
MsgChatTalkReceiveTable.__index = MsgChatTalkReceiveTable;
MsgAllDefaultAvatarRequestTable.__index = MsgAllDefaultAvatarRequestTable;
MsgAllDefaultAvatarReceiveTable.__index = MsgAllDefaultAvatarReceiveTable;
MsgUpdatePersonalInfoRequestTable.__index = MsgUpdatePersonalInfoRequestTable;
MsgUpdatePersonalInfoReceiveTable.__index = MsgUpdatePersonalInfoReceiveTable;

---创建 获取密钥请求 协议对象
function MsgSecretKeyRequestTable:New()
    local o  = {};
    setmetatable(o, MsgSecretKeyRequestTable);

    return o;
end

---创建 获取密钥响应 协议对象
function MsgSecretKeyReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgSecretKeyReceiveTable);

    return o;
end

---创建 心跳包请求 协议对象
function MsgHeartbeatRequestTable:New()
    local o  = {};
    setmetatable(o, MsgHeartbeatRequestTable);

    return o;
end

---创建 心跳包响应 协议对象
function MsgHeartbeatReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgHeartbeatReceiveTable);

    return o;
end

---创建 跑马灯请求 协议对象
function MsgPaoMaDengRequestTable:New()
    local o  = {};
    setmetatable(o, MsgPaoMaDengRequestTable);

    return o;
end

---创建 跑马灯响应 协议对象
function MsgPaoMaDengReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgPaoMaDengReceiveTable);

    return o;
end

---创建 更新金币请求 协议对象
function MsgUpdateGoldCoinRequestTable:New()
    local o  = {};
    setmetatable(o, MsgUpdateGoldCoinRequestTable);

    return o;
end

---创建 更新金币响应 协议对象
function MsgUpdateGoldCoinReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgUpdateGoldCoinReceiveTable);

    return o;
end

---创建 查询个人信息请求 协议对象
function MsgInquirePersonalInfoRequestTable:New()
    local o  = {};
    setmetatable(o, MsgInquirePersonalInfoRequestTable);

    return o;
end

---创建 查询个人信息响应 协议对象
function MsgInquirePersonalInfoReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgInquirePersonalInfoReceiveTable);

    return o;
end

---创建 版本号查询请求 协议对象
function MsgGameVersionRequestTable:New()
    local o  = {};
    setmetatable(o, MsgGameVersionRequestTable);

    return o;
end

---创建 版本号查询响应 协议对象
function MsgGameVersionReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgGameVersionReceiveTable);

    return o;
end

---创建 聊天讲话的请求 协议对象
function MsgChatTalkRequestTable:New()
    local o  = {};
    setmetatable(o, MsgChatTalkRequestTable);

    return o;
end

---创建 聊天讲话的响应 协议对象
function MsgChatTalkReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgChatTalkReceiveTable);

    return o;
end

---创建 全部默认头像名称请求 协议对象
function MsgAllDefaultAvatarRequestTable:New()
    local o  = {};
    setmetatable(o, MsgAllDefaultAvatarRequestTable);

    return o;
end

---创建 全部默认头像响应 协议对象
function MsgAllDefaultAvatarReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgAllDefaultAvatarReceiveTable);

    return o;
end

---创建 更新个人信息请求。不更新玩家名称，玩家名称的修改是单独的消息 协议对象
function MsgUpdatePersonalInfoRequestTable:New()
    local o  = {};
    setmetatable(o, MsgUpdatePersonalInfoRequestTable);

    return o;
end

---创建 更新个人信息响应。不更新玩家名称，玩家名称的修改是单独的消息 协议对象
function MsgUpdatePersonalInfoReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgUpdatePersonalInfoReceiveTable);

    return o;
end

