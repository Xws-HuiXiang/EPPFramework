require "Managers/CtrlLuaManager"
require "Common/Panel"
require "Common/LuaTimer"
require "Protocol/MsgID"
require "Managers/NetworkLuaManager"
require "Common/LuaDefine"
require "Managers/UnityAdsLuaManager"

Main = {};
local this = Main;

function this.Main()
    --lua框架入口函数
    FDebugger.Log("启动Lua框架，Main函数执行");

    --初始化一些东西
    CtrlLuaManager.New();
    --UnityAda初始化
    --UnityAdsLuaManager.Init();

    --添加所有协议处理方法
    NetworkLuaManager.AddProtocolHandles();

    --创建例子的第一个面板
    Panel.OpenPanel(CtrlNames.theFirstCtrl);
end
