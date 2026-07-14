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
    public class UIExample_IAwake : AwakeSystem<UIExample>
    {
        public override void Awake(UIExample self)
        {
            self.m_window = null;
        }
    }

    // 析构函数
    public class UIExample_IDestroy : DestroySystem<UIExample>
    {
        public override void Destroy(UIExample self)
        {
            self.m_window = null;
        }
    }


    [FriendClassAttribute(typeof(UIExample))]
    public static class UIExampleSystem 
    {
        public static void OnClickClose(this UIExample self)
        {
            UIComponent.Instance.HideWindow("UIExample");
        }
    }
}
