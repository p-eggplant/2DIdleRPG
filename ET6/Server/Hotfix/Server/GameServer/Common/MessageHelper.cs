/*----------------------------------------------------------------
* 文件名:	MessageHelper
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/26 19:21:00
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
    public static class MessageHelper
    {

        public static void SendToClient(Player pPlayer, IActorMessage message)
        {
            SendActor(pPlayer.m_lSessionInstanceId, message);
        }
        /// <summary>
        /// 发送协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        public static void SendActor(long InstanceId, IActorMessage message)
        {
            ActorMessageSenderComponent.Instance.Send(InstanceId, message);
        }

        /// <summary>
        /// 发送RPC协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallActor(long InstanceId, IActorRequest message)
        {
            return await ActorMessageSenderComponent.Instance.Call(InstanceId, message);
        }
    }
}
