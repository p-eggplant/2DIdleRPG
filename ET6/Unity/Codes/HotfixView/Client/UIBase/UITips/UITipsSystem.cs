/*----------------------------------------------------------------
* 文件名:	UITestSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/20 15:02:47
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ET
{
    // 构造函数
    public class UITips_IAwake : AwakeSystem<UITips>
    {
        public override void Awake(UITips self)
        {
            self.m_window = null;
        }
    }

    // 析构函数
    public class UITips_IDestroy : DestroySystem<UITips>
    {
        public override void Destroy(UITips self)
        {
            self.m_window = null;
        }
    }


    [FriendClassAttribute(typeof(UITips))]
    public static class UITipsSystem
    {
        public static void OnClickClose(this UITips self)
        {
            UIComponent.Instance.HideWindowDestroy("UITips");
        }
    }
}
