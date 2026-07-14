/*----------------------------------------------------------------
* 文件名:	GmMain
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/22 16:14:47
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
    public class UIGmMain : Entity, IAwake , IDestroy
    {
        public GameObject m_window = null;
    }
}
