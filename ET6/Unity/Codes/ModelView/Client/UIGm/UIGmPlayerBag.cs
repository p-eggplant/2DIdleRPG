/*----------------------------------------------------------------
* 文件名:	UIGmPlayerBag
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/1 13:24:05
* 创建人:   陈澍
* 描  述:	GM背包UI

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
using UnityEngine;

namespace ET
{

    [ComponentOf(typeof(UIComponent))]
    public class UIGmPlayerBag : Entity, IAwake, IDestroy
    {
        public GameObject m_window = null;
    }
}

