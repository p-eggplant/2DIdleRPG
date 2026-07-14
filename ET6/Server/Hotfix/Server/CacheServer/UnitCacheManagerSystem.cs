/*----------------------------------------------------------------
* 文件名:	UnitCacheManagerSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2023/5/5 15:01:54
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
    [Timer(TimerType.ScheduledStorage)]
    [FriendClassAttribute(typeof(ET.UnitCacheManager))]
    public class UnitCacheManageTimeout : ATimer<UnitCacheManager>
    {
        public override async void Run(UnitCacheManager self)
        {
            try
            {
                //等待存储完毕重新计时
                await self.DBStored();
                //启动定时器 定时存储
                self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + self.Update_time, TimerType.ScheduledStorage, self);
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }
    [FriendClassAttribute(typeof(ET.UnitCacheNode))]
    public class UnitCacheManageAwakeSystem : AwakeSystem<UnitCacheManager>
    {
        public override void Awake(UnitCacheManager self)
        {
            //将全部组件列表进行清空
            self.UnitCacheKeyList.Clear();
            //遍历全部组件的类型
            foreach (Type type in Game.EventSystem.GetTypes().Values)
            {
                //判断类中是否有IUnitCache这个接口
                if (type != typeof(IUnitCacheNode) && typeof(IUnitCacheNode).IsAssignableFrom(type))
                {   //有的话将类型填充到列表中
                    self.UnitCacheKeyList.Add(type.Name);
                }
            }
            //根据全部类型添加到字典中
            foreach (string Typename in self.UnitCacheKeyList)
            {
                //创建
                UnitCacheNode unitCache = self.AddChild<UnitCacheNode>();
                //设置组件名称
                unitCache.ComponentTypeName = Typename;

                self.UnitCaches.Add(Typename, unitCache);
            }
            //启动定时器 定时存储
            self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + self.Update_time, TimerType.ScheduledStorage, self);
        }
    }
    public class UnitCacheManageDestroySystem : DestroySystem<UnitCacheManager>
    {
        public override void Destroy(UnitCacheManager self)
        {
            foreach (var unitCache in self.UnitCaches.Values)
            {
                //进行销毁
                unitCache?.Dispose();
            }
            //清空
            self.UnitCaches.Clear();
        }
    }
    [FriendClassAttribute(typeof(UnitCacheManager))]
    [FriendClassAttribute(typeof(UnitCacheNode))]
    public static class UnitCacheManagerSystem
    {
        /// <summary>
        /// 根据玩家playerID进行获取 没有就创建
        /// </summary>
        /// <param name="self"></param>
        /// <param name="playerId"></param>
        /// <param name="Typename"></param>
        /// <returns></returns>
        public static async ETTask<Dictionary<string, Entity>> GetComponentData(this UnitCacheManager self, long playerId, List<string> ComponentTypeList, int zone)
        {
            //字典对象   
            Dictionary<string, Entity> dictionary = new Dictionary<string, Entity>();

            //当要获取的组件为空时
            if (ComponentTypeList.Count == 0)
            {   //获取全部组件信息
                dictionary.Add(nameof(Unit), null);
                foreach (string s in self.UnitCacheKeyList)
                {//将所有类型填充到字典中
                    dictionary.Add(s, null);
                }
            }
            else
            {
                foreach (string s in ComponentTypeList)
                {//填充列表
                    dictionary.Add(s, null);
                }
            }

            //根据字典获取数据
            foreach (var typename in dictionary.Keys)
            {
                //从数据库查找组件不能有ET.
                string Typename = typename.Replace("ET.", "");

                //寻找当前组件是否有进行缓存没有就创建
                if (false == self.UnitCaches.TryGetValue(Typename, out UnitCacheNode unitCache))
                {   //创建
                    unitCache = self.AddChild<UnitCacheNode>();
                    //组件名称
                    unitCache.ComponentTypeName = Typename;
                    //添加到管理器中
                    self.UnitCaches.Add(Typename, unitCache);

                }
                //获取同一个玩家的多个组件数据
                dictionary[typename] = await unitCache.Get(playerId, zone);

            }

            return dictionary;

        }

        /// <summary>
        /// 删除指定玩家的指定组件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="playerId">玩家Id</param>
        /// <param name="Typename">组件类型</param>
        public static void Delete(this UnitCacheManager self, long playerId, string Typename)
        {
            //获取相应的组件的UnitCache
            if (self.UnitCaches.TryGetValue(Typename, out UnitCacheNode unitCache))
            {
                //根据ID进行删除
                unitCache.Delete(playerId);
            }
        }
        /// <summary>
        /// 添加 或者更新组件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="playerId"></param>
        /// <param name="UnitList"></param>
        public static void AddOrUpdateComponent(this UnitCacheManager self, long playerId, ListComponent<Entity> UnitList, int zone)
        {
            //遍历组件列表
            foreach (Entity entity in UnitList)
            {
                //获取类型
                string Typename = entity.GetType().Name;
                //没有找到相应类型进行创建到缓存服中
                if (false == self.UnitCaches.TryGetValue(Typename, out UnitCacheNode unitCache))
                {
                    //创建
                    unitCache = self.AddChild<UnitCacheNode>();
                    unitCache.ComponentTypeName = Typename;
                    self.UnitCaches.Add(Typename, unitCache);

                }
                //执行当前玩家数据的创建或者更新
                unitCache.AddOrUpdate(entity);
            }
            //将数据添加存储列表中
            self.Add_DBNotStoredEntityDic(playerId, UnitList, zone).Coroutine();

        }
        /// <summary>
        /// 添加未存储列表中
        /// </summary>
        /// <param name="self"></param>
        /// <param name="playerId"></param>
        /// <param name="UnitList"></param>
        /// <returns></returns>
        public static async ETTask Add_DBNotStoredEntityDic(this UnitCacheManager self, long playerId, ListComponent<Entity> UnitList, int zone)
        {
            //根据缓存服的连接进行上锁
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DBStored, StartSceneConfigCategory.Instance.CacheConfig.InstanceId))
            {
                //是否有当前用户的Id
                if (false == self.DBNotStoredEntityDic.TryGetValue(playerId, out var node))
                {
                    //进行添加当前用户的信息
                    node = new UnitCacheManager.DBNotStoredNode();
                    node.dbZoneId = zone;
                    node.notStroredDic = new Dictionary<string, Entity>();
                    self.DBNotStoredEntityDic.Add(playerId, node);
                }
                //判断是否初始
                if (node.notStroredDic == null)
                {
                    node.notStroredDic = new Dictionary<string, Entity>();
                }
                foreach (Entity entity in UnitList)
                {
                    //获取类型
                    string Typename = entity.GetType().Name;

                    //获取是否已经存储值有的话进行移除老的
                    if (node.notStroredDic.TryGetValue(Typename, out Entity oldentity))
                    {
                        node.notStroredDic.Remove(Typename);
                    }
                    //添加当前组件
                    node.notStroredDic.Add(Typename, entity);

                }

            }
        }
        /// <summary>
        /// 数据库存储
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask DBStored(this UnitCacheManager self)
        {
            //根据缓存服的连接进行上锁
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DBStored, StartSceneConfigCategory.Instance.CacheConfig.InstanceId))
            {
                List<Entity> entities = new List<Entity>();
                //判断是否有值
                if (self.DBNotStoredEntityDic.Count > 0)
                {
                    //遍历获取当前ID还未保存的数据
                    foreach (var kv in self.DBNotStoredEntityDic)
                    {
                        //遍历获取各个组件
                        foreach (var entity in kv.Value.notStroredDic.Values)
                        {
                            entities.Add(entity);
                        }
                        //进行存储
                        await DBManagerComponent.Instance.GetZoneDB(kv.Value.dbZoneId).Save(kv.Key, entities);
                        entities.Clear();
                    }
                    self.DBNotStoredEntityDic.Clear();
                }
            }
        }

    }
}
