/*----------------------------------------------------------------
* 文件名:	PlayerMgrComponent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/26 17:37:58
* 创建人:   王星莅
* 描  述:	玩家管理器，用字典引用所有的玩家对象，也同时挂在这所有Player对象Entity

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class PlayerMgrComponentIAwakeSystem : AwakeSystem<PlayerMgrComponent>
    {
        public override void Awake(PlayerMgrComponent self)
        {
            foreach (var player in self.m_dicPlayer.Values)
            {
                player?.Dispose();
            }
            self.m_dicPlayer.Clear();
        }
    }


    public class PlayerMgrComponentDestroySystem : DestroySystem<PlayerMgrComponent>
    {
        public override void Destroy(PlayerMgrComponent self)
        {
            foreach (var player in self.m_dicPlayer.Values)
            {
                player?.Dispose();
            }
            self.m_dicPlayer.Clear();
        }
    }

    


    [FriendClassAttribute(typeof(ET.PlayerMgrComponent))]
    public static class PlayerMgrComponentSystem
    {
        /// <summary>
        /// 添加一个玩家对象引用
        /// </summary>
        /// <param name="self"></param>
        /// <param name="lPlayerId">玩家的PlayerId</param>
        /// <param name="pPlayer">玩家对象</param>
        /// <exception cref="Exception"></exception>
        public static void Add(this PlayerMgrComponent self, long lPlayerId, Player pPlayer)
        {
            if (true == self.m_dicPlayer.ContainsKey(lPlayerId))
            {
                throw new Exception($"player对象重复创建 playerid = {lPlayerId}");
            }
            self.m_dicPlayer.Add(lPlayerId, pPlayer);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="lPlayerId"></param>
        public static void Remove(this PlayerMgrComponent self, long lPlayerId)
        {
            if (self.m_dicPlayer.ContainsKey(lPlayerId))
            {
                self.m_dicPlayer[lPlayerId]?.Dispose();
                self.m_dicPlayer.Remove(lPlayerId);
            }
        }

        public static Player Get(this PlayerMgrComponent self, long lPlayerId)
        {
            if (false == self.m_dicPlayer.TryGetValue(lPlayerId, out Player pPlayer))
            {
                throw new Exception($"不存在的玩家对象 playerid = {lPlayerId}");
            }
            return pPlayer;
        }

        public static bool IsExist(this PlayerMgrComponent self, long lPlayerId)
        {
            return self.m_dicPlayer.ContainsKey(lPlayerId);
        }
    }
}
