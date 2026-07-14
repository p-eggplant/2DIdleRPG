/*----------------------------------------------------------------
* 文件名:	UIDoTweenTestEvent
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
    [FriendClass(typeof(UIDoTweenTest))]
    // 告诉系统 UI的id
    [UIEventAttribute(nameof(UIDoTweenTest))]

    // 组件名+UIEvent 定义你的UI事件处理类
    public class UIDoTweenTestUIEvent : UIEventBase<UIDoTweenTest>
    {

        public override void OnCreateWindow(ref UIComponent pUIComponent)
        {
            // 系统启动时回调CreateUI
            // 1. 往UI上挂在自己写的窗口组件
            pUIComponent.AddComponent<UIDoTweenTest>();

        }

        public override void OnLoadResouse(UIDoTweenTest self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
        {
            // 当AB被加载后，会调用LoadUI
            self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UILogin.unity3d");
            GameObject pUIGmMain = (GameObject)ResourcesComponent.Instance.GetAsset("UILogin.unity3d", "UIDoTweenTest");
            self.m_window = GameObject.Instantiate(pUIGmMain, dicLayer[EUILayer.low]);
            self.m_window.name = szWindowName;

            pUIGmMain.GetComponent<RectTransform>().localScale = Vector3.one;
            pUIGmMain.GetComponent<RectTransform>().localPosition = Vector3.zero;

            // 注册事件
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Close")?.AddListener(self.OnClickClose);
        }

        public override void OnDestroyResouse(UIDoTweenTest self)
        {
            // 窗口AB资源被销毁前，会调用UnloadUI
            UITools.DestroyChildren(self.m_window);
            GameObject.DestroyImmediate(self.m_window);
            self.m_window = null;
        }



        public override void OnShowWindow(UIDoTweenTest self, object showData)
        {
            // 窗口被显示的时候，会调用ShowUI
            // 在这里处理界面打开的刷新
            var pELb_MoveTest1 = UITools.FindChildComponent<DoTweenObj>(self.m_window, "ELb_MoveTest1");
            pELb_MoveTest1.NumJump(0, 66666666);
            var pELb_MoveTest2 = UITools.FindChildComponent<DoTweenObj>(self.m_window, "ELb_MoveTest2");
            pELb_MoveTest2.NumJump(0, 500000000.99);
        }

        public override void OnHideWindow(UIDoTweenTest self)
        {
            // 界面被关闭的时候，会调用HideUI
            // 关闭掉一些东西，例如定时器
        }


    }
}
