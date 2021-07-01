require 'View/SettingPanel'
require "Managers/RoomLuaManager"

SettingCtrl = {};
local this = SettingCtrl;
local panelStruct;--当前面板的结构对象
local panel;--当前物体对应的panel

function this.New()
    return this;
end

function this.OpenPanel()
    panelStruct = PanelManager.OpenPanel('Common', 'SettingPanel', this.Awake, this.Start, this.Enable, this.Close, true);
    panel = SettingPanel.New(panelStruct.panelGameObject.transform);
    panelStruct:Init();--执行周期函数
end

function this.Awake()
	UIEventBinds.ButtonAddOnClick(panel.bg_Right_Buttons_QuitRoom, this.OnQuitRoomBtnButtonClick);
	UIEventBinds.ButtonAddOnClick(panel.bg_Right_Buttons_LogOut, this.OnLogOutBtnClick);
	UIEventBinds.ButtonAddOnClick(panel.bg_Close, this.OnCloseButtonClick);

    UIEventBinds.ToggleAddValueChange(panel.bg_Right_Viewport_Content_Setting_Volume_Background_Mute, this.OnBackgroundMuteToggleValueChange);
    UIEventBinds.ToggleAddValueChange(panel.bg_Right_Viewport_Content_Setting_Volume_Effect_Mute, this.OnEffectMuteToggleValueChange);
    UIEventBinds.ToggleAddValueChange(panel.bg_Right_Viewport_Content_Setting_Broadcast_ShowInGame_Toggle, this.OnBroadcastIsShowToggleValueChange);
    UIEventBinds.ToggleAddValueChange(panel.bg_Right_Viewport_Content_Setting_AutoLogin_AutoLogin_Toggle, this.OnAutoLoginToggleValueChange);

    UIEventBinds.SliderAddValueChange(panel.bg_Right_Viewport_Content_Setting_Volume_Background_Slider, this.OnBackgroundVolumeValueChange);
    UIEventBinds.SliderAddValueChange(panel.bg_Right_Viewport_Content_Setting_Volume_Effect_Slider, this.OnEffectVolumeValueChange);

    --给左侧toggle添加事件
    local toggleCount = panel.bg_Left_Viewport_Content.childCount;
    for i = 0, toggleCount - 1 do
        local toggleTrans = panel.bg_Left_Viewport_Content:GetChild(i);
        UIEventBinds.ToggleAddValueChange(toggleTrans, function(isOn)
            this.OnLeftItemToggleValueChange(isOn, toggleTrans);
        end);
    end

    Panel.DOOpenPanelTween(panel.bg, nil);

    --查询当前游戏版本号
    this.SendGameVersionRequest();
end

function this.Start()

end

function this.Enable()
    --将配置中的值回显到面板上
    local bgmVolume = tonumber(SaveConfig.GetConfigValue(SaveConfig.ConfigListField.AudioBGMVolume, 0.4));
    local effectVolume = tonumber(SaveConfig.GetConfigValue(SaveConfig.ConfigListField.AudioEffectVolume, 1));
    panel.bg_Right_Viewport_Content_Setting_Volume_Background_Slider:GetComponent("Slider").value = bgmVolume;
    panel.bg_Right_Viewport_Content_Setting_Volume_Effect_Slider:GetComponent("Slider").value = effectVolume;
    local bgmIsMute = Utils.ParseBoolean(SaveConfig.GetConfigValue(SaveConfig.ConfigListField.AudioBGMIsMute, false));
    panel.bg_Right_Viewport_Content_Setting_Volume_Background_Mute:GetComponent("Toggle").isOn = bgmIsMute;
    local effectIsMute = Utils.ParseBoolean(SaveConfig.GetConfigValue(SaveConfig.ConfigListField.AudioEffectIsMute, false));
    panel.bg_Right_Viewport_Content_Setting_Volume_Effect_Mute:GetComponent("Toggle").isOn = effectIsMute;
    --广播
    local showPaoMaDengInGame = Utils.ParseBoolean(SaveConfig.GetConfigValue(SaveConfig.ConfigListField.ShowPaoMaDengInGame, true));
    panel.bg_Right_Viewport_Content_Setting_Broadcast_ShowInGame_Toggle:GetComponent("Toggle").isOn = showPaoMaDengInGame;
    --自动登陆
    local autoLogin = Utils.ParseBoolean(SaveConfig.GetConfigValue(SaveConfig.ConfigListField.OpenAutoLogin, true));
    panel.bg_Right_Viewport_Content_Setting_AutoLogin_AutoLogin_Toggle:GetComponent("Toggle").isOn = autoLogin;

    --如果在房间则显示退出房间按钮，不在房间则不显示
    if(RoomLuaManager.roomID == nil) then
        --不在房间内
        panel.bg_Right_Buttons_QuitRoom.gameObject:SetActive(false);
    else
        --在房间内
        panel.bg_Right_Buttons_QuitRoom.gameObject:SetActive(true);
    end
    --如果在游戏中则不能注销登陆，否则可以
    if(UNOGameCtrl.roomID == nil) then
        --不在房间内
        panel.bg_Right_Buttons_LogOut:GetComponent("Button").interactable = true;
    else
        --在房间内
        panel.bg_Right_Buttons_LogOut:GetComponent("Button").interactable = false;
    end
end

---退出房间按钮点击事件
function this.OnQuitRoomBtnButtonClick()
    if(UNOGameCtrl.roomID == nil) then
        --不在游戏房间内
    else
        --在游戏房间内 发送退出房间的请求
        this.SendQuitRoomMsg(GameLuaManager.RoomID);
    end

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);
end

---注销登陆按钮点击事件
function this.OnLogOutBtnClick()
    TipWindowCtrl.ShowTipWindow("确定要注销登陆吗？", TipWindowCtrl.TipWindowStyle.OKAndCancel,function()
        --确定。关闭当前所有的窗口，打开登陆并且清除自动登陆的相关数据
        --先把bgm停了
        if(GameLuaManager.bgmAudioSource ~= nil)then
            GameLuaManager.bgmAudioSource:Stop();
        end

        local isOpenAutoLogin = SaveConfig.GetConfigValue(SaveConfig.ConfigListField.OpenAutoLogin, "True");
        SaveConfig.SetConfigValue(SaveConfig.ConfigListField.OpenAutoLogin, Utils.ParseBoolean(isOpenAutoLogin), false);
        SaveConfig.SetConfigValue(SaveConfig.ConfigListField.Username, "", false);
        SaveConfig.SetConfigValue(SaveConfig.ConfigListField.Password, "", true);

        Panel.CloseAllPanel();

        Panel.OpenPanel(CtrlNames.loginCtrl);

        TipText.ShowTipText("您已注销登陆");
    end, nil);

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);
end

---关闭按钮点击事件
function this.OnCloseButtonClick()
    Panel.DOClosePanelTween(panel.bg, function()
        Panel.ClosePanel(CtrlNames.settingCtrl);
    end);

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);
end

---左侧列表toggle点击事件
function this.OnLeftItemToggleValueChange(isOn, toggleTrans)
    if(isOn)then
        this.HideAllRightItem();

        --开启对应名称的面板
        local trans = panel.bg_Right_Viewport_Content:Find(toggleTrans.name);
        trans.gameObject:SetActive(true);

        --播放按钮音效
        AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);
    end
end

---隐藏右侧全部内容
function this.HideAllRightItem()
    local count = panel.bg_Right_Viewport_Content.childCount;
    for i = 0, count - 1 do
        local trans = panel.bg_Right_Viewport_Content:GetChild(i);
        trans.gameObject:SetActive(false);
    end
end

---背景音乐的静音toggle值改变事件
function this.OnBackgroundMuteToggleValueChange(isOn)
    SaveConfig.SetConfigValue(SaveConfig.ConfigListField.AudioBGMIsMute, isOn, false);

    if(GameLuaManager.bgmAudioSource ~= nil)then
        GameLuaManager.bgmAudioSource.mute = isOn;
    end

    AudioManager.Instance.BGMMute = isOn;

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);
end

---音效音乐的静音toggle值改变事件
function this.OnEffectMuteToggleValueChange(isOn)
    SaveConfig.SetConfigValue(SaveConfig.ConfigListField.AudioEffectIsMute, isOn, false);

    AudioManager.Instance.EffectMute = isOn;

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);
end

---广播是否在游戏时显示的toggle值改变事件
function this.OnBroadcastIsShowToggleValueChange(isOn)
    SaveConfig.SetConfigValue(SaveConfig.ConfigListField.ShowPaoMaDengInGame, isOn, false);

    --回显到界面上

    --if(res)then
    --    if(not isOn)then
    --        Panel.OpenPanel(CtrlNames.paoMaDengCtrl);
    --    end
    --else
    --    if(isOn)then
    --
    --    end
    --end

    --如果在房间内则有回显
    if(UNOGameCtrl.roomID ~= nil) then
        if(isOn)then
            Panel.OpenPanel(CtrlNames.paoMaDengCtrl);
            --将跑马灯显示在设置界面下面
            local paoMaDengGameObject = Panel.GetPanelGameObject(CtrlNames.paoMaDengCtrl);
            if(paoMaDengGameObject ~= nil)then
                local index = PanelManager.MainCanvasRectTrans.childCount - 2;
                paoMaDengGameObject.transform:SetSiblingIndex(index);
            end
        else
            --如果面板开启则关闭
            local res = Panel.CheckPanelIsOpenInMainCanvas(CtrlNames.paoMaDengCtrl);
            if(res)then
                Panel.ClosePanel(CtrlNames.paoMaDengCtrl);
            end
        end
    end

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);
end

---自动登陆toggle值改变事件
function this.OnAutoLoginToggleValueChange(isOn)
    SaveConfig.SetConfigValue(SaveConfig.ConfigListField.OpenAutoLogin, isOn, false);

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);
end

---背景音乐滑动条值改变事件
function this.OnBackgroundVolumeValueChange(value)
    SaveConfig.SetConfigValue(SaveConfig.ConfigListField.AudioBGMVolume, value, false);

    --修改背景音乐时，同步修改播放背景音乐的AudioSource的音量
    if(GameLuaManager.bgmAudioSource ~= nil)then
        GameLuaManager.bgmAudioSource.volume = value;

        AudioManager.Instance.BGMVolume = value;
    end
end

---音效音乐滑动条值改变事件
function this.OnEffectVolumeValueChange(value)
    SaveConfig.SetConfigValue(SaveConfig.ConfigListField.AudioEffectVolume, value, false);

    --同步修改音效的音量
    AudioManager.Instance.EffectVolume = value;
end

function this.Close()
    --保存设置到本地
    ConfigData.SaveConfig();
end

---发送退出房间请求
function this.SendQuitRoomMsg(roomID)
    local msg = MsgQuitRoomRequestTable:New();
    msg.playerGUID = GameLuaManager.PlayerInfo.GUID;
    msg.roomID = roomID;

    local data = NetworkLuaManager.EncodeMsg(MsgRoomMethodStringID.QuitRoomRequest, msg);
    NetworkManager.Instance:SendMsgFromLua(MsgHandleID.Room, MsgRoomMethodID.QuitRoomRequest, data);
end

---发送游戏版本号查询请求
function this.SendGameVersionRequest()
    local msg = MsgGameVersionRequestTable:New();
    msg.playerGUID = GameLuaManager.PlayerInfo.GUID;

    local data = NetworkLuaManager.EncodeMsg(MsgCommonMethodStringID.GameVersionRequest, msg);
    NetworkManager.Instance:SendMsgFromLua(MsgHandleID.Common, MsgCommonMethodID.GameVersionRequest, data);
end

---游戏版本号查询响应
function this.OnGameVersionReceive(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgCommonMethodStringID.GameVersionReceive, data);

    panel.bg_Right_Viewport_Content_Help_GameVersion:GetComponent("Text").text = msg.version;
end