LoginPanel = {};
local this = LoginPanel;

function this.New(panelTransform)
    this.bg_Login = panelTransform:Find('Bg/Login');
    this.bg_Register = panelTransform:Find('Bg/Register');
    this.bg_AccountNumberInputField = panelTransform:Find('Bg/AccountNumberInputField');
    this.bg_PasswordInputField = panelTransform:Find('Bg/PasswordInputField');

    this.bg_Tips = panelTransform:Find('Bg/Tips');

    return this;
end
