/*----------------------------------------------------------------
* 文件名:	PlayerSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/26 17:47:52
* 创建人:   王星莅
* 描  述:	玩家对象System

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
    [ObjectSystem]
    public class PlayerAwakeSystem : AwakeSystem<Player, long, int>
    {
        public override void Awake(Player self, long playerId, int zoneid)
        {
            self.m_lPlayerId = playerId;
            self.m_nDBZoneId = zoneid;
        }
    }

    [ObjectSystem]
    public class PlayerDestroySystem : DestroySystem<Player>
    {
        public override void Destroy(Player self)
        {
            self.m_lPlayerId = 0;
        }
    }


    [FriendClass(typeof(Player))]
    public static class PlayerSystem
    {

    }
}