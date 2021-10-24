using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EPPFramework.Common
{
    public class ConfigData
    {
        /// <summary>
        /// 
        /// </summary>
        public int ResVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int LuaVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataBaseConnectionString { get; set; }
        /// <summary>
        /// 客户端显示的版本号
        /// </summary>
        public string ShowGameVersion { get; set; }
        /// <summary>
        /// 加载提示文件的路径。如果以‘Local://’开头则以当前exe文件的路径为根路径；以‘http;//’或‘https://’开头则为网络连接；以‘File://’开头则为本机的文件完整路径。默认为local
        /// </summary>
        public string LoadingTipsFilePath { get; set; }
        private string defaultAvatarPath;
        /// <summary>
        /// 默认头像的文件夹完整路径。本机的绝对路径
        /// </summary>
        public string DefaultAvatarPath
        {
            get
            {
                return defaultAvatarPath;
            }
            set
            {
                defaultAvatarPath = value.Replace('/', Path.DirectorySeparatorChar);
            }
        }
        /// <summary>
        /// 服务器控制台相关指令
        /// </summary>
        public Command Command { get; set; }

        /// <summary>
        /// 创建默认的配置数据对象
        /// </summary>
        /// <returns></returns>
        public static ConfigData CreateDefaultConfig()
        {
            ConfigData obj = new ConfigData();

            obj.ResVersion = 1;
            obj.LuaVersion = 1;
            obj.DataBaseConnectionString = "server=127.0.0.1;port=3306;user=root;password=MySQLPassword;database=unogame;Allow User Variables=True;";
            obj.ShowGameVersion = "1.0.0";
            obj.LoadingTipsFilePath = "https://www.qinghuixiang.com/File/UNOHotfixFile/LoadingTips.txt";
            obj.DefaultAvatarPath = null;
            Command command = new Command();
            command.Active = false;
            command.CommandExePath = "";
            command.IP = "127.0.0.1";
            command.Port = 1132;
            obj.Command = command;

            return obj;
        }
    }

    /// <summary>
    /// 服务器控制台相关配置属性
    /// </summary>
    public class Command
    {
        /// <summary>
        /// 是否开启服务器控制台
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// 自动打开控制台的exe程序
        /// </summary>
        public bool AutoOpenExe { get; set; }
        /// <summary>
        /// 服务器控制台程序的完整路径（支持路由标头）
        /// </summary>
        public string CommandExePath { get; set; }
        /// <summary>
        /// 服务器控制台的IP，一般为本机127.0.0.1
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 服务器控制台程序占用的端口号
        /// </summary>
        public int Port { get; set; }
    }
}
