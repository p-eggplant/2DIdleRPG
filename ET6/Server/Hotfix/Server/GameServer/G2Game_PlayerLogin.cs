/*----------------------------------------------------------------
* 文件名:	G2Game_PlayerLogin
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/26 17:00:57
* 创建人:   王星莅
* 描  述:	玩家登录时的网络业务逻辑处理

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class G2Game_PlayerLoginHandler : AMActorRpcHandler<Scene, G2Game_PlayerLogin, Game2G_PlayerLogin>
    {
        protected override async ETTask Run(Scene scene, G2Game_PlayerLogin request, Game2G_PlayerLogin response, Action reply)
        {

            // 该服务器的ZoneScene不是所需类型
            if (scene.DomainScene().SceneType != SceneType.Game)
            {
                throw new Exception($"请求的Scene错误，当前为Game, 请求为：{scene.DomainScene().SceneType}");
            }


            // 玩家管理器
            PlayerMgrComponent playerMgrComponent = scene.GetComponent<PlayerMgrComponent>();
            if (playerMgrComponent == null)
            {
                throw new Exception("playerMgrComponent == null");
            }


            // 玩家对象存在
            Player player = null;
            if (playerMgrComponent.IsExist(request.PlayerID))
            {
                //player对象已存在，只改变sessionInstanceId
                player = playerMgrComponent.Get(request.PlayerID);
            }
            // 玩家对象不存在
            else
            {
                // 创建一个Player空对象
                player = playerMgrComponent.AddChildWithId<Player, long, int>(request.PlayerID, request.PlayerID, scene.Zone);
                playerMgrComponent.Add(request.PlayerID, player);
                player.AddComponent<MailBoxComponent>();

                // 玩家对象的创建
                PlayerHelper.CreateComponent(ref player);

                // 访问数据库查询数据
                (bool isExistDBData, Dictionary<string, Entity> Entitydic) = await UnitCacheHelper.Get(player);


                // 如果玩家已经有了数据
                if (true == isExistDBData)
                {
                    //根据组件名创建组件数据
                    PlayerHelper.ImprotDBData(ref player, Entitydic);
                }
                else
                {
                    // 执行玩家第一次创建逻辑
                    PlayerHelper.FirstCreate(ref player);
                    PlayerHelper.ExportDBData(ref player);
                    // 将玩家数据回写进数据库
                    UnitCacheHelper.Set(player);
                }

                // 初始化玩家数据
                PlayerHelper.Init(ref player);
            }


            // 互相交换 InstanceId
            response.PlayerInstanceId = player.InstanceId;
            player.m_lSessionInstanceId = request.SessionInstanceId;
            reply();

            // 导出玩家登陆数据
            PlayerLoginInfo pLoginInfo = PlayerHelper.ExportLoginData(ref player);
            await TimerComponent.Instance.WaitAsync(1000);
            MessageHelper.SendToClient(player, pLoginInfo);
            //player对象创建成功

        }
    }


    public class G2Game_PlayerLogoutHandler : AMActorRpcHandler<Scene, G2Game_PlayerLogout, Game2G_PlayerLogout>
    {
        protected override async ETTask Run(Scene scene, G2Game_PlayerLogout request, Game2G_PlayerLogout response, Action reply)
        {
                // 该服务器的ZoneScene不是所需类型
            if (scene.DomainScene().SceneType != SceneType.Game)
            {
                throw new Exception($"请求的Scene错误，当前为Game, 请求为：{scene.DomainScene().SceneType}");
            }


            // 玩家管理器
            PlayerMgrComponent playerMgrComponent = scene.GetComponent<PlayerMgrComponent>();
            if (playerMgrComponent == null)
            {
                throw new Exception("playerMgrComponent == null");
            }


            // 玩家对象存在
            if (false == playerMgrComponent.IsExist(request.PlayerID))
            {

                throw new Exception($"不存在的玩家 playerid = {request.PlayerID}"); 
            }

            Player pPlayer = playerMgrComponent.Get(request.PlayerID);

            // 将玩家数据回写进数据库
            PlayerHelper.ExportDBData(ref pPlayer);
            UnitCacheHelper.Set(pPlayer);

            playerMgrComponent.Remove(pPlayer.m_lPlayerId);
            pPlayer.Dispose();
                
            reply(); 
            await ETTask.CompletedTask;
        }
    }
}
