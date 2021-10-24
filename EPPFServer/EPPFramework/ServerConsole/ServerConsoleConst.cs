using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPPFramework.ServerConsole
{
    public class ServerConsoleConst
    {
    }

    /// <summary>
    /// 命令类型
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// 查询类的命令
        /// </summary>
        Select = 0,
        /// <summary>
        /// 广播类的命令
        /// </summary>
        BroadCast = 1,
        /// <summary>
        /// 数据库操作类的命令
        /// </summary>
        MySQL = 2,
        /// <summary>
        /// 房间类的命令
        /// </summary>
        Room = 3,
        /// <summary>
        /// 修改数据类的命令
        /// </summary>
        ChangeData = 4,
        /// <summary>
        /// 控制台相关的命令
        /// </summary>
        Console = 5,
        /// <summary>
        /// 其他命令
        /// </summary>
        Other = 6
    }

    /// <summary>
    /// 命令ID
    /// </summary>
    public enum CommandID
    {
        Help = 0,
        Quit = 1,
        Clear = 2,
        SayHello = 3,

        PaoMaDeng = 1000,
        BroadCast = 1001,

        WriteMaJiangHuInfoToMySQL = 2000,//写入麻将胡牌信息到MySQL数据库中
        LoadMaJiangHuInfo = 2001,//将麻将胡牌信息加载到内存中
        UnloadMaJiangHuInfo = 2002,//将麻将胡牌信息从内存中卸载
    }
}
