/*----------------------------------------------------------------
* 文件名:	UIGmPlayerPropertyEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/2 11:13:37
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof(UIGmPlayerProperty))]
    // 告诉系统 UI的id
    [UIEventAttribute(nameof(UIGmPlayerProperty))]

    // 组件名+UIEvent 定义你的UI事件处理类
    public class UIGmPlayerPropertyUIEvent : UIEventBase<UIGmPlayerProperty>
    {

        public override void OnCreateWindow(ref UIComponent pUIComponent)
        {
            // 系统启动时回调CreateUI
            // 1. 往UI上挂在自己写的窗口组件
            pUIComponent.AddComponent<UIGmPlayerProperty>();

        }

        public override void OnLoadResouse(UIGmPlayerProperty self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
        {
            self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UILogin.unity3d");
            GameObject pUIGmMain = (GameObject)ResourcesComponent.Instance.GetAsset("UILogin.unity3d", "UIGMProperty");
            self.m_window = GameObject.Instantiate(pUIGmMain, dicLayer[EUILayer.high]);
            self.m_window.name = szWindowName;

            pUIGmMain.GetComponent<RectTransform>().localScale = Vector3.one;
            pUIGmMain.GetComponent<RectTransform>().localPosition = Vector3.zero;

            //Item交给UIComponent Hold住
            var pE_SL_GMPropertyList = UITools.FindChild(self.m_window, "E_SL_GMPropertyList");
            GameObject pItem_GMPropertyList = UITools.FindChild(pE_SL_GMPropertyList, "Item_GMPropertyList");
            UIComponent.Instance.ItemHold(pItem_GMPropertyList);

            var pE_SL_GMPropertySwitch = UITools.FindChild(self.m_window, "E_SL_GMPropertySwitch");
            GameObject pItem_GMPropertySwitch = UITools.FindChild(pE_SL_GMPropertySwitch, "Item_GMPropertySwitch");
            UIComponent.Instance.ItemHold(pItem_GMPropertySwitch);


            // 当AB被加载后，会调用LoadUI
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Back")?.AddListener(self.OnClickClose);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_All")?.AddListenerAsync(self.OnClickTotalProp);

        }

        public override void OnDestroyResouse(UIGmPlayerProperty self)
        {
            // 窗口AB资源被销毁前，会调用UnloadUI
            // 在这里自己销毁掉自己创建的各种东西
            UITools.DestroyChildren(self.m_window);
            GameObject.DestroyImmediate(self.m_window);
            self.m_window = null;

            UIComponent.Instance.ItemDestory("Item_GMPropertyList");
            UIComponent.Instance.ItemDestory("Item_GMPropertySwitch");
        }

        public override void OnShowWindow(UIGmPlayerProperty self, object showData)
        {
            // 窗口被显示的时候，会调用ShowUI
            // 在这里处理界面打开的刷新
            self.Refresh();
            self.OnClickTotalProp().Coroutine();
        }

        public override void OnHideWindow(UIGmPlayerProperty self)
        {
            // 界面被关闭的时候，会调用HideUI
            // 关闭掉一些东西，例如定时器
        }


    }
}





