---商城商品列表请求
MsgMallGoodsRequestTable = {
    ---玩家GUID
    playerGUID,
    ---查询什么类型的商品
    selectGoodsType,
};

---商城商品列表响应
MsgMallGoodsReceiveTable = {
    ---查询的玩家GUID
    playerGUID,
    ---查询什么类型的商品
    selectGoodsType,
    ---商品查询结果。每项之间用‘|’分隔
    result,
};

---添加到购物车请求
MsgAddToShoppingCardRequestTable = {
    ---玩家GUID
    playerGUID,
    ---商品ID
    goodsID,
    ---添加数量
    amount,
};

---添加到购物车响应
MsgAddToShoppingCardReceiveTable = {
    ---添加结果
    state,
    ---商品ID
    goodsID,
};

---购物车列表请求
MsgShoppingCardRequestTable = {
    ---玩家GUID
    playerGUID,
};

---购物车列表响应
MsgShoppingCardReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---商品信息。每项之间用‘|’分隔
    result,
};

---更新购物车商品数量请求
MsgUpdateShoppingCardGoodsAmountRequestTable = {
    ---玩家GUID
    playerGUID,
    ---更新数量是增加还是减少
    updateType,
    ---商品ID
    goodsID,
    ---商品增加的数量
    goodsAddAmount,
};

---更新购物车商品数量响应
MsgUpdateShoppingCardGoodsAmountReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---商品ID
    goodsID,
    ---商品当前的数量
    goodsAmount,
};

---购物车结算请求
MsgSettlementRequestTable = {
    ---玩家GUID
    playerGUID,
};

---购物车结算响应
MsgSettlementReceiveTable = {
    ---玩家GUID
    playerGUID,
    ---结算结果
    state,
};

MsgMallGoodsRequestTable.__index = MsgMallGoodsRequestTable;
MsgMallGoodsReceiveTable.__index = MsgMallGoodsReceiveTable;
MsgAddToShoppingCardRequestTable.__index = MsgAddToShoppingCardRequestTable;
MsgAddToShoppingCardReceiveTable.__index = MsgAddToShoppingCardReceiveTable;
MsgShoppingCardRequestTable.__index = MsgShoppingCardRequestTable;
MsgShoppingCardReceiveTable.__index = MsgShoppingCardReceiveTable;
MsgUpdateShoppingCardGoodsAmountRequestTable.__index = MsgUpdateShoppingCardGoodsAmountRequestTable;
MsgUpdateShoppingCardGoodsAmountReceiveTable.__index = MsgUpdateShoppingCardGoodsAmountReceiveTable;
MsgSettlementRequestTable.__index = MsgSettlementRequestTable;
MsgSettlementReceiveTable.__index = MsgSettlementReceiveTable;

---创建 商城商品列表请求 协议对象
function MsgMallGoodsRequestTable:New()
    local o  = {};
    setmetatable(o, MsgMallGoodsRequestTable);

    return o;
end

---创建 商城商品列表响应 协议对象
function MsgMallGoodsReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgMallGoodsReceiveTable);

    return o;
end

---创建 添加到购物车请求 协议对象
function MsgAddToShoppingCardRequestTable:New()
    local o  = {};
    setmetatable(o, MsgAddToShoppingCardRequestTable);

    return o;
end

---创建 添加到购物车响应 协议对象
function MsgAddToShoppingCardReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgAddToShoppingCardReceiveTable);

    return o;
end

---创建 购物车列表请求 协议对象
function MsgShoppingCardRequestTable:New()
    local o  = {};
    setmetatable(o, MsgShoppingCardRequestTable);

    return o;
end

---创建 购物车列表响应 协议对象
function MsgShoppingCardReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgShoppingCardReceiveTable);

    return o;
end

---创建 更新购物车商品数量请求 协议对象
function MsgUpdateShoppingCardGoodsAmountRequestTable:New()
    local o  = {};
    setmetatable(o, MsgUpdateShoppingCardGoodsAmountRequestTable);

    return o;
end

---创建 更新购物车商品数量响应 协议对象
function MsgUpdateShoppingCardGoodsAmountReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgUpdateShoppingCardGoodsAmountReceiveTable);

    return o;
end

---创建 购物车结算请求 协议对象
function MsgSettlementRequestTable:New()
    local o  = {};
    setmetatable(o, MsgSettlementRequestTable);

    return o;
end

---创建 购物车结算响应 协议对象
function MsgSettlementReceiveTable:New()
    local o  = {};
    setmetatable(o, MsgSettlementReceiveTable);

    return o;
end

