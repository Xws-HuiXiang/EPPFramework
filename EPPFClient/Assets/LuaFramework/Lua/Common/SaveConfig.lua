SaveConfig = {};
local this = SaveConfig;

---配置项字段
this.ConfigListField = {
    ---是否开启自动登陆
    OpenAutoLogin = "OpenAutoLogin",
    ---用户名
    Username = "Username",
    ---密码
    Password = "Password"
};

---设置配置项。方法内会先加密再写入文件
function this.SetConfigValue(key, value, autoSave)
    local keyString = Ciphertext.AES.AESEncryptToString(tostring(key), NetworkManager.SecretKey);
    local valueString = value;
    if(value ~= "" and value ~= nil)then
        valueString = Ciphertext.AES.AESEncryptToString(tostring(value), NetworkManager.SecretKey);
    end

    ConfigData.SetConfigValue(keyString, valueString, autoSave);
end

---读取配置项
function this.GetConfigValue(key, defaultValue)
    local keyString = Ciphertext.AES.AESEncryptToString(tostring(key), NetworkManager.SecretKey);
    local value = ConfigData.GetConfigValue(keyString, tostring(defaultValue));
    --因为如果没有key则返回默认值，所以这里判断是否为默认值再决定是否用AES解密
    if(value ~= tostring(defaultValue))then
        if(value == "" or value == nil)then
            --如果为nil或空串则返回空串
            value = "";
        else
            --解密。
            return Ciphertext.AES.AESDecryptToString(value, NetworkManager.SecretKey);
        end
    end

    return value;
end