/*----------------------------------------------------------------
* 文件名:	UnitCacheNodeSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2023/5/5 15:01:32
* 创建人:
* 描  述:	

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
    [FriendClassAttribute(typeof(ET.UnitCacheNode))]
    public static class UnitCacheNodeSystem
    {
        /// <summary>
        /// 添加到相应组件的字典中
        /// </summary>
        /// <param name="self"></param>
        /// <param name="Unit">当前组件</param>
        public static void AddOrUpdate(this UnitCacheNode self, Entity Unit)
        {
            if (Unit == null)
            {
                return;
            }
            //判断Unit和oldUnit是否相同
            if (self.UnitCacheDic.TryGetValue(Unit.InstanceId, out Entity oldUnit))
            {
                if (Unit != oldUnit)
                {
                    oldUnit.Dispose();
                }
                self.UnitCacheDic.Remove(Unit.InstanceId);
            }
            self.UnitCacheDic.Add(Unit.InstanceId, Unit);
        }
        public static void Delete(this UnitCacheNode self, long playerId)
        {
            //获取当前字典中是否有该Id的信息
            if (self.UnitCacheDic.TryGetValue(playerId, out Entity Unit))
            {
                Unit.Dispose();
                self.UnitCacheDic.Remove(playerId);
            }

        }
        /// <summary>
        /// 根据玩家账号进行获取
        /// </summary>
        /// <param name="self"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public static async ETTask<Entity> Get(this UnitCacheNode self, long playerId, int zone)
        {
            Entity Unit = null;
            if (!self.UnitCacheDic.TryGetValue(playerId, out Unit))
            {//没有就读取数据库    根据玩家Id和组件类型名称
                Unit = await DBManagerComponent.Instance.GetZoneDB(zone).Query<Entity>(playerId, self.ComponentTypeName);
                //将其保存在缓存服中
                if (Unit != null)
                {
                    self.AddOrUpdate(Unit);
                }
            }
            return Unit;

        }

    }
}
