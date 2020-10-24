require 'View/TestLogonPanel'

TestLogonCtrl = {};
local this = TestLogonCtrl;
local panelStruct;--当前面板的结构对象
local panel;--当前物体对应的panel

function this.New()
    return this;
end

function this.OpenPanel()
    panelStruct = PanelManager.OpenPanel('MainPage.ab', 'TestLogonPanel', this.Awake, this.Start, this.Enable, this.Close, true);
    panel = TestLogonPanel.New(panelStruct.panelGameObject.transform);
    panelStruct:Init();--执行周期函数
end

function this.Awake()



end

function this.Start()

end

function this.Enable()

end




function this.Close()

end
