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
    UnityAdsLuaManager.Init();
    --按钮音效
    GameLuaManager.ButtonAudioClip = ResourcesManager.Instance:GetAudioClipFromAssetBundle("AudioEffect", "ButtonEffect");

    --添加所有协议处理方法
    NetworkLuaManager.AddProtocolHandles();
    --初始化游戏的规则信息
    GameLuaManager.InitAllGameRule();

    --创建登陆面板
    Panel.OpenPanel(CtrlNames.loginCtrl);
end
