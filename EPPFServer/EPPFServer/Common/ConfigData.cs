using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EPPFServer.Common
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

            return obj;
        }
    }
}
