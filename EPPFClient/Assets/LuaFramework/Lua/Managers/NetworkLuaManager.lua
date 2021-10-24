require "Protocol/MsgID"
require "Protocol/MsgStringID"
require "Protocol/Tables/CommonTable"
require "Common/LuaTimer"

NetworkLuaManager = {};
local this = NetworkLuaManager;

---每个消息的包名
this.msgPackageName = "Example.";

---加载全部.pb文件的协议
function this.LoadProtocolSchema()
    local path = AppConst.ProtoFilePath;
    FDebugger.Log("proto文件路径：" .. path);
    local pbFileArray = Utils.GetFilesByDirectory(path, "*.*", {".meta"});
    for i = 0, pbFileArray.Length - 1 do
        assert(pb.loadfile(pbFileArray[i]));
    end
end

---添加所有的协议处理方法
function this.AddProtocolHandles()
    --NetworkManager.AddProtocolHandle(MsgHandleID.Login, MsgLoginMethodID.LoginReceive, LoginCtrl.OnMsgLoginHandleMethodCallback);
end

---获取私钥响应
function this.OnSecretKeyReceive(data)
    --注意decode的第一个参数，如果proto带有package，则需要写package.message
    local msg = NetworkLuaManager.DecodeMsg(MsgCommonMethodStringID.SecretKeyReceive, data);
    NetworkManager.SetSecretKey(msg.secretKey);
    NetworkManager.SetMsgSecretKey(msg.msgPublicKey);

    --收到密钥以后，持续发送心跳包
    LuaTimer.AddLoopTimer(NetworkManager.PING_INTERVAL, this.SendLastPingMsg, true);
end

---发送心跳包
function this.SendLastPingMsg()
    local msg = MsgHeartbeatRequestTable:New();
    --用C#传回string，再转为number。直接传long貌似不支持这个类型
    msg.pingTime = tonumber(Utils.GetTimeStampString());

    --local data = assert(pb.encode('QiLieGuaner.HeartbeatRequest', msg));
    local data = NetworkLuaManager.EncodeMsg(MsgCommonMethodStringID.HeartbeatRequest, msg);
    NetworkManager.Instance:SendMsgFromLua(MsgHandleID.Common, MsgCommonMethodID.HeartbeatRequest, data);

    FDebugger.Log("发送心跳包");
end

--- 使用pb.encode序列化一个消息
--- @param msgStringID string 消息的字符串标识
--- @param msg string 消息表对象
--- @returns 返回值：使用protobuf序列化后的字节数组
function this.EncodeMsg(msgStringID, msg)
    local fullMsgName = this.msgPackageName .. msgStringID;
    local data = assert(pb.encode(fullMsgName, msg));

    return data;
end

--- 使用pb.decode反序列化一个消息
--- @param msgStringID string 消息的字符串标识
--- @param data any 消息字节数组，参数为字节数组类型
--- @returns 返回值：使用protobuf反序列化后的消息对象
function this.DecodeMsg(msgStringID, data)
    local fullMsgName = this.msgPackageName .. msgStringID;
    local msg = assert(pb.decode(fullMsgName, data));

    return msg;
end

---组合包名和提供的类型名 返回完整的类型名字符串
function this.CombineFullPbName(name)
    return this.msgPackageName .. name;
end

---NetworkManager的异常捕获
function this.SocketException(exceptionTipString)
    FDebugger.LogError("网络异常，错误信息：" .. exceptionTipString);

    TipWindowCtrl.ShowTipWindow(exceptionTipString, TipWindowCtrl.TipWindowStyle.OnlyOK, nil, nil);
end

---NetworkManager的异常捕获
function this.Exception(exceptionTipString)
    FDebugger.LogError("捕获了一个异常，错误信息：" .. exceptionTipString);

    TipWindowCtrl.ShowTipWindow(exceptionTipString, TipWindowCtrl.TipWindowStyle.OnlyOK, nil, nil);
end

---服务器关闭了与客户端的连接
function this.OnLogOutReceive(data)
    local msg = NetworkLuaManager.DecodeMsg(MsgLoginMethodStringID.LogOutReceive, data);

    TipWindowCtrl.ShowTipWindow("该账号在另一设备上登陆。如果这不是您自己操作，请立即修改密码", TipWindowCtrl.TipWindowStyle.OnlyOK, nil, nil);
end