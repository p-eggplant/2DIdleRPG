using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof(DlgRoleUp))]
    [UIEventAttribute(nameof(DlgRoleUp))]
    public class DlgRoleUpUIEvent : UIEventBase<DlgRoleUp>
    {
        public override void OnCreateWindow(ref UIComponent pUIComponent)
        {
            pUIComponent.AddComponent<DlgRoleUp>();
        }

        public override void OnLoadResouse(DlgRoleUp self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
        {
            self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UILogin.unity3d");
            GameObject prefab = (GameObject)ResourcesComponent.Instance.GetAsset("UILogin.unity3d", "dlg_role_up");
            self.m_window = GameObject.Instantiate(prefab, dicLayer[EUILayer.high]);
            self.m_window.name = szWindowName;

            self.m_window.GetComponent<RectTransform>().localScale = Vector3.one;
            self.m_window.GetComponent<RectTransform>().localPosition = Vector3.zero;

            UITools.FindChildComponent<Button>(self.m_window, "EBt_Back")?.AddListener(self.OnClickClose);
            UITools.FindChildComponent<Button>(self.m_window, "EBt_LevelUp")?.AddListenerAsync(self.OnClickLevelUp);
        }

        public override void OnDestroyResouse(DlgRoleUp self)
        {
            UITools.DestroyChildren(self.m_window);
            GameObject.DestroyImmediate(self.m_window);
            self.m_window = null;
        }

        public override void OnShowWindow(DlgRoleUp self, object showData)
        {
            self.OnRefresh();
        }

        public override void OnHideWindow(DlgRoleUp self)
        {
        }
    }
}
