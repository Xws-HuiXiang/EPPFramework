using EPPFramework.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPPFramework.FSM
{
    /// <summary>
    /// 有限状态机
    /// </summary>
    public class FSMSystem
    {
        /// <summary>
        /// 空状态的状态码
        /// </summary>
        public const int NULL_STATE_ID = -1;

        private FSMStateBase currentState = null;
        /// <summary>
        /// 当前状态机的状态
        /// </summary>
        public FSMStateBase CurrentState { get { return currentState; } }
        private int currentStateID;
        /// <summary>
        /// 状态机当前状态的StateID
        /// </summary>
        public int CurrentStateID { get { return currentStateID; } }

        /// <summary>
        /// 包含所有状态的列表
        /// </summary>
        private List<FSMStateBase> states = new List<FSMStateBase>();

        public FSMSystem()
        {

        }

        /// <summary>
        /// 向状态机中添加一个状态。如果是第一个添加的状态则设置为默认状态
        /// </summary>
        /// <param name="state">状态类</param>
        public void AddState(FSMStateBase state)
        {
            if(state.StateID == NULL_STATE_ID)
            {
                Debug.L.Error("不能向状态机中添加空状态");

                return;
            }

            //数量等于0说明还没有状态，设置为默认状态
            if (states.Count == 0)
            {
                currentState = state;
                currentStateID = state.StateID;
                states.Add(state);

                return;
            }

            //判断状态添加是否重复
            foreach (FSMStateBase item in states)
            {
                if (item.StateID == state.StateID)
                {
                    Debug.L.Error("尝试向状态机中添加重复的状态。StateID为：" + state.StateID);

                    return;
                }
            }

            states.Add(state);
        }

        /// <summary>
        /// 从状态机中删除一个状态
        /// </summary>
        /// <param name="id">状态StateID</param>
        public void DeleteState(int id)
        {
            if(id == NULL_STATE_ID)
            {
                Debug.L.Error("不能删除空状态。或请检查状态ID不能为-1");

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

            Debug.L.Error("尝试删除不存在的状态。状态StateID：" + id);
        }

        /// <summary>
        /// 执行状态转换
        /// </summary>
        /// <param name="trans">转换条件</param>
        public void Transform(int trans)
        {
            //获取这个条件转换到哪个状态
            int id = CurrentState.GetTransformState(trans);
            if (id == NULL_STATE_ID)
            {
                Debug.L.Error("状态StateID：" + CurrentStateID.ToString() + "在转换时指定的转换条件：" + trans.ToString() + "没有对应的目标状态");

                return;
            }

            //调用状态离开的方法和新状态的进入状态方法
            currentStateID = id;
            foreach (FSMStateBase state in states)
            {
                if (CurrentStateID == state.StateID)
                {
                    CurrentState.DoBeforeLeaving();

                    currentState = state;

                    CurrentState.DoBeforeEntering();

                    break;
                }
            }
        }
    }
}
