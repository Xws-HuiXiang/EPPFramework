using EPPFramework.Log;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPPFramework.DataBase
{
    public class ExampleDBUtil : DBUtilBase
    {
        private static ExampleDBUtil instance;
        public static ExampleDBUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExampleDBUtil();
                }

                return instance;
            }
        }

        /// <summary>
        /// 查询表中是否有指定的用户名
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool HasUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                Debug.L.Warn("查询表中是否有某个用户名时，提供的用户名为空");

                return false;
            }

            using (MySqlCommand cmd = new MySqlCommand("select * from user where username = @username", MySQLDataBase.Conn))
            {
                cmd.Parameters.AddWithValue("username", username);

                bool res = false;
                try
                {
                    using MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        res = true;
                    }
                    else
                    {
                        res = false;
                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.L.Error(string.Format("查询表中是否有指定的用户名 时数据库查询出错，错误信息：{0}", e.Message));
                }

                return res;
            }
        }
    }
}
