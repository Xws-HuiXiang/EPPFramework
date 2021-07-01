/* 
 * 说明：这个是有限状态机的代码模板。参考Unity官方提供的有限状态机代码
 *       官方代码地址：http://wiki.unity3d.com/index.php/Finite_State_Machine
 * 使用：先修改当前类中的StateID和Transition枚举。分别是状态ID和转换条件
 *       构造一个FSMSystem类
 *       自定义的类继承自FSMState类。并指定父类中的StateID字段后调用AddTransition(Transition trans, StateID id)方法向映射中添加转换条件
 *       使用FSMSystem对象中的AddState(FSMState state)方法把刚刚自定义的状态类添加到状态集合中
 *       如果要执行状态转换则调用FSMSystem类中的PreformTransition(Transition trans)方法实现状态转换
 *       
 * 这个例子的状态机可以直接用，但是在项目中的C#侧是没有使用的。在Lua侧有一套使用Lua写的有限状态机
 */

using System.Collections.Generic;
using UnityEngine;

namespace FSMExample
{
    /// <summary>
    /// 有限状态机的状态ID
    /// </summary>
    public enum StateID
    {
        NullStateID = 0,
        Walk,
        Run,
        Sit
    }

    /// <summary>
    /// 有限状态机的状态转换条件
    /// </summary>
    public enum Transition
    {
        NullTransition = 0,
        WalkToRun,
        RunToWalk,
        WalkToSit,
        SitToWalk
    }

    /// <summary>
    /// 有限状态机
    /// </summary>
    public class FSMSystem
    {
        /// <summary>
        /// 包含所有状态的列表
        /// </summary>
        private List<FSMStateBase> states = new List<FSMStateBase>();

        //当前状态的ID
        private StateID currentStateID;
        /// <summary>
        /// 状态机当前状态的StateID
        /// </summary>
        public StateID CurrentStateID
        {
            get
            {
                return currentStateID;
            }
        }
        //当前状态的FSMState类
        private FSMStateBase currentFSMState;
        /// <summary>
        /// 当前状态的FSMState类
        /// </summary>
        public FSMStateBase CurrentFSMState
        {
            get
            {
                return currentFSMState;
            }
        }

        /// <summary>
        /// 向状态机中添加一个状态。如果是第一个添加的状态则设置为默认状态
        /// </summary>
        /// <param name="state">状态类</param>
        public void AddState(FSMStateBase state)
        {
            //不能添加空的状态ID
            if (state.StateID == StateID.NullStateID)
            {
                Debug.LogError("不能向状态机中添加空状态");
                return;
            }

            //数量等于0说明还没有状态，设置为默认状态
            if (states.Count == 0)
            {
                currentFSMState = state;
                currentStateID = state.StateID;
                states.Add(state);
                return;
            }

            //判断状态添加是否重复
            foreach (FSMStateBase item in states)
            {
                if (item.StateID == state.StateID)
                {
                    Debug.LogError("尝试向状态机中添加重复的状态。StateID为：" + state.StateID);
                    return;
                }
            }

            states.Add(state);
        }

        /// <summary>
        /// 从状态机中删除一个状态
        /// </summary>
        /// <param name="id">状态StateID</param>
        public void DeleteState(StateID id)
        {
            //不能移除空状态ID
            if (id == StateID.NullStateID)
            {
                Debug.LogError("不能删除默认的空状态");
                return;
            }

            //遍历所有的状态集合，根据ID判断是否是要删除的。如果没有在集合中找到则报错
            foreach (FSMStateBase item in states)
            {
                if (item.StateID == id)
                {
                    states.Remove(item);
                    return;
                }
            }

            Debug.LogError("尝试删除不存在的状态。状态StateID：" + id);
        }

        /// <summary>
        /// 执行状态转换
        /// </summary>
        /// <param name="trans">转换条件</param>
        public void PreformTransition(Transition trans)
        {
            //空转换不可以用在实际转换中
            if (trans == Transition.NullTransition)
            {
                Debug.LogError("状态机不可以转换为NullTransition");
                return;
            }

            //获取这个条件转换到哪个状态
            StateID id = CurrentFSMState.GetOutputState(trans);
            if (id == StateID.NullStateID)
            {
                Debug.LogError("状态StateID：" + CurrentStateID.ToString() + "在转换时指定的转换条件：" + trans.ToString() + "没有对应的目标状态");
                return;
            }

            //调用状态离开的方法和新状态的进入状态方法
            currentStateID = id;
            foreach (FSMStateBase state in states)
            {
                if (CurrentStateID == state.StateID)
                {
                    CurrentFSMState.DoBeforeLeaving();

                    currentFSMState = state;

                    CurrentFSMState.DoBeforeEntering();

                    break;
                }
            }
        }
    }

    /// <summary>
    /// 有限状态机的状态基类
    /// </summary>
    public abstract class FSMStateBase
    {
        /// <summary>
        /// 保存所有转换条件的映射
        /// </summary>
        protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();

        /// <summary>
        /// 当前持有该状态的FSMSystem
        /// </summary>
        protected FSMSystem fsm;

        /// <summary>
        /// 状态ID
        /// </summary>
        protected StateID stateID;
        /// <summary>
        /// 状态ID
        /// </summary>
        public StateID StateID
        {
            get
            {
                return stateID;
            }
        }

        public FSMStateBase(FSMSystem fsm)
        {
            this.fsm = fsm;
        }

        /// <summary>
        /// 向条件-状态映射中添加状态转换条件
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="id"></param>
        public void AddTransition(Transition trans, StateID id)
        {
            //不能添加空转换条件
            if (trans == Transition.NullTransition)
            {
                Debug.LogError("尝试向条件-状态映射中添加空转换条件");
                return;
            }

            //不能添加空状态ID
            if (id == StateID.NullStateID)
            {
                Debug.Log("尝试向条件-状态映射中添加空状态ID");
                return;
            }

            //不能重复添加
            if (map.ContainsKey(trans))
            {
                Debug.LogError("添加转换映射时，转换条件：" + trans.ToString() + "重复添加。当前StateID：" + StateID.ToString() + "。准备添加的StateID：" + id.ToString());
                return;
            }

            map.Add(trans, id);
        }

        /// <summary>
        /// 从条件-状态映射中移除转换条件
        /// </summary>
        /// <param name="trans"></param>
        public void DeleteTransition(Transition trans)
        {
            //不能删除空条件
            if (trans == Transition.NullTransition)
            {
                Debug.LogError("不能移除空转换条件");
                return;
            }

            //如果包含这个转换条件则移除，否则报错
            if (map.ContainsKey(trans))
            {
                map.Remove(trans);
                return;
            }
            Debug.LogError("移除条件映射时，字典中不存在转换条件：" + trans.ToString() + "。当前StateID为：" + StateID.ToString());
        }

        /// <summary>
        /// 指定一个转换条件，返回这个条件可以转换为的StateID
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public StateID GetOutputState(Transition trans)
        {
            if (map.ContainsKey(trans))
            {
                return map[trans];
            }
            return StateID.NullStateID;
        }

        /// <summary>
        /// 当进入这个状态时会调用这个方法
        /// </summary>
        public virtual void DoBeforeEntering() { }

        /// <summary>
        /// 当离开这个状态时会调用这个方法
        /// </summary>
        public virtual void DoBeforeLeaving() { }

        /// <summary>
        /// 这个函数用来判断当前是否需要转换状态
        /// </summary>
        public abstract void Reason();

        /// <summary>
        /// 这个函数用来实现在当前状态下需要执行什么操作
        /// </summary>
        public abstract void CarryOut();
    }
}
