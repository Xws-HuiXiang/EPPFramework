require "Ctrl/TheFirstCtrl"

CtrlLuaManager = {};
local this = CtrlLuaManager;

this.CtrlList = {};

CtrlNames = {
	theFirstCtrl = "TheFirstCtrl",
};

function this.New()
	this.CtrlList[CtrlNames.theFirstCtrl] = TheFirstCtrl.New();
end
