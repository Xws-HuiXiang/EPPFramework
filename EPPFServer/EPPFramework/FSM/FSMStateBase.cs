using EPPFramework.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPPFramework.FSM
{
    public abstract class FSMStateBase
    {
        /// <summary>
        /// 保存所有转换条件的映射
        /// </summary>
        protected Dictionary<int, int> map = new Dictionary<int, int>();

        protected int stateID;
        /// <summary>
        /// 当前状态的ID
        /// </summary>
        public int StateID { get { return stateID; } }

        /// <summary>
        /// 当前持有该状态的FSMSystem
        /// </summary>
        protected FSMSystem fsm;

        public FSMStateBase(FSMSystem fsm)
        {
            this.fsm = fsm;
        }

        /// <summary>
        /// 向条件-状态映射中添加状态转换条件
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="id"></param>
        public void AddTransition(int trans, int id)
        {
            //不能添加空转换条件
            if (trans == FSMSystem.NULL_STATE_ID)
            {
                Debug.L.Error("尝试向条件-状态映射中添加空转换条件");

                return;
            }

            //不能添加空状态ID
            if (id == FSMSystem.NULL_STATE_ID)
            {
                Debug.L.Warn("尝试向条件-状态映射中添加空状态ID");

                return;
            }

            //不能重复添加
            if (map.ContainsKey(trans))
            {
                Debug.L.Error("添加转换映射时，转换条件：" + trans.ToString() + "重复添加。当前StateID：" + StateID.ToString() + "。准备添加的StateID：" + id.ToString());

                return;
            }

            map.Add(trans, id);
        }

        /// <summary>
        /// 从条件-状态映射中移除转换条件
        /// </summary>
        /// <param name="trans"></param>
        public void DeleteTransition(int trans)
        {
            //不能删除空条件
            if (trans == FSMSystem.NULL_STATE_ID)
            {
                Debug.L.Error("不能移除空转换条件");

                return;
            }

            //如果包含这个转换条件则移除，否则报错
            if (map.ContainsKey(trans))
            {
                map.Remove(trans);

                return;
            }

            Debug.L.Error("移除条件映射时，字典中不存在转换条件：" + trans.ToString() + "。当前StateID为：" + StateID.ToString());
        }

        /// <summary>
        /// 指定一个转换条件，返回这个条件可以转换为的StateID
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int GetTransformState(int trans)
        {
            if (map.ContainsKey(trans))
            {
                return map[trans];
            }

            return FSMSystem.NULL_STATE_ID;
        }

        /// <summary>
        /// 当进入这个状态时会调用这个方法
        /// </summary>
        public virtual void DoBeforeEntering() { }

        /// <summary>
        /// 当离开这个状态时会调用这个方法
        /// </summary>
        public virtual void DoBeforeLeaving() { }
    }
}
