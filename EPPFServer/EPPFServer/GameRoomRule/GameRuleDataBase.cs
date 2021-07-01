using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFServer.GameRoomRule
{
    /// <summary>
    /// 游戏房间规则基类
    /// </summary>
    public class GameRuleDataBase
    {
        protected int playerCount;
        /// <summary>
        /// 玩家数量
        /// </summary>
        public int PlayerCount { get { return playerCount; } }

        protected int outCardCountdown;
        /// <summary>
        /// 出牌倒计时
        /// </summary>
        public int OutCardCountdown { get { return outCardCountdown; } }

        /// <summary>
        /// 设置玩家数量
        /// </summary>
        /// <param name="playerCount"></param>
        public void SetPlayerCount(int playerCount)
        {
            this.playerCount = playerCount;
        }

        /// <summary>
        /// 设置出牌倒计时
        /// </summary>
        /// <param name="outCardCountdown"></param>
        public void SetOutCardCountdown(int outCardCountdown)
        {
            this.outCardCountdown = outCardCountdown;
        }
    }
}
