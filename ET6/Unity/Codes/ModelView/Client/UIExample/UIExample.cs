/*----------------------------------------------------------------
* 文件名:	UITest
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/20 14:02:59
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
namespace ET
{
    [ComponentOf(typeof(UIComponent))]
    public class UIExample : Entity, IAwake, IDestroy
    {
        public GameObject m_window = null;
    }
}
