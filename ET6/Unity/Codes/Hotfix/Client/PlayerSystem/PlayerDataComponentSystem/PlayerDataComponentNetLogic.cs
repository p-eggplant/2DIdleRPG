/*----------------------------------------------------------------
* 文件名:	PlayerDataComponentNetLogic
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/28 16:22:34
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
    public class GGame2C_PlayerDataChangeNetLogic : AMHandler<Game2C_PlayerDataChange>
    {
        protected override void Run(Session session, Game2C_PlayerDataChange message)
        {

            Scene zoneScene = (Scene)session.DomainScene();
            Player pPlayer = zoneScene.GetComponent<Player>();
            if (pPlayer == null)
            {
                throw new Exception("pPlayer == null");
            }
            PlayerDataComponent pPlayerDataComponent = pPlayer.GetComponent<PlayerDataComponent>();
            if (pPlayerDataComponent == null)
            {
                throw new Exception("pPlayerDataComponent == null");
            }


            // 只接受需要同步的
            if(message.EnumId > (int)EPlayerDataType.Send2ClientStart &&  message.EnumId <= (int)EPlayerDataType.Send2ClientEnd)
            {
                pPlayerDataComponent.SetData((EPlayerDataType)message.EnumId, message.Value);
            }
            else
            {
                throw new Exception($"无效的PlayerDataType被同步下来  message.EnumId = {message.EnumId}");
            }

        }
    }

}
