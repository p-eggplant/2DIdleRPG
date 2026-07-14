/*----------------------------------------------------------------
* 文件名:	AccountPlayer
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/25 11:44:25
* 创建人:
* 描  述:	在账号服务器的玩家角色数据

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
    public class AccountPlayer : Entity,IAwake,IDestroy
    {
        // 玩家ID
        public long lPlayerID = 0;
        // 所属服务器服务器ID
        public int nGameServerID = 0;
        // 所属账号
        public long lAccountID = 0;
        // 创建时间
        public long lCreateTime = 0;

        // 角色名字 
        public string szName = string.Empty;
        // 战力
        public long lFightingCapacity = 0;

    }
}
