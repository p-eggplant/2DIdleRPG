/*----------------------------------------------------------------
* 文件名:	UIGmMainEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/29 16:05:02
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof(UIGmMain))]
    // 告诉系统 
    [UIEventAttribute(nameof(UIGmMain))]

    // 组件名+UIEvent 定义你的UI事件处理类
    public class UIGmMainUIEvent : UIEventBase<UIGmMain>
    {


        public override void OnCreateWindow(ref UIComponent pUIComponent)
        {
            // 系统启动时回调CreateUI
            pUIComponent.AddComponent<UIGmMain>();

        }



        public override void OnLoadResouse(UIGmMain self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
        {
            self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UILogin.unity3d");
            GameObject pUIGmMain = (GameObject)ResourcesComponent.Instance.GetAsset("UILogin.unity3d", "UIGMMain");
            self.m_window = GameObject.Instantiate(pUIGmMain, dicLayer[EUILayer.high]);
            self.m_window.name = szWindowName;

            pUIGmMain.GetComponent<RectTransform>().localScale = Vector3.one;
            pUIGmMain.GetComponent<RectTransform>().localPosition = Vector3.zero;
            

            // 挂在后注册关闭事件
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Back")?.AddListener(self.OnClickClose);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Data")?.AddListener(self.OnClickPlayerData);
            //UITools.FindChildComponent<Button>(self.m_window, "Bt_Logout")?.AddListenerAsync(self.OnClickLogout);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Bag")?.AddListener(self.OnClickPlayerBag);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Oss")?.AddListener(self.OnClickPlayerOss);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Numerical")?.AddListener(self.OnClickPlayerProperty);
        }




        public override void OnDestroyResouse(UIGmMain self)
        {
            UITools.DestroyChildren(self.m_window);
            GameObject.DestroyImmediate(self.m_window);
            self.m_window = null;
        }


        public override void OnShowWindow(UIGmMain self, object showData)
        {
            
        }



        public override void OnHideWindow(UIGmMain self)
        {
           
        }


    }
}
