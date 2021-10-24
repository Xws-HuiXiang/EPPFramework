using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFramework.Common
{
    public class Singleton<T> where T : new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }
    }
}
