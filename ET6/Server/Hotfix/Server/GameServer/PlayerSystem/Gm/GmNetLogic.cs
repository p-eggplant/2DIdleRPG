/*----------------------------------------------------------------
* 文件名:	PlayerDataComponentNetLogic
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/1 9:06:19
* 创建人:   
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Xml.Linq;


namespace ET
{

    /// <summary>
    /// Gm修改玩家对应数值
    /// </summary>
    public class C2Game_GmDataChangeHandler : AMActorRpcHandler<Player, C2Game_GmDataChange, Game2C_GmDataChange>
    {
        protected override async ETTask Run(Player player, C2Game_GmDataChange request, Game2C_GmDataChange response, Action reply)
        {
            try
            {
                //获取玩家数值组件
                PlayerDataComponent pPlayerDataComponent = player.GetComponent<PlayerDataComponent>();
                if (pPlayerDataComponent == null)
                {
                    throw new Exception(" pPlayerDataComponent = null");
                }
                //判断是否能够修改
                foreach(var node in request.DataNodes)
                {
                    if (node == null)
                    {
                        throw new Exception("request.DataNodes.node = null");
                    }
                    if (false == pPlayerDataComponent.isCanSetData((EPlayerDataType)node.EnumId, node.Value)){
                        response.Error = ErrorCode.ERR_SystemError;
                        reply();
                        return;
                    }
                }
                //执行数值修改
                foreach (var node in request.DataNodes)
                {
                    pPlayerDataComponent.SetData((EPlayerDataType)node.EnumId, node.Value, EOssType.GM, "数值GM命令修改");
                }

                reply();
                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                response.Error = ErrorCode.ERR_SystemError;
                reply();
            }

        }
    }

    /// <summary>
    /// GM背包添加物品数量
    /// </summary>
    public class C2Game_GmBagAddHandler : AMActorRpcHandler<Player, C2Game_GmBagAdd, Game2C_GmBagAdd>
    {
        protected override async ETTask Run(Player player, C2Game_GmBagAdd request, Game2C_GmBagAdd response, Action reply)
        {
            try
            {
                //获取玩家背包组件
                PlayerBagComponent pPlayerBagComponent = player.GetComponent<PlayerBagComponent>();
                if (pPlayerBagComponent == null)
                {
                    throw new Exception(" pPlayerBagComponent = null");
                }
                //判断是否能添加
                if (false == pPlayerBagComponent.isCanAddItem(request.ConfigId, request.Num))
                {
                    response.Error = ErrorCode.ERR_SystemError;
                    reply();
                    return;
                }

                //执行添加
                pPlayerBagComponent.AddItem(request.ConfigId, request.Num, EOssType.GM, "背包GM命令修改");

                reply();
                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                response.Error = ErrorCode.ERR_SystemError;
                reply();
            }

        }
    }

    /// <summary>
    /// 查询总属性网络消息
    /// </summary>
    public class C2Game_GmTotalPropHandler : AMActorRpcHandler<Player, C2Game_GmTotalProp, Game2C_GmTotalProp>
    {
        protected override async ETTask Run(Player player, C2Game_GmTotalProp request, Game2C_GmTotalProp response, Action reply)
        {
            try
            {
                //获取玩家属性组件
                PlayerPropertyComponent pPlayerPropertyComponent = player.GetComponent<PlayerPropertyComponent>();
                if (pPlayerPropertyComponent == null)
                {
                    throw new Exception(" pPlayerPropertyComponent = null");
                }


                //获取全部属性
                for (int i = 1; i < (int)EPlayerPropertyType.Max; i++)
                {
                    PlayerPropGmProto pPlayerPropGmProto = new PlayerPropGmProto();


                    pPlayerPropGmProto.nPropEnumId = i;
                    pPlayerPropGmProto.lPropNum = pPlayerPropertyComponent.GetTotalProperty((EPlayerPropertyType)i);
                    response.pListTotalProp.Add(pPlayerPropGmProto);
                }

                reply();
                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                response.Error = ErrorCode.ERR_SystemError;
                reply();
            }

        }
    }

    /// <summary>
    /// 查询系统属性网络消息
    /// </summary>
    public class C2Game_GmSystemPropHandler : AMActorRpcHandler<Player, C2Game_GmSystemProp, Game2C_GmSystemProp>
    {
        protected override async ETTask Run(Player player, C2Game_GmSystemProp request, Game2C_GmSystemProp response, Action reply)
        {
            try
            {
                //获取玩家属性组件
                PlayerPropertyComponent pPlayerPropertyComponent = player.GetComponent<PlayerPropertyComponent>();
                if (pPlayerPropertyComponent == null)
                {
                    throw new Exception(" pPlayerPropertyComponent = null");
                }


                //获取该系统全部属性
                for (int i = 1; i < (int)EPlayerPropertyType.Max; i++)
                {
                    PlayerPropGmProto pPlayerPropGmProto = new PlayerPropGmProto();

                    pPlayerPropGmProto.nPropEnumId = i;
                    pPlayerPropGmProto.lPropNum = pPlayerPropertyComponent.GetProperty((ESystemType)request.nSystemEnumId ,(EPlayerPropertyType)i);
                    response.pListSystemProp.Add(pPlayerPropGmProto);
                }

                reply();
                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                response.Error = ErrorCode.ERR_SystemError;
                reply();
            }

        }
    }




}
