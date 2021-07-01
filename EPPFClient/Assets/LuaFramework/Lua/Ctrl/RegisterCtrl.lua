require 'View/RegisterPanel'
require "Common/TipText"
require "Protocol/MsgStringID"
require "Protocol/Tables/LoginTable"

RegisterCtrl = {};
local this = RegisterCtrl;
local panelStruct;--当前面板的结构对象
local panel;--当前物体对应的panel

function this.New()
    return this;
end

function this.OpenPanel()
    panelStruct = PanelManager.OpenPanel('login', 'RegisterPanel', this.Awake, this.Start, this.Enable, this.Close, true);
    panel = RegisterPanel.New(panelStruct.panelGameObject.transform);
    panelStruct:Init();--执行周期函数
end

function this.Awake()
	UIEventBinds.ButtonAddOnClick(panel.bg_Register, this.OnRegisterButtonClick);
	UIEventBinds.ButtonAddOnClick(panel.bg_GoBack, this.OnGoBackButtonClick);
end

function this.Start()

end

function this.Enable()

end

local accountCache, passwordCache;--账号和密码缓存

--注册按钮点击事件
function this.OnRegisterButtonClick()
    local accountString = panel.bg_AccountNumberInputField:GetComponent("InputField").text;
    local passwordString = panel.bg_PasswordInputField:GetComponent("InputField").text;
    local emailString = panel.bg_EmailInputField:GetComponent("InputField").text;
    local phoneString = panel.bg_PhoneInputField:GetComponent("InputField").text;
    --给除了密码以外的所有字符串去空格
    accountString = Utils.StringTrim(accountString);
    emailString = Utils.StringTrim(emailString);
    phoneString = Utils.StringTrim(phoneString);
    --检查是否符合规则
    if(accountString == "" or accountString == nil) then
        --账号为空
        TipText.ShowTipText("账号不能为空");
        return;
    end
    if(passwordString == "" or passwordString == nil) then
        --密码为空
        TipText.ShowTipText("密码不能为空");
        return;
    end
    --重复密码是否与密码相同
    local repeatPasswordString = panel.bg_RepeatPasswordInputField:GetComponent("InputField").text;
    if(repeatPasswordString ~= passwordString)then
        --密码与重复密码不同
        TipText.ShowTipText("密码与重复密码不同");
        return;
    end
    --用户名长度最少为四个字符
    local accountLength = Utils.GetStringLength(accountString);
    if(accountLength < 4)then
        TipText.ShowTipText("用户名长度最少为4个字符");
        return;
    end
    --用户名长度最高为十六个字符。因为在unity中有限定，所以这里判断不是必须
    if(accountLength > 16)then--utf-8编码中，一个字符占3个字节，一个英文占1个字节。。。真滴麻烦
        TipText.ShowTipText("用户名长度最多为16个字符");
        return;
    end
    --密码长度最少为6个字符
    local passwordLength = Utils.GetStringLength(passwordString);
    if(passwordLength < 6)then
        TipText.ShowTipText("密码长度最少为6个字符");
        return;
    end
    --密码长度最多为45个字符。因为在unity中有限定，所以这里判断不是必须
    if(passwordLength > 45)then
        TipText.ShowTipText("密码长度最多为45个字符");
        return;
    end
    --邮箱合法性验证。正则规则串：^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$
    if(emailString ~= nil and emailString ~= "")then
        local res = RegexUtil.IsMatch(emailString, "^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
        if(not res)then
            TipText.ShowTipText("邮箱格式错误");
            return;
        end
    end
    --手机号合法性验证。正则规则串：^(13[0-9]|14[5|7]|15[0|1|2|3|4|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$
    if(phoneString ~= nil and phoneString ~= "") then
        local res = RegexUtil.IsMatch(phoneString, "^(13[0-9]|14[5|7]|15[0|1|2|3|4|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\\d{8}$");
        if(not res)then
            TipText.ShowTipText("手机号格式错误");
            return;
        end
    end

    this.SendRegisterMsg(accountString, passwordString, emailString, phoneString);

    accountCache = accountString;
    passwordCache = passwordString;

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);

    --将按home或返回键的次数归零
    LoginCtrl.ResetClickHomeKeyCount();
end

--返回登陆界面按钮点击事件
function this.OnGoBackButtonClick()
    Panel.OpenPanel(CtrlNames.loginCtrl);
    Panel.ClosePanel(CtrlNames.registerCtrl);

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);

    --将按home或返回键的次数归零
    LoginCtrl.ResetClickHomeKeyCount();
end

---注册方法回调
function this.OnMsgRegisterBackHandleMethodCallback(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgLoginMethodStringID.RegisterReceive, data);
    if(msg.state == 1) then
        --注册成功
        TipText.ShowTipText("注册成功");

        --用新注册的账号密码登陆
        LoginCtrl.SendLoginMsg(accountCache, passwordCache);
    elseif msg.state == 2 then
        --用户名重复
        TipText.ShowTipText("注册失败，账号已存在");
    elseif msg.state == 3 then
        --数据库异常
        TipText.ShowTipText("注册失败，服务器异常");
    elseif msg.state == 4 then
        --名称不合法
        TipText.ShowTipText("用户名不符合规则，包含不允许使用的特殊字符");
    elseif msg.state == 5 then
        --服务器校验失败
        TipText.ShowTipText("服务器校验失败，请检查填写的信息");
    else
        TipText.ShowTipText("遇到意外的错误");
        FDebugger.LogWarning("当前情况未处理。state：" .. msg.state);
    end
end

function this.Close()

end

--- 发送注册消息
--- @param accountString string 账号字符串
--- @param passwordString string 密码字符串
--- @param emailString string 邮箱字符串
--- @param phoneString string 手机号字符串
--- @returns 无返回值
function this.SendRegisterMsg(accountString, passwordString, emailString, phoneString)
    local msg = MsgRegisterRequestTable:New();
    msg.username = accountString;
    msg.password = passwordString;
    msg.email = emailString;
    msg.phoneNumber = phoneString;

    local data = NetworkLuaManager.EncodeMsg(MsgLoginMethodStringID.RegisterRequest, msg);
    NetworkManager.Instance:SendMsgFromLua(MsgHandleID.Login, MsgLoginMethodID.RegisterRequest, data);
end
