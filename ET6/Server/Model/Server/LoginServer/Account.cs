/*----------------------------------------------------------------
* 文件名:	Account
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/22 19:03:41
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
    
    public  class Account: Entity ,IAwake, IDestroy
    {
        // 账户名
        public string szAccount = "";
        // 账户ID
        public long nAccountID = 0;
        // 创建时间
        public long lCreateTime = 0;
        // 上次登陆时间
        public long lLastLoginTime = 0;
        // 最后一次登陆的服务器ID
        public int nLastGameServerID = 0;
       
    }
}
