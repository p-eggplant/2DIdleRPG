/*----------------------------------------------------------------
* 文件名:	AccountSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/29 19:46:56
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
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UI;

namespace ET
{
    [ObjectSystem]
    public class Account_Awake : AwakeSystem<Account>
    {
        public override void Awake(Account self)
        {
            self.m_lAccountID = 0;
            self.m_nDefaultGameServerID = 0;
            self.m_szAccount = string.Empty;
            self.m_nDefaultGameServerName = string.Empty;
            self.m_listPlayerHave = null;
            self.m_listServer = null;
        }
    }

    [ObjectSystem]
    public class Account_Destroy : DestroySystem<Account>
    {
        public override void Destroy(Account self)
        {
            self.m_lAccountID = 0;
            self.m_nDefaultGameServerID = 0;
            self.m_szAccount = string.Empty;
            self.m_nDefaultGameServerName = string.Empty;
            self.m_listPlayerHave = null;
            self.m_listServer = null;
        }
    }


    [FriendClass(typeof(Account))]
    public static class AccountSystem
    {
        public static void CleanAccount(this Account self)
        {
            self.m_nDefaultGameServerID = 0;
            self.m_nDefaultGameServerName = string.Empty;
            self.m_listPlayerHave = null;
            self.m_listServer = null;
        }
    }
}
