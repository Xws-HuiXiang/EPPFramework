using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using EPPFServer.DataBase.Models;
using EPPFServer.Log;

namespace EPPFServer.DataBase
{
    public class UserDBUtil : DBUtilBase
    {
        private static UserDBUtil instance;
        public static UserDBUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserDBUtil();
                }

                return instance;
            }
        }

        /// <summary>
        /// 根据用户GUID查询该用户的金币数量
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public int SelectUserCoinByGUID(int guid)
        {
            using (MySqlCommand cmd = new MySqlCommand("select `goldcoin` from user where guid = @guid;", MySQLDataBase.Conn))
            {
                cmd.Parameters.AddWithValue("guid", guid);

                int res = -1;
                try
                {
                    using MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        //有查询结果
                        res = reader.GetInt32(0);
                    }
                    else
                    {
                        //没有查询到玩家，返回-1
                        res = -1;
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.L.Error(string.Format("根据用户GUID查询该用户的金币数量 时出错，错误信息：", e.Message));
                }

                return res;
            }
        }

        /// <summary>
        /// 根据玩家GUID查询玩家头像图片的名称
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public int SelectUserAvatarByGUID(int guid)
        {
            using (MySqlCommand cmd = new MySqlCommand("select avatar from user where guid = @guid;", MySQLDataBase.Conn))
            {
                cmd.Parameters.AddWithValue("guid", guid);

                int res = 0;
                try
                {
                    using MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        //有查询结果
                        res = reader.GetInt32(0);
                    }
                    else
                    {
                        //数据库中表为空，返回1作为默认值
                        res = 1;
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.L.Error(string.Format("根据玩家GUID查询玩家头像图片的名称 时出错，错误信息：", e.Message));
                }

                return res;
            }
        }

        /// <summary>
        /// 查询User中最大的GUID
        /// </summary>
        /// <returns></returns>
        public int SelectMaxUserGUID()
        {
            using (MySqlCommand cmd = new MySqlCommand("select `guid` from user order by `guid` desc limit 1;", MySQLDataBase.Conn))
            {
                int res = 100000;
                try
                {
                    using MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        //有查询结果
                        res = reader.GetInt32(0);
                    }
                    else
                    {
                        //数据库中表为空，返回100000作为默认值
                        res = 100000;
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.L.Error(string.Format("查询User中最大的GUID 时出错，错误信息：", e.Message));
                }

                return res;
            }
        }

        /// <summary>
        /// 根据guid查询玩家昵称
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string SelectPlayerNameByGUID(int guid)
        {
            using (MySqlCommand cmd = new MySqlCommand("select `username` from user where guid = @guid;", MySQLDataBase.Conn))
            {
                cmd.Parameters.AddWithValue("guid", guid);

                string res = null;
                try
                {
                    using MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        //有查询结果
                        res = reader.GetString(0);
                    }
                    else
                    {
                        //不存在指定的guid
                        res = null;
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.L.Error(string.Format("根据guid查询玩家昵称 时出错，错误信息：", e.Message));
                }

                return res;
            }
        }

        /// <summary>
        /// 用户登陆时检查账号密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User SelectUserByUsernameAndPassword(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Debug.L.Warn("用户登陆时，用户名或密码为空");

                return null;
            }

            using(MySqlCommand cmd = new MySqlCommand("select `username`, `guid`, `goldcoin` from user where username = @username and password = @password;", MySQLDataBase.Conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);

                User res = null;
                try
                {
                    using MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        res = new User();
                        //账号密码正确
                        res.UserName = reader.GetString("username");
                        res.GUID = reader.GetInt32("guid");
                        res.GoldCoin = reader.GetInt32("goldcoin");
                    }
                    else
                    {
                        //账号或密码错误
                        res = null;
                    }

                    reader.Close();
                }
                catch(Exception e)
                {
                    Debug.L.Error(string.Format("用户登陆时检查账号密码 时数据库查询出错，错误信息：{0}", e.Message));
                }

                return res;
            }
        }

        /// <summary>
        /// 根据邮箱和密码返回用户数据
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User SelectUserByEmailAndPassword(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Debug.L.Warn("根据邮箱和密码返回用户数据，邮箱或密码为空");

                return null;
            }

            MySqlCommand cmd = new MySqlCommand("select `username`, `guid`, `goldcoin` from user where email = @email and password = @password;", MySQLDataBase.Conn);

            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("password", password);

            User res = null;
            try
            {
                using MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    res = new User();
                    //邮箱密码正确
                    res.UserName = reader.GetString("username");
                    res.GUID = reader.GetInt32("guid");
                    res.GoldCoin = reader.GetInt32("goldcoin");
                }
                else
                {
                    //邮箱或密码错误
                    res = null;
                }

                reader.Close();
            }
            catch (Exception e)
            {
                Debug.L.Error(string.Format("根据邮箱和密码返回用户数据 时数据库查询出错，错误信息：{0}", e.Message));
            }

            return res;
        }

        /// <summary>
        /// 根据手机号和密码返回用户数据
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User SelectUserByPhoneAndPassword(string phone, string password)
        {
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                Debug.L.Warn("根据手机号和密码返回用户数据，邮箱或密码为空");

                return null;
            }

            MySqlCommand cmd = new MySqlCommand("select `username`, `guid`, `goldcoin` from user where phone = @phone and password = @password;", MySQLDataBase.Conn);

            cmd.Parameters.AddWithValue("phone", phone);
            cmd.Parameters.AddWithValue("password", password);

            User res = null;
            try
            {
                using MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    res = new User();
                    //手机号密码正确
                    res.UserName = reader.GetString("username");
                    res.GUID = reader.GetInt32("guid");
                    res.GoldCoin = reader.GetInt32("goldcoin");
                }
                else
                {
                    //手机号或密码错误
                    res = null;
                }

                reader.Close();
            }
            catch (Exception e)
            {
                Debug.L.Error(string.Format("根据手机号和密码返回用户数据 时数据库查询出错，错误信息：{0}", e.Message));
            }

            return res;
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

        /// <summary>
        /// 向User中添加一条新用户
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="goldCoin"></param>
        /// <param name="avatar"></param>
        /// <returns></returns>
        public bool InsertNewUser(int guid, string username, string password, string email, string phoneNumber, int goldCoin, int avatar)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Debug.L.Warn("注册新用户时，用户名或密码为空");

                return false;
            }

            using(MySqlCommand cmd = new MySqlCommand("insert into user (guid, username, password, email, phone, goldcoin, avatar) value(@guid, @username, @password, @email, @phone, @goldCoin, @avatar);", MySQLDataBase.Conn))
            {
                cmd.Parameters.AddWithValue("guid", guid);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("phone", phoneNumber);
                cmd.Parameters.AddWithValue("goldCoin", goldCoin);
                cmd.Parameters.AddWithValue("avatar", avatar);

                bool res = false;
                try
                {
                    int count = cmd.ExecuteNonQuery();
                    if(count == 1)
                    {
                        res = true;
                    }
                    else if(count != 0)
                    {
                        Debug.L.Warn(string.Format("注册时，一条注册语句影响了不止一个值，共影响{0}条数据", count));

                        res = true;
                    }
                    else
                    {
                        Debug.L.Error("注册语句共影响0条数据，写入数据库失败");

                        res = false;
                    }
                }
                catch(Exception e)
                {
                    Debug.L.Error(string.Format("向User中添加一条新用户 时数据库查询出错，错误信息：{0}", e.Message));
                }

                return res;
            }
        }

        /// <summary>
        /// 通过玩家GUID查询玩家在数据库的ID值。没有结果将返回-1
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public int SelectUserIDByGUID(int guid)
        {
            using(MySqlCommand cmd = new MySqlCommand("select `id` from user where guid = @guid", MySQLDataBase.Conn))
            {
                cmd.Parameters.AddWithValue("guid", guid);

                int res = -1;
                try
                {
                    using MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        //有结果
                        res = reader.GetInt32(0);
                    }
                    else
                    {
                        //没有结果
                        res = -1;
                    }
                }
                catch(Exception e)
                {
                    Debug.L.Error(string.Format("通过玩家GUID查询玩家在数据库的ID值 时数据库查询出错，错误信息：{0}", e.Message));
                }

                return res;
            }
        }

        /// <summary>
        /// 修改对应玩家的金币
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool ChangeGoldCoin(int guid, float amount)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = MySQLDataBase.Conn;
                MySqlTransaction transaction = MySQLDataBase.Conn.BeginTransaction();
                cmd.Transaction = transaction;

                bool res;
                try
                {
                    cmd.CommandText = "update user set goldcoin = goldcoin + @goldCoin where guid = @guid;";
                    cmd.Parameters.AddWithValue("goldCoin", amount);
                    cmd.Parameters.AddWithValue("guid", guid);

                    cmd.ExecuteNonQuery();
                    transaction.Commit();

                    res = true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    res = false;

                    Debug.L.Error(string.Format("给指定用户增加（扣除）金币 时事务执行出错，已回滚。错误信息：{0}", e.Message));
                }

                return res;
            }
        }

        /// <summary>
        /// 根据玩家GUID查询该玩家的个性签名
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string SelectPlayerSignatureByGUID(int guid)
        {
            using MySqlCommand cmd = new MySqlCommand("select signature from user where guid = @guid", MySQLDataBase.Conn);

            cmd.Parameters.AddWithValue("guid", guid);

            string res = null;
            try
            {
                using MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    //有结果
                    //res = reader.GetString(0);//会报错。如果该列为空，则抛出：Data is Null. This method or property cannot be called on Null values异常
                    res = reader[0].ToString();
                }
                else
                {
                    //没有结果
                    res = "";
                }
                reader.Close();
            }
            catch(Exception e)
            {
                Debug.L.Error(string.Format("根据玩家GUID查询该玩家的个性签名 时数据库查询出错，错误信息：{0}", e.Message));
            }

            return res;
        }

        /// <summary>
        /// 根据玩家GUID查询该玩家的总游戏次数
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public int SelectTotalNumberOfGamesByGUID(int guid)
        {
            using MySqlCommand cmd = new MySqlCommand("select totalnumofgames from user where guid = @guid", MySQLDataBase.Conn);

            cmd.Parameters.AddWithValue("guid", guid);

            int res = 0;
            try
            {
                using MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    //有结果
                    res = reader.GetInt32(0);
                }
                else
                {
                    //没有结果
                    res = 0;
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Debug.L.Error(string.Format("根据玩家GUID查询该玩家的总游戏次数 时数据库查询出错，错误信息：{0}", e.Message));
            }

            return res;
        }

        /// <summary>
        /// 根据玩家GUID查询该玩家的游戏胜利次数
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public int SelectNumberOfWinsByGUID(int guid)
        {
            using MySqlCommand cmd = new MySqlCommand("select numberofwins from user where guid = @guid", MySQLDataBase.Conn);

            cmd.Parameters.AddWithValue("guid", guid);

            int res = 0;
            try
            {
                using MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    //有结果
                    res = reader.GetInt32(0);
                }
                else
                {
                    //没有结果
                    res = 0;
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Debug.L.Error(string.Format("根据玩家GUID查询该玩家的游戏胜利次数 时数据库查询出错，错误信息：{0}", e.Message));
            }

            return res;
        }

        /// <summary>
        /// 根据玩家GUID查询该玩家的最佳战绩
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public int SelectBestRecordByGUID(int guid)
        {
            using MySqlCommand cmd = new MySqlCommand("select bestrecord from user where guid = @guid", MySQLDataBase.Conn);

            cmd.Parameters.AddWithValue("guid", guid);

            int res = 0;
            try
            {
                using MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    //有结果
                    res = reader.GetInt32(0);
                }
                else
                {
                    //没有结果
                    res = 0;
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Debug.L.Error(string.Format("根据玩家GUID查询该玩家的最佳战绩 时数据库查询出错，错误信息：{0}", e.Message));
            }

            return res;
        }

        /// <summary>
        /// 根据GUID更新玩家的个性签名
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool UpdateSignatureByGUID(int guid, string signature)
        {
            using MySqlCommand cmd = new MySqlCommand("update user set signature = @signature where guid = @guid;", MySQLDataBase.Conn);

            cmd.Parameters.AddWithValue("guid", guid);
            if (string.IsNullOrEmpty(signature))
            {
                cmd.Parameters.AddWithValue("signature", "");
            }
            else
            {
                cmd.Parameters.AddWithValue("signature", signature);
            }

            bool res = false;
            try
            {
                int count = cmd.ExecuteNonQuery();
                if (count == 1)
                {
                    res = true;
                }
                else if (count != 0)
                {
                    Debug.L.Warn(string.Format("根据GUID更新玩家的个性签名时，一条注册语句影响了不止一个值，共影响{0}条数据", count));

                    res = true;
                }
                else
                {
                    Debug.L.Error("根据GUID更新玩家的个性签名共影响0条数据，写入数据库失败");

                    res = false;
                }
            }
            catch (Exception e)
            {
                Debug.L.Error(string.Format("根据GUID更新玩家的个性签名 时数据库查询出错，错误信息：{0}", e.Message));
            }

            return res;
        }

        /// <summary>
        /// 根据GUID更新玩家的头像名称
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="avatar"></param>
        /// <returns></returns>
        public bool UpdateAvatarByGUID(int guid, string avatar)
        {
            using MySqlCommand cmd = new MySqlCommand("update user set avatar = @avatar where guid = @guid;", MySQLDataBase.Conn);

            cmd.Parameters.AddWithValue("guid", guid);
            cmd.Parameters.AddWithValue("avatar", avatar);

            bool res = false;
            try
            {
                int count = cmd.ExecuteNonQuery();
                if (count == 1)
                {
                    res = true;
                }
                else if (count != 0)
                {
                    Debug.L.Warn(string.Format("根据GUID更新玩家的头像名称时，一条注册语句影响了不止一个值，共影响{0}条数据", count));

                    res = true;
                }
                else
                {
                    Debug.L.Error("根据GUID更新玩家的头像名称共影响0条数据，写入数据库失败");

                    res = false;
                }
            }
            catch (Exception e)
            {
                Debug.L.Error(string.Format("根据GUID更新玩家的头像名称 时数据库查询出错，错误信息：{0}", e.Message));
            }

            return res;
        }

        /// <summary>
        /// 增加总对战次数
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public bool AddTotalNumOfGames(int guid, int addCount = 1)
        {
            using MySqlCommand cmd = new MySqlCommand("update user set totalnumofgames = totalnumofgames + @count where guid = @guid;", MySQLDataBase.Conn);

            cmd.Parameters.AddWithValue("guid", guid);
            cmd.Parameters.AddWithValue("count", addCount);

            bool res = false;
            try
            {
                int count = cmd.ExecuteNonQuery();
                if (count == 1)
                {
                    res = true;
                }
                else if (count != 0)
                {
                    Debug.L.Warn(string.Format("增加总对战次数时，一条注册语句影响了不止一个值，共影响{0}条数据", count));

                    res = true;
                }
                else
                {
                    Debug.L.Error("增加总对战次数 共影响0条数据，写入数据库失败");

                    res = false;
                }
            }
            catch (Exception e)
            {
                Debug.L.Error(string.Format("增加总对战次数 时数据库查询出错，错误信息：{0}", e.Message));
            }

            return res;
        }

        /// <summary>
        /// 增加胜利场次
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public bool AddNumOfWins(int guid, int addCount = 1)
        {
            using MySqlCommand cmd = new MySqlCommand("update user set numberofwins = numberofwins + @count where guid = @guid;", MySQLDataBase.Conn);

            cmd.Parameters.AddWithValue("guid", guid);
            cmd.Parameters.AddWithValue("count", addCount);

            bool res = false;
            try
            {
                int count = cmd.ExecuteNonQuery();
                if (count == 1)
                {
                    res = true;
                }
                else if (count != 0)
                {
                    Debug.L.Warn(string.Format("增加胜利场次时，一条注册语句影响了不止一个值，共影响{0}条数据", count));

                    res = true;
                }
                else
                {
                    Debug.L.Error("增加胜利场次 共影响0条数据，写入数据库失败");

                    res = false;
                }
            }
            catch (Exception e)
            {
                Debug.L.Error(string.Format("增加胜利场次 时数据库查询出错，错误信息：{0}", e.Message));
            }

            return res;
        }

        /// <summary>
        /// 更新最佳战绩
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="bestRecord"></param>
        /// <returns></returns>
        public bool UpdateBestRecord(int guid, int bestRecord)
        {
            using MySqlCommand cmd = new MySqlCommand("update user set bestrecord = @bestRecord where guid = @guid;", MySQLDataBase.Conn);

            cmd.Parameters.AddWithValue("guid", guid);
            cmd.Parameters.AddWithValue("bestRecord", bestRecord);

            bool res = false;
            try
            {
                int count = cmd.ExecuteNonQuery();
                if (count == 1)
                {
                    res = true;
                }
                else if (count != 0)
                {
                    Debug.L.Warn(string.Format("更新最佳战绩时，一条注册语句影响了不止一个值，共影响{0}条数据", count));

                    res = true;
                }
                else
                {
                    Debug.L.Error("更新最佳战绩 共影响0条数据，写入数据库失败");

                    res = false;
                }
            }
            catch (Exception e)
            {
                Debug.L.Error(string.Format("更新最佳战绩 时数据库查询出错，错误信息：{0}", e.Message));
            }

            return res;
        }
    }
}
