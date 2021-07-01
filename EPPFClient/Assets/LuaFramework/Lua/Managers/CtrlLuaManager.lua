require "Ctrl/Game/OthelloGameCtrl"
require "Ctrl/AddQuickPhraseCtrl"
require "Ctrl/RoomPasswordCtrl"
require "Ctrl/AvatarSelectCtrl"
require "Ctrl/LoadingCtrl"
require "Ctrl/AddShoppingCastAmountCtrl"
require "Ctrl/QuickPhraseCtrl"
require "Ctrl/PersonalInfoCtrl"
require "Ctrl/RankingCtrl"
require "Ctrl/MallCtrl"
require "Ctrl/ActivityCtrl"
require "Ctrl/AboutCtrl"
require "Ctrl/JoinRoomCtrl"
require "Ctrl/GameRuleInfoCtrl"
require "Ctrl/PaoMaDengCtrl"
require "Ctrl/TipWindowCtrl"
require "Ctrl/GameResult/UNOGameResultCtrl"
require "Ctrl/WildCardSelectColorCtrl"
require "Ctrl/RoomListCtrl"
require "Ctrl/SettingCtrl"
require "Ctrl/Game/UNOGameCtrl"
require "Ctrl/MainPageCtrl"
require "Ctrl/CreateRoomCtrl"
require "Ctrl/RegisterCtrl"
require "Ctrl/LoginCtrl"

CtrlLuaManager = {};
local this = CtrlLuaManager;

this.CtrlList = {};

CtrlNames = {
	othelloGameCtrl = "OthelloGameCtrl",
	addQuickPhraseCtrl = "AddQuickPhraseCtrl",
	roomPasswordCtrl = "RoomPasswordCtrl",
	avatarSelectCtrl = "AvatarSelectCtrl",
	loadingCtrl = "LoadingCtrl",
	addShoppingCastAmountCtrl = "AddShoppingCastAmountCtrl",
	quickPhraseCtrl = "QuickPhraseCtrl",
	personalInfoCtrl = "PersonalInfoCtrl",
	rankingCtrl = "RankingCtrl",
	mallCtrl = "MallCtrl",
	activityCtrl = "ActivityCtrl",
	aboutCtrl = "AboutCtrl",
	joinRoomCtrl = "JoinRoomCtrl",
	gameRuleInfoCtrl = "GameRuleInfoCtrl",
	paoMaDengCtrl = "PaoMaDengCtrl",
	tipWindowCtrl = "TipWindowCtrl",
	unoGameResultCtrl = "UNOGameResultCtrl",
	wildCardSelectColorCtrl = "WildCardSelectColorCtrl",
	roomListCtrl = "RoomListCtrl",
	settingCtrl = "SettingCtrl",
	unoGameCtrl = "UNOGameCtrl",
	mainPageCtrl = "MainPageCtrl",
	createRoomCtrl = "CreateRoomCtrl",
	registerCtrl = "RegisterCtrl",
	loginCtrl = "LoginCtrl",
};

function this.New()
	this.CtrlList[CtrlNames.othelloGameCtrl] = OthelloGameCtrl.New();
	this.CtrlList[CtrlNames.addQuickPhraseCtrl] = AddQuickPhraseCtrl.New();
	this.CtrlList[CtrlNames.roomPasswordCtrl] = RoomPasswordCtrl.New();
	this.CtrlList[CtrlNames.avatarSelectCtrl] = AvatarSelectCtrl.New();
	this.CtrlList[CtrlNames.loadingCtrl] = LoadingCtrl.New();
	this.CtrlList[CtrlNames.addShoppingCastAmountCtrl] = AddShoppingCastAmountCtrl.New();
	this.CtrlList[CtrlNames.quickPhraseCtrl] = QuickPhraseCtrl.New();
	this.CtrlList[CtrlNames.personalInfoCtrl] = PersonalInfoCtrl.New();
	this.CtrlList[CtrlNames.rankingCtrl] = RankingCtrl.New();
	this.CtrlList[CtrlNames.mallCtrl] = MallCtrl.New();
	this.CtrlList[CtrlNames.activityCtrl] = ActivityCtrl.New();
	this.CtrlList[CtrlNames.aboutCtrl] = AboutCtrl.New();
	this.CtrlList[CtrlNames.joinRoomCtrl] = JoinRoomCtrl.New();
	this.CtrlList[CtrlNames.gameRuleInfoCtrl] = GameRuleInfoCtrl.New();
	this.CtrlList[CtrlNames.paoMaDengCtrl] = PaoMaDengCtrl.New();
	this.CtrlList[CtrlNames.tipWindowCtrl] = TipWindowCtrl.New();
	this.CtrlList[CtrlNames.unoGameResultCtrl] = UNOGameResultCtrl.New();
	this.CtrlList[CtrlNames.wildCardSelectColorCtrl] = WildCardSelectColorCtrl.New();
	this.CtrlList[CtrlNames.roomListCtrl] = RoomListCtrl.New();
	this.CtrlList[CtrlNames.settingCtrl] = SettingCtrl.New();
	this.CtrlList[CtrlNames.unoGameCtrl] = UNOGameCtrl.New();
	this.CtrlList[CtrlNames.mainPageCtrl] = MainPageCtrl.New();
	this.CtrlList[CtrlNames.createRoomCtrl] = CreateRoomCtrl.New();
	this.CtrlList[CtrlNames.registerCtrl] = RegisterCtrl.New();
    this.CtrlList[CtrlNames.loginCtrl] = LoginCtrl.New();
end
