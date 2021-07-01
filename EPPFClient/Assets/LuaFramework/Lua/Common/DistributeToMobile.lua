DistributeToMobile = {};
local this = DistributeToMobile;

this.Windows = "Windows";
this.Editor = "Windows";
this.Android = "Android";
this.IOS = "iOS";

---拷贝内容到剪切板
function this.CopyToClipboard(str)
    local platformName = AppConst.GetPlatformName();
    if(platformName == this.Windows)then
        UnityEngine.GUIUtility.systemCopyBuffer = str;
        TipText.ShowTipText("复制房间ID成功");
    elseif platformName == this.Editor then
        UnityEngine.GUIUtility.systemCopyBuffer = str;
        TipText.ShowTipText("复制房间ID成功");
    elseif platformName == this.Android then
        AndroidManager.Instance:CopyToClipboard(str);
    elseif platformName == this.IOS then
        TipText.ShowTipText("苹果端复制房间ID暂未实现");
    else
        FDebugger.LogError("未知的平台名称：" .. platformName .. " 方法调用失败");
    end
end

---弹一个toast提示
function this.MakeToast(text)
    local platformName = AppConst.GetPlatformName();
    if(platformName == this.Windows)then
        TipText.ShowTipText(text);
    elseif platformName == this.Editor then
        TipText.ShowTipText(text);
    elseif platformName == this.Android then
        AndroidManager.Instance:MakeToast(text);
    elseif platformName == this.IOS then
        TipText.ShowTipText("苹果端弹出土司提示的功能暂未实现");
    else
        FDebugger.LogError("未知的平台名称：" .. platformName .. " 方法调用失败");
    end
end