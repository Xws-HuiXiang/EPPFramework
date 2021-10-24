require "Common/LuaDefine"

GameLuaManager = {};
local this = GameLuaManager;

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
