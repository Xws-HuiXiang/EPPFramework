require 'View/TipWindowPanel'

TipWindowCtrl = {};
local this = TipWindowCtrl;
local panelStruct;--当前面板的结构对象
local panel;--当前物体对应的panel

---提示窗口样式
TipWindowCtrl.TipWindowStyle = {
    ---显示确定和取消按钮
    OKAndCancel = 0,
    ---只显示确定按钮
    OnlyOK = 1
};

---附加的 确定按钮事件
local okBtnEvent = nil;
---附加的 取消按钮事件
local cancelBtnEvent = nil;

function this.New()
    return this;
end

function this.OpenPanel()
    panelStruct = PanelManager.OpenPanel('Common', 'TipWindowPanel', this.Awake, this.Start, this.Enable, this.Close, true);
    panel = TipWindowPanel.New(panelStruct.panelGameObject.transform);
    panelStruct:Init();--执行周期函数
end

function this.Awake()
	UIEventBinds.ButtonAddOnClick(panel.bg_OK, this.OnOKButtonClick);
	UIEventBinds.ButtonAddOnClick(panel.bg_Cancel, this.OnCancelButtonClick);
	UIEventBinds.ButtonAddOnClick(panel.bg_SimpleOK, this.OnSimpleOKButtonClick);

    Panel.DOOpenPanelTween(panel.bg, nil);
end

function this.Start()

end

function this.Enable()

end

---显示提示窗口。取消按钮中自带关闭提示窗口的函数
function this.ShowTipWindow(tipString, windowStyle, okFunction, cancelFunction)
    Panel.OpenPanel(CtrlNames.tipWindowCtrl);

    --设置窗口样式
    if(windowStyle == this.TipWindowStyle.OKAndCancel)then
        panel.bg_OK.gameObject:SetActive(true);
        panel.bg_Cancel.gameObject:SetActive(true);
        panel.bg_SimpleOK.gameObject:SetActive(false);
    elseif(windowStyle == this.TipWindowStyle.OnlyOK)then
        panel.bg_OK.gameObject:SetActive(false);
        panel.bg_Cancel.gameObject:SetActive(false);
        panel.bg_SimpleOK.gameObject:SetActive(true);
    else
        FDebugger.LogWarning("指定了一个未定义的提示窗口样式，将使用默认的TipWindowStyle.OKAndCancel样式显示");

        panel.bg_OK.gameObject:SetActive(true);
        panel.bg_Cancel.gameObject:SetActive(true);
        panel.bg_SimpleOK.gameObject:SetActive(false);
    end
    okBtnEvent = okFunction;
    cancelBtnEvent = cancelFunction;
    --提示文字修改
    panel.bg_TipText:GetComponent("Text").text = tipString;
end

---确定按钮点击事件
function this.OnOKButtonClick()
    if(okBtnEvent ~= nil)then
        okBtnEvent();
    end

    Panel.DOClosePanelTween(panel.bg, function()
        Panel.ClosePanel(CtrlNames.tipWindowCtrl);
    end);

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);
end

---取消按钮点击事件
function this.OnCancelButtonClick()
    if(cancelBtnEvent ~= nil)then
        cancelBtnEvent();
    end

    Panel.DOClosePanelTween(panel.bg, function()
        Panel.ClosePanel(CtrlNames.tipWindowCtrl);
    end);

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);
end

---单个确定按钮点击事件
function this.OnSimpleOKButtonClick()
    if(okBtnEvent ~= nil)then
        okBtnEvent();
    end

    Panel.DOClosePanelTween(panel.bg, function()
        Panel.ClosePanel(CtrlNames.tipWindowCtrl);
    end);

    --播放按钮音效
    AudioManager.Instance:PlayEffect(GameLuaManager.ButtonAudioClip);
end

---修改标题文字
function this.ChangeTitleText(titleString)
    panel.bg_Title_TitleTextBg_TitleText:GetComponent("Text").text = titleString;
end

---修改确定按钮的文字
function this.ChangeOKBtnText(text)
    panel.bg_OK_Text:GetComponent("Text").text = text;
end

---修改取消按钮的文字
function this.ChangeCancelBtnText(text)
    panel.bg_Cancel_Text:GetComponent("Text").text = text;
end

---修改单个确定按钮的文字
function this.ChangeSimpleOKBtnText(text)
    panel.bg_SimpleOK_Text:GetComponent("Text").text = text;
end

function this.Close()
    okBtnEvent = nil;
    cancelBtnEvent = nil;
end
