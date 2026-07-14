/*----------------------------------------------------------------
* 文件名:	UIGmPlayerDataEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/29 16:07:22
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
    [FriendClass(typeof(UIGmPlayerData))]
    // 告诉系统 UI的id
    [UIEventAttribute(nameof(UIGmPlayerData))]

    // 组件名+UIEvent 定义你的UI事件处理类
    public class UIGmPlayerDataUIEvent : UIEventBase<UIGmPlayerData>
    {

        public override void OnCreateWindow(ref UIComponent pUIComponent)
        {
            // 系统启动时回调CreateUI
            // 1. 往UI上挂在自己写的窗口组件
            pUIComponent.AddComponent<UIGmPlayerData>();

        }

        public override void OnLoadResouse(UIGmPlayerData self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
        {
            self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UILogin.unity3d");
            GameObject pUIGmMain = (GameObject)ResourcesComponent.Instance.GetAsset("UILogin.unity3d", "UIGMData");
            self.m_window = GameObject.Instantiate(pUIGmMain, dicLayer[EUILayer.high]);
            self.m_window.name = szWindowName;

            pUIGmMain.GetComponent<RectTransform>().localScale = Vector3.one;
            pUIGmMain.GetComponent<RectTransform>().localPosition = Vector3.zero;

            // Item交给UIComponent Hold住
            var pE_SL_GMDataList = UITools.FindChild(self.m_window, "E_SL_GMDataList");
            GameObject pItem_GMDataList = UITools.FindChild(pE_SL_GMDataList, "Item_GMDataList");
            UIComponent.Instance.ItemHold(pItem_GMDataList);


            // 当AB被加载后，会调用LoadUI
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Back")?.AddListener(self.OnClickClose);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Determine")?.AddListenerAsync(self.OnClickSet);
        }

        public override void OnDestroyResouse(UIGmPlayerData self)
        {
            // 窗口AB资源被销毁前，会调用UnloadUI
            // 在这里自己销毁掉自己创建的各种东西
            UITools.DestroyChildren(self.m_window);
            GameObject.DestroyImmediate(self.m_window);
            self.m_window = null;

            UIComponent.Instance.ItemDestory("Item_GMDataList");
        }



        public override void OnShowWindow(UIGmPlayerData self, object showData)
        {
            // 窗口被显示的时候，会调用ShowUI
            // 在这里处理界面打开的刷新
            self.OnShow();
        }

        public override void OnHideWindow(UIGmPlayerData self)
        {
            // 界面被关闭的时候，会调用HideUI
            // 关闭掉一些东西，例如定时器
        }


    }
}




