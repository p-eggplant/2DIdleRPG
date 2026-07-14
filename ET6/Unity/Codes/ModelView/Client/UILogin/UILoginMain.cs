/*----------------------------------------------------------------
* 文件名:	UILoginMain
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/22 16:13:14
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
    public class UILoginMain : Entity, IAwake,IDestroy
    {
        public GameObject m_window = null;
        public string szAccount;
        public int nEnterGameServerID;
        public string nEnterGameServerName;
        public long lAccountID;
    }
}
