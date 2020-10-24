using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocols
{
    /// <summary>
    /// 初始化所有协议类（不包含继承Monobehaviour的类，在用到时需要自行添加）的集合类
    /// </summary>
    public Protocols()
    {
        //密钥请求相关协议
        MsgSecretKeyHandle secretKeyHandle = new MsgSecretKeyHandle();
        NetworkManager.AddProtocolHandle(secretKeyHandle.ProtocolHandleID, secretKeyHandle.ProtocolHandleMethodID, secretKeyHandle.SecretKeyMethodHandle);//密钥请求

        //游戏版本相关协议
        //GameVersionHandle gameVersionHandle = new GameVersionHandle(LoadingManager.Instance);
        //NetworkManager.AddProtocolHandle(gameVersionHandle.ProtocolHandleID, gameVersionHandle.ProtocolHandleMethodID, gameVersionHandle.GetGameHotFixVersion);//获取服务器热更资源版本
    }
}
