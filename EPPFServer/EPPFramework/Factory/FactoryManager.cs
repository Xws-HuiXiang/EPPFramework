using EPPFramework.Common;
using EPPFramework.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPPFramework.FSM;
using EPPFramework.Factory.FSM;

namespace EPPFramework.Factory
{
    /// <summary>
    /// 工厂管理器
    /// </summary>
    public class FactoryManager : Singleton<FactoryManager>
    {
        private FSMSystemFactory fsmSystemFactory;
        /// <summary>
        /// 有限状态机系统工厂
        /// </summary>
        public FSMSystemFactory FSMSystemFactory { get { return fsmSystemFactory; } }

        public FactoryManager()
        {

        }

        /// <summary>
        /// 初始化。用于创建所有的工厂对象
        /// </summary>
        public void Init()
        {
            fsmSystemFactory = new FSMSystemFactory();
        }
    }
}
