/*----------------------------------------------------------------
* 文件名:	UIGmPlayerBagEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/1 13:25:17
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof(UIGmPlayerBag))]
    // 告诉系统 UI的id
    [UIEventAttribute(nameof(UIGmPlayerBag))]

    // 组件名+UIEvent 定义你的UI事件处理类
    public class UIGmPlayerBagEvent : UIEventBase<UIGmPlayerBag>
    {

        public override void OnCreateWindow(ref UIComponent pUIComponent)
        {
            // 系统启动时回调CreateUI
            // 1. 往UI上挂在自己写的窗口组件
            pUIComponent.AddComponent<UIGmPlayerBag>();

        }



        public override void OnLoadResouse(UIGmPlayerBag self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
        {
            // 当AB被加载后，会调用LoadUI
            self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UILogin.unity3d");
            GameObject pUIGmMain = (GameObject)ResourcesComponent.Instance.GetAsset("UILogin.unity3d", "Gm_PlayerBag");
            self.m_window = GameObject.Instantiate(pUIGmMain, dicLayer[EUILayer.high]);
            self.m_window.name = szWindowName;

            pUIGmMain.GetComponent<RectTransform>().localScale = Vector3.one;
            pUIGmMain.GetComponent<RectTransform>().localPosition = Vector3.zero;

            // Item交给UIComponent Hold住
            var pESV_List = UITools.FindChild(self.m_window, "ESV_List");
            GameObject pItem_LoginPlayerList = UITools.FindChild(pESV_List, "Gm_PlayerBag_Item");
            UIComponent.Instance.ItemHold(pItem_LoginPlayerList);

            // 注册事件
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Close")?.AddListener(self.OnClickClose);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Add")?.AddListenerAsync(self.OnClickAdd);

        }

        public override void OnDestroyResouse(UIGmPlayerBag self)
        {
            // 窗口AB资源被销毁前，会调用UnloadUI
            UITools.DestroyChildren(self.m_window);
            GameObject.DestroyImmediate(self.m_window);
            self.m_window = null;

            UIComponent.Instance.ItemDestory("Gm_PlayerBag_Item");
        }



        public override void OnShowWindow(UIGmPlayerBag self, object showData)
        {
            // 窗口被显示的时候，会调用ShowUI
            // 在这里处理界面打开的刷新
            self.OnShow();
        }

        public override void OnHideWindow(UIGmPlayerBag self)
        {
            // 界面被关闭的时候，会调用HideUI
            // 关闭掉一些东西，例如定时器
        }


    }
}

