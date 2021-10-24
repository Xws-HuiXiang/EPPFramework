using LitJson;
using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UNOServer.Common;
using ProtoBuf;
using Ciphertext;

/// <summary>
/// 消息队列结构体
/// </summary>
public struct MsgQueue
{
    /// <summary>
    /// 消息队列结构体
    /// </summary>
    /// <param name="handleID"></param>
    /// <param name="handleMethodID"></param>
    /// <param name="msgByteArray"></param>
    public MsgQueue(int handleID, int handleMethodID, LuaByteBuffer msgByteArray)
    {
        this.HandleID = handleID;
        this.HandleMethodID = handleMethodID;
        this.MsgByteArray = msgByteArray;
    }

    public int HandleID { get; set; }
    public int HandleMethodID { get; set; }
    public LuaByteBuffer MsgByteArray { get; set; }
}

public class NetworkManager : MonoSingleton<NetworkManager>
{
    private static string publicKey = "EPPFrameworkServer";
    /// <summary>
    /// 公钥
    /// </summary>
    public static string PublicKey { get { return publicKey; } }

    private static string secretKey = null;
    /// <summary>
    /// 密钥
    /// </summary>
    public static string SecretKey { get { return secretKey; } }

    private static byte[] msgPrivateKey = null;
    /// <summary>
    /// 消息协议加密用的密钥
    /// </summary>
    public static byte[] MsgPrivateKey { get { return msgPrivateKey; } }
    private static byte[] msgSecretKey = null;
    /// <summary>
    /// 消息协议加密用的对称密钥
    /// </summary>
    public static byte[] MsgSecretKey { get { return msgSecretKey; } }

    /// <summary>
    /// 与服务器的连接套接字
    /// </summary>
    private Socket socket;
    /// <summary>
    /// 消息缓存
    /// </summary>
    private static MessageBuffer messageBuffer;

    /// <summary>
    /// 消息处理方法的字典
    /// </summary>
    private static Dictionary<int, Dictionary<int, Action<LuaByteBuffer>>> protocolHandleDict;

    /// <summary>
    /// 前台一直检测的消息队列
    /// </summary>
    private static Queue<MsgQueue> msgFrontDeskQueue;
    /// <summary>
    /// 后台一直检测的消息队列
    /// </summary>
    private static Queue<MsgQueue> msgBackStageQueue;
    /// <summary>
    /// 后台一直检测的消息队列的线程
    /// </summary>
    private static Thread backStageQueueThread = null;

    /// <summary>
    /// 心跳包发送时间
    /// </summary>
    public const long PING_INTERVAL = 30;

    private bool isConnection = false;
    /// <summary>
    /// 是否连接到游戏服务器。在游戏重启之前，只要连接过服务器则为true
    /// </summary>
    public bool IsConnection { get { return isConnection; } }

    /// <summary>
    /// 尝试连接次数
    /// </summary>
    private int connectionCount = 0;

    private void Update()
    {
        //处理消息队列
        if(msgFrontDeskQueue.Count > 0)
        {
            lock (msgFrontDeskQueue)
            {
                MsgQueue msgItem = msgFrontDeskQueue.Dequeue();
                if (protocolHandleDict.ContainsKey(msgItem.HandleID))
                {
                    Dictionary<int, Action<LuaByteBuffer>> dict = protocolHandleDict[msgItem.HandleID];
                    if (dict.ContainsKey(msgItem.HandleMethodID))
                    {
                        dict[msgItem.HandleMethodID].Invoke(msgItem.MsgByteArray);
                    }
                    else
                    {
                        FDebugger.LogWarningFormat("在ID为[{0}]的处理类中未找到ID为[{1}]的消息处理方法", msgItem.HandleID, msgItem.HandleMethodID);
                    }
                }
                else
                {
                    FDebugger.LogWarningFormat("未找到ID为[{0}]的协议处理类", msgItem.HandleID);
                }
            }
        }
    }

    /// <summary>
    /// 初始化NetworkManager这个管理器
    /// </summary>
    public static void Init()
    {
        NetworkManager.messageBuffer = new MessageBuffer();
        protocolHandleDict = new Dictionary<int, Dictionary<int, Action<LuaByteBuffer>>>();
        msgFrontDeskQueue = new Queue<MsgQueue>();
        msgBackStageQueue = new Queue<MsgQueue>();

        //开启后台线程处理后台消息队列
        backStageQueueThread = new Thread(BackStageQueueThreadAction);
        backStageQueueThread.IsBackground = true;
        backStageQueueThread.Start();
    }

    /// <summary>
    /// 向协议处理字典中添加一项
    /// </summary>
    /// <param name="handleID">协议处理类ID</param>
    /// <param name="handleMethodID">协议处理类中方法ID</param>
    /// <param name="msgAction">协议内容</param>
    public static void AddProtocolHandle(int handleID, int handleMethodID, Action<LuaByteBuffer> msgAction)
    {
        if (protocolHandleDict.ContainsKey(handleID))
        {
            Dictionary<int, Action<LuaByteBuffer>> dict = protocolHandleDict[handleID];
            if (dict.ContainsKey(handleMethodID))
            {
                FDebugger.LogWarningFormat("在ID为[{0}]消息处理类中已经存在ID为[{1}]的消息处理方法", handleID, handleMethodID);
            }
            else
            {
                dict.Add(handleMethodID, msgAction);
            }
        }
        else
        {
            Dictionary<int, Action<LuaByteBuffer>> dict = new Dictionary<int, Action<LuaByteBuffer>>();
            dict.Add(handleMethodID, msgAction);

            protocolHandleDict.Add(handleID, dict);
        }
    }
    
    /// <summary>
    /// 连接服务器
    /// </summary>
    public void Connection()
    {
        //连接服务器
        IPAddress ipAddress = IPAddress.Parse(AppConst.IP);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.BeginConnect(ipAddress, AppConst.Port, OnConnectCallback, socket);

        connectionCount++;
    }

    /// <summary>
    /// 连接成功回调
    /// </summary>
    /// <param name="ar"></param>
    private void OnConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = ar.AsyncState as Socket;
            socket.EndConnect(ar);

            socket.BeginReceive(messageBuffer.Data, messageBuffer.WriteIndex, messageBuffer.Remain, SocketFlags.None, OnReceiveCallback, socket);

            isConnection = true;

            FDebugger.Log("服务器已连接");

            //生成密钥对并发送给服务器
            GenerateClientMsgSecretKeyPair();
        }
        catch(SocketException e)
        {
            if(connectionCount < 5)
            {
                FDebugger.LogWarningFormat("连接服务器失败，第{0}次尝试再次连接", connectionCount);
                LoadingManager.Instance.SetProgressTextAsync(string.Format("连接服务器失败，第{0}次尝试再次连接", connectionCount));

                Connection();
            }
            else
            {
                LoadingManager.Instance.SetProgressTextAsync(string.Format("无法连接到服务器，请稍后重试"));

                FDebugger.LogError("无法连接到服务器，请稍后重试。错误信息：" + e.Message);
            }
        }
        catch (Exception e)
        {
            FDebugger.LogError("连接失败：" + e.Message);
        }
    }

    /// <summary>
    /// 接收信息回调
    /// </summary>
    /// <param name="ar"></param>
    private void OnReceiveCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = ar.AsyncState as Socket;

            if (socket != null && socket.Connected)
            {
                int count = socket.EndReceive(ar);
                if (count != 0)
                {
                    messageBuffer.WriteIndex += count;

                    //粘包分包处理
                    while (messageBuffer.Length > 12)
                    {
                        bool decodeRes = MessageBuffer.DecodeMsg(messageBuffer, NetworkManager.MsgSecretKey, out int protocolHandleID, out int protocolHandleMethodID, out byte[] msg);
                        if (decodeRes)
                        {
                            LuaByteBuffer byteArray = new LuaByteBuffer(msg);
                            MsgQueue queue = new MsgQueue(protocolHandleID, protocolHandleMethodID, byteArray);
                            //TODO：区分前台处理消息和后台处理消息。配置写到本地的config中
                            lock (msgFrontDeskQueue)
                            {
                                msgFrontDeskQueue.Enqueue(queue);
                            }
                        }
                        else
                        {
                            //消息不完整
                            break;
                        }
                    }

                    //如果缓冲区剩余空间太小则移动数据到开头
                    if (messageBuffer.Remain <= 12)
                    {
                        messageBuffer.CheckAndMoveData();

                        while (messageBuffer.Remain <= 0)
                        {
                            //当消息长度大于1024时，需要扩展缓冲区的大小
                            int expandSize = messageBuffer.Length < MessageBuffer.DEFAULT_SIZE ? MessageBuffer.DEFAULT_SIZE : messageBuffer.Length;
                            messageBuffer.ReSize(expandSize * 2);

                            FDebugger.Log("扩展了一次缓冲区大小，当前大小为：" + expandSize * 2);
                        }
                    }

                    //重新开始接收
                    if (socket != null && socket.Connected)
                    {
                        socket.BeginReceive(messageBuffer.Data, messageBuffer.WriteIndex, messageBuffer.Remain, SocketFlags.None, OnReceiveCallback, socket);
                    }
                    else
                    {
                        FDebugger.Log("准备接收信息时连接已关闭");
                    }
                }
                else
                {
                    //服务器关闭了连接
                    FDebugger.Log("服务器发送的数据长度为0，关闭了连接");
                    Close();
                }
            }
            else
            {
                //服务器关闭了连接
                FDebugger.Log("服务器关闭了连接");
                Close();
            }
        }
        catch(SocketException e)
        {
            FDebugger.LogError("服务器关闭了连接，错误信息：" + e.Message);

            GameManager.Instance.AddUpdateAction(SocketExceptionAction);

            return;
        }
        catch(Exception e)
        {
            FDebugger.LogError("接收信息时发生异常，错误信息：" + e.Message);

            GameManager.Instance.AddUpdateAction(ExceptionAction);

            return;
        }
    }

    /// <summary>
    /// OnReceiveCallback回调函数中，捕获SocketException异常时调用函数
    /// </summary>
    private void SocketExceptionAction()
    {
        LuaFunction func = GameManager.LuaState.GetFunction("NetworkLuaManager.SocketException");
        if(func != null)
        {
            func.Call("与服务器的连接已断开");
        }
        else
        {
            FDebugger.LogWarning("Lua端不存在NetworkLuaManager.SocketException方法");
        }

        GameManager.Instance.RemoveUpdateAction(SocketExceptionAction);
    }

    /// <summary>
    /// OnReceiveCallback回调函数中，捕获Exception异常时调用函数
    /// </summary>
    private void ExceptionAction()
    {
        LuaFunction func = GameManager.LuaState.GetFunction("NetworkLuaManager.Exception");
        if(func != null)
        {
            func.Call("接收信息时发生异常");
        }
        else
        {
            FDebugger.LogWarning("Lua端不存在NetworkLuaManager.Exception方法");
        }

        GameManager.Instance.RemoveUpdateAction(ExceptionAction);
    }

    /// <summary>
    /// 关闭当前客户端的连接
    /// </summary>
    private void Close()
    {
        if(socket != null)
        {
            socket.Close();
        }
    }

    /// <summary>
    /// 从Lua中发送消息到服务器
    /// </summary>
    /// <param name="protocolHandleID"></param>
    /// <param name="protocolMethodID"></param>
    /// <param name="bytes"></param>
    public void SendMsgFromLua(int protocolHandleID, int protocolMethodID, LuaByteBuffer bytes)
    {
        if (bytes.buffer.Length == 0)
        {
            FDebugger.LogWarningFormat("尝试发送一条空消息，处理ID为{0}，处理方法ID为{1}", protocolHandleID, protocolMethodID);

            return;
        }

        if (this.socket == null)
        {
            FDebugger.LogError("socket为空，等待网络连接后再发送消息");

            return;
        }
        if (!this.socket.Connected)
        {
            FDebugger.LogError("socket连接已断开，发送消息失败");

            return;
        }

        byte[] data = bytes.buffer;
        //使用AES加密。如果是获取密钥的协议则使用公钥加密，否则使用密钥加密
        if (protocolHandleID == 0 && protocolMethodID == 0)
        {
            data = AES.AESEncrypt(data, NetworkManager.PublicKey);
        }
        else
        {
            string msgSecretKeyString = DHExchange.GetString(NetworkManager.MsgSecretKey);
            data = AES.AESEncrypt(data, msgSecretKeyString);
        }

        //消息组拼
        data = MessageBuffer.EncodeMsg(protocolHandleID, protocolMethodID, data);

        try
        {
            this.socket.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
        }
        catch (Exception e)
        {
            FDebugger.LogErrorFormat(string.Format("发送消息失败：{0}", e.Message));
        }
    }

    /// <summary>
    /// 设置密钥
    /// </summary>
    /// <param name="secretKey"></param>
    public static void SetSecretKey(string secretKey)
    {
        NetworkManager.secretKey = secretKey;
    }

    /// <summary>
    /// 设置消息加密用的对称密钥
    /// </summary>
    /// <param name="clientMsgPublicKey">客户端公钥</param>
    public static void SetMsgSecretKey(string clientMsgPublicKey)
    {
        byte[] publicKeyByteArray = Convert.FromBase64String(clientMsgPublicKey);
        NetworkManager.msgSecretKey = DHExchange.GenerateKeySecret(NetworkManager.MsgPrivateKey, publicKeyByteArray);
    }
    /// <summary>
    /// 设置消息加密用的密钥
    /// </summary>
    /// <param name="msgPrivateKey"></param>
    public static void SetMsgPrivateKey(byte[] msgPrivateKey)
    {
        NetworkManager.msgPrivateKey = msgPrivateKey;
    }

    /// <summary>
    /// 后台执行的消息处理函数。单独跑在一个线程中
    /// </summary>
    private static void BackStageQueueThreadAction()
    {
        while (true)
        {
            //处理消息队列
            if (msgBackStageQueue.Count > 0)
            {
                MsgQueue msgItem = msgBackStageQueue.Dequeue();
                if (protocolHandleDict.ContainsKey(msgItem.HandleID))
                {
                    Dictionary<int, Action<LuaByteBuffer>> dict = protocolHandleDict[msgItem.HandleID];
                    if (dict.ContainsKey(msgItem.HandleMethodID))
                    {
                        dict[msgItem.HandleMethodID].Invoke(msgItem.MsgByteArray);
                    }
                    else
                    {
                        FDebugger.LogWarningFormat("在ID为[{0}]的处理类中未找到ID为[{1}]的消息处理方法", msgItem.HandleID, msgItem.HandleMethodID);
                    }
                }
                else
                {
                    FDebugger.LogWarningFormat("未找到ID为[{0}]的协议处理类", msgItem.HandleID);
                }
            }
        }
    }

    /// <summary>
    /// 生成客户端用于消息加密的密钥对并发送公钥给服务器
    /// </summary>
    private void GenerateClientMsgSecretKeyPair()
    {
        //生成客户端密钥对
        byte[] clientSocketPrivateKey = new byte[DHExchange.DH_KEY_LENGTH];
        byte[] clientSocketPublicKey = new byte[DHExchange.DH_KEY_LENGTH];
        DHExchange.GenerateKeyPair(clientSocketPublicKey, clientSocketPrivateKey);

        //保存私钥
        SetMsgPrivateKey(clientSocketPrivateKey);

        //发送公钥给服务器
        LuaFunction mainAwakeFun = GameManager.LuaState.GetFunction("AwakeMain.SendPublicKeyRequest");
        if (mainAwakeFun != null)
        {
            string publicKeyString = DHExchange.GetString(clientSocketPublicKey);
            mainAwakeFun.Call(publicKeyString);
        }
        else
        {
            FDebugger.LogWarning("不存在Main.SendPublicKeyRequest()方法。该方法在Main方法之前调用，用于发送客户端公钥给服务器");
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (socket != null)
        {
            socket.Close();
            socket.Dispose();
            socket = null;
        }
        if (backStageQueueThread != null)
        {
            backStageQueueThread.Abort();
        }
    }
}
