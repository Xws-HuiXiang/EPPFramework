---排行榜请求
MsgRankingRequestTable = {
    ---玩家GUID
    playerGUID,
    ---查询什么类型的排行榜
    selectRankingType,
};

---排行榜响应
MsgRankingReceiveTable = {
    ---查询的玩家GUID
    playerGUID,
    ---查询什么类型的排行榜
    selectRankingType,
    ---排行榜查询结果。每项之间用‘|’分隔
    result,
};

MsgRankingRequestTable.__index = MsgRankingRequestTable;
MsgRankingReceiveTable.__index = MsgRankingReceiveTable;

---创建 排行榜请求 协议对象
function MsgRankingRequestTable:New()
    local o  = {};
    setmetatable(o, MsgRankingRequestTable);

    return o;
end

---创建 排行榜响应 协议对象
function MsgRankingReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgRankingReceiveTable);

    return o;
end

