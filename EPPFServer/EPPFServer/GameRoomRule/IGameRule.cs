using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFServer.GameRoomRule
{
    /// <summary>
    /// 游戏规则类的接口
    /// </summary>
    public interface IGameRule
    {
        /// <summary>
        /// 解析一个放假的游戏规则串并返回对应的规则对象
        /// </summary>
        /// <param name="gameRuleString"></param>
        /// <param name="gameRuleData"></param>
        /// <returns></returns>
        bool ParseGameRuleString(string gameRuleString, out GameRuleDataBase gameRuleData);
    }
}
