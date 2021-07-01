---注册请求
MsgRegisterRequestTable = {
    ---用户名
    username,
    ---密码
    password,
    ---邮箱
    email,
    ---手机号
    phoneNumber,
};

---注册响应
MsgRegisterReceiveTable = {
    ---注册结果
    state,
};

---登陆请求
MsgLoginRequestTable = {
    ---登陆类型
    loginType,
    ---用户名 或 token
    username,
    ---密码
    password,
};

---登陆响应
MsgLoginReceiveTable = {
    ---登陆结果
    state,
    ---玩家的GUID
    guid,
    ---玩家游戏昵称
    username,
    ---玩家积分
    score,
    ---头像图片的名称
    avatar,
    ---加密后的玩家密码，用户下次自动登陆
    password,
};

---踢掉线请求
MsgLogOutRequestTable = {
};

---踢掉线响应
MsgLogOutReceiveTable = {
    ---玩家GUID
    guid,
};

MsgRegisterRequestTable.__index = MsgRegisterRequestTable;
MsgRegisterReceiveTable.__index = MsgRegisterReceiveTable;
MsgLoginRequestTable.__index = MsgLoginRequestTable;
MsgLoginReceiveTable.__index = MsgLoginReceiveTable;
MsgLogOutRequestTable.__index = MsgLogOutRequestTable;
MsgLogOutReceiveTable.__index = MsgLogOutReceiveTable;

---创建 注册请求 协议对象
function MsgRegisterRequestTable:New()
    local o  = {};
    setmetatable(o, MsgRegisterRequestTable);

    return o;
end

---创建 注册响应 协议对象
function MsgRegisterReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgRegisterReceiveTable);

    return o;
end

---创建 登陆请求 协议对象
function MsgLoginRequestTable:New()
    local o  = {};
    setmetatable(o, MsgLoginRequestTable);

    return o;
end

---创建 登陆响应 协议对象
function MsgLoginReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgLoginReceiveTable);

    return o;
end

---创建 踢掉线请求 协议对象
function MsgLogOutRequestTable:New()
    local o  = {};
    setmetatable(o, MsgLogOutRequestTable);

    return o;
end

---创建 踢掉线响应 协议对象
function MsgLogOutReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgLogOutReceiveTable);

    return o;
end

