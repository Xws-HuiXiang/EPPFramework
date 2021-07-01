using QiLieGuaner;
using System;
using System.Collections.Generic;
using System.Text;
using EPPFServer.Common;
using EPPFServer.GameRoomRule;
using EPPFServer.Log;
using EPPFServer.Network;
using EPPFServer.Room;

namespace EPPFServer.Room.Factory
{
    /// <summary>
    /// 创建游戏房间的工厂类
    /// </summary>
    public class RoomFactory : Singleton<RoomFactory>
    {
        /// <summary>
        /// 创建一个游戏房间
        /// </summary>
        /// <param name="gameType"></param>
        /// <param name="clientSocket"></param>
        /// <param name="roomName"></param>
        /// <param name="roomPassword"></param>
        /// <param name="ruleString"></param>
        /// <returns></returns>
        public RoomBase CreateGameRoom(GameType gameType, ClientSocket clientSocket, string roomName, string roomPassword, string ruleString)
        {
            RoomBase room = null;
            switch (gameType)
            {
                //case GameType.Uno:
                //    UNOGameRule rule = CreateGameRule(gameType) as UNOGameRule;
                //    if(rule.ParseGameRuleString(ruleString, out GameRuleDataBase gameRuleData))
                //    {
                //        UNOGameRuleData ruleData = gameRuleData as UNOGameRuleData;
                //        room = new UNORoom(clientSocket, roomName, roomPassword, ruleData);
                //    }
                //    else
                //    {
                //        Debug.L.Error(string.Format("创建UNO房间时游戏规则串解析失败。规则串：{0}", ruleString));
                //    }
                //    break;
                //case GameType.Othello:
                //    OthelloGameRule othelloRule = CreateGameRule(gameType) as OthelloGameRule;
                //    if (othelloRule.ParseGameRuleString(ruleString, out GameRuleDataBase othelloRuleBase))
                //    {
                //        OthelloGameRuleData ruleData = othelloRuleBase as OthelloGameRuleData;
                //        room = new OthelloRoom(clientSocket, roomName, roomPassword, ruleData);
                //    }
                //    else
                //    {
                //        Debug.L.Error(string.Format("创建UNO房间时游戏规则串解析失败。规则串：{0}", ruleString));
                //    }
                //    break;
                //default:
                //    Debug.L.Error(string.Format("创建房间的工厂没有处理的情况：{0}", gameType));
                //    break;
            }

            return room;
        }

        /// <summary>
        /// 创建房间规则信息
        /// </summary>
        /// <param name="gameType"></param>
        /// <returns></returns>
        public IGameRule CreateGameRule(GameType gameType)
        {
            IGameRule rule = null;
            switch (gameType)
            {
                //case GameType.Uno:
                //    rule = new UNOGameRule();
                //    break;
                //case GameType.Othello:
                //    rule = new OthelloGameRule();
                //    break;
                //default:
                //    Debug.L.Error(string.Format("创建房间规则的工厂没有处理的情况：{0}", gameType));
                //    break;
            }

            return rule;
        }
    }
}
