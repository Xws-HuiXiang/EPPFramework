using EPPFramework.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPPFramework.Factory.FSM
{
    public interface IFSMFactory
    {
        FSMSystem CreateFSMSystem(FSMSystemType fsmSystemType);
    }
}
