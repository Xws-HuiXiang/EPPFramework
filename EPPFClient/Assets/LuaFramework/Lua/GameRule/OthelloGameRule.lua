OthelloGameRule = {};

---初始化UNO游戏的规则信息
function OthelloGameRule.Init()
    local ruleItem = {};--规则项
    OthelloGameRule["GameName"] = "黑白棋";
    OthelloGameRule["GameID"] = 1;
    OthelloGameRule["IsOpen"] = true;
    OthelloGameRule["RuleItem"] = ruleItem;

    --人数设置
    local item = {};
    item["Name"] = "人数";
    item["IsSingleChoice"] = true;
    local listItem_PlayerCount_2 = {};
    listItem_PlayerCount_2["Key"] = "2人";
    listItem_PlayerCount_2["Value"] = 2;
    listItem_PlayerCount_2["IsDefault"] = true;
    local listTable = {};
    table.insert(listTable, listItem_PlayerCount_2);
    item["List"] = listTable;
    table.insert(ruleItem, item);

    --出牌倒计时
    item = {};
    item["Name"] = "倒计时";
    item["IsSingleChoice"] = true;
    local listItem_Countdown_15 = {};
    listItem_Countdown_15["Key"] = "15秒";
    listItem_Countdown_15["Value"] = 15;
    listItem_Countdown_15["IsDefault"] = false;
    local listItem_Countdown_30 = {};
    listItem_Countdown_30["Key"] = "30秒";
    listItem_Countdown_30["Value"] = 30;
    listItem_Countdown_30["IsDefault"] = true;
    local listItem_Countdown_60 = {};
    listItem_Countdown_60["Key"] = "60秒";
    listItem_Countdown_60["Value"] = 60;
    listItem_Countdown_60["IsDefault"] = false;
    local listItem_Countdown_UnLimit = {};
    listItem_Countdown_UnLimit["Key"] = "无限制";
    listItem_Countdown_UnLimit["Value"] = -1;
    listItem_Countdown_UnLimit["IsDefault"] = false;
    listTable = {};
    table.insert(listTable, listItem_Countdown_15);
    table.insert(listTable, listItem_Countdown_30);
    table.insert(listTable, listItem_Countdown_60);
    table.insert(listTable, listItem_Countdown_UnLimit);
    item["List"] = listTable;
    item["IsOpen"] = true;
    table.insert(ruleItem, item);
end