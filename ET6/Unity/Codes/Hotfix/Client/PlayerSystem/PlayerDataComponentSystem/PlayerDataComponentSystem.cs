/*----------------------------------------------------------------
* 文件名:	PlayerDataComponentSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/28 15:42:56
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
    [FriendClassAttribute(typeof(PlayerDataComponent))]

    // 构造函数
    public class PlayerDataComponent_IAwake : AwakeSystem<PlayerDataComponent>
    {
        public override void Awake(PlayerDataComponent self)
        {
            // 清理总数组
            for (int i = 0; i < self.m_arrData.Length; i++)
                self.m_arrData[i] = 0;

        }
    }

    // 析构函数
    public class PlayerDataComponent_IDestroy : DestroySystem<PlayerDataComponent>
    {
        public override void Destroy(PlayerDataComponent self)
        {
            // 清理总数组
            for (int i = 0; i < self.m_arrData.Length; i++)
                self.m_arrData[i] = 0;
        }
    }

    [FriendClassAttribute(typeof(PlayerDataComponent))]
    public static class PlayerDataComponentSystem
    {

        /// <summary>
        /// 得到玩家数值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eType">类型</param>
        /// <returns>数值</returns>
        public static long GetData(this PlayerDataComponent self, EPlayerDataType eType)
        {
            return self.m_arrData[(int)eType];
        }


        // 得到所有的数据
        public static Dictionary<EPlayerDataType,long> GetAllData(this PlayerDataComponent self)
        {
            Dictionary<EPlayerDataType, long> res = new Dictionary<EPlayerDataType, long>();
            for (int i = 1; i < self.m_arrData.Length; ++i)
            {
                res.Add((EPlayerDataType)i, self.m_arrData[i]);
            }
            return res;
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eType">数值类型</param>
        /// <param name="value">数值 只能为正整数</param>
        /// <param name="eReason">记录原因</param>
        /// <exception cref="Exception"></exception>
        public static void SetData(this PlayerDataComponent self, EPlayerDataType eType, long value)
        {
            if (value < 0)
            {
                throw new Exception($"数值小于零 eType = {eType.ToString()} value = {value}");
            }

            //设置数值
            long curValue = self.m_arrData[(int)eType];
            self.m_arrData[(int)eType] = value;

        }
    }

}
