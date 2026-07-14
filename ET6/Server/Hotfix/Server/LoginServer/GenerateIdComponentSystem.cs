/*----------------------------------------------------------------
* 文件名:	GeneratePlayerIdComponentSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/24 18:14:56
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using SharpCompress.Compressors.Xz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    
    [FriendClass(typeof(GenerateIdComponent))]
    public  static class GenerateIdComponentSystem
    {
        public static long GenerateIdByTimeAndServerid(this GenerateIdComponent self, uint nServerID)
        {

            long nCurTimestamp = TimeHelper.ServerNow();
            if (nCurTimestamp != self.m_NowTime)
            {
                self.m_Index = 0;
            }

            self.m_Index++;
            self.m_NowTime = nCurTimestamp;

            if(self.m_Index >= 1024)
            {
                throw new Exception($"PlayerID生成失败 索引值超出最大范围 m_Index = {self.m_Index}");
            }

            if(nServerID >= 1024)
            {
                throw new Exception($"PlayerID生成失败 服务器ID超出最大值 nServerID = {nServerID}");
            }
            // 拼装ID
            long Id = (self.m_NowTime << 20) |
                (nServerID << 10)| 
                self.m_Index;

            return Id;

        }
    }
}
