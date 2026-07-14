using UnityEngine;

namespace ET
{
    [ComponentOf(typeof(UIComponent))]
    public class DlgRoleUp : Entity, IAwake, IDestroy
    {
        public GameObject m_window = null;
    }
}
