/*----------------------------------------------------------------
* 文件名:	EnterGameEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/28 13:46:38
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
    public class EnterGameEvent : AEvent<EventType.EnterGame>
    {
        protected override void Run(EventType.EnterGame args)
        {

            

            UIComponent.Instance.DestroyAllWindow();
            UIComponent.Instance.ShowWindow("UIPackage");

        }
    }

    public class ExitGameEvent : AEvent<EventType.ExitGame>
    {
        protected override void Run(EventType.ExitGame args)
        {

            UIComponent.Instance.DestroyAllWindow();
            Player pPlayer = args.ZoneScene.GetComponent<Player>();
            //args.ZoneScene.RemoveComponent<Session>
            SessionComponent pSessionComponent = args.ZoneScene.GetComponent<SessionComponent>();
            pSessionComponent.Session.Dispose();
            args.ZoneScene.RemoveComponent<SessionComponent>();

            // PlayerHelper.Instance.
            Game.EventSystem.Publish(new EventType.EnterLogin() { ZoneScene = args.ZoneScene });
        }
    }
}
