namespace EPPFServer.Protocol.Builder
{
    public class MsgID { }

    /// <summary>
    /// 协议处理ID
    /// </summary>
    public enum MsgHandleID
    {
        /// <summary>
        /// 活动相关协议
        /// </summary>
        Activity = 6,
        /// <summary>
        /// 广告相关的协议
        /// </summary>
        Ads = 8,
        /// <summary>
        /// 通用协议
        /// </summary>
        Common = 0,
        /// <summary>
        /// 登陆相关逻辑
        /// </summary>
        Login = 1,
        /// <summary>
        /// 商城相关协议
        /// </summary>
        Mall = 5,
        /// <summary>
        /// 翻转棋游戏相关协议
        /// </summary>
        OthelloGame = 7,
        /// <summary>
        /// 排行榜相关协议
        /// </summary>
        Ranking = 4,
        /// <summary>
        /// 游戏房间相关逻辑
        /// </summary>
        Room = 2,
        /// <summary>
        /// UNO游戏相关协议
        /// </summary>
        UNOGame = 3,
    }

    /// <summary>
    /// 活动相关协议
    /// </summary>
    public enum MsgActivityMethodID
    {
        /// <summary>
        /// 活动列表请求
        /// </summary>
        ActivityListRequest = 0,
        /// <summary>
        /// 活动列表响应
        /// </summary>
        ActivityListReceive = 1,
    }

    /// <summary>
    /// 广告相关的协议
    /// </summary>
    public enum MsgAdsMethodID
    {
        /// <summary>
        /// Unity广告观看完成请求
        /// </summary>
        UnityAdsWatchFinishRequest = 0,
        /// <summary>
        /// Unity广告观看完成响应
        /// </summary>
        UnityAdsWatchFinishReceive = 1,
    }

    /// <summary>
    /// 通用协议
    /// </summary>
    public enum MsgCommonMethodID
    {
        /// <summary>
        /// 获取密钥请求
        /// </summary>
        SecretKeyRequest = 0,
        /// <summary>
        /// 获取密钥响应
        /// </summary>
        SecretKeyReceive = 1,
        /// <summary>
        /// 心跳包请求
        /// </summary>
        HeartbeatRequest = 2,
        /// <summary>
        /// 心跳包响应
        /// </summary>
        HeartbeatReceive = 3,
        /// <summary>
        /// 跑马灯请求
        /// </summary>
        PaoMaDengRequest = 4,
        /// <summary>
        /// 跑马灯响应
        /// </summary>
        PaoMaDengReceive = 5,
        /// <summary>
        /// 更新金币请求
        /// </summary>
        UpdateGoldCoinRequest = 6,
        /// <summary>
        /// 更新金币响应
        /// </summary>
        UpdateGoldCoinReceive = 7,
        /// <summary>
        /// 查询个人信息请求
        /// </summary>
        InquirePersonalInfoRequest = 8,
        /// <summary>
        /// 查询个人信息响应
        /// </summary>
        InquirePersonalInfoReceive = 9,
        /// <summary>
        /// 版本号查询请求
        /// </summary>
        GameVersionRequest = 10,
        /// <summary>
        /// 版本号查询响应
        /// </summary>
        GameVersionReceive = 11,
        /// <summary>
        /// 聊天讲话的请求
        /// </summary>
        ChatTalkRequest = 12,
        /// <summary>
        /// 聊天讲话的响应
        /// </summary>
        ChatTalkReceive = 13,
        /// <summary>
        /// 全部默认头像名称请求
        /// </summary>
        AllDefaultAvatarRequest = 14,
        /// <summary>
        /// 全部默认头像响应
        /// </summary>
        AllDefaultAvatarReceive = 15,
        /// <summary>
        /// 更新个人信息请求。不更新玩家名称，玩家名称的修改是单独的消息
        /// </summary>
        UpdatePersonalInfoRequest = 16,
        /// <summary>
        /// 更新个人信息响应。不更新玩家名称，玩家名称的修改是单独的消息
        /// </summary>
        UpdatePersonalInfoReceive = 17,
    }

    /// <summary>
    /// 登陆相关逻辑
    /// </summary>
    public enum MsgLoginMethodID
    {
        /// <summary>
        /// 注册请求
        /// </summary>
        RegisterRequest = 0,
        /// <summary>
        /// 注册响应
        /// </summary>
        RegisterReceive = 1,
        /// <summary>
        /// 登陆请求
        /// </summary>
        LoginRequest = 2,
        /// <summary>
        /// 登陆响应
        /// </summary>
        LoginReceive = 3,
        /// <summary>
        /// 踢掉线请求
        /// </summary>
        LogOutRequest = 4,
        /// <summary>
        /// 踢掉线响应
        /// </summary>
        LogOutReceive = 5,
    }

    /// <summary>
    /// 商城相关协议
    /// </summary>
    public enum MsgMallMethodID
    {
        /// <summary>
        /// 商城商品列表请求
        /// </summary>
        MallGoodsRequest = 0,
        /// <summary>
        /// 商城商品列表响应
        /// </summary>
        MallGoodsReceive = 1,
        /// <summary>
        /// 添加到购物车请求
        /// </summary>
        AddToShoppingCardRequest = 2,
        /// <summary>
        /// 添加到购物车响应
        /// </summary>
        AddToShoppingCardReceive = 3,
        /// <summary>
        /// 购物车列表请求
        /// </summary>
        ShoppingCardRequest = 4,
        /// <summary>
        /// 购物车列表响应
        /// </summary>
        ShoppingCardReceive = 5,
        /// <summary>
        /// 更新购物车商品数量请求
        /// </summary>
        UpdateShoppingCardGoodsAmountRequest = 6,
        /// <summary>
        /// 更新购物车商品数量响应
        /// </summary>
        UpdateShoppingCardGoodsAmountReceive = 7,
        /// <summary>
        /// 购物车结算请求
        /// </summary>
        SettlementRequest = 8,
        /// <summary>
        /// 购物车结算响应
        /// </summary>
        SettlementReceive = 9,
    }

    /// <summary>
    /// 翻转棋游戏相关协议
    /// </summary>
    public enum MsgOthelloGameMethodID
    {
        /// <summary>
        /// 轮到下一个玩家操作的请求
        /// </summary>
        TurnToNextPlayerRequest = 0,
        /// <summary>
        /// 轮到下一个玩家操作的响应
        /// </summary>
        TurnToNextPlayerReceive = 1,
        /// <summary>
        /// 放下一个棋子的请求
        /// </summary>
        PutDownPieceRequest = 2,
        /// <summary>
        /// 放下一个棋子的响应
        /// </summary>
        PutDownPieceReceive = 3,
        /// <summary>
        /// 断线重连请求
        /// </summary>
        ReconnectionRequest = 4,
        /// <summary>
        /// 断线重连响应
        /// </summary>
        ReconnectionReceive = 5,
    }

    /// <summary>
    /// 排行榜相关协议
    /// </summary>
    public enum MsgRankingMethodID
    {
        /// <summary>
        /// 排行榜请求
        /// </summary>
        RankingRequest = 0,
        /// <summary>
        /// 排行榜响应
        /// </summary>
        RankingReceive = 1,
    }

    /// <summary>
    /// 游戏房间相关逻辑
    /// </summary>
    public enum MsgRoomMethodID
    {
        /// <summary>
        /// 创建房间请求
        /// </summary>
        CreateRoomRequest = 0,
        /// <summary>
        /// 创建房间响应
        /// </summary>
        CreateRoomReceive = 1,
        /// <summary>
        /// 房间列表请求
        /// </summary>
        RoomListRequest = 2,
        /// <summary>
        /// 房间列表响应
        /// </summary>
        RoomListReceive = 3,
        /// <summary>
        /// 加入房间请求
        /// </summary>
        JoinRoomRequest = 4,
        /// <summary>
        /// 加入房间响应
        /// </summary>
        JoinRoomReceive = 5,
        /// <summary>
        /// 退出房间请求
        /// </summary>
        QuitRoomRequest = 6,
        /// <summary>
        /// 退出房间响应
        /// </summary>
        QuitRoomReceive = 7,
        /// <summary>
        /// 其他玩家加入房间请求
        /// </summary>
        OtherPlayerJoinRoomRequest = 8,
        /// <summary>
        /// 其他玩家加入房间响应
        /// </summary>
        OtherPlayerJoinRoomReceive = 9,
        /// <summary>
        /// 其他玩家退出房间请求
        /// </summary>
        OtherPlayerQuitRoomRequest = 10,
        /// <summary>
        /// 其他玩家退出房间响应
        /// </summary>
        OtherPlayerQuitRoomReceive = 11,
        /// <summary>
        /// 快速加入房间请求
        /// </summary>
        QuickJoinRoomRequest = 12,
        /// <summary>
        /// 快速加入房间响应
        /// </summary>
        QuickJoinRoomReceive = 13,
        /// <summary>
        /// 玩家准备请求
        /// </summary>
        ReadyRequest = 14,
        /// <summary>
        /// 玩家准备响应
        /// </summary>
        ReadyReceive = 15,
        /// <summary>
        /// 玩家取消准备请求
        /// </summary>
        CancelReadyRequest = 16,
        /// <summary>
        /// 玩家取消准备响应
        /// </summary>
        CancelReadyReceive = 17,
        /// <summary>
        /// 游戏开始请求
        /// </summary>
        StartGameRequest = 18,
        /// <summary>
        /// 游戏开始响应
        /// </summary>
        StartGameReceive = 19,
        /// <summary>
        /// 游戏结束请求
        /// </summary>
        GameOverRequest = 20,
        /// <summary>
        /// 游戏结束响应
        /// </summary>
        GameOverReceive = 21,
        /// <summary>
        /// 其他玩家准备请求
        /// </summary>
        OtherPlayerReadyRequest = 22,
        /// <summary>
        /// 其他玩家准备响应
        /// </summary>
        OtherPlayerReadyReceive = 23,
        /// <summary>
        /// 其他玩家取消准备请求
        /// </summary>
        OtherPlayerCancelReadyRequest = 24,
        /// <summary>
        /// 其他玩家取消准备响应
        /// </summary>
        OtherPlayerCancelReadyReceive = 25,
    }

    /// <summary>
    /// UNO游戏相关协议
    /// </summary>
    public enum MsgUNOGameMethodID
    {
        /// <summary>
        /// 发牌请求
        /// </summary>
        SendCardRequest = 0,
        /// <summary>
        /// 发牌响应
        /// </summary>
        SendCardReceive = 1,
        /// <summary>
        /// 出牌请求
        /// </summary>
        PutCardRequest = 2,
        /// <summary>
        /// 出牌响应
        /// </summary>
        PutCardReceive = 3,
        /// <summary>
        /// 轮到下一个玩家出牌的请求
        /// </summary>
        TurnToPutCardPlayerRequest = 4,
        /// <summary>
        /// 轮到下一个玩家出牌的响应
        /// </summary>
        TurnToPutCardPlayerReceive = 5,
        /// <summary>
        /// 特殊牌的逻辑请求
        /// </summary>
        SpecialCardRequest = 6,
        /// <summary>
        /// 特殊牌的逻辑响应
        /// </summary>
        SpecialCardReceive = 7,
        /// <summary>
        /// 断线重连请求
        /// </summary>
        ReconnectionRequest = 8,
        /// <summary>
        /// 断线重连响应
        /// </summary>
        ReconnectionReceive = 9,
        /// <summary>
        /// 洗弃牌堆请求
        /// </summary>
        MessUpDiscardHeapRequest = 10,
        /// <summary>
        /// 洗弃牌堆响应
        /// </summary>
        MessUpDiscardHeapReceive = 11,
    }

}