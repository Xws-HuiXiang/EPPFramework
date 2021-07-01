using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMExample
{
    /// <summary>
    /// 有限状态机状态实现类。
    /// </summary>
    public class SitState : FSMStateBase
    {
        public SitState(FSMSystem fsm) : base(fsm)
        {
            stateID = StateID.Sit;
        }

        public override void Reason()
        {
            Debug.LogFormat("状态：{0} 执行Reason方法", StateID.ToString());
        }

        public override void CarryOut()
        {
            Debug.LogFormat("状态：{0} 执行CarryOut方法", StateID.ToString());
        }

        public override void DoBeforeEntering()
        {
            Debug.LogFormat("状态：{0} 执行DoBeforeEntering方法", StateID.ToString());
        }

        public override void DoBeforeLeaving()
        {
            Debug.LogFormat("状态：{0} 执行DoBeforeLeaving方法", StateID.ToString());
        }
    }
}
