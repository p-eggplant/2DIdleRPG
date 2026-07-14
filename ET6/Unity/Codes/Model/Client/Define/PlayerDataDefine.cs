/*----------------------------------------------------------------
* 文件名:	PlayerDataDefine
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/27 16:39:25
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
    public enum EPlayerDataType
    {
        //绑定钻石、普通钻石、代金券
        None,
        Send2ClientStart = None,       // 需要同步开始-------------------
        FreeDiamond,                    // 绑定钻石
        PayDiamond,                     // 普通钻石
        Voucher,                        // 代金券
        Exp,  		                    // 当前经验
        Send2ClientEnd = Exp,    // 需要同步结束--------------------

        GMAccount,                      // 用户是否是GM账号，0不是，1是
        //--------------------------抽奖--------------------------
        RaffleguaranteedNum,        //抽奖保底数


        //--------------------------充值--------------------------
        CurrencyCount,          //（通天阁）累计充值记录

        //--------------------------修炼---------------------------
        RankFinal,               //修炼时期
        RankLevel,               //修炼等级       
        Max
    }

}
