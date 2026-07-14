using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof(DlgBagSell))]
    // 告诉系统 UI的id
    [UIEventAttribute(nameof(DlgBagSell))]

    // 组件名+UIEvent 定义你的UI事件处理类
    public class DlgBagSellUIEvent : UIEventBase<DlgBagSell>
    {

        public override void OnCreateWindow(ref UIComponent pUIComponent)
        {
            // 系统启动时回调CreateUI
            // 1. 往UI上挂在自己写的窗口组件
            pUIComponent.AddComponent<DlgBagSell>();

        }

        public override void OnLoadResouse(DlgBagSell self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
        {
            // 当AB被加载后，会调用LoadUI
            self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UILogin.unity3d");
            GameObject pUIGmMain = (GameObject)ResourcesComponent.Instance.GetAsset("UILogin.unity3d", "dlg_bag_sell");
            self.m_window = GameObject.Instantiate(pUIGmMain, dicLayer[EUILayer.high]);
            self.m_window.name = szWindowName;

            pUIGmMain.GetComponent<RectTransform>().localScale = Vector3.one;
            pUIGmMain.GetComponent<RectTransform>().localPosition = Vector3.zero;

            // 注册事件
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Close")?.AddListener(self.OnClickClose);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_Sell")?.AddListenerAsync(self.OnClickSell);

        }

        public override void OnDestroyResouse(DlgBagSell self)
        {
            // 窗口AB资源被销毁前，会调用UnloadUI
            UITools.DestroyChildren(self.m_window);
            GameObject.DestroyImmediate(self.m_window);
            self.m_window = null;
        }


        public override void OnShowWindow(DlgBagSell self, object showData)
        {
            // 窗口被显示的时候，会调用ShowUI
            // 在这里处理界面打开的刷新
            self.m_nMaterialId = (int)showData;
        }

        public override void OnHideWindow(DlgBagSell self)
        {
            // 界面被关闭的时候，会调用HideUI
            // 关闭掉一些东西，例如定时器
            self.m_nMaterialId = 0;
        }


    }
}

