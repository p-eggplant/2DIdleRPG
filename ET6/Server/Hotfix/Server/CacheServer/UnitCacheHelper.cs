/*----------------------------------------------------------------
* 文件名:	UnitCacheHelper
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2023/5/5 15:08:02
* 创建人:   林逸煌
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
    public static class UnitCacheHelper
    {
        /// <summary>
        /// 添加或者更新 player全部组件
        /// </summary>
        /// <typeparam name="T">组件</typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static void Set(Player player)
        {
            if (player == null)
                return;
            //创建一条信息
            Other2UnitCache_AddOrUpdateUnit message = new Other2UnitCache_AddOrUpdateUnit() { playerId = player.m_lPlayerId };
            message.ZoneId = player.m_nDBZoneId;

            // 获取该组件挂载的子组件
            foreach ((Type key, Entity entity) in player.Components)
            {   //判断当前类型是否有IUnitCache
                if (!typeof(IUnitCacheNode).IsAssignableFrom(key))
                {
                    continue;
                }
                // 添加该组件挂载的子组件类型名称
                message.ComponentTypeList.Add(key.FullName);
                //添加该组件挂载的子组件转化成字节流
                message.ComponentBytes.Add(MongoHelper.ToBson(entity));
            }
            //发送信息
            MessageHelper.SendActor(StartSceneConfigCategory.Instance.CacheConfig.InstanceId, message);
        }
        /// <summary>
        /// 添加或者更新 单个组件
        /// </summary>
        /// <typeparam name="T">组件</typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static void SetOne<T>(this T self) where T : Entity, IUnitCacheNode
        {
            Player player = self.GetParent<Player>();
            if (player == null)
                return;
            long playeId = player.m_lPlayerId;
            //创建一条信息
            Other2UnitCache_AddOrUpdateUnit message = new Other2UnitCache_AddOrUpdateUnit() { playerId = playeId };
            message.ZoneId = player.m_nDBZoneId;
            //添加其组件类型名称
            message.ComponentTypeList.Add(typeof(T).FullName);
            //转化成二进制流
            message.ComponentBytes.Add(MongoHelper.ToBson(self));
            //发送信息
            MessageHelper.SendActor(StartSceneConfigCategory.Instance.CacheConfig.InstanceId, message);
        }

        /// <summary>
        /// 获取 玩家的部分组件缓存  
        /// </summary>
        /// <param name="player">玩家ID</param>
        /// <returns>返回字典是否有组件     和   组件字典</returns>
        public static async ETTask<(bool, Dictionary<string, Entity>)> Get(Player player)
        {
            if (player == null)
                return (false, null);
            long playerId = player.m_lPlayerId;
            //创建一条信息
            Other2UnitCache_GetUnit message = new Other2UnitCache_GetUnit() { playerId = playerId };
            message.ZoneId = player.m_nDBZoneId;

            //添加组件名称
            //message.ComponentTypeList.Add(typeof(Player).FullName);

            //  获取该组件挂载的子组件的类型名称
            foreach ((Type key, Entity entity) in player.Components)
            {   //判断当前类型是否有IUnitCache
                if (!typeof(IUnitCacheNode).IsAssignableFrom(key))
                {
                    continue;
                }
                //添加其组件类型名称
                message.ComponentTypeList.Add(key.FullName);
            }

            //获取缓存服连接
            long instanceId = StartSceneConfigCategory.Instance.CacheConfig.InstanceId;
            //获取数据
            UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit)await MessageHelper.CallActor(instanceId, message);
            //将数据进行装填
            Dictionary<string, Entity> Component_dic = new Dictionary<string, Entity>();

            //获取失败
            if (queryUnit.Error != ErrorCode.ERR_Success)
            {//返回空
                Log.Error("");
                return (false, null);
            }

            // 没有数据
            if (queryUnit.ComponentTypeList.Count <= 0)
            {
                return (false, null);
            }

            //成功
            if (queryUnit.Error == ErrorCode.ERR_Success && queryUnit.ComponentBytes.Count > 0)
            {

                for (int i = 0; i < queryUnit.ComponentBytes.Count; i++)
                {

                    //进行获取 类型
                    Type type = Game.EventSystem.GetType(queryUnit.ComponentTypeList[i]);

                    //解析流
                    Entity entity = (Entity)MongoHelper.FromBson(type, queryUnit.ComponentBytes[i]);
                    //添加到字典中
                    Component_dic.Add(queryUnit.ComponentTypeList[i], entity);
                }
            }
            return (true, Component_dic);

        }

        /// <summary>
        /// 删除玩家组件缓存
        /// </summary>
        /// <param name="playerId">玩家Id</param>
        /// <returns></returns>
        public static async ETTask DeleteUnitCache(long playerId, int zone)
        {
            //创建一条信息
            Other2UnitCache_DeleteUnit message = new Other2UnitCache_DeleteUnit() { playerId = playerId, ZoneId = zone };
            //发送到缓存服
            long instanceId = StartSceneConfigCategory.Instance.CacheConfig.InstanceId;
            await MessageHelper.CallActor(instanceId, message);
        }

        /// <summary>
        /// 添加或者更新 class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="playerId"></param>
        /// <param name="EmailId"></param>
        public static void SetOneClass<T>(this T self) where T : Entity, IUnitCacheNode
        {


            long playerId = self.GetParent<Player>().m_lPlayerId;
            //创建一条信息
            Other2UnitCache_AddOrUpdateUnit message = new Other2UnitCache_AddOrUpdateUnit() { playerId = playerId };
            //添加其组件类型名称
            message.ComponentTypeList.Add(typeof(T).FullName);
            //转化成二进制流
            message.ComponentBytes.Add(MongoHelper.ToBson(self));
            //发送信息
            MessageHelper.SendActor(StartSceneConfigCategory.Instance.CacheConfig.InstanceId, message);

        }
    }
}
