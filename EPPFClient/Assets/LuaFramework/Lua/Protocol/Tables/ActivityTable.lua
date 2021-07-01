---活动列表请求
MsgActivityListRequestTable = {
    ---玩家GUID
    playerGUID,
};

---活动列表响应
MsgActivityListReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---活动内容
    result,
};

MsgActivityListRequestTable.__index = MsgActivityListRequestTable;
MsgActivityListReceiveTable.__index = MsgActivityListReceiveTable;

---创建 活动列表请求 协议对象
function MsgActivityListRequestTable:New()
    local o  = {};
    setmetatable(o, MsgActivityListRequestTable);

    return o;
end

---创建 活动列表响应 协议对象
function MsgActivityListReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgActivityListReceiveTable);

    return o;
end

