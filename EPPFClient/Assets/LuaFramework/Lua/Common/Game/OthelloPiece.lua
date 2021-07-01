OthelloPiece = {};
local this = OthelloPiece;

OthelloPiece.__index = OthelloPiece;

this.transform = nil;
this.x = 0;
this.y = 0;
this.anim = nil;
this.color = nil;

--- 黑白棋的颜色枚举
OthelloPiece.PieceColor = {
    Black = 0,
    White = 1
}

--- 创建一个翻转棋的棋子对象表
function this.New(x, y, transform, color)
    local o = {};
    setmetatable(o, OthelloPiece);

    o.transform = transform;
    o.x = x;
    o.y = y;
    o.anim = transform.gameObject:GetComponent("Animator");
    o.color = color;

    return o;
end

--- 根据颜色索引返回对应的颜色枚举对象
function this.GetPieceColorByColorIndex(colorIndex)
    if(colorIndex == 0)then
        return OthelloPiece.PieceColor.Black;
    elseif(colorIndex == 1)then
        return OthelloPiece.PieceColor.White;
    else
        FDebugger.LogError("根据索引获取翻转棋颜色时，未处理的索引值：" .. colorIndex .. "将返回默认的黑色");
    end

    return OthelloPiece.PieceColor.Black;
end

--- 设置当前棋子颜色
function this.SetColor(piece, color)
    local animName = this.GetColorIdleAnimNameByColor(color);
    piece.anim:Play(animName);
end