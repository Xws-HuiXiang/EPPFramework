Panel = {};
local this = Panel;

--打开指定名称的面板
function this.OpenPanel(ctrlName)
    if(ctrlName == nil or ctrlName == "")then
        FDebugger.LogWarning("打开面板时提供的名称为空");

        return;
    end

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

---检查指定面板是否处于开启状态（在MainCanvas下有没有对应名称的物体）
function this.CheckPanelIsOpenInMainCanvas(ctrlName, includeDisableState)
    return PanelManager.CheckPanelIsOpenInMainCanvas(ctrlName, includeDisableState);
end

---检查指定面板是否处于开启状态（在UpperCanvas下有没有对应名称的物体）
function this.CheckPanelIsOpenInUpperLayerCanvas(ctrlName, includeDisableState)
    return PanelManager.CheckPanelIsOpenInUpperLayerCanvas(ctrlName, includeDisableState);
end

---面板开启的动画
function this.DOOpenPanelTween(canvasGroupTrans, onComplete)
    if(canvasGroupTrans ~= nil)then
        local canvasGroup = canvasGroupTrans:GetComponent("CanvasGroup");

        if(canvasGroup ~= nil)then
            --初始值
            canvasGroup.alpha = 0;
            canvasGroupTrans.localScale = Vector3.New(0.75, 0.75, 0.75);

            --渐显
            canvasGroup:DOFade(1, 0.15);
            --缩放
            local tween = canvasGroupTrans:DOScale(Vector3.one, 0.15):SetEase(DG.Tweening.Ease.InOutBack);
            if(onComplete ~= nil)then
                tween:OnComplete(function()
                    onComplete();
                end);
            end
        else
            FDebugger.LogWarning("尝试播放面板开启动画，但指定的物体上不存在‘CanvasGroup’组件");
        end
    else
        FDebugger.LogWarning("尝试为空物体播放面板打开动画，请指定一个带有‘CanvasGroup’组件的物体");
    end
end

---面板关闭的动画
function this.DOClosePanelTween(canvasGroupTrans, onComplete)
    if(canvasGroupTrans ~= nil)then
        local canvasGroup = canvasGroupTrans:GetComponent("CanvasGroup");

        if(canvasGroup ~= nil)then
            --初始值
            canvasGroup.alpha = 1;

            --渐显
            canvasGroup:DOFade(0, 0.12):SetEase(DG.Tweening.Ease.InBack);
            --缩放
            local tween = canvasGroupTrans:DOScale(Vector3.New(0.55, 0.55, 0.55), 0.12):SetEase(DG.Tweening.Ease.InBack);
            if(onComplete ~= nil)then
                tween:OnComplete(function()
                    onComplete();
                end);
            end
        else
            FDebugger.LogWarning("尝试播放面板开启动画，但指定的物体上不存在‘CanvasGroup’组件");
        end
    else
        FDebugger.LogWarning("尝试为空物体播放面板打开动画，请指定一个带有‘CanvasGroup’组件的物体");
    end
end

---关闭所有面板
function this.CloseAllPanel()
    PanelManager.CloseAllPanel();
end

---获取最后一个打开的面板的名称。没有打开面板则返回nil
function this.GetLastPanelCtrlName()
    local lastPanelStruct = PanelManager.GetLastPanelStruct();
    if(lastPanelStruct ~= nil)then
        return PanelNameUtil.TryGetCtrlName(lastPanelStruct.panelGameObject.name);
    end

    return nil;
end

---获取场景中的面板物体
function this.GetPanelGameObject(ctrlName)
    local panelName = PanelNameUtil.TryGetPanelName(ctrlName);

    return PanelManager.MainCanvasRectTrans:Find(panelName);
end