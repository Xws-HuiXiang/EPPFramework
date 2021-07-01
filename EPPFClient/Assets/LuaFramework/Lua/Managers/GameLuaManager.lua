require "Common/LuaDefine"

require "GameRule/UNOGameRule"
require "GameRule/OthelloGameRule"
require "GameRule/WeiQiGameRule"
require "GameRule/ChineseChessGameRule"

GameLuaManager = {};
local this = GameLuaManager;

---玩家信息
GameLuaManager.PlayerInfo = nil;

this.startGamePlayerAmount = 0;

---按钮音效的AudioClip
this.ButtonAudioClip = nil;
---播放背景音乐的AudioSource
this.bgmAudioSource = nil;

---全部的游戏规则列表
this.gameRuleList = {
    UNOGameRule,
    OthelloGameRule,
    WeiQiGameRule,
    ChineseChessGameRule
};

function this.SetPlayerInfo(playerInfo)
    GameLuaManager.PlayerInfo = playerInfo;
end

---初始化全部游戏规则的表
function this.InitAllGameRule()
    for k, v in pairs(this.gameRuleList) do
        local initFunc = v.Init;
        if(initFunc ~= nil)then
            initFunc();
        else
            FDebugger.LogWarning("规则初始化的方法为空，初始化方法应为：Init()。游戏名称：" .. v["GameName"]);
        end
    end
end

---根据GameID返回对应的游戏规则表
function this.GetGameRuleTableByGameID(gameID)
    for k, v in pairs(this.gameRuleList) do
        if(v["GameID"] == gameID)then
            return v;
        end
    end

    FDebugger.LogWarning("未知的游戏ID：" .. gameID);

    return nil;
end

---根据图片的字节数据创建sprite对象。已经弃用，在C#端实现
function this.GetSpriteByBytes(width, height, bytesData)
    --Texture2D
    local tex = UnityEngine.Texture2D.New(width, height);
    tex:LoadImage(bytesData);--加载图片信息
    tex:Compress(true);--对图片进行压缩
    --Sprite对象
    local rect = UnityEngine.Rect.New(0, 0, tex.width, tex.height);
    local sprite = UnityEngine.Sprite.Create(tex, rect, Vector2.New(0.5, 0.5));

    return sprite;
end

---组拼头像的完整url下载地址
function this.GetAvatarFullPath(avatarName)
    if(avatarName ~= nil)then
        return DefaultAvatarRootURL .. avatarName .. ".png";
    end

    return nil;
end
