using EPPFramework.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPPFramework.Factory.FSM
{
    public enum FSMSystemType
    {

    }

    public class FSMSystemFactory : IFSMFactory
    {
        public FSMSystem CreateFSMSystem(FSMSystemType fsmSystemType)
        {
            FSMSystem fsm = new FSMSystem();

            switch (fsmSystemType)
            {
                default:
                    break;
            }

            return fsm;
        }
    }
}
