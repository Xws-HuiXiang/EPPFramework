Panel = {};
local this = Panel;

--打开指定名称的面板
function this.OpenPanel(ctrlName)
    CtrlLuaManager.CtrlList[ctrlName].OpenPanel();
end

--关闭指定名称的面板
function this.ClosePanel(ctrlName)
    PanelManager.ClosePanel(ctrlName);
end

--关闭最后一个打开的面板
function this.CloseLastPanel()
    PanelManager.CloseLastPanel();
end

--打开一个新面板并关闭上一个面板
function this.OpenPanelAndCloseLast(ctrlName)
    local lastPanelStruct = PanelManager.GetLastPanelStruct();
    
    this.OpenPanel(ctrlName);
    --this.ClosePanel(lastPanelStruct);
end
