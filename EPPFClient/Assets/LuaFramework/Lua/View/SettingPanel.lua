SettingPanel = {};
local this = SettingPanel;

function this.New(panelTransform)
	this.bg = panelTransform:Find("Bg");

	this.bg_Close = panelTransform:Find('Bg/Close');
	this.bg_Right_Buttons_LogOut = panelTransform:Find("Bg/Right/Buttons/LogOut");
	this.bg_Right_Buttons_QuitRoom = panelTransform:Find("Bg/Right/Buttons/QuitRoom");

	this.bg_Left_Viewport_Content = panelTransform:Find("Bg/Left/Viewport/Content");
	this.bg_Right_Viewport_Content = panelTransform:Find("Bg/Right/Viewport/Content");

	this.bg_Right_Viewport_Content_Setting_Volume_Background_Mute = panelTransform:Find('Bg/Right/Viewport/Content/Setting/Volume/Background/Mute');
	this.bg_Right_Viewport_Content_Setting_Volume_Effect_Mute = panelTransform:Find('Bg/Right/Viewport/Content/Setting/Volume/Effect/Mute');
	this.bg_Right_Viewport_Content_Setting_Broadcast_ShowInGame_Toggle = panelTransform:Find('Bg/Right/Viewport/Content/Setting/Broadcast/ShowInGame/Toggle');
	this.bg_Right_Viewport_Content_Setting_Volume_Background_Slider = panelTransform:Find('Bg/Right/Viewport/Content/Setting/Volume/Background/Slider');
	this.bg_Right_Viewport_Content_Setting_Volume_Effect_Slider = panelTransform:Find('Bg/Right/Viewport/Content/Setting/Volume/Effect/Slider');
	this.bg_Right_Viewport_Content_Setting_AutoLogin_AutoLogin_Toggle = panelTransform:Find('Bg/Right/Viewport/Content/Setting/AutoLogin/AutoLogin/Toggle');

	this.bg_Right_Viewport_Content_Help_GameVersion = panelTransform:Find('Bg/Right/Viewport/Content/Help/GameVersion');

    return this;
end
