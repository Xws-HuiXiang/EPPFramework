PlayerInfo = {};
local this = PlayerInfo;

PlayerInfo.__index = PlayerInfo;

---用户名 字符串
PlayerInfo.Username = nil;
---guid int类型(number类型)
PlayerInfo.GUID = nil;
---用户金币 int类型(number类型)
PlayerInfo.Score = nil;
---用户头像名称
PlayerInfo.AvatarName = nil;

function this.New()
    local o = {};
    setmetatable(o, PlayerInfo);

    return o;
end

---设置玩家名称
function this.SetUsername(username)
    this.Username = username;
end

---设置玩家的GUID
function this.SetGUID(guid)
    this.GUID = guid;
end

---设置玩家的积分
function this.SetScore(score)
    this.Score = score;
end

---设置玩家头像的图片名称
function this.SetAvatar(avatarName)
    this.AvatarName = avatarName;
end