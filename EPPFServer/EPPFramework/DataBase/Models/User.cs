using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFramework.DataBase.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int GUID { get; set; }
        public int GoldCoin { get; set; }
    }
}
