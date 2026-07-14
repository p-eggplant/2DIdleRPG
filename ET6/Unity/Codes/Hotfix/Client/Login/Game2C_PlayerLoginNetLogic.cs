/*----------------------------------------------------------------
* 文件名:	Game2C_PlayerLogin
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/28 13:11:58
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [MessageHandler]
    public class Game2C_PlayerLoginNetLogic : AMHandler<PlayerLoginInfo>
    {
        protected override void Run(Session session, PlayerLoginInfo message)
        {
            // 
            Log.Debug("PlayerLoginInfo");

            Scene zoneScene = (Scene)session.DomainScene();
            Player pPlayer = null;
            pPlayer = zoneScene.GetComponent<Player>();
            if (pPlayer != null)
            {
                Log.Debug("pPlayer != null");
            }


            // 导入数据
            PlayerHelper.Instance.ImprotLoginData(ref pPlayer, ref message);

            // 初始化
            PlayerHelper.Instance.Init(ref pPlayer);

            // 玩家数据全部准备完毕后，开始发进入游戏消息
            Game.EventSystem.Publish(new EventType.EnterGame() { ZoneScene = zoneScene });

        }
    }
}
