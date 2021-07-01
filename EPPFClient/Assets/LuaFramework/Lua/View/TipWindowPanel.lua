TipWindowPanel = {};
local this = TipWindowPanel;

function this.New(panelTransform)
	this.bg = panelTransform:Find('Bg');
	this.bg_OK = panelTransform:Find('Bg/OK');

	this.bg_Cancel = panelTransform:Find('Bg/Cancel');
	this.bg_SimpleOK = panelTransform:Find('Bg/SimpleOK');


	this.bg_Title_TitleTextBg_TitleText = panelTransform:Find('Bg/Title/TitleTextBg/TitleText');
	this.bg_TipText = panelTransform:Find('Bg/TipText');
	this.bg_OK_Text = panelTransform:Find('Bg/OK/Text');
	this.bg_Cancel_Text = panelTransform:Find('Bg/Cancel/Text');
	this.bg_SimpleOK_Text = panelTransform:Find('Bg/SimpleOK/Text');


    return this;
end
