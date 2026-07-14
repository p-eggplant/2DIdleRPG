/*----------------------------------------------------------------
* 文件名:	EnterLoginEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/29 19:27:27
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
    public class EnterLoginEvent : AEvent<EventType.EnterLogin>
    {
        protected override void Run(EventType.EnterLogin args)
        {
           
            UIComponent.Instance.DestroyAllWindow();
            UIComponent.Instance.ShowWindow("UILoginMain");
        }
    }

    public class ExitLoginEvent : AEvent<EventType.ExitLogin>
    {
        protected override void Run(EventType.ExitLogin args)
        {
            Account pAccount = args.ZoneScene.GetComponent<Account>();
            pAccount.CleanAccount();
            UIComponent.Instance.ShowWindow("UILoading","正在登录游戏".Trans());
           
        }
    }
}
