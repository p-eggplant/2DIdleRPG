/*----------------------------------------------------------------
* 文件名:	Account
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/29 19:42:29
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class Account : Entity , IAwake, IDestroy
    {
        public string m_szAccount { get; set; }                         // 玩家的账户，上线前手动输入，上线后由SDK回传Token
        public long m_lAccountID { get; set; }                          // 玩家ID，是我们LoginServer给玩家生成的唯一ID
        public int m_nDefaultGameServerID { get; set; }                 // 登录界面默认服务器ID 
        public string m_nDefaultGameServerName { get; set; }            // 登录界面默认服务器名字

        // 玩家角色列表
        public List<LoginPlayerInfoProto> m_listPlayerHave  { get;set; }      
        // 玩家服务器列表
        public List<LoginServerInfoProto> m_listServer { get; set; }
    }
}
