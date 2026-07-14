/*----------------------------------------------------------------
* 文件名:	UILoginServerListEvent
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
    [FriendClass(typeof(UILoginServerList))]
    // 告诉系统 UI的id
    [UIEvent(nameof(UILoginServerList))]

    // 组件名+UIEvent 定义你的UI事件处理类
    public class UILoginServerListUIEvent : UIEventBase<UILoginServerList>
    {

        public override void OnCreateWindow(ref UIComponent pUIComponent)
        {
            // 系统启动时回调CreateUI
            // 1. 往UI上挂在自己写的窗口组件
            pUIComponent.AddComponent<UILoginServerList>();

        }

        public override void OnLoadResouse(UILoginServerList self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
        {
            // 当AB被加载后，会调用LoadUI
            self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UILogin.unity3d");
            GameObject pUIGmMain = (GameObject)ResourcesComponent.Instance.GetAsset("UILogin.unity3d", "UILoginServerList");
            self.m_window = GameObject.Instantiate(pUIGmMain, dicLayer[EUILayer.mid]);
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

            // 缓存住两个列表item
            var pE_SL_LoginServerList = UITools.FindChild(self.m_window, "E_SL_LoginServerList");
            var pE_SL_LoginPlayerList = UITools.FindChild(self.m_window, "E_SL_LoginPlayerList");

            GameObject pItem_LoginPlayerList = UITools.FindChild(pE_SL_LoginPlayerList, "Item_LoginPlayerList");
            UIComponent.Instance.ItemHold(pItem_LoginPlayerList);

            GameObject pItem_LoginServerList = UITools.FindChild(pE_SL_LoginServerList, "Item_LoginServerList");
            UIComponent.Instance.ItemHold(pItem_LoginServerList);


            UITools.FindChildComponent<Button>(self.m_window, "EBt_BG")?.AddListener(self.OnClickClose);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Back")?.AddListener(self.OnClickClose);

            UITools.FindChildComponent<Button>(self.m_window, "EBt_ServerList")?.AddListener(self.OnClickServerList);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_PlayerList")?.AddListener(self.OnClickPlayerList);
        }

        public override void OnDestroyResouse(UILoginServerList self)
        {
            // 窗口AB资源被销毁前，会调用UnloadUI
            // 在这里自己销毁掉自己创建的各种东西
            UITools.DestroyChildren(self.m_window);
            GameObject.DestroyImmediate(self.m_window);
            self.m_window = null;


            UIComponent.Instance.ItemDestory( "Item_LoginPlayerList");
            UIComponent.Instance.ItemDestory( "Item_LoginServerList");
        }



        public override void OnShowWindow(UILoginServerList self, object showData)
        {
            // 窗口被显示的时候，会调用ShowUI
            // 在这里处理界面打开的刷新
            Scene zoneScene = self.DomainScene();
            Account pAccount = zoneScene.GetComponent<Account>();

            if (pAccount.m_listPlayerHave.Count > 0)
            {
                self.OnClickPlayerList();
            }
            else
            {
                self.OnClickServerList();
            }
        }

        public override void OnHideWindow(UILoginServerList self)
        {
            // 界面被关闭的时候，会调用HideUI
            // 关闭掉一些东西，例如定时器
            var pServerScrollView = UITools.FindChild(self.m_window, "E_SL_LoginServerList", EUISearch.Deep);
            var pServerContent = UITools.FindChild(pServerScrollView, "Content", EUISearch.Deep);
            UITools.DestroyChildren(pServerContent);

            var pPlayerScrollView = UITools.FindChild(self.m_window, "E_SL_LoginPlayerList", EUISearch.Deep);
            var pPlayerContent = UITools.FindChild(pPlayerScrollView, "Content", EUISearch.Deep);
            UITools.DestroyChildren(pPlayerContent);
        }
    }
}
