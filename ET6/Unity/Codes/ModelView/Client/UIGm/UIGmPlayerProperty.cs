/*----------------------------------------------------------------
* 文件名:	UIGmPlayerProperty
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/2 11:15:26
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

    [ComponentOf(typeof(UIComponent))]
    public class UIGmPlayerProperty : Entity, IAwake, IDestroy
    {
        public GameObject m_window = null;
        public ESystemType m_eType = ESystemType.RankSystem;        //GM属性系统分类,默认境界系统
    }
}