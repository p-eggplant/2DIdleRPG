/*----------------------------------------------------------------
* 文件名:	PlayerHelper
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/28 17:40:21
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

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public sealed class PlayerHelper : Entity, IAwake, IDestroy
    {
        public static PlayerHelper Instance = null;
        public List<IPlayerComponentEvent> m_dicPlayerEvent = new List<IPlayerComponentEvent>();

    }

}
