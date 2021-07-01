UNOCard = {};
local this = UNOCard;

UNOCard.__index = UNOCard;

this.CardType = nil;
this.CardColor = nil;

---当前这张牌是否为选择状态
this.isSelect = false;
---卡牌图片的RectTransform
this.cardImageRectTrans = nil;
---卡牌父物体对象
this.cardGO = nil;

---这张牌是否正在被打出（发出了打牌消息，服务器端还没有回应的期间，为true）
this.outCardPlaying = false;

---UNO卡牌类型
UNOCardType =
{
    ---卡牌背面
    CardBack = -1,
    ---0
    Number0 = 0,
    ---1
    Number1 = 1,
    ---2
    Number2 = 2,
    ---3
    Number3 = 3,
    ---4
    Number4 = 4,
    ---5
    Number5 = 5,
    ---6
    Number6 = 6,
    ---7
    Number7 = 7,
    ---8
    Number8 = 8,
    ---9
    Number9 = 9,
    ---下家摸两张牌并且不能出牌 可以再打出一张+2
    DrawTwoFunction = 10,
    ---反转出牌顺序
    ReverseFunction = 11,
    ---禁止下家出牌
    SkipFunction = 12,
    ---Wild的功能便是可以不论上一张出牌的颜色，而随意指定下家出牌的颜色。Wild牌可以在任何时候出，但在被Draw2或者Wild Draw4时就不能出
    Wild = 13,
    ---下家将罚摸4张，并且不能出牌，而且打出此牌的玩家可指定出牌颜色
    WildAddFour = 14
}

---UNO卡牌颜色
UNOCardColor =
{
    ---没有颜色
    None = -1,
    ---蓝色
    Blue = 0,
    ---绿色
    Green = 1,
    ---红色
    Red = 2,
    ---黄色
    Yellow = 3
}

function this.New()
    local o = {};
    setmetatable(o, UNOCard);

    return o;
end

function this.New(cardType, cardColor)
    local o = {};
    setmetatable(o, UNOCard);

    o.CardType = cardType;
    o.CardColor = cardColor;
    o.isSelect = false;

    return o;
end

---获得card对应的图片资源名称
function this.GetUNOCardSpriteName(cardType, cardColor)
    if(cardType == UNOCardType.CardBack)then
        return "Card_CardBack";
    end

    local cardFunctionStr = "";
    local cardTypeStr = "";
    if (cardType == UNOCardType.Number0) then
        cardFunctionStr = "CommonCard";
        cardTypeStr = "0";
    elseif (cardType == UNOCardType.Number1) then
        cardFunctionStr = "CommonCard";
        cardTypeStr = "1";
    elseif (cardType == UNOCardType.Number2) then
        cardFunctionStr = "CommonCard";
        cardTypeStr = "2";
    elseif (cardType == UNOCardType.Number3) then
        cardFunctionStr = "CommonCard";
        cardTypeStr = "3";
    elseif (cardType == UNOCardType.Number4) then
        cardFunctionStr = "CommonCard";
        cardTypeStr = "4";
    elseif (cardType == UNOCardType.Number5) then
        cardFunctionStr = "CommonCard";
        cardTypeStr = "5";
    elseif (cardType == UNOCardType.Number6) then
        cardFunctionStr = "CommonCard";
        cardTypeStr = "6";
    elseif (cardType == UNOCardType.Number7) then
        cardFunctionStr = "CommonCard";
        cardTypeStr = "7";
    elseif (cardType == UNOCardType.Number8) then
        cardFunctionStr = "CommonCard";
        cardTypeStr = "8";
    elseif (cardType == UNOCardType.Number9) then
        cardFunctionStr = "CommonCard";
        cardTypeStr = "9";
    elseif (cardType == UNOCardType.DrawTwoFunction) then
        cardFunctionStr = "FunctionCard";
        cardTypeStr = "DrawTwo";
    elseif (cardType == UNOCardType.ReverseFunction) then
        cardFunctionStr = "FunctionCard";
        cardTypeStr = "Reverse";
    elseif (cardType == UNOCardType.SkipFunction) then
        cardFunctionStr = "FunctionCard";
        cardTypeStr = "Skip";
    elseif (cardType == UNOCardType.Wild) then
        cardFunctionStr = "WildCard";
        cardTypeStr = "Wild";
    elseif (cardType == UNOCardType.WildAddFour) then
        cardFunctionStr = "WildCard";
        cardTypeStr = "WildDrawFour";
    end

    local cardColorStr = "";
    if(cardColor == UNOCardColor.Blue) then
        cardColorStr = "Blue";
    elseif cardColor == UNOCardColor.Green then
        cardColorStr = "Green";
    elseif cardColor == UNOCardColor.Red then
        cardColorStr = "Red";
    elseif(cardColor == UNOCardColor.Yellow) then
        cardColorStr = "Yellow";
    elseif cardColor == UNOCardColor.None then
        --无颜色
        cardColorStr = "None";
    end

    --黑牌如果有颜色则为打出的黑牌，手牌的黑牌没有颜色
    return "Card_" .. cardFunctionStr .. "_" .. cardColorStr .. "_" .. cardTypeStr;
end

---根据数字类型的卡牌类型返回对应的卡牌类型
function this.GetCardTypeFromTypeNumber(cardType)
    local res;
    if(cardType == 0)then
        res = UNOCardType.Number0;
    elseif cardType == 1 then
        res = UNOCardType.Number1;
    elseif cardType == 2 then
        res = UNOCardType.Number2;
    elseif cardType == 3 then
        res = UNOCardType.Number3;
    elseif cardType == 4 then
        res = UNOCardType.Number4;
    elseif cardType == 5 then
        res = UNOCardType.Number5;
    elseif cardType == 6 then
        res = UNOCardType.Number6;
    elseif cardType == 7 then
        res = UNOCardType.Number7;
    elseif cardType == 8 then
        res = UNOCardType.Number8;
    elseif cardType == 9 then
        res = UNOCardType.Number9;
    elseif cardType == 10 then
        res = UNOCardType.DrawTwoFunction;
    elseif cardType == 11 then
        res = UNOCardType.ReverseFunction;
    elseif cardType == 12 then
        res = UNOCardType.SkipFunction;
    elseif cardType == 13 then
        res = UNOCardType.Wild;
    elseif cardType == 14 then
        res = UNOCardType.WildAddFour;
    else
        res = UNOCardType.CardBack;
    end

    return res;
end

---根据数字类型的卡牌颜色返回对应的卡牌颜色
function this.GetCardColorFromColorNumber(cardColor)
    local res;
    if(cardColor == 0)then
        res = UNOCardColor.Blue;
    elseif(cardColor == 1)then
        res = UNOCardColor.Green;
    elseif(cardColor == 2)then
        res = UNOCardColor.Red;
    elseif(cardColor == 3)then
        res = UNOCardColor.Yellow;
    else
        res = UNOCardColor.None;
    end

    return res;
end

---根据卡牌颜色返回unity的color对象
function this.GetUnityColorFromColorNumber(cardColor)
    local res;
    if(cardColor == UNOCardColor.Blue)then
        res = Color.New(0, 0.5764706, 0.8627451, 0.2039216);
    elseif(cardColor == UNOCardColor.Green)then
        res = Color.New(0, 0.572549, 0.2470588, 0.2039216);
    elseif(cardColor == UNOCardColor.Red)then
        res = Color.New(0.8509804, 0.145098, 0.1098039, 0.2039216);
    elseif(cardColor == UNOCardColor.Yellow)then
        res = Color.New(0.9921569, 0.9607843, 0.007843138, 0.2039216);
    else
        FDebugger.LogWarning("未知的颜色类型：" .. cardColor);
        res = Color.New(1, 1, 1, 0.2039216);
    end

    return res;
end

---根据卡牌的颜色枚举返回对应的说明文字
function this.GetColorTextFromCardColor(cardColor)
    local res;
    if(cardColor == UNOCardColor.Blue)then
        res = "蓝色";
    elseif(cardColor == UNOCardColor.Green)then
        res = "绿色";
    elseif(cardColor == UNOCardColor.Red)then
        res = "红色";
    elseif(cardColor == UNOCardColor.Yellow)then
        res = "黄色";
    else
        FDebugger.LogWarning("未知的颜色类型：" .. cardColor);
        res = "";
    end

    return res;
end

---根据卡牌类型枚举返回对应的说明文字
function this.GetTypeTextFromCardType(cardType)
    local res;
    if (cardType == UNOCardType.Number0) then
        res = "0";
    elseif (cardType == UNOCardType.Number1) then
        res = "1";
    elseif (cardType == UNOCardType.Number2) then
        res = "2";
    elseif (cardType == UNOCardType.Number3) then
        res = "3";
    elseif (cardType == UNOCardType.Number4) then
        res = "4";
    elseif (cardType == UNOCardType.Number5) then
        res = "5";
    elseif (cardType == UNOCardType.Number6) then
        res = "6";
    elseif (cardType == UNOCardType.Number7) then
        res = "7";
    elseif (cardType == UNOCardType.Number8) then
        res = "8";
    elseif (cardType == UNOCardType.Number9) then
        res = "9";
    elseif (cardType == UNOCardType.DrawTwoFunction) then
        res = "";
    elseif (cardType == UNOCardType.ReverseFunction) then
        res = "";
    elseif (cardType == UNOCardType.SkipFunction) then
        res = "";
    elseif (cardType == UNOCardType.Wild) then
        res = "";
    elseif (cardType == UNOCardType.WildAddFour) then
        res = "";
    else
        res = "";
    end

    return res;
end