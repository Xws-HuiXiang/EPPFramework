---协议处理ID
MsgHandleStringID = {
    ---活动相关协议
    Activity = "Activity",
    ---广告相关的协议
    Ads = "Ads",
    ---通用协议
    Common = "Common",
    ---登陆相关逻辑
    Login = "Login",
    ---商城相关协议
    Mall = "Mall",
    ---翻转棋游戏相关协议
    OthelloGame = "OthelloGame",
    ---排行榜相关协议
    Ranking = "Ranking",
    ---游戏房间相关逻辑
    Room = "Room",
    ---UNO游戏相关协议
    UNOGame = "UNOGame",
}

---活动相关协议 的处理方法ID
MsgActivityMethodStringID = {
    ---活动列表请求
    ActivityListRequest = "ActivityListRequest",
    ---活动列表响应
    ActivityListReceive = "ActivityListReceive",
}

---广告相关的协议 的处理方法ID
MsgAdsMethodStringID = {
    ---Unity广告观看完成请求
    UnityAdsWatchFinishRequest = "UnityAdsWatchFinishRequest",
    ---Unity广告观看完成响应
    UnityAdsWatchFinishReceive = "UnityAdsWatchFinishReceive",
}

---通用协议 的处理方法ID
MsgCommonMethodStringID = {
    ---获取密钥请求
    SecretKeyRequest = "SecretKeyRequest",
    ---获取密钥响应
    SecretKeyReceive = "SecretKeyReceive",
    ---心跳包请求
    HeartbeatRequest = "HeartbeatRequest",
    ---心跳包响应
    HeartbeatReceive = "HeartbeatReceive",
    ---跑马灯请求
    PaoMaDengRequest = "PaoMaDengRequest",
    ---跑马灯响应
    PaoMaDengReceive = "PaoMaDengReceive",
    ---更新金币请求
    UpdateGoldCoinRequest = "UpdateGoldCoinRequest",
    ---更新金币响应
    UpdateGoldCoinReceive = "UpdateGoldCoinReceive",
    ---查询个人信息请求
    InquirePersonalInfoRequest = "InquirePersonalInfoRequest",
    ---查询个人信息响应
    InquirePersonalInfoReceive = "InquirePersonalInfoReceive",
    ---版本号查询请求
    GameVersionRequest = "GameVersionRequest",
    ---版本号查询响应
    GameVersionReceive = "GameVersionReceive",
    ---聊天讲话的请求
    ChatTalkRequest = "ChatTalkRequest",
    ---聊天讲话的响应
    ChatTalkReceive = "ChatTalkReceive",
    ---全部默认头像名称请求
    AllDefaultAvatarRequest = "AllDefaultAvatarRequest",
    ---全部默认头像响应
    AllDefaultAvatarReceive = "AllDefaultAvatarReceive",
    ---更新个人信息请求。不更新玩家名称，玩家名称的修改是单独的消息
    UpdatePersonalInfoRequest = "UpdatePersonalInfoRequest",
    ---更新个人信息响应。不更新玩家名称，玩家名称的修改是单独的消息
    UpdatePersonalInfoReceive = "UpdatePersonalInfoReceive",
}

---登陆相关逻辑 的处理方法ID
MsgLoginMethodStringID = {
    ---注册请求
    RegisterRequest = "RegisterRequest",
    ---注册响应
    RegisterReceive = "RegisterReceive",
    ---登陆请求
    LoginRequest = "LoginRequest",
    ---登陆响应
    LoginReceive = "LoginReceive",
    ---踢掉线请求
    LogOutRequest = "LogOutRequest",
    ---踢掉线响应
    LogOutReceive = "LogOutReceive",
}

---商城相关协议 的处理方法ID
MsgMallMethodStringID = {
    ---商城商品列表请求
    MallGoodsRequest = "MallGoodsRequest",
    ---商城商品列表响应
    MallGoodsReceive = "MallGoodsReceive",
    ---添加到购物车请求
    AddToShoppingCardRequest = "AddToShoppingCardRequest",
    ---添加到购物车响应
    AddToShoppingCardReceive = "AddToShoppingCardReceive",
    ---购物车列表请求
    ShoppingCardRequest = "ShoppingCardRequest",
    ---购物车列表响应
    ShoppingCardReceive = "ShoppingCardReceive",
    ---更新购物车商品数量请求
    UpdateShoppingCardGoodsAmountRequest = "UpdateShoppingCardGoodsAmountRequest",
    ---更新购物车商品数量响应
    UpdateShoppingCardGoodsAmountReceive = "UpdateShoppingCardGoodsAmountReceive",
    ---购物车结算请求
    SettlementRequest = "SettlementRequest",
    ---购物车结算响应
    SettlementReceive = "SettlementReceive",
}

---翻转棋游戏相关协议 的处理方法ID
MsgOthelloGameMethodStringID = {
    ---轮到下一个玩家操作的请求
    TurnToNextPlayerRequest = "TurnToNextPlayerRequest",
    ---轮到下一个玩家操作的响应
    TurnToNextPlayerReceive = "TurnToNextPlayerReceive",
    ---放下一个棋子的请求
    PutDownPieceRequest = "PutDownPieceRequest",
    ---放下一个棋子的响应
    PutDownPieceReceive = "PutDownPieceReceive",
    ---断线重连请求
    ReconnectionRequest = "ReconnectionRequest",
    ---断线重连响应
    ReconnectionReceive = "ReconnectionReceive",
}

---排行榜相关协议 的处理方法ID
MsgRankingMethodStringID = {
    ---排行榜请求
    RankingRequest = "RankingRequest",
    ---排行榜响应
    RankingReceive = "RankingReceive",
}

---游戏房间相关逻辑 的处理方法ID
MsgRoomMethodStringID = {
    ---创建房间请求
    CreateRoomRequest = "CreateRoomRequest",
    ---创建房间响应
    CreateRoomReceive = "CreateRoomReceive",
    ---房间列表请求
    RoomListRequest = "RoomListRequest",
    ---房间列表响应
    RoomListReceive = "RoomListReceive",
    ---加入房间请求
    JoinRoomRequest = "JoinRoomRequest",
    ---加入房间响应
    JoinRoomReceive = "JoinRoomReceive",
    ---退出房间请求
    QuitRoomRequest = "QuitRoomRequest",
    ---退出房间响应
    QuitRoomReceive = "QuitRoomReceive",
    ---其他玩家加入房间请求
    OtherPlayerJoinRoomRequest = "OtherPlayerJoinRoomRequest",
    ---其他玩家加入房间响应
    OtherPlayerJoinRoomReceive = "OtherPlayerJoinRoomReceive",
    ---其他玩家退出房间请求
    OtherPlayerQuitRoomRequest = "OtherPlayerQuitRoomRequest",
    ---其他玩家退出房间响应
    OtherPlayerQuitRoomReceive = "OtherPlayerQuitRoomReceive",
    ---快速加入房间请求
    QuickJoinRoomRequest = "QuickJoinRoomRequest",
    ---快速加入房间响应
    QuickJoinRoomReceive = "QuickJoinRoomReceive",
    ---玩家准备请求
    ReadyRequest = "ReadyRequest",
    ---玩家准备响应
    ReadyReceive = "ReadyReceive",
    ---玩家取消准备请求
    CancelReadyRequest = "CancelReadyRequest",
    ---玩家取消准备响应
    CancelReadyReceive = "CancelReadyReceive",
    ---游戏开始请求
    StartGameRequest = "StartGameRequest",
    ---游戏开始响应
    StartGameReceive = "StartGameReceive",
    ---游戏结束请求
    GameOverRequest = "GameOverRequest",
    ---游戏结束响应
    GameOverReceive = "GameOverReceive",
    ---其他玩家准备请求
    OtherPlayerReadyRequest = "OtherPlayerReadyRequest",
    ---其他玩家准备响应
    OtherPlayerReadyReceive = "OtherPlayerReadyReceive",
    ---其他玩家取消准备请求
    OtherPlayerCancelReadyRequest = "OtherPlayerCancelReadyRequest",
    ---其他玩家取消准备响应
    OtherPlayerCancelReadyReceive = "OtherPlayerCancelReadyReceive",
}

---UNO游戏相关协议 的处理方法ID
MsgUNOGameMethodStringID = {
    ---发牌请求
    SendCardRequest = "SendCardRequest",
    ---发牌响应
    SendCardReceive = "SendCardReceive",
    ---出牌请求
    PutCardRequest = "PutCardRequest",
    ---出牌响应
    PutCardReceive = "PutCardReceive",
    ---轮到下一个玩家出牌的请求
    TurnToPutCardPlayerRequest = "TurnToPutCardPlayerRequest",
    ---轮到下一个玩家出牌的响应
    TurnToPutCardPlayerReceive = "TurnToPutCardPlayerReceive",
    ---特殊牌的逻辑请求
    SpecialCardRequest = "SpecialCardRequest",
    ---特殊牌的逻辑响应
    SpecialCardReceive = "SpecialCardReceive",
    ---断线重连请求
    ReconnectionRequest = "ReconnectionRequest",
    ---断线重连响应
    ReconnectionReceive = "ReconnectionReceive",
    ---洗弃牌堆请求
    MessUpDiscardHeapRequest = "MessUpDiscardHeapRequest",
    ---洗弃牌堆响应
    MessUpDiscardHeapReceive = "MessUpDiscardHeapReceive",
}

