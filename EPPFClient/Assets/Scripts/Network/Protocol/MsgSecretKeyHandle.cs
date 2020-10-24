using CommonProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MsgSecretKeyHandle : MsgBase
{
    public MsgSecretKeyHandle()
    {
        base.ProtocolHandleID = (int)MsgSecretKeyHandleMethod.GetSecretKey;
        base.ProtocolHandleMethodID = (int)MsgSecretKeyHandleMethod.GetSecretKey;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="msgBytes"></param>
    public void SecretKeyMethodHandle(byte[] msgBytes)
    {
        MsgSecretKey msg = NetworkManager.GetObjectOfBytes<MsgSecretKey>(msgBytes);
        if(msg != null)
        {
            NetworkManager.SetSecretKey(msg.secretKey);
            FDebugger.Log("密钥：" + msg.secretKey);

            //跳转到加载界面
            SceneManager.LoadScene(AppConst.SceneNameList[1]);

            //收到密钥后发送一次心跳包
            NetworkManager.Instance.SendLastPing();
        }
        else
        {
            FDebugger.LogError("服务器返回的对象数组无法反序列化为MsgSecretKey对象");
        }
    }
}
