/*----------------------------------------------------------------
* 文件名:	UIPackageEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/5 13:45:22
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
    [FriendClass(typeof(UIPackage))]
    // 告诉系统 UI的id
    [UIEventAttribute(nameof(UIPackage))]

    // 组件名+UIEvent 定义你的UI事件处理类
    public class UIPackageUIEvent : UIEventBase<UIPackage>
    {

        public override void OnCreateWindow(ref UIComponent pUIComponent)
        {
            // 系统启动时回调CreateUI
            // 1. 往UI上挂在自己写的窗口组件
            pUIComponent.AddComponent<UIPackage>();

        }

        public override void OnLoadResouse(UIPackage self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
        {
            // 当AB被加载后，会调用LoadUI
            self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UILogin.unity3d");
            GameObject pUIGmMain = (GameObject)ResourcesComponent.Instance.GetAsset("UILogin.unity3d", "dlgpackagemain");
            self.m_window = GameObject.Instantiate(pUIGmMain, dicLayer[EUILayer.low]);
            self.m_window.name = szWindowName;

            pUIGmMain.GetComponent<RectTransform>().localScale = Vector3.one;
            pUIGmMain.GetComponent<RectTransform>().localPosition = Vector3.zero;

            // Item交给UIComponent Hold住
            var pE_SL_MaterialList = UITools.FindChild(self.m_window, "E_SL_MaterialList");
            GameObject pItem_dlgpackagemain_MaterialList = UITools.FindChild(pE_SL_MaterialList, "Item_dlgpackagemain_MaterialList");
            UIComponent.Instance.ItemHold(pItem_dlgpackagemain_MaterialList);
            
            // 注册事件
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Back")?.AddListenerAsync(self.OnClickLogout);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Gm")?.AddListener(self.OnClickGm);

            //升级
            UITools.FindChildComponent<Button>(self.m_window, "EBt_PlayerLevelUp")?.AddListener(self.OnClickLevelUp);

        }

        public override void OnDestroyResouse(UIPackage self)
        {
            // 窗口AB资源被销毁前，会调用UnloadUI
            UITools.DestroyChildren(self.m_window);
            GameObject.DestroyImmediate(self.m_window);
            self.m_window = null;
            // 销毁Gm_Item
            UIComponent.Instance.ItemDestory("Item_dlgpackagemain_MaterialList");
        }

        public override void OnShowWindow(UIPackage self, object showData)
        {
            // 窗口被显示的时候，会调用ShowUI
            // 在这里处理界面打开的刷新
            self.OnRefresh();
        }

        public override void OnHideWindow(UIPackage self)
        {
            // 界面被关闭的时候，会调用HideUI
            // 关闭掉一些东西，例如定时器            
        }


    }
}

