/*----------------------------------------------------------------
* 文件名:	UILoginServerList
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/25 14:59:07
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using UnityEngine;

namespace ET
{
    
    [ComponentOf(typeof(UIComponent))]
    public class UILoginServerList:Entity , IAwake, IDestroy
    {
        public GameObject m_window = null;
    }
}
