
/*----------------------------------------------------------------
* 文件名:	UIGmPlayerOssEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/1 14:58:06
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
    [FriendClass(typeof(UIGmPlayerOss))]
    // 告诉系统 UI的id
    [UIEventAttribute(nameof(UIGmPlayerOss))]

    // 组件名+UIEvent 定义你的UI事件处理类
    public class UIGmPlayerOssUIEvent : UIEventBase<UIGmPlayerOss>
    {

        public override void OnCreateWindow(ref UIComponent pUIComponent)
        {
            // 系统启动时回调CreateUI
            // 1. 往UI上挂在自己写的窗口组件
            pUIComponent.AddComponent<UIGmPlayerOss>();
        }

        public override void OnLoadResouse(UIGmPlayerOss self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
        {
            // 当AB被加载后，会调用LoadUI
            self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UILogin.unity3d");
            GameObject pUIGmMain = (GameObject)ResourcesComponent.Instance.GetAsset("UILogin.unity3d", "UIGMOss");
            self.m_window = GameObject.Instantiate(pUIGmMain, dicLayer[EUILayer.high]);
            self.m_window.name = szWindowName;

            pUIGmMain.GetComponent<RectTransform>().localScale = Vector3.one;
            pUIGmMain.GetComponent<RectTransform>().localPosition = Vector3.zero;

            // Item交给UIComponent Hold住
            var pESV_List = UITools.FindChild(self.m_window, "E_SL_GMOssList");
            GameObject pItem_LoginPlayerList = UITools.FindChild(pESV_List, "Item_GMOssList");
            UIComponent.Instance.ItemHold(pItem_LoginPlayerList);

            // 注册事件
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Back")?.AddListener(self.OnClickClose);
            // 各系统按钮事件
            UITools.FindChildComponent<Button>(self.m_window, "EBt_PlayerData")?.AddListenerWithId(self.OnShow, (int)EOssPageType.DataSystem);
            //UITools.FindChildComponent<Button>(self.m_window, "EBt_PlayerBag")?.AddListenerWithId(self.OnShow, (int)EOssPageType.BagSystem);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_PlayerProperty")?.AddListenerWithId(self.OnShow, (int)EOssPageType.PropSystem);
            // 上一页，下一眼按钮
            UITools.FindChildComponent<Button>(self.m_window, "EBt_LastPage")?.AddListener(self.OnClickLastPage);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_NextPage")?.AddListener(self.OnClickNextPage);
        }

        public override void OnDestroyResouse(UIGmPlayerOss self)
        {
            // 窗口AB资源被销毁前，会调用UnloadUI
            UITools.DestroyChildren(self.m_window);
            GameObject.DestroyImmediate(self.m_window);
            self.m_window = null;
            // 销毁Gm_Item
            UIComponent.Instance.ItemDestory("Item_GMOssList");
        }

        public override void OnShowWindow(UIGmPlayerOss self, object showData)
        {
            // 窗口被显示的时候，会调用ShowUI
            // 在这里处理界面打开的刷新
            self.OnShow();
        }

        public override void OnHideWindow(UIGmPlayerOss self)
        {
            // 界面被关闭的时候，会调用HideUI
            // 关闭掉一些东西，例如定时器
            self.m_nPageNum = 0;
            self.m_eType = EOssPageType.DataSystem;

            //UIComponent.Instance.ItemDestory("Gm_OssItem");
        }


    }
}