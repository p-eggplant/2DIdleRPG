/*----------------------------------------------------------------
* 文件名:	Other2UnitCacheHandler
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2023/5/5 15:08:35
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
    [FriendClassAttribute(typeof(ET.UnitCacheManager))]
    public class Other2UnitCache_GetUnitHandler : AMActorRpcHandler<Scene, Other2UnitCache_GetUnit, UnitCache2Other_GetUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_GetUnit request, UnitCache2Other_GetUnit response, Action reply)
        {
            UnitCacheManager unitCacheComponent = scene.GetComponent<UnitCacheManager>();
            Dictionary<string, Entity> dictionary = MonoPool.Instance.Fetch(typeof(Dictionary<string, Entity>)) as Dictionary<string, Entity>;
            try
            {
                try
                {
                    //获取
                    dictionary = await unitCacheComponent.GetComponentData(request.playerId, request.ComponentTypeList, request.ZoneId);
                    //转为字节流
                    foreach (var key in dictionary.Keys)
                    {

                        //判断是否为空，不为空进行序列化
                        if (dictionary.TryGetValue(key, out Entity entity) && entity != null)
                        {
                            response.ComponentTypeList.Add(key);
                            response.ComponentBytes.Add(MongoHelper.ToBson(entity));
                        }
                    }

                    reply();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    response.Error = ErrorCode.ERR_CacheGet;
                    reply();
                }
            }
            finally
            {
                dictionary.Clear();
                MonoPool.Instance.Recycle(dictionary);
            }
        }
    }
    public class Other2UnitCache_DeleteUnitHandler : AMActorRpcHandler<Scene, Other2UnitCache_DeleteUnit, UnitCache2Other_DeleteUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_DeleteUnit request, UnitCache2Other_DeleteUnit response, Action reply)
        {
            UnitCacheManager unitCacheComponent = scene.GetComponent<UnitCacheManager>();
            unitCacheComponent.Delete(request.playerId, request.ComponentName);
            reply();
            await ETTask.CompletedTask;
        }
    }
    public class Other2UnitCache_AddOrUpdateUnitHandler : AMActorHandler<Scene, Other2UnitCache_AddOrUpdateUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_AddOrUpdateUnit message)
        {
            UnitCacheManager unitCacheComponent = scene.GetComponent<UnitCacheManager>();
            using (ListComponent<Entity> entityList = ListComponent<Entity>.Create())
            {
                for (int index = 0; index < message.ComponentTypeList.Count; index++)
                {
                    //进行解析 类型
                    Type type = Game.EventSystem.GetType(message.ComponentTypeList[index]);
                    Entity entity = (Entity)MongoHelper.FromBson(type, message.ComponentBytes[index]);
                    //添加到列表中
                    entityList.Add(entity);
                }
                //进行保存到缓存服和数据库中
                unitCacheComponent.AddOrUpdateComponent(message.playerId, entityList, message.ZoneId);
                await ETTask.CompletedTask;
            }
        }
    }
    public class Other2UnitCache_AddOrUpdateClassHandler : AMActorHandler<Scene, Other2UnitCache_AddOrUpdateClass>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_AddOrUpdateClass message)
        {
            //缓存服组件
            UnitCacheManager unitCacheComponent = scene.GetComponent<UnitCacheManager>();

            //if (typeof(SystemEmailState).Name == message.ClassType)
            //{
            //    using (ListComponent<SystemEmailState> systemEmailStateList = ListComponent<SystemEmailState>.Create())
            //    {
            //        foreach (var bytes in message.ClassBytes)
            //        {
            //            SystemEmailState systemEmailState = (SystemEmailState)MongoHelper.FromBson(typeof(SystemEmailState), bytes);
            //            //添加到列表中
            //            systemEmailStateList.Add(systemEmailState);
            //        }

            //        //进行保存到缓存服和数据库中
            //        unitCacheComponent.AddOrUpdateSystemEmailState(message.playerId, systemEmailStateList);


            //    }
            //}

            await ETTask.CompletedTask;
        }
    }
    public class Other2UnitCache_GetClassHandler : AMActorRpcHandler<Scene, Other2UnitCache_GetClass, UnitCache2Other_GetClass>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_GetClass request, UnitCache2Other_GetClass response, Action reply)
        {
            UnitCacheManager unitCacheComponent = scene.GetComponent<UnitCacheManager>();

            try
            {
                //switch (request.classType)
                //{
                //    case "SystemEmailState":
                //        //获取
                //        List<SystemEmailState> SystemEmailStatelist = await unitCacheComponent.GetSystemEmailStates(request.playerId);
                //        //转为字节流
                //        foreach (var SystemEmailState in SystemEmailStatelist)
                //        {//判断是否为空，不为空进行序列化
                //            if (SystemEmailState != null)
                //            {
                //                response.ClassBytes.Add(MongoHelper.ToBson(SystemEmailState));
                //            }
                //        }
                //        break;
                //}
                reply();
            }
            catch (Exception e)
            {
                Log.Error(e);
                response.Error = ErrorCode.ERR_CacheGet;
                reply();
            }
            await ETTask.CompletedTask;
        }
    }
}
