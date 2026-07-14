/*----------------------------------------------------------------
* 文件名:	PlayerMgrComponent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/26 17:39:28
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
    [ChildType(typeof(Player))]
    public class PlayerMgrComponent : Entity, IAwake, IDestroy
    {
        //PlayerId，Player对象
        public Dictionary<long, Player> m_dicPlayer = new Dictionary<long, Player>();
    }
}
