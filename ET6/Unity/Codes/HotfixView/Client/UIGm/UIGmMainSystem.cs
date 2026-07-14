/*----------------------------------------------------------------
* 文件名:	GmMain
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/22 16:16:27
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using ET.EventType;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIGmMain_IAwake : AwakeSystem<UIGmMain>
    {
        public override void Awake(UIGmMain self)
        {
            self.m_window = null;
        }
    }

    // 析构函数
    public class UIGmMain_IDestroy : DestroySystem<UIGmMain>
    {
        public override void Destroy(UIGmMain self)
        {
            self.m_window = null;
        }
    }



    [FriendClassAttribute(typeof(UIGmMain))]


    public static  class UIGmMainSystem
    {
        public static void OnClickPlayerProperty(this UIGmMain self)
        {
            UIComponent.Instance.ShowWindow("UIGmPlayerProperty");
        }
        public static void OnClickPlayerOss(this UIGmMain self)
        {
            UIComponent.Instance.ShowWindow("UIGmPlayerOss");
        }
        public static void OnClickPlayerData(this UIGmMain self)
        {
            UIComponent.Instance.ShowWindow("UIGmPlayerData");
        }

        public static void OnClickPlayerBag(this UIGmMain self)
        {
            UIComponent.Instance.ShowWindow("UIGmPlayerBag");
        }

        public static void OnClickClose(this UIGmMain self)
        {
            UIComponent.Instance.HideWindow("UIGmMain");
            UIComponent.Instance.GetWindow<UIPackage>()?.OnRefresh();
        }


        public static async ETTask OnClickLogout(this UIGmMain self)
        {
            Scene zoneScene =  self.DomainScene();
            SessionComponent pSessionComponent =  zoneScene.GetComponent<SessionComponent>();

            G2C_LogoutGate g2CLoginGate = (G2C_LogoutGate)await pSessionComponent.Session.Call(
                new C2G_LogoutGate() );

            if(g2CLoginGate.Error == ErrorCode.ERR_Success)
            {
                Game.EventSystem.Publish(new EventType.ExitGame() { ZoneScene = zoneScene });                
            }


        }

    }
}
