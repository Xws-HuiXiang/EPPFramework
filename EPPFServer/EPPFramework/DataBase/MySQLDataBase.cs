using LitJson;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using EPPFramework.Common;
using EPPFramework.Log;

namespace EPPFramework.DataBase
{
    public class MySQLDataBase : Singleton<MySQLDataBase>
    {
        private static MySqlConnection conn;

        /// <summary>
        /// MySQL数据库连接对象
        /// </summary>
        public static MySqlConnection Conn { get { return conn; } }

        /// <summary>
        /// 初始化MySQL数据库
        /// </summary>
        /// <param name="connectionString">连接MySQL的字符串</param>
        public bool Init(string connectionString)
        {
            return Connection(connectionString);
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="connectionString"></param>
        private bool Connection(string connectionString)
        {
            conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();
                Debug.L.Debug("数据库已连接并打开");

                return true;
            }
            catch (MySqlException e)
            {
                Debug.L.Error("数据库连接失败：" + e.Message);
            }

            return false;
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseDataBase()
        {
            if(conn != null)
            {
                conn.Close();
                conn = null;
                Debug.L.Info("数据库关闭成功");
            }
        }

        /// <summary>
        /// 发送一个刷新mysql时间戳的线程方法。避免mysql八小时自动断开连接的问题
        /// </summary>
        public void MySQLRefreshThread()
        {
            while (true)
            {
                using(MySqlCommand cmd = new MySqlCommand("select `id` from goods where id = 1;", MySQLDataBase.Conn))
                {
                    using MySqlDataReader reader = cmd.ExecuteReader();
                }

                //4小时查询（刷新）一次
                Thread.Sleep(14400000);
            }
        }
    }
}
