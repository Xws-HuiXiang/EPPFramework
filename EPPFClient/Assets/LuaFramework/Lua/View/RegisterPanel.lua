RegisterPanel = {};
local this = RegisterPanel;

function this.New(panelTransform)
	--this.register = panelTransform:Find('Register');
    --this.goBack = panelTransform:Find("GoBack");
    --
    --this.accountNumberInputField = panelTransform:Find("AccountNumberInputField");
    --this.passwordInputField = panelTransform:Find("PasswordInputField");
    --this.emailInputField = panelTransform:Find("EmailInputField");
    --this.phoneInputField = panelTransform:Find("PhoneInputField");

    this.bg_AccountNumberInputField = panelTransform:Find('Bg/AccountNumberInputField');
    this.bg_PasswordInputField = panelTransform:Find('Bg/PasswordInputField');
    this.bg_RepeatPasswordInputField = panelTransform:Find('Bg/RepeatPasswordInputField');
    this.bg_EmailInputField = panelTransform:Find('Bg/EmailInputField');
    this.bg_PhoneInputField = panelTransform:Find('Bg/PhoneInputField');

    this.bg_Register = panelTransform:Find('Bg/Register');
    this.bg_GoBack = panelTransform:Find('Bg/GoBack');

    return this;
end
