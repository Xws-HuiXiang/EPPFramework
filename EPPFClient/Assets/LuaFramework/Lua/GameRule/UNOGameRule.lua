UNOGameRule = {};

--[[
表结构：
UNOGameRule = {
    "GameName" = "string",
    "GameID" = int,
    "IsOpen" = boolean,
    "RuleItem" = {
        "Name" = "string",
        "IsSingleChoice" = boolean,
        "List" = {
            {
                "Key" = "string",
                "Value" = int,
                "IsDefault" = boolean
            },
            {
                "Key" = "string",
                "Value" = int,
                "IsDefault" = boolean
            },
            {
                "Key" = "string",
                "Value" = int,
                "IsDefault" = boolean
            }...
        },
        {

        }...
    }
}
--]]
---初始化UNO游戏的规则信息
function UNOGameRule.Init()
    local ruleItem = {};--规则项
    UNOGameRule["GameName"] = "UNO";
    UNOGameRule["GameID"] = 0;
    UNOGameRule["IsOpen"] = true;
    UNOGameRule["RuleItem"] = ruleItem;

    --人数设置
    local item = {};
    item["Name"] = "人数";
    item["IsSingleChoice"] = true;
    local listItem_PlayerCount_2 = {};
    listItem_PlayerCount_2["Key"] = "2人";
    listItem_PlayerCount_2["Value"] = 2;
    listItem_PlayerCount_2["IsDefault"] = false;
    local listItem_PlayerCount_3 = {};
    listItem_PlayerCount_3["Key"] = "3人";
    listItem_PlayerCount_3["Value"] = 3;
    listItem_PlayerCount_3["IsDefault"] = true;
    local listItem_PlayerCount_4 = {};
    listItem_PlayerCount_4["Key"] = "4人";
    listItem_PlayerCount_4["Value"] = 4;
    listItem_PlayerCount_4["IsDefault"] = false;
    local listItem_PlayerCount_5 = {};
    listItem_PlayerCount_5["Key"] = "5人";
    listItem_PlayerCount_5["Value"] = 5;
    listItem_PlayerCount_5["IsDefault"] = false;
    --local listItem_PlayerCount_6 = {};
    --listItem_PlayerCount_6["Key"] = "6人";
    --listItem_PlayerCount_6["Value"] = 6;
    --listItem_PlayerCount_6["IsDefault"] = false;
    local listTable = {};
    table.insert(listTable, listItem_PlayerCount_2);
    table.insert(listTable, listItem_PlayerCount_3);
    table.insert(listTable, listItem_PlayerCount_4);
    table.insert(listTable, listItem_PlayerCount_5);
    --table.insert(listTable, listItem_PlayerCount_6);
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