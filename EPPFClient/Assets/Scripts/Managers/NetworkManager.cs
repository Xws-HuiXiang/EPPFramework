using CommonProtocol;
using LitJson;
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
    public MsgQueue(int handleID, int handleMethodID, byte[] msgByteArray)
    {
        this.HandleID = handleID;
        this.HandleMethodID = handleMethodID;
        this.MsgByteArray = msgByteArray;
    }

    public int HandleID { get; set; }
    public int HandleMethodID { get; set; }
    public byte[] MsgByteArray { get; set; }
}

public class NetworkManager : MonoSingleton<NetworkManager>
{
    private static string publicKey = "UNOServer";
    /// <summary>
    /// 公钥
    /// </summary>
    public static string PublicKey { get { return publicKey; } }

    private static string secretKey = null;
    /// <summary>
    /// 密钥
    /// </summary>
    public static string SecretKey { get { return secretKey; } }

    /// <summary>
    /// 与服务器的连接套接字
    /// </summary>
    private Socket socket;
    /// <summary>
    /// 消息缓存
    /// </summary>
    private static MessageUtils messageUtils;

    /// <summary>
    /// 消息处理方法的字典
    /// </summary>
    private static Dictionary<int, Dictionary<int, Action<byte[]>>> protocolHandleDict;

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
    private const long pingInterval = 30;
    /// <summary>
    /// 心跳包发送计时器
    /// </summary>
    private float pingIntervalTimer = 0;

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
                    Dictionary<int, Action<byte[]>> dict = protocolHandleDict[msgItem.HandleID];
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

        //心跳包
        pingIntervalTimer += Time.deltaTime;
        if (pingIntervalTimer > pingInterval)
        {
            //发送心跳包
            SendLastPing();
        }
    }

    public static void Init()
    {
        NetworkManager.messageUtils = new MessageUtils();
        protocolHandleDict = new Dictionary<int, Dictionary<int, Action<byte[]>>>();
        msgFrontDeskQueue = new Queue<MsgQueue>();
        msgBackStageQueue = new Queue<MsgQueue>();
        //添加所有协议
        new Protocols();

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
    public static void AddProtocolHandle(int handleID, int handleMethodID, Action<byte[]> msgAction)
    {
        if (protocolHandleDict.ContainsKey(handleID))
        {
            Dictionary<int, Action<byte[]>> dict = protocolHandleDict[handleID];
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
            Dictionary<int, Action<byte[]>> dict = new Dictionary<int, Action<byte[]>>();
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

            socket.BeginReceive(messageUtils.Data, messageUtils.WriteIndex, messageUtils.Remain, SocketFlags.None, OnReceiveCallback, socket);
            //连接成功后需要等待服务器返回密钥才能有后续操作
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
            int count = socket.EndReceive(ar);
            if(count != 0)
            {
                messageUtils.WriteIndex += count;
                MessageUtils.DecodeMsg(messageUtils, out int protocolHandleID, out int protocolHandleMethodID, out byte[] msgByteArray);
                MsgQueue queue = new MsgQueue(protocolHandleID, protocolHandleMethodID, msgByteArray);

                //添加消息到待处理队列。根据处理类ID和处理方法ID手动区分是后台处理还是前台处理
                //if (protocolHandleID == (int)MsgHandle.Ping && protocolHandleMethodID == (int)MsgPingMethod.Ping)
                //{
                //    //心跳处理协议，添加到后台
                //    msgBackStageQueue.Enqueue(queue);
                //}
                //else
                //{
                lock (msgFrontDeskQueue)
                {
                    msgFrontDeskQueue.Enqueue(queue);
                }
                //}

                //重新开始接收
                if (socket != null && socket.Connected)
                {
                    socket.BeginReceive(messageUtils.Data, messageUtils.WriteIndex, messageUtils.Remain, SocketFlags.None, OnReceiveCallback, socket);
                }
                else
                {
                    FDebugger.Log("准备接收信息时连接已关闭");
                }
            }
            else
            {
                //服务器关闭了连接
                FDebugger.Log("服务器关闭了连接");
                Close();
            }
        }
        catch(Exception e)
        {
            FDebugger.LogError("接收信息时发生异常：" + e.Message);

            return;
        }
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
    /// 发送消息到服务器
    /// </summary>
    /// <param name="protocolHandleID"></param>
    /// <param name="protocolMethodID"></param>
    /// <param name="msg"></param>
    public void SendMsg(int protocolHandleID, int protocolMethodID, MsgBase msg)
    {
        if(this.socket == null)
        {
            FDebugger.LogError("socket为空，等待网络连接后再发送消息");
        }
        if (!this.socket.Connected)
        {
            FDebugger.LogError("socket连接已断开，发送消息失败");
        }

        //以特定的格式组拼消息
        byte[] data = MessageUtils.EncodeMsg(protocolHandleID, protocolMethodID, msg);

        try
        {
            this.socket.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
        }
        catch (Exception e)
        {
            Debug.LogError("发送消息失败：" + e.Message);
        }
    }

    /// <summary>
    /// 发送消息到服务器
    /// </summary>
    /// <param name="protocolHandleID"></param>
    /// <param name="protocolMethodID"></param>
    /// <param name="msg"></param>
    public void SendMsg(MsgBase msg)
    {
        SendMsg(msg.ProtocolHandleID, msg.ProtocolHandleMethodID, msg);
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
    /// 根据消息数组反序列化获取对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="msgBytes"></param>
    /// <returns></returns>
    public static T GetObjectOfBytes<T>(byte[] msgBytes) where T : class
    {
        T obj = null;
        string msgJsonString = Encoding.UTF8.GetString(msgBytes);
        try
        {
            obj = JsonMapper.ToObject<T>(msgJsonString);
        }
        catch(Exception e)
        {
            FDebugger.LogErrorFormat("反序列化Json串<{0}>出错：{1}", msgJsonString, e.Message);
        }

        return obj;
    }

    /// <summary>
    /// 后台执行的消息处理函数
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
                    Dictionary<int, Action<byte[]>> dict = protocolHandleDict[msgItem.HandleID];
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
    /// 发送心跳包
    /// </summary>
    public void SendLastPing()
    {
        LastPing lastPing = new LastPing();
        lastPing.lastPingTime = MessageUtils.GetTimeStamp();
        NetworkManager.Instance.SendMsg(lastPing);

        pingIntervalTimer = 0;
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
