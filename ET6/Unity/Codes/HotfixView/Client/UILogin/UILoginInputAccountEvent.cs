/*----------------------------------------------------------------
* 文件名:	UILoginInputAccountEvent
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
    [FriendClass(typeof(UILoginInputAccount))]
    // 告诉系统 UI的id
    [UIEventAttribute(nameof(UILoginInputAccount))]

    // 组件名+UIEvent 定义你的UI事件处理类
    public class UILoginInputAccountUIEvent : UIEventBase<UILoginInputAccount>
    {

        public override void OnCreateWindow(ref UIComponent pUIComponent)
        {
            // 系统启动时回调CreateUI
            // 1. 往UI上挂在自己写的窗口组件
            pUIComponent.AddComponent<UILoginInputAccount>();

        }

        public override void OnLoadResouse(UILoginInputAccount self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
        {
            self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UILogin.unity3d");
            GameObject pUIGmMain = (GameObject)ResourcesComponent.Instance.GetAsset("UILogin.unity3d", "UILoginInputAccount");
            self.m_window = GameObject.Instantiate(pUIGmMain, dicLayer[EUILayer.high]);
            self.m_window.name = szWindowName;

            pUIGmMain.GetComponent<RectTransform>().localScale = Vector3.one;
            pUIGmMain.GetComponent<RectTransform>().localPosition = Vector3.zero;

            // 语言翻译
            TranslateList pTranslateList = self.m_window.GetComponent<TranslateList>();
            if (pTranslateList != null)
            {
                foreach (Text t in pTranslateList.m_arrText)
                {
                    t.text = t.text.Trans();
                }
            }


            // 当AB被加载后，会调用LoadUI
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Handle")?.AddListener(self.OnClickSure);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Close")?.AddListener(self.OnClickClose);
        }

        public override void OnDestroyResouse(UILoginInputAccount self)
        {
            // 窗口AB资源被销毁前，会调用UnloadUI
            // 在这里自己销毁掉自己创建的各种东西
            UITools.DestroyChildren(self.m_window);
            GameObject.DestroyImmediate(self.m_window);
            self.m_window = null;
        }



        public override void OnShowWindow(UILoginInputAccount self, object showData)
        {
            // 窗口被显示的时候，会调用ShowUI
            // 在这里处理界面打开的刷新
        }

        public override void OnHideWindow(UILoginInputAccount self)
        {
            // 界面被关闭的时候，会调用HideUI
            // 关闭掉一些东西，例如定时器
        }


    }
}
