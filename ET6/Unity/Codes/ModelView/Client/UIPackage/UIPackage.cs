/*----------------------------------------------------------------
* 文件名:	UIPackage
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/5 13:44:30
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
namespace ET
{
    [ComponentOf(typeof(UIComponent))]
    public class UIPackage : Entity, IAwake, IDestroy
    {
        public GameObject m_window = null;
    }
}

