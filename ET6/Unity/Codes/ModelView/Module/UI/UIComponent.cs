/*----------------------------------------------------------------
* 文件名:	UIManager
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/20 13:54:53
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System.Collections.Generic;
using UnityEngine;
namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class UIComponent : Entity, IAwake, IDestroy
    {
        // 异步锁
        public uint m_AsyncLock = 0;
        // 单例
        public static UIComponent Instance;
        // item临时挂在点
        public GameObject m_ItemParent = null;
        // 挂在层
        public Dictionary<EUILayer, Transform> m_dicLayer = new Dictionary<EUILayer, Transform>();
        // 挂在事件
        public Dictionary<string, IUIEvent> m_dicEvent = new Dictionary<string, IUIEvent>();

    }
  
}
