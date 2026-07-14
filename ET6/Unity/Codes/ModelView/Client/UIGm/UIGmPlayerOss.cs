/*----------------------------------------------------------------
* 文件名:	UIGmPlayerOss
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/1 14:57:31
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using UnityEngine;

namespace ET
{
    public enum EOssPageType
    {
        DataSystem,                 // 数值系统 
        BagSystem,                  // 背包系统
        PropSystem,                 // 属性系统
    }

    [ComponentOf(typeof(UIComponent))]
    public class UIGmPlayerOss : Entity, IAwake, IDestroy
    {
        public GameObject m_window = null;
        public EOssPageType m_eType = EOssPageType.DataSystem;      //默认数值系统
        public int m_nPageNum = 0;                          //当前页数
    }
}
