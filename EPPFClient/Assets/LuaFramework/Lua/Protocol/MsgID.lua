---协议处理ID
MsgHandleID = {
    ---活动相关协议
    Activity = 6,
    ---广告相关的协议
    Ads = 8,
    ---通用协议
    Common = 0,
    ---登陆相关逻辑
    Login = 1,
    ---商城相关协议
    Mall = 5,
    ---翻转棋游戏相关协议
    OthelloGame = 7,
    ---排行榜相关协议
    Ranking = 4,
    ---游戏房间相关逻辑
    Room = 2,
    ---UNO游戏相关协议
    UNOGame = 3,
}

---活动相关协议 的处理方法ID
MsgActivityMethodID = {
    ---活动列表请求
    ActivityListRequest = 0,
    ---活动列表响应
    ActivityListReceive = 1,
}

---广告相关的协议 的处理方法ID
MsgAdsMethodID = {
    ---Unity广告观看完成请求
    UnityAdsWatchFinishRequest = 0,
    ---Unity广告观看完成响应
    UnityAdsWatchFinishReceive = 1,
}

---通用协议 的处理方法ID
MsgCommonMethodID = {
    ---获取密钥请求
    SecretKeyRequest = 0,
    ---获取密钥响应
    SecretKeyReceive = 1,
    ---心跳包请求
    HeartbeatRequest = 2,
    ---心跳包响应
    HeartbeatReceive = 3,
    ---跑马灯请求
    PaoMaDengRequest = 4,
    ---跑马灯响应
    PaoMaDengReceive = 5,
    ---更新金币请求
    UpdateGoldCoinRequest = 6,
    ---更新金币响应
    UpdateGoldCoinReceive = 7,
    ---查询个人信息请求
    InquirePersonalInfoRequest = 8,
    ---查询个人信息响应
    InquirePersonalInfoReceive = 9,
    ---版本号查询请求
    GameVersionRequest = 10,
    ---版本号查询响应
    GameVersionReceive = 11,
    ---聊天讲话的请求
    ChatTalkRequest = 12,
    ---聊天讲话的响应
    ChatTalkReceive = 13,
    ---全部默认头像名称请求
    AllDefaultAvatarRequest = 14,
    ---全部默认头像响应
    AllDefaultAvatarReceive = 15,
    ---更新个人信息请求。不更新玩家名称，玩家名称的修改是单独的消息
    UpdatePersonalInfoRequest = 16,
    ---更新个人信息响应。不更新玩家名称，玩家名称的修改是单独的消息
    UpdatePersonalInfoReceive = 17,
}

---登陆相关逻辑 的处理方法ID
MsgLoginMethodID = {
    ---注册请求
    RegisterRequest = 0,
    ---注册响应
    RegisterReceive = 1,
    ---登陆请求
    LoginRequest = 2,
    ---登陆响应
    LoginReceive = 3,
    ---踢掉线请求
    LogOutRequest = 4,
    ---踢掉线响应
    LogOutReceive = 5,
}

---商城相关协议 的处理方法ID
MsgMallMethodID = {
    ---商城商品列表请求
    MallGoodsRequest = 0,
    ---商城商品列表响应
    MallGoodsReceive = 1,
    ---添加到购物车请求
    AddToShoppingCardRequest = 2,
    ---添加到购物车响应
    AddToShoppingCardReceive = 3,
    ---购物车列表请求
    ShoppingCardRequest = 4,
    ---购物车列表响应
    ShoppingCardReceive = 5,
    ---更新购物车商品数量请求
    UpdateShoppingCardGoodsAmountRequest = 6,
    ---更新购物车商品数量响应
    UpdateShoppingCardGoodsAmountReceive = 7,
    ---购物车结算请求
    SettlementRequest = 8,
    ---购物车结算响应
    SettlementReceive = 9,
}

---翻转棋游戏相关协议 的处理方法ID
MsgOthelloGameMethodID = {
    ---轮到下一个玩家操作的请求
    TurnToNextPlayerRequest = 0,
    ---轮到下一个玩家操作的响应
    TurnToNextPlayerReceive = 1,
    ---放下一个棋子的请求
    PutDownPieceRequest = 2,
    ---放下一个棋子的响应
    PutDownPieceReceive = 3,
    ---断线重连请求
    ReconnectionRequest = 4,
    ---断线重连响应
    ReconnectionReceive = 5,
}

---排行榜相关协议 的处理方法ID
MsgRankingMethodID = {
    ---排行榜请求
    RankingRequest = 0,
    ---排行榜响应
    RankingReceive = 1,
}

---游戏房间相关逻辑 的处理方法ID
MsgRoomMethodID = {
    ---创建房间请求
    CreateRoomRequest = 0,
    ---创建房间响应
    CreateRoomReceive = 1,
    ---房间列表请求
    RoomListRequest = 2,
    ---房间列表响应
    RoomListReceive = 3,
    ---加入房间请求
    JoinRoomRequest = 4,
    ---加入房间响应
    JoinRoomReceive = 5,
    ---退出房间请求
    QuitRoomRequest = 6,
    ---退出房间响应
    QuitRoomReceive = 7,
    ---其他玩家加入房间请求
    OtherPlayerJoinRoomRequest = 8,
    ---其他玩家加入房间响应
    OtherPlayerJoinRoomReceive = 9,
    ---其他玩家退出房间请求
    OtherPlayerQuitRoomRequest = 10,
    ---其他玩家退出房间响应
    OtherPlayerQuitRoomReceive = 11,
    ---快速加入房间请求
    QuickJoinRoomRequest = 12,
    ---快速加入房间响应
    QuickJoinRoomReceive = 13,
    ---玩家准备请求
    ReadyRequest = 14,
    ---玩家准备响应
    ReadyReceive = 15,
    ---玩家取消准备请求
    CancelReadyRequest = 16,
    ---玩家取消准备响应
    CancelReadyReceive = 17,
    ---游戏开始请求
    StartGameRequest = 18,
    ---游戏开始响应
    StartGameReceive = 19,
    ---游戏结束请求
    GameOverRequest = 20,
    ---游戏结束响应
    GameOverReceive = 21,
    ---其他玩家准备请求
    OtherPlayerReadyRequest = 22,
    ---其他玩家准备响应
    OtherPlayerReadyReceive = 23,
    ---其他玩家取消准备请求
    OtherPlayerCancelReadyRequest = 24,
    ---其他玩家取消准备响应
    OtherPlayerCancelReadyReceive = 25,
}

---UNO游戏相关协议 的处理方法ID
MsgUNOGameMethodID = {
    ---发牌请求
    SendCardRequest = 0,
    ---发牌响应
    SendCardReceive = 1,
    ---出牌请求
    PutCardRequest = 2,
    ---出牌响应
    PutCardReceive = 3,
    ---轮到下一个玩家出牌的请求
    TurnToPutCardPlayerRequest = 4,
    ---轮到下一个玩家出牌的响应
    TurnToPutCardPlayerReceive = 5,
    ---特殊牌的逻辑请求
    SpecialCardRequest = 6,
    ---特殊牌的逻辑响应
    SpecialCardReceive = 7,
    ---断线重连请求
    ReconnectionRequest = 8,
    ---断线重连响应
    ReconnectionReceive = 9,
    ---洗弃牌堆请求
    MessUpDiscardHeapRequest = 10,
    ---洗弃牌堆响应
    MessUpDiscardHeapReceive = 11,
}

