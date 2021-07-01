require "Common/DistributeToMobile"

CommonFunction = {};
local this = CommonFunction;

--- 拷贝房间ID按钮点击事件
function this.OnCopyRoomIDButtonClick()
    DistributeToMobile.CopyToClipboard(this.roomID);

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);
end

--- 游戏内设置按钮点击事件
function this.OnSettingBtnButtonClick()
    Panel.OpenPanel(CtrlNames.settingCtrl);
end

--- 初始化游戏界面中其他玩家的信息 全部设置为空
function this.SetOtherPlayerInfo(playerCount, playerChairTrans)
    --初始化其他玩家的信息
    for i = 0, playerCount - 1 do
        local playerTrans = playerChairTrans:Find("Player" .. i);
        playerTrans:Find("PlayerNameBg/PlayerName"):GetComponent("Text").text = "";
        playerTrans:Find("PlayerScoreBg/ScoreName"):GetComponent("Text").text = "";
        --隐藏准备按钮
        playerTrans:Find("Ready").gameObject:SetActive(false);
    end
end

---游戏界面设置玩家自身信息显示
function this.SetSelfInfo(player0Trans)
    player0Trans:Find("PlayerNameBg/PlayerName"):GetComponent("Text").text = GameLuaManager.PlayerInfo.Username;
    player0Trans:Find("PlayerScoreBg/ScoreName"):GetComponent("Text").text = GameLuaManager.PlayerInfo.Score;
    local avatarFullPath = GameLuaManager.GetAvatarFullPath(GameLuaManager.PlayerInfo.AvatarName);
    TextureManager.Instance:GetSprite(avatarFullPath, 100, 100, function(sprite)
        local img = player0Trans:Find("AvatarFramework/Avatar").gameObject:GetComponent("Image");
        img.sprite = sprite;
        img.enabled = true;
    end);
end