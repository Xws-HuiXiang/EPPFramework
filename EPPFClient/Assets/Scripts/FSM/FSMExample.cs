using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMExample
{
    public class FSMExample : MonoBehaviour
    {
        private FSMSystem fsm;

        private void Start()
        {
            fsm = new FSMSystem();

            SitState sit = new SitState(fsm);
            sit.AddTransition(Transition.SitToWalk, StateID.Walk);
            fsm.AddState(sit);

            RunState run = new RunState(fsm);
            run.AddTransition(Transition.RunToWalk, StateID.Walk);
            fsm.AddState(run);

            WalkState walk = new WalkState(fsm);
            walk.AddTransition(Transition.WalkToRun, StateID.Run);
            walk.AddTransition(Transition.WalkToSit, StateID.Sit);
            fsm.AddState(walk);
        }

        private void Update()
        {
            //每帧检测的函数
            fsm.CurrentFSMState.Reason();
            //当前状态下要执行的帧函数
            fsm.CurrentFSMState.CarryOut();

            //测试状态转换
            if (Input.GetKeyDown(KeyCode.A))
            {
                fsm.PreformTransition(Transition.SitToWalk);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                fsm.PreformTransition(Transition.WalkToSit);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                fsm.PreformTransition(Transition.WalkToRun);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                fsm.PreformTransition(Transition.RunToWalk);
            }
        }
    }
}
