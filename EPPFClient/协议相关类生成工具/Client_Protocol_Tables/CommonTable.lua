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

MsgSecretKeyRequestTable.__index = MsgSecretKeyRequestTable;
MsgSecretKeyReceiveTable.__index = MsgSecretKeyReceiveTable;
MsgHeartbeatRequestTable.__index = MsgHeartbeatRequestTable;
MsgHeartbeatReceiveTable.__index = MsgHeartbeatReceiveTable;

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

