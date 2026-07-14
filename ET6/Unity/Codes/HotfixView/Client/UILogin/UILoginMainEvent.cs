/*----------------------------------------------------------------
* 文件名:	UILoginMainEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/29 16:00:11
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
    [FriendClass(typeof(UILoginMain))]
    // 告诉系统 UI的id
    [UIEventAttribute(nameof(UILoginMain))]

    // 组件名+UIEvent 定义你的UI事件处理类
    public class UILoginMainUIEvent : UIEventBase<UILoginMain>
    {

        public override void OnCreateWindow(ref UIComponent pUIComponent)
        {
            // 系统启动时回调CreateUI
            // 1. 往UI上挂在自己写的窗口组件
            pUIComponent.AddComponent<UILoginMain>();

        }

        public override void OnLoadResouse(UILoginMain self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
        {
            // 当AB被加载后，会调用LoadUI
            self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UILogin.unity3d");
            GameObject pUIGmMain = (GameObject)ResourcesComponent.Instance.GetAsset("UILogin.unity3d", "UILoginMain");
            self.m_window = GameObject.Instantiate(pUIGmMain, dicLayer[EUILayer.low]);
            self.m_window.name = szWindowName;

            pUIGmMain.GetComponent<RectTransform>().localScale = Vector3.one;
            pUIGmMain.GetComponent<RectTransform>().localPosition = Vector3.zero;

            TranslateList pTranslateList = self.m_window.GetComponent<TranslateList>();
            if(pTranslateList != null)
            {
                foreach(Text t in pTranslateList.m_arrText)
                {
                    t.text = t.text.Trans();
                }
            }
            
            UITools.FindChildComponent<Button>(self.m_window, "Bt_InputAccount")?.AddListenerAsync(self.OnClickInputAccount);
            UITools.FindChildComponent<Button>(self.m_window, "Bt_EnterGame")?.AddListenerAsync(self.OnClickEnterGame);
            UITools.FindChildComponent<Button>(self.m_window, "Bt_SelectServer")?.AddListenerAsync(self.OnRequestServerList);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Setting")?.AddListener(self.OnClickDoTweenTest);
        }

        public override void OnDestroyResouse(UILoginMain self)
        {
            // 窗口AB资源被销毁前，会调用UnloadUI
            // 在这里自己销毁掉自己创建的各种东西
            UITools.DestroyChildren(self.m_window);
            GameObject.DestroyImmediate(self.m_window);
            self.m_window = null;
        }



        public override void OnShowWindow(UILoginMain self, object showData)
        {

            // 窗口被显示的时候，会调用ShowUI
            // 在这里处理界面打开的刷新
            UITools.FindChild(self.m_window, "Panel_Account", EUISearch.Deep)?.SetActive(true);
            UITools.FindChild(self.m_window, "Panel_EnterGame", EUISearch.Deep)?.SetActive(false);


           

        }

        public override void OnHideWindow(UILoginMain self)
        {
            // 界面被关闭的时候，会调用HideUI
            // 关闭掉一些东西，例如定时器
        }


    }
}
