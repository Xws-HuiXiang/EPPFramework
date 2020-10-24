require "Ctrl/TestLogonCtrl"
CtrlLuaManager = {};
local this = CtrlLuaManager;

this.CtrlList = {};

CtrlNames = {
	testLogonCtrl = "TestLogonCtrl",

};

function this.New()
	this.CtrlList[CtrlNames.testLogonCtrl] = TestLogonCtrl.New();

end
