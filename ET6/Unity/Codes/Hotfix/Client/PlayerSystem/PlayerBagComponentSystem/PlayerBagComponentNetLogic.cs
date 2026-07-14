/*----------------------------------------------------------------
* 文件名:	PlayerBagComponentNetLogic
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/28 16:18:04
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
    [MessageHandler]
    public class Game2C_BagChangeNetLogic : AMHandler<Game2C_BagChange>
    {
        protected override void Run(Session session, Game2C_BagChange message)
        {

            Scene zoneScene = (Scene)session.DomainScene();
            Player pPlayer = zoneScene.GetComponent<Player>();
            if (pPlayer == null)
            {
                throw new Exception("pPlayer == null");
            }
            PlayerBagComponent pPlayerBagComponent = pPlayer.GetComponent<PlayerBagComponent>();
            if(pPlayerBagComponent == null)
            {
                throw new Exception("pPlayerBagComponent == null");
            }

            foreach (var item in message.BagNodes)
            {
                pPlayerBagComponent.SetItemNum(item.ConfigId, item.Num);
            }

        }
    }
}
