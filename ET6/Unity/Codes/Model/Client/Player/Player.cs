/*----------------------------------------------------------------
* 文件名:	Player
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/28 13:19:00
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;


namespace ET
{
    [ComponentOf(typeof(Scene))]
    public sealed class Player : Entity, IAwake, IDestroy
    {
        // 玩家的ID 永远不变
        public long m_lPlayerId { get; set; }
     
    }
}

