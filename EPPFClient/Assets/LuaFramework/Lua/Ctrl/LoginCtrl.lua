require "View/LoginPanel"
require "Common/PlayerInfo"
require "Managers/GameLuaManager"
require "Protocol/Tables/LoginTable"
require "Common/SaveConfig"

LoginCtrl = {};
local this = LoginCtrl;
local panelStruct;--当前面板的结构对象
local panel;--当前物体对应的panel

---按下home键的次数
local clickHomeKeyCount = 0;

function this.New()
    return this;
end

function this.OpenPanel()
    panelStruct = PanelManager.OpenPanel("login", "LoginPanel", this.Awake, this.Start, this.Enable, this.Close, true);
    panel = LoginPanel.New(panelStruct.panelGameObject.transform);
    panelStruct:Init();--执行周期函数
end

function this.Awake()
    UIEventBinds.ButtonAddOnClick(panel.bg_Login, this.OnLoginButtonClick);
    UIEventBinds.ButtonAddOnClick(panel.bg_Register, this.OnRegisterButtonClick);

    --添加帧函数
    GameManager.Instance:AddUpdateAction(this.Update);
end

function this.Start()

end

function this.Enable()
    --获取一个提示文字并显示到登陆界面
    local tipURL = AppConst.LoadingTipsRequestURL;
    HttpRequestManager.HttpGetRequestAsync(tipURL, function(content)
        if(content ~= nil and panel.bg_Tips ~= nil) then
            local jsonData = LitJson.JsonMapper.ToObject(content);
            local tipText = assert(jsonData:GetValue("Content"));
            tipText = assert(tipText:ToString());
            panel.bg_Tips:GetComponent("Text").text = tipText;
        end
    end);
end

---Unity主线程帧函数
function this.Update()
    --如果当前处于登陆、注册或主页面时，按两下home（或返回）键则退出应用。第一次弹出气泡，第二次退出应用
    if(UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Escape) or UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Home))then
        clickHomeKeyCount = clickHomeKeyCount + 1;
        local ctrlName = Panel.GetLastPanelCtrlName();
        if(ctrlName == CtrlNames.loginCtrl or ctrlName == CtrlNames.registerCtrl or ctrlName == CtrlNames.mainPageCtrl or ctrlName == CtrlNames.paoMaDengCtrl)then
            if(clickHomeKeyCount == 1)then
                --弹出土司
                DistributeToMobile.MakeToast("再按一次退出程序");
            elseif(clickHomeKeyCount >= 2)then
                --退出应用
                UnityEngine.Application.Quit();
            else
                FDebugger.LogWarning("home和返回键的按键次数出现异常情况。值为：" .. clickHomeKeyCount);
            end
        end
    end
end

function this.Close()
    panelStruct.panelGameObject:SetActive(false);

    --将按home或返回键的次数归零
    LoginCtrl.ResetClickHomeKeyCount();
end

---登陆按钮点击事件
function this.OnLoginButtonClick()
    local account = panel.bg_AccountNumberInputField:GetComponent("InputField").text;
    local password = panel.bg_PasswordInputField:GetComponent("InputField").text;
    if(account == "" or account == nil) then
        --账号为空
        FDebugger.Log("账号为空");
        return;
    end
    if(password == "" or password == nil) then
        --密码为空
        FDebugger.Log("密码为空");

        return;
    end

    this.SendLoginMsg(account, password);

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);

    --将按home或返回键的次数归零
    LoginCtrl.ResetClickHomeKeyCount();
end

---注册按钮点击事件
function this.OnRegisterButtonClick()
    Panel.OpenPanel(CtrlNames.registerCtrl);

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);

    --将按home或返回键的次数归零
    LoginCtrl.ResetClickHomeKeyCount();
end

---发送登陆请求
function this.SendLoginMsg(username, password)
    local msg = MsgLoginRequestTable:New();
    local enumFullName = NetworkLuaManager.CombineFullPbName("LoginRequest.LoginTypeEnum");
    msg.loginType = pb.enum(enumFullName, "Account");
    msg.username = username;
    msg.password = password;

    local data = NetworkLuaManager.EncodeMsg(MsgLoginMethodStringID.LoginRequest, msg);
    NetworkManager.Instance:SendMsgFromLua(MsgHandleID.Login, MsgLoginMethodID.LoginRequest, data);
end

---登陆消息返回的数据
local loginReceiveMsg = nil;
---登陆消息返回
function this.OnMsgLoginHandleMethodCallback(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgLoginMethodStringID.LoginReceive, data);
    loginReceiveMsg = msg;
    if(msg.state == 1) then
        ----登陆成功
        ----如果第二阶段的资源没有加载完成则等待
        --if(not GameManager.Instance.SecondResLoaded)then
        --    Panel.OpenPanel(CtrlNames.loadingCtrl);
        --
        --    StartCoroutine(this.LoginSuccessActionIE);
        --else
        --    this.LoginSuccessAction();
        --end
        this.LoginSuccessAction();
        --local player = PlayerInfo.New();
        --player.SetUsername(msg.username);
        --player.SetGUID(msg.guid);
        --player.SetScore(msg.score);
        --GameLuaManager.SetPlayerInfo(player);
        --
        --Panel.OpenPanel(CtrlNames.mainPageCtrl);
        --Panel.ClosePanel(CtrlNames.loginCtrl);
        --
        --FDebugger.Log("登陆成功");
        --
        ----如果开启了自动登陆则保存账号密码
        --local isOpenAutoLogin = SaveConfig.GetConfigValue(SaveConfig.ConfigListField.OpenAutoLogin, "True");
        --if(isOpenAutoLogin)then
        --    SaveConfig.SetConfigValue(SaveConfig.ConfigListField.OpenAutoLogin, Utils.ParseBoolean(isOpenAutoLogin), false);
        --    SaveConfig.SetConfigValue(SaveConfig.ConfigListField.Username, msg.username, false);
        --    --服务器传回来的密码已经进行加密，这里直接存储
        --    local passwordString = Ciphertext.AES.AESEncrypt(SaveConfig.ConfigListField.Password, NetworkManager.SecretKey);
        --    ConfigData.SetConfigValue(passwordString, msg.password, true);
        --else
        --    SaveConfig.SetConfigValue(SaveConfig.ConfigListField.OpenAutoLogin, Utils.ParseBoolean(isOpenAutoLogin), false);
        --    SaveConfig.SetConfigValue(SaveConfig.ConfigListField.Username, "", false);
        --    SaveConfig.SetConfigValue(SaveConfig.ConfigListField.Password, "", true);
        --end
    elseif(msg.state == 2)then
        TipText.ShowTipText("登陆失败，账号或密码错误");

        FDebugger.Log("登陆失败，账号或密码错误");
    else
        TipText.ShowTipText("登陆失败，遇到未知的错误");
        FDebugger.LogError("登陆请求中未处理的状态值：" .. msg.state);
    end
end

---尝试自动登陆
function this.TryAutoLogin()
    --检查是否开启了自动登陆
    local isOpenAutoLogin = SaveConfig.GetConfigValue(SaveConfig.ConfigListField.OpenAutoLogin, "True");
    isOpenAutoLogin = Utils.ParseBoolean(isOpenAutoLogin);
    if(isOpenAutoLogin)then
        --自动登陆
        local username = SaveConfig.GetConfigValue(SaveConfig.ConfigListField.Username, "");
        local password = SaveConfig.GetConfigValue(SaveConfig.ConfigListField.Password, "");
        if(username == "")then
            FDebugger.Log("尝试自动登录时，用户名为空串");

            return;
        end
        if(password == "")then
            FDebugger.Log("尝试自动登录时，密码为空串");

            return;
        end

        this.SendLoginMsg(username, password);
    end
end

---将按home键的次数归零
function this.ResetClickHomeKeyCount()
    clickHomeKeyCount = 0;
end

---登陆成功后的具体逻辑。协程
function this.LoginSuccessActionIE()
    while(true)do
        if(not GameManager.Instance.SecondResLoaded)then
            WaitForSeconds(0.5);
        else
            break;
        end
    end

    Panel.ClosePanel(CtrlNames.loadingCtrl);
    this.LoginSuccessAction();
end

---登陆成功后的具体逻辑
function this.LoginSuccessAction()
    local player = PlayerInfo.New();
    player.SetUsername(loginReceiveMsg.username);
    player.SetGUID(loginReceiveMsg.guid);
    player.SetScore(loginReceiveMsg.score);
    player.SetAvatar(loginReceiveMsg.avatar);
    GameLuaManager.SetPlayerInfo(player);

    Panel.OpenPanel(CtrlNames.mainPageCtrl);
    Panel.ClosePanel(CtrlNames.loginCtrl);

    FDebugger.Log("登陆成功");

    --如果开启了自动登陆则保存账号密码
    local isOpenAutoLogin = SaveConfig.GetConfigValue(SaveConfig.ConfigListField.OpenAutoLogin, "True");
    if(isOpenAutoLogin)then
        SaveConfig.SetConfigValue(SaveConfig.ConfigListField.OpenAutoLogin, Utils.ParseBoolean(isOpenAutoLogin), false);
        SaveConfig.SetConfigValue(SaveConfig.ConfigListField.Username, loginReceiveMsg.username, false);
        --服务器传回来的密码已经进行加密，这里直接存储
        local passwordString = Ciphertext.AES.AESEncrypt(SaveConfig.ConfigListField.Password, NetworkManager.SecretKey);
        ConfigData.SetConfigValue(passwordString, loginReceiveMsg.password, true);
    else
        SaveConfig.SetConfigValue(SaveConfig.ConfigListField.OpenAutoLogin, Utils.ParseBoolean(isOpenAutoLogin), false);
        SaveConfig.SetConfigValue(SaveConfig.ConfigListField.Username, "", false);
        SaveConfig.SetConfigValue(SaveConfig.ConfigListField.Password, "", true);
    end
end