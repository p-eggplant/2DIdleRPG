/*----------------------------------------------------------------
* 文件名:	PlayerBagComponentSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/28 16:13:38
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
    [FriendClassAttribute(typeof(PlayerBagComponent))]

    // 构造函数
    public class PlayerBagComponent_IAwake : AwakeSystem<PlayerBagComponent>
    {
        public override void Awake(PlayerBagComponent self)
        {
            // 清理总数组
            self.m_dicBag.Clear();

        }
    }

    // 析构函数
    public class PlayerBagComponent_IDestroy : DestroySystem<PlayerBagComponent>
    {
        public override void Destroy(PlayerBagComponent self)
        {
            // 清理总数组
            self.m_dicBag.Clear();
        }
    }



    [FriendClassAttribute(typeof(PlayerBagComponent))]
    public static class PlayerBagComponentSystem
    {
        /// <summary>
        /// 如果背包中不存在该configId，则返回0
        /// </summary>
        public static int GetItemNum(this PlayerBagComponent self, int nConfigId)
        {
            int nNum = 0;
            self.m_dicBag.TryGetValue(nConfigId, out nNum);
            return nNum;
        }

        /// <summary>
        /// 设置数量
        /// </summary>
        /// <param name="self"></param>
        /// <param name="nConfigId">配置ID</param>
        /// <param name="nNum">数据</param>
        public static void SetItemNum(this PlayerBagComponent self, int nConfigId, int nNum)
        {
            
            if (true == self.m_dicBag.ContainsKey(nConfigId))
            {
                self.m_dicBag[nConfigId] = nNum;
            }
            else
            {
                self.m_dicBag.Add(nConfigId, nNum);
            }
        }

    }
}