require "Protocol/MsgID"
require "Protocol/MsgStringID"
require "Managers/NetworkLuaManager"

pb = require 'pb'
protoc = require "LuaProtobuf/protoc"

AwakeMain = {};
local this = AwakeMain;

---这个函数会在Main函数之前调用。用于处理业务逻辑之前的部分事件。这个函数调用时还没有连接服务器
function this.Awake()
    --注册获取私钥响应事件
    NetworkManager.AddProtocolHandle(MsgHandleID.Common, MsgCommonMethodID.SecretKeyReceive, NetworkLuaManager.OnSecretKeyReceive);

    --添加所有.pb的schema
    NetworkLuaManager.LoadProtocolSchema();
end

--- 发送获取公钥请求。这个方法是从C#侧调用的。调用时机为成功连接服务器后
function this.SendPublicKeyRequest(clientPublicKeyString)
    local msg = MsgSecretKeyRequestTable:New();

    msg.publicKey = clientPublicKeyString;

    local data = NetworkLuaManager.EncodeMsg(MsgCommonMethodStringID.SecretKeyRequest, msg);
    NetworkManager.Instance:SendMsgFromLua(MsgHandleID.Common, MsgCommonMethodID.SecretKeyRequest, data);
end
